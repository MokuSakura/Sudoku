using System.Reflection;
using log4net;
using MokuSakura.Sudoku.Core.Requirement.Common;

namespace MokuSakura.Sudoku.Core.Requirement;

public class SudokuRequirementReflectionUtils
{
    private static ILog Log => LogManager.GetLogger(typeof(SudokuRequirementReflectionUtils));
    public IDictionary<string, RequirementTemplate> RequirementTemplates { get; init; } = new Dictionary<string, RequirementTemplate>();
    public static SudokuRequirementReflectionUtils Instance { get; } = new();
    
    public static Type[] GetGenericArguments(Type type, Type genericType)
    {
        return type.GetInterfaces() //取类型的接口
            .Where(IsGenericType) //筛选出相应泛型接口
            .SelectMany(i => i.GetGenericArguments()) //选择所有接口的泛型参数
            .ToArray(); //ToArray

        bool IsGenericType(Type type1)
            => type1.IsGenericType && type1.GetGenericTypeDefinition() == genericType;
    }
    public object? CreateRequirement(string requirementName)
    {
        if (RequirementTemplates.TryGetValue(requirementName, out RequirementTemplate requirementTemplate))
        {
            return requirementTemplate.ParameterlessConstructor.Invoke(null);
        }

        return null;
    }

    private SudokuRequirementReflectionUtils()
    {
        Type requirementType = typeof(IRequirement<,>);
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (requirementType == type)
                {
                    continue;
                }
                if (type.GetInterface(requirementType.Name) == null)
                {
                    continue;
                }

                ConstructorInfo? constructorInfo = type.GetConstructor(Array.Empty<Type>());
                if (constructorInfo == null)
                {
                    Log.Warn($"{type.FullName} does not contain a parameterless constructor. Cannot initialize it.");
                    continue;
                }

                object requirement = constructorInfo.Invoke(Array.Empty<object>());
                RequirementTemplates[type.Name] = new RequirementTemplate()
                {
                    RequirementType = type,
                    Template = requirement,
                    ParameterlessConstructor = constructorInfo,
                };
            }
        }
    }

    public struct RequirementTemplate
    {
        public object Template { get; set; }
        public Type RequirementType { get; set; }
        public ConstructorInfo ParameterlessConstructor { get; set; }
    }
}
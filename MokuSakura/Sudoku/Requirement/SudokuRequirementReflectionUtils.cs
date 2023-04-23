using System.Reflection;
using log4net;

namespace MokuSakura.Sudoku.Requirement;

public class SudokuRequirementReflectionUtils
{
    private static ILog Log => LogManager.GetLogger(typeof(SudokuRequirementReflectionUtils));
    public IDictionary<String, RequirementTemplate> RequirementTemplates { get; init; } = new Dictionary<String, RequirementTemplate>();
    public static SudokuRequirementReflectionUtils Instance { get; } = new();
    public IRequirement? CreateRequirement(String requirementName)
    {
        if (RequirementTemplates.TryGetValue(requirementName, out RequirementTemplate requirementTemplate))
        {
            return (IRequirement) requirementTemplate.ParameterlessConstructor.Invoke(null);
        }

        return null;
    }
    private SudokuRequirementReflectionUtils()
    {
        Type requirementType = typeof(IRequirement);
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (requirementType == type )
                {
                    continue;
                }

                if (!requirementType.IsAssignableFrom(type))
                {
                    continue;
                }

                ConstructorInfo? constructorInfo = type.GetConstructor(Array.Empty<Type>());
                if (constructorInfo == null)
                {
                    Log.Warn($"{type.FullName} does not contain a parameterless constructor. Cannot initialize it.");
                    continue;
                }

                IRequirement requirement = (IRequirement)constructorInfo.Invoke(Array.Empty<Object>());
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
        public IRequirement Template { get; set; }
        public Type RequirementType { get; set; }
        public ConstructorInfo ParameterlessConstructor { get; set; }
    }
}
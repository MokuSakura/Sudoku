using System.Reflection;
using log4net;
using MokuSakura.Sudoku.Requirement;
using MokuSakura.Sudoku.Starter;

namespace MokuSakura.Sudoku;

public class SudokuGeneralEntryPoint
{
    private static ILog Log => LogManager.GetLogger(typeof(SudokuGeneralEntryPoint));

    public static ISudokuStarter DefaultStarter => new JsonSudokuStarter();

    public static SudokuGeneralEntryPoint Instance => new();
    public IDictionary<String, ISudokuStarter> SudokuStarters { get; init; } = new Dictionary<String, ISudokuStarter>();
    public IDictionary<String, Type> RequirementTypes { get; init; } = new Dictionary<String, Type>();

    public void Run(String starterName, IDictionary<String, Object> args)
    {
        if (!SudokuStarters.TryGetValue(starterName, out ISudokuStarter? starter))
        {
            Log.Error($"No starter {starterName} found. Use Default starter.");
            starter = DefaultStarter;
        }

        starter.Run(args);
    }

    private SudokuGeneralEntryPoint()
    {
        Type requirementType = typeof(IRequirement);
        Type starterType = typeof(ISudokuStarter);
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (requirementType == type || starterType == type)
                {
                    continue;
                }

                if (requirementType.IsAssignableFrom(type))
                {
                    ConstructorInfo? constructorInfo = type.GetConstructor(Array.Empty<Type>());
                    if (constructorInfo == null)
                    {
                        Log.Warn($"{type.FullName} does not contain a parameterless constructor. Cannot initialize it.");
                        continue;
                    }

                    IRequirement requirement = (IRequirement)constructorInfo.Invoke(Array.Empty<Object>());
                    RequirementTypes[requirement.RegisterName] = requirement.GetType();
                }

                if (starterType.IsAssignableFrom(type))
                {
                    ConstructorInfo? constructorInfo = type.GetConstructor(Array.Empty<Type>());
                    if (constructorInfo == null)
                    {
                        Log.Warn($"{type.FullName} does not contain a parameterless constructor. Cannot initialize it.");
                        continue;
                    }

                    ISudokuStarter starter = (ISudokuStarter)constructorInfo.Invoke(Array.Empty<Object>());
                    foreach (String s in starter.RegisterName)
                    {
                        SudokuStarters[s] = starter;
                    }
                }
            }
        }
    }
}
using System.Reflection;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using MokuSakura.Sudoku.Requirement;
using MokuSakura.Sudoku.Starter;

namespace MokuSakura.Sudoku;

public sealed class ConsoleApp
{
    private static ILog Log => LogManager.GetLogger(typeof(ConsoleApp));
    private static volatile ConsoleApp? instance;
    public static void Main(String[] args)
    {
        Instance.Run(args);
    }
    public static ConsoleApp Instance
    {
        get 
        {
            if (instance != null)
            {
                return instance;
            }

            lock (typeof(ConsoleApp)) 
            {
                if (instance == null) 
                    instance = new ConsoleApp();
            }

            return instance;
        }
    }
    public IDictionary<String, ISudokuStarter> SudokuStarters { get; init; } = new Dictionary<String, ISudokuStarter>();
    public IDictionary<String, Type> RequirementTypes { get; init; } = new Dictionary<String, Type>();

    private ConsoleApp()
    {
        Type requirementType = typeof(IRequirement);
        Type starterType = typeof(ISudokuStarter);
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (requirementType.IsAssignableFrom(type))
                {
                    ConstructorInfo? constructorInfo = type.GetConstructor(Array.Empty<Type>());
                    if (constructorInfo == null)
                    {
                        Log.Error($"{type.FullName} does not contain a parameterless constructor. Cannot initialize it.");
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
                        Log.Error($"{type.FullName} does not contain a parameterless constructor. Cannot initialize it.");
                        continue;
                    }

                    ISudokuStarter starter = (ISudokuStarter)constructorInfo.Invoke(Array.Empty<Object>());
                    SudokuStarters[starter.RegisterName] = starter;
                }
            }
        }
    }


    public void Run(String[] args)
    {
        Log.Info("Starters info: ");
        foreach ((String key, ISudokuStarter value) in SudokuStarters)
        {
            Log.Info($"{key}: {value.GetType().FullName}");
        }
        Log.Debug("Requirements info: ");
        foreach ((String key, Type requirementType) in RequirementTypes)
        {
            Log.Debug($"{key}: {requirementType.FullName}");
        }
    }
}
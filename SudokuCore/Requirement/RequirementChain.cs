using System.Reflection;
using log4net;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public class RequirementChain : IRequirement<Object>
{
    private static ILog Log => LogManager.GetLogger(typeof(RequirementChain));

    private static MethodInfo? GetMethodOrDefault(Type instanceType, Type interfaceType, String name, Type[] argumentTypes)
    {
        return instanceType.GetMethod(name, argumentTypes) ?? instanceType.GetMethod(name, argumentTypes);
    }

    protected struct RequirementMethods
    {
        public Object Requirement { get; init; }
        public MethodInfo FitRequirement { get; init; }
        public MethodInfo Init { get; init; }
        public MethodInfo Step { get; init; }
        public MethodInfo Rollback { get; init; }
    }

    protected List<RequirementMethods> Requirements { get; init; }

    public RequirementChain(ICollection<Object> requirements)
    {
        Requirements = new List<RequirementMethods>();
        foreach (Object requirement in requirements)
        {
            Type instanceType = requirement.GetType();
            Type? interfaceType = instanceType.GetInterface("IRequirement`1");
            if (interfaceType == null)
            {
                Log.Error($"{requirement.GetType()} does not implements IRequirement<>.");
                throw new ArgumentException();
            }

            MethodInfo fitRequirementMethod = GetMethodOrDefault(instanceType, interfaceType, "FitRequirement",
                new[] { typeof(ISudokuGame), typeof(ICoordination), typeof(Int32) })!;
            MethodInfo initMethod = GetMethodOrDefault(instanceType, interfaceType, "Init", new[] { typeof(ISudokuGame) })!;
            MethodInfo rollbackMethod = GetMethodOrDefault(instanceType, interfaceType, "RollBack", new[] { typeof(ISudokuGame), typeof(ICoordination) })!;
            MethodInfo steptMethod = GetMethodOrDefault(instanceType, interfaceType, "Step",
                new[] { typeof(ISudokuGame), typeof(ICoordination), typeof(Int32) })!;
            // instanceType.GetMethod("FitRequirement", ) ??
            // requirement.GetType().GetInterface("IRequirement`1")!.GetMethod("FitRequirement", new[] { typeof(ISudokuGame), typeof(ICoordination), typeof(Int32) })!;
            // MethodInfo initMethod =
            //     requirement.GetType().GetMethod("Init", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
            //         new[] { typeof(ISudokuGame) }) ??;
            // MethodInfo steptMethod =
            //     requirement.GetType().GetMethod("Step", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
            //         new[] { typeof(ISudokuGame), typeof(ICoordination), typeof(Int32) })!;
            // MethodInfo rollbackMethod =
            //     requirement.GetType().GetMethod("RollBack", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
            // new[] { typeof(ISudokuGame), typeof(ICoordination) })!;
            Requirements.Add(new RequirementMethods
                { Requirement = requirement, FitRequirement = fitRequirementMethod, Init = initMethod, Rollback = rollbackMethod, Step = steptMethod });
        }
    }

    public Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        Boolean res = true;
        foreach (var requirement in Requirements)
        {
            res = res && (Boolean)requirement.FitRequirement.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num })!;
            if (!res)
            {
                break;
            }
        }

        return res;
    }

    public void Init(ISudokuGame sudokuGame)
    {
        foreach (var requirement in Requirements)
        {
            // requirement.Requirement.Init(sudokuGame);
            requirement.Init.Invoke(requirement.Requirement, new Object?[] { sudokuGame });
        }
    }

    public void Step(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        foreach (var requirement in Requirements)
        {
            requirement.Step.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num });
        }
    }

    public void RollBack(ISudokuGame sudokuGame, ICoordination coordination)
    {
        foreach (var requirement in Requirements)
        {
            requirement.Rollback.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination });
        }
    }
}
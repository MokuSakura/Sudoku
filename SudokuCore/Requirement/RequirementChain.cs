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
        return instanceType.GetMethod(name, argumentTypes) ?? interfaceType.GetMethod(name, argumentTypes);
    }

    protected struct RequirementMethods
    {
        public Object Requirement { get; init; }
        public MethodInfo FitRequirement { get; init; }
        public MethodInfo Init { get; init; }
        public MethodInfo Step { get; init; }
        public MethodInfo Rollback { get; init; }
        public Func<ISudokuGame, ICoordination, Int32, Boolean> FitRequirementDelegate { get; init; }
        public Action<ISudokuGame, ICoordination, Int32> StepDelegate { get; init; }
        public Action<ISudokuGame, ICoordination> RollBackDelegate { get; init; }
        public Action<ISudokuGame> InitDelegate { get; init; }
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
            Func<ISudokuGame, ICoordination, Int32, Boolean> fitRequirementDelegate = (Func<ISudokuGame, ICoordination, Int32, Boolean>)
                Delegate.CreateDelegate(typeof(Func<ISudokuGame, ICoordination, Int32, Boolean>), requirement, fitRequirementMethod);
            Action<ISudokuGame, ICoordination, Int32> stepDelegate = (Action<ISudokuGame, ICoordination, Int32>)
                Delegate.CreateDelegate(typeof(Action<ISudokuGame, ICoordination, Int32>), requirement, steptMethod);
            Action<ISudokuGame, ICoordination> rollBackDelegate = (Action<ISudokuGame, ICoordination>)
                Delegate.CreateDelegate(typeof(Action<ISudokuGame, ICoordination>), requirement, rollbackMethod);
            Action<ISudokuGame> initDelegate = (Action<ISudokuGame>)
                Delegate.CreateDelegate(typeof(Action<ISudokuGame>), requirement, initMethod);

            Requirements.Add(new RequirementMethods
            {
                Requirement = requirement,
                FitRequirement = fitRequirementMethod,
                Init = initMethod,
                Rollback = rollbackMethod,
                Step = steptMethod,
                FitRequirementDelegate = fitRequirementDelegate,
                StepDelegate = stepDelegate,
                RollBackDelegate = rollBackDelegate,
                InitDelegate = initDelegate
            });
        }
    }

    public Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        Boolean res = true;
        foreach (var requirement in Requirements)
        {
            // res = res && (Boolean)requirement.FitRequirement.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num })!;
            res = res && requirement.FitRequirementDelegate(sudokuGame, coordination, num);
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
            // requirement.Init.Invoke(requirement.Requirement, new Object?[] { sudokuGame });
            requirement.InitDelegate(sudokuGame);
        }
    }

    public void Step(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        foreach (var requirement in Requirements)
        {
            // requirement.Step.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num });
            requirement.StepDelegate(sudokuGame, coordination, num);
        }
    }

    public void RollBack(ISudokuGame sudokuGame, ICoordination coordination)
    {
        foreach (var requirement in Requirements)
        {
            // requirement.Rollback.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination });
            requirement.RollBackDelegate(sudokuGame, coordination);
        }
    }
}
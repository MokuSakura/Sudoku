using System.Reflection;
using log4net;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public class RequirementChain<TSudokuGameType, TCoordinationType>
where TCoordinationType : ICoordination
where TSudokuGameType : ISudokuGame<TCoordinationType>
{
    private static ILog Log => LogManager.GetLogger(typeof(RequirementChain<TSudokuGameType,TCoordinationType>));

    protected struct RequirementMethods
    {
        public Object Requirement { get; init; }
        public MethodInfo FitRequirement { get; init; }
        public MethodInfo Init { get; init; }
        public MethodInfo Step { get; init; }
        public MethodInfo Rollback { get; init; }
        public Delegate FitRequirementDelegate { get; init; }
        public Delegate StepDelegate { get; init; }
        public Delegate RollBackDelegate { get; init; }
        public Delegate InitDelegate { get; init; }
    }

    // protected List<RequirementMethods> Requirements { get; init; }
    protected List<IRequirement<TSudokuGameType, TCoordinationType>> Requirements2 { get; init; }

    public RequirementChain(ICollection<IRequirement<TSudokuGameType, TCoordinationType>> requirements)
    {
        Requirements2 = new List<IRequirement<TSudokuGameType, TCoordinationType>>(requirements);
        
        // Requirements = new List<RequirementMethods>();
        // foreach (Object requirement in requirements)
        // {
        //     Type instanceType = requirement.GetType();
        //     Type? interfaceType = instanceType.GetInterface("IRequirement`3");
        //     if (interfaceType == null)
        //     {
        //         Log.Error($"{requirement.GetType()} does not implements IRequirement<>.");
        //         throw new ArgumentException();
        //     }
        //
        //     Type[] typeArgument = SudokuRequirementReflectionUtils.GetGenericArguments(instanceType, typeof(IRequirement<,,>));
        //     MethodInfo fitRequirementMethod = GetMethodOrDefault(instanceType, interfaceType, "FitRequirement",
        //         new[] { typeArgument[1], typeArgument[2], typeof(Int32) })!;
        //     MethodInfo initMethod = GetMethodOrDefault(instanceType, interfaceType, "Init", new[] { typeArgument[1] })!;
        //     MethodInfo rollbackMethod = GetMethodOrDefault(instanceType, interfaceType, "Rollback", new[] { typeArgument[1], typeArgument[2] })!;
        //     MethodInfo steptMethod = GetMethodOrDefault(instanceType, interfaceType, "Step",
        //         new[] { typeArgument[1], typeArgument[2], typeof(Int32) })!;
        //     Delegate fitRequirementDelegate =
        //         Delegate.CreateDelegate(typeof(Func<,,,>).MakeGenericType(typeof(TSudokuGameType), typeof(Int32), typeof(Int32), typeof(Boolean)), requirement,
        //             fitRequirementMethod);
        //     Delegate stepDelegate =
        //         Delegate.CreateDelegate(typeof(Action<,,>).MakeGenericType(typeof(TSudokuGameType), typeof(Int32)), requirement, steptMethod);
        //     Delegate rollBackDelegate =
        //         Delegate.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(TSudokuGameType), typeof(Int32)), requirement, rollbackMethod);
        //     Delegate initDelegate =
        //         Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(typeof(TSudokuGameType)), requirement, initMethod);
        //
        //     Requirements.Add(new RequirementMethods
        //     {
        //         Requirement = requirement,
        //         FitRequirement = fitRequirementMethod,
        //         Init = initMethod,
        //         Rollback = rollbackMethod,
        //         Step = steptMethod,
        //         FitRequirementDelegate = fitRequirementDelegate,
        //         StepDelegate = stepDelegate,
        //         RollBackDelegate = rollBackDelegate,
        //         InitDelegate = initDelegate
        //     });
        // }
    }

    public Boolean FitRequirement(TSudokuGameType sudokuGame, TCoordinationType coordination, Int32 num)
    {
        Boolean res = true;
        // foreach (var requirement in Requirements)
        // {
        //     // res = res && (Boolean)requirement.FitRequirement.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num })!;
        //     res = res && (Boolean)requirement.FitRequirementDelegate.DynamicInvoke(sudokuGame, idx, num)!;
        //     if (!res)
        //     {
        //         break;
        //     }
        // }
        foreach (var requirement in Requirements2)
        {
            // res = res && (Boolean)requirement.FitRequirement.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num })!;
            res = res && requirement.FitRequirement(sudokuGame, coordination, num);
            if (!res)
            {
                break;
            }
        }

        return res;
    }

    public void Init(TSudokuGameType sudokuGame)
    {
        // foreach (var requirement in Requirements)
        // {
        //     // requirement.Requirement.Init(sudokuGame);
        //     // requirement.Init.Invoke(requirement.Requirement, new Object?[] { sudokuGame });
        //     requirement.InitDelegate.DynamicInvoke(sudokuGame);
        // }
        foreach (var requirement in Requirements2)
        {
            // requirement.Requirement.Init(sudokuGame);
            // requirement.Init.Invoke(requirement.Requirement, new Object?[] { sudokuGame });
            requirement.Init(sudokuGame);
        }
    }

    public void Step(TSudokuGameType sudokuGame, TCoordinationType coordination, Int32 num)
    {
        // foreach (var requirement in Requirements)
        // {
        //     // requirement.Step.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num });
        //     requirement.StepDelegate.DynamicInvoke(sudokuGame, idx, num);
        // }
        foreach (var requirement in Requirements2)
        {
            // requirement.Step.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num });
            requirement.Step(sudokuGame, coordination, num);
        }
    }

    public void RollBack(TSudokuGameType sudokuGame, TCoordinationType coordination)
    {
        // foreach (var requirement in Requirements)
        // {
        //     // requirement.Rollback.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination });
        //     requirement.RollBackDelegate.DynamicInvoke(sudokuGame, idx);
        // }
        foreach (var requirement in Requirements2)
        {
            // requirement.Rollback.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination });
            requirement.Rollback(sudokuGame, coordination);
        }
    }
}
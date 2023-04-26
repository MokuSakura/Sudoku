using System.Reflection;
using log4net;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public class RequirementChain : IRequirement<Object, ISudokuGame<ICoordination>, ICoordination>
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
        public Delegate FitRequirementDelegate { get; init; }
        public Delegate StepDelegate { get; init; }
        public Delegate RollBackDelegate { get; init; }
        public Delegate InitDelegate { get; init; }
    }

    protected List<RequirementMethods> Requirements { get; init; }


    public RequirementChain(ICollection<Object> requirements)
    {
        Requirements = new List<RequirementMethods>();
        foreach (Object requirement in requirements)
        {
            Type instanceType = requirement.GetType();
            Type? interfaceType = instanceType.GetInterface("IRequirement`3");
            if (interfaceType == null)
            {
                Log.Error($"{requirement.GetType()} does not implements IRequirement<>.");
                throw new ArgumentException();
            }

            Type[] typeArgument = SudokuRequirementReflectionUtils.GetGenericArguments(instanceType, typeof(IRequirement<,,>));
            MethodInfo fitRequirementMethod = GetMethodOrDefault(instanceType, interfaceType, "FitRequirement",
                new[] { typeArgument[1], typeArgument[2], typeof(Int32) })!;
            MethodInfo initMethod = GetMethodOrDefault(instanceType, interfaceType, "Init", new[] {typeArgument[1] })!;
            MethodInfo rollbackMethod = GetMethodOrDefault(instanceType, interfaceType, "RollBack", new[] { typeArgument[1], typeArgument[2] })!;
            MethodInfo steptMethod = GetMethodOrDefault(instanceType, interfaceType, "Step",
                new[] { typeArgument[1], typeArgument[2], typeof(Int32) })!;
            Delegate fitRequirementDelegate =
                Delegate.CreateDelegate(typeof(Func<, , , >).MakeGenericType(typeArgument[1], typeArgument[2],typeof(Int32), typeof(Boolean)), requirement, fitRequirementMethod);
            Delegate stepDelegate = 
                Delegate.CreateDelegate(typeof(Action<ISudokuGame, ICoordination, Int32>), requirement, steptMethod);
            Delegate rollBackDelegate = 
                Delegate.CreateDelegate(typeof(Action<ISudokuGame, ICoordination>), requirement, rollbackMethod);
            Delegate initDelegate = 
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

    public Boolean FitRequirement(ISudokuGame<ICoordination> sudokuGame, ICoordination coordination, Int32 num)
    {
        Boolean res = true;
        foreach (var requirement in Requirements)
        {
            // res = res && (Boolean)requirement.FitRequirement.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num })!;
            res = res && (Boolean) requirement.FitRequirementDelegate.DynamicInvoke(sudokuGame, coordination, num)!;
            if (!res)
            {
                break;
            }
        }

        return res;
    }

    public void Init(ISudokuGame<ICoordination> sudokuGame)
    {
        foreach (var requirement in Requirements)
        {
            // requirement.Requirement.Init(sudokuGame);
            // requirement.Init.Invoke(requirement.Requirement, new Object?[] { sudokuGame });
            requirement.InitDelegate.DynamicInvoke(sudokuGame);
        }
    }

    public void Step(ISudokuGame<ICoordination> sudokuGame, ICoordination coordination, Int32 num)
    {
        foreach (var requirement in Requirements)
        {
            // requirement.Step.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination, num });
            requirement.StepDelegate.DynamicInvoke(sudokuGame, coordination, num);
        }
    }

    public void RollBack(ISudokuGame<ICoordination> sudokuGame, ICoordination coordination)
    {
        foreach (var requirement in Requirements)
        {
            // requirement.Rollback.Invoke(requirement.Requirement, new Object?[] { sudokuGame, coordination });
            requirement.RollBackDelegate.DynamicInvoke(sudokuGame, coordination);
        }
    }
}
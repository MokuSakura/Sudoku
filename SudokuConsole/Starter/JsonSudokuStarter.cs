using System.Reflection;
using log4net;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;
using MokuSakura.Sudoku.Core.Setting;
using MokuSakura.Sudoku.Core.Solver;
using MokuSakura.SudokuConsole.Exception;
using Newtonsoft.Json;

namespace MokuSakura.SudokuConsole.Starter;

public class JsonSudokuStarter : ISudokuStarter
{
    public IEnumerable<String> RegisterName => new[] { GetType().Name, "J", "j", "json", "JsonStarter" };
    private static ILog Log => LogManager.GetLogger(typeof(JsonSudokuStarter));
    private String Json { get; }
    
    static Type[] GetGenericArguments(Type type, Type genericType)
    {
        return type.GetInterfaces() //取类型的接口
            .Where(IsGenericType) //筛选出相应泛型接口
            .SelectMany(i => i.GetGenericArguments()) //选择所有接口的泛型参数
            .ToArray(); //ToArray

        Boolean IsGenericType(Type type1)
            => type1.IsGenericType && type1.GetGenericTypeDefinition() == genericType;
    }

    public JsonSudokuStarter(String json)
    {
        this.Json = json;
    }

    public ICollection<ISudokuGame> Run()
    {
        return Run(out Int64 _);
    }

    public ICollection<ISudokuGame> Run(out Int64 solveTime)
    {
        JsonSerializerSettings settings = new()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.Default
        };
        JsonConfig jsonConfig = JsonConvert.DeserializeObject<JsonConfig>(Json, settings) ?? new JsonConfig();
        List<Object> requirements = new();
        SudokuRequirementReflectionUtils requirementReflectionUtils = SudokuRequirementReflectionUtils.Instance;
        foreach (RequirementConfig jsonConfigRequirement in jsonConfig.Requirements)
        {
            Object? requirement = requirementReflectionUtils.CreateRequirement(jsonConfigRequirement.RequirementName);
            if (requirement == null)
            {
                Log.Error($"Cannot find requirement {jsonConfigRequirement.RequirementName}");
                throw new NoRequirementFoundException(jsonConfigRequirement.RequirementName);
            }

           
            Type requirementType = requirement.GetType();
            Type configType = GetGenericArguments(requirementType, typeof(IRequirement<>))[0];
            // Type configType = requirementType.GetMethod("Configure", new Type[] { requirementType.GetInterfaces().Where(type => type.ContainsGenericParameters && ) });

            Object config = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonConfigRequirement.Configuration), configType)!;
            Type makeGenericType =typeof(IRequirement<>).MakeGenericType(configType);
            MethodInfo configureMethodInfo = makeGenericType.GetMethod("Configure")!;
            // requirementType.InvokeMember("Configure", BindingFlags.Instance | BindingFlags.InvokeMethod, null, requirement, new []{config});
            configureMethodInfo.Invoke(requirement, new[] { config });
            requirements.Add(requirement);
        }

        RequirementChain requirementChain = new(requirements);
        SudokuSetting setting = new()
        {
            AvailableSet = jsonConfig.AvailableSet,
            GameBoardSizeX = jsonConfig.GameBoardSizeX,
            GameBoardSizeY = jsonConfig.GameBoardSizeY,
            SubGridSizeX = jsonConfig.SubGridSizeX,
            SubGridSizeY = jsonConfig.SubGridSizeY
        };
        SudokuGame sudokuGame = new(setting, jsonConfig.GameBoard);
        ISolver solver = new DfsSolver();
        DateTime t1 = DateTime.Now;
        ICollection<ISudokuGame> res = solver.Solve(sudokuGame, requirementChain);
        DateTime t2 = DateTime.Now;
        solveTime = (t2 - t1).Milliseconds;
        return res;
    }
}
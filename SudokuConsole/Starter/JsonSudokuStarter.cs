using System.Reflection;
using log4net;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;
using MokuSakura.Sudoku.Core.Setting;
using MokuSakura.Sudoku.Core.Solver;
using MokuSakura.SudokuConsole.Exception;
using MokuSakura.SudokuConsole.JsonConverter;
using Newtonsoft.Json;

namespace MokuSakura.SudokuConsole.Starter;

public class JsonSudokuStarter : ISudokuStarter<SudokuGame, Coordinate>
{
    public IEnumerable<String> RegisterName => new[] { GetType().Name, "J", "j", "json", "JsonStarter" };
    private static ILog Log => LogManager.GetLogger(typeof(JsonSudokuStarter));
    private String Json { get; }


    public JsonSudokuStarter(String json)
    {
        this.Json = json;
    }

    public SudokuGame Run()
    {
        return Run(out Int64 _);
    }

    public SudokuGame Run(out Int64 solveTime)
    {
        JsonSerializerSettings settings = new()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.Default
        };
        settings.Converters.Add(new CoordinateConverter());
        JsonConfig jsonConfig = JsonConvert.DeserializeObject<JsonConfig>(Json, settings) ?? new JsonConfig();
        List<IRequirement<SudokuGame, Coordinate>> requirements = new();
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
            Type? configurableInterface = requirementType.GetInterface("IConfigurable`1");
            if (configurableInterface != null)
            {
                Type configType = SudokuRequirementReflectionUtils.GetGenericArguments(requirementType, typeof(IConfigurable<>))[0];
                Object config = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonConfigRequirement.Configuration), configType, settings)!;
                // Type makeGenericType = configurableInterface.MakeGenericType(configType);
                MethodInfo configureMethodInfo = configurableInterface.GetMethod("Configure")!;
                // requirementType.InvokeMember("Configure", BindingFlags.Instance | BindingFlags.InvokeMethod, null, requirement, new []{config});
                configureMethodInfo.Invoke(requirement, new[] { config });
            }


            requirements.Add((IRequirement<SudokuGame, Coordinate>)requirement);
        }

        RequirementChain<SudokuGame, Coordinate> requirementChain = new(requirements);
        SudokuSetting setting = new()
        {
            AvailableSet = jsonConfig.AvailableSet,
            GameBoardSizeX = jsonConfig.GameBoardSizeX,
            GameBoardSizeY = jsonConfig.GameBoardSizeY,
            SubGridSizeX = jsonConfig.SubGridSizeX,
            SubGridSizeY = jsonConfig.SubGridSizeY
        };
        SudokuGame sudokuGame = new(setting, jsonConfig.GameBoard);
        DfsSolver<SudokuGame, Coordinate> solver = new();
        DateTime t1 = DateTime.Now;
        var res = solver.Solve(sudokuGame, requirementChain);
        DateTime t2 = DateTime.Now;
        solveTime = (t2 - t1).Milliseconds;
        return res;
    }
}
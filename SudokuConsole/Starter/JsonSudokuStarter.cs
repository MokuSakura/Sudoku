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
    public IEnumerable<string> RegisterName => new[] { GetType().Name, "J", "j", "json", "JsonStarter" };
    private static ILog Log => LogManager.GetLogger(typeof(JsonSudokuStarter));
    private string Json { get; }


    public JsonSudokuStarter(string json)
    {
        this.Json = json;
    }

    public SudokuGame Run()
    {
        return Run(out long _);
    }

    public SudokuGame Run(out long solveTime)
    {
        JsonSerializerSettings settings = new()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.Default,
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };
        settings.Converters.Add(new CoordinateConverter());
        JsonConfig jsonConfig = JsonConvert.DeserializeObject<JsonConfig>(Json, settings) ?? new JsonConfig();
        List<IRequirement<SudokuGame, Coordinate>> requirements = new();
        SudokuRequirementReflectionUtils requirementReflectionUtils = SudokuRequirementReflectionUtils.Instance;
        foreach (RequirementConfig jsonConfigRequirement in jsonConfig.Requirements)
        {
            object? requirement = requirementReflectionUtils.CreateRequirement(jsonConfigRequirement.RequirementName);
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
                object config = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonConfigRequirement.Configuration), configType, settings)!;
                MethodInfo configureMethodInfo = configurableInterface.GetMethod("Configure")!;
                configureMethodInfo.Invoke(requirement, new[] { config });
            }


            requirements.Add((IRequirement<SudokuGame, Coordinate>)requirement);
        }

        RequirementChain<SudokuGame, Coordinate> requirementChain = new(requirements);
        SudokuSetting setting = new()
        {
            AvailableSet = new HashSet<int>(jsonConfig.AvailableSet),
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
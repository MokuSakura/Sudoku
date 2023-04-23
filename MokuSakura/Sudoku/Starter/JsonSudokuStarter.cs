using log4net;
using MokuSakura.Sudoku.Game;
using MokuSakura.Sudoku.Requirement;
using MokuSakura.Sudoku.Setting;
using MokuSakura.Sudoku.Solver;
using MokuSakura.Sudoku.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MokuSakura.Sudoku.Starter;

public class JsonSudokuStarter : ISudokuStarter
{
    public IEnumerable<String> RegisterName => new[] { GetType().Name, "J", "j", "json", "JsonStarter" };
    private static ILog Log => LogManager.GetLogger(typeof(JsonSudokuStarter));
    private String Json { get; }

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
        List<IRequirement> requirements = new();
        SudokuRequirementReflectionUtils requirementReflectionUtils = SudokuRequirementReflectionUtils.Instance;
        foreach (RequirementConfig jsonConfigRequirement in jsonConfig.Requirements)
        {
            IRequirement? requirement = requirementReflectionUtils.CreateRequirement(jsonConfigRequirement.RequirementName);
            if (requirement == null)
            {
                Log.Error($"Cannot find requirement {jsonConfigRequirement.RequirementName}");
                continue;
            }

            IDictionary<String, Object> config = JObject.Parse(jsonConfigRequirement.Configuration).ToDictionary();
            requirement.Configure(config);
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
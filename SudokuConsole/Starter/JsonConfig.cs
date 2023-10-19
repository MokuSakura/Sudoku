using Newtonsoft.Json.Linq;

namespace MokuSakura.SudokuConsole.Starter;

public class JsonConfig
{
    public IList<RequirementConfig> Requirements { get; set; } = new List<RequirementConfig>();
    public int[,] GameBoard { get; set; } = { };
    public int GameBoardSizeX { get; set; } = 9;
    public int GameBoardSizeY { get; set; } = 9;
    public int SubGridSizeX { get; set; } = 3;
    public int SubGridSizeY { get; set; } = 3;
    public HashSet<int> AvailableSet { get; set; } = new(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
}

public class RequirementConfig
{
    public RequirementConfig()
    {
    }

    public string RequirementName { get; set; } = "";
    public JObject Configuration { get; set; } = new JObject();
}
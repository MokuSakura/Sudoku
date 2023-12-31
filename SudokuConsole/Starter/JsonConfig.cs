using Newtonsoft.Json.Linq;

namespace MokuSakura.SudokuConsole.Starter;

public class JsonConfig
{
    public IList<RequirementConfig> Requirements { get; set; } = new List<RequirementConfig>();
    public Int32[,] GameBoard { get; set; } = { };
    public Int32 GameBoardSizeX { get; set; } = 9;
    public Int32 GameBoardSizeY { get; set; } = 9;
    public Int32 SubGridSizeX { get; set; } = 3;
    public Int32 SubGridSizeY { get; set; } = 3;
    public HashSet<Int32> AvailableSet { get; set; } = new(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
}

public class RequirementConfig
{
    public RequirementConfig()
    {
    }

    public String RequirementName { get; set; } = "";
    public JObject Configuration { get; set; } = new JObject();
}
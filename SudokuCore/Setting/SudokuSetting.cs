namespace MokuSakura.Sudoku.Core.Setting;

public class SudokuSetting
{
    public int GameBoardSizeX { get; set; } = 9;
    public int GameBoardSizeY { get; set; } = 9;
    public int SubGridSizeX { get; set; } = 3;
    public int SubGridSizeY { get; set; } = 3;
    public ISet<int> AvailableSet { get; set; } = new HashSet<int>(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

    public SudokuSetting()
    {
    }
}
using System.Collections.Immutable;

namespace MokuSakura.Sudoku.Setting;

public class SudokuSetting
{
    public Int32 GameBoardSizeX { get; set; } = 9;
    public Int32 GameBoardSizeY { get; set; } = 9;
    public Int32 SubGridSizeX { get; set; } = 3;
    public Int32 SubGridSizeY { get; set; } = 3;
    public ISet<Int32> AvailableSet { get; set; } = new HashSet<Int32>(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

    public SudokuSetting()
    {
    }
}
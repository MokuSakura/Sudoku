using MokuSakura.Sudoku.Core.Coordination;

namespace MokuSakura.Sudoku.Core.Game;

public interface ISudokuGame
{
    public Int32 RowNum { get; }
    public Int32 ColNum { get; }
    public Int32 SubGridNum { get; }
    public Int32 SubGridSizeX { get; }
    public Int32 SubGridSizeY { get; }
    public ISet<Int32> AvailableSet { get; }
    public Int32 NumToFill { get; }
    public Int32 GetNum(ICoordination coordination);
    public Int32 SetNum(ICoordination coordination, Int32 num);
    public Int32 MapCoordinationToIndex(ICoordination coordination);
    public ICoordination MapIndexToCoordination(Int32 idx);
    public ISudokuGame Clone();
    public void PrintGameBoard(TextWriter writer);
}
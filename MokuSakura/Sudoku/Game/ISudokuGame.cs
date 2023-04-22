

using MokuSakura.Sudoku.Coordination;

namespace MokuSakura.Sudoku.Game;

public interface ISudokuGame
{
    public Int32 RowNum { get; }
    public Int32 ColNum { get; }
    public Int32 SubGridNum { get; }
    public Int32 SubGridSizeX { get; }
    public Int32 SubGridSizeY { get; }
    public ISet<Int32> AvailableSet { get; }
    public Int32 NumToFill { get; }
    public Int32 GetRowIdx(ICoordination coordination);
    public Int32 GetColIdx(ICoordination coordination);
    public Int32 GetSubGridIdx(ICoordination coordination);
    public Int32 GetNum(ICoordination coordination);
    public Int32 SetNum(ICoordination coordination, Int32 num);
    public ICoordination GetSubGridBeginCoordinate(ICoordination coordination);
    public Int32 MapCoordinationToIndex(ICoordination coordination);
    public ICoordination MapIndexToCoordination(Int32 idx);
    public ISudokuGame Clone();
    public void PrintGameBoard(TextWriter writer);
}
using MokuSakura.Sudoku.Core.Coordination;

namespace MokuSakura.Sudoku.Core.Game;

public interface ISudokuGame<TCoordinationType>
where TCoordinationType : ICoordination
{
    public int RowNum { get; }
    public int ColNum { get; }
    public int SubGridNum { get; }
    public int SubGridSizeX { get; }
    public int SubGridSizeY { get; }
    public ISet<int> AvailableSet { get; }
    public int NumToFill { get; }
    public int GetNum(TCoordinationType coordination);
    public int SetNum(TCoordinationType coordination, int num);
    public int MapCoordinationToIndex(TCoordinationType coordination);
    public TCoordinationType MapIndexToCoordination(int idx);
    public void PrintGameBoard(TextWriter writer);
}
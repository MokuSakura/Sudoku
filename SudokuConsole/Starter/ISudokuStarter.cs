using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.SudokuConsole.Starter;

public interface ISudokuStarter<TSudokuGameType, TCoordinationType>
where TCoordinationType : ICoordination
where TSudokuGameType : ISudokuGame<TCoordinationType>
{
    public ICollection<TSudokuGameType> Run();
}
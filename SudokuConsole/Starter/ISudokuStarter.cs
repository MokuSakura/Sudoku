using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.SudokuConsole.Starter;

public interface ISudokuStarter<out TSudokuGameType, TCoordinationType>
where TCoordinationType : ICoordination
where TSudokuGameType : ISudokuGame<TCoordinationType>
{
    public TSudokuGameType Run();
}
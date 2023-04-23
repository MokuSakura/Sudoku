using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.SudokuConsole.Starter;

public interface ISudokuStarter
{
    public ICollection<ISudokuGame> Run();
}
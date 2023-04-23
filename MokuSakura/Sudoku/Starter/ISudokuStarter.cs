using MokuSakura.Sudoku.Game;

namespace MokuSakura.Sudoku.Starter;

public interface ISudokuStarter
{
    public ICollection<ISudokuGame> Run();
}
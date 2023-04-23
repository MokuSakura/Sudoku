namespace MokuSakura.Sudoku.Starter;

public interface ISudokuStarter
{
    public ICollection<String> RegisterName => new[] { GetType().Name };

    public void Run(IDictionary<String, Object> args);
}
namespace MokuSakura.Sudoku.Starter;

public class JsonSudokuStarter : ISudokuStarter
{
    public ICollection<String> RegisterName => new[] { GetType().Name, "J", "j", "json", "JsonStarter" };

    public void Run(IDictionary<String, Object> args)
    {
        
    }
}
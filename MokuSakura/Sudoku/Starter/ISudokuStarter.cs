namespace MokuSakura.Sudoku.Starter;

public interface ISudokuStarter
{
    public String RegisterName => this.GetType().Name;
    public void Run(String[] args);
}
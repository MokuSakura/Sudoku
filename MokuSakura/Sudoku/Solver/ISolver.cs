using MokuSakura.Sudoku.Game;
using MokuSakura.Sudoku.Requirement;


namespace MokuSakura.Sudoku.Solver;

public interface ISolver
{
    public ICollection<ISudokuGame> Solve(ISudokuGame sudokuGame, RequirementChain requirement);
}
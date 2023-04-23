using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;

namespace MokuSakura.Sudoku.Core.Solver;

public interface ISolver
{
    public ICollection<ISudokuGame> Solve(ISudokuGame sudokuGame, RequirementChain requirement);
}
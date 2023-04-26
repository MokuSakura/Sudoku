using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;

namespace MokuSakura.Sudoku.Core.Solver;

public interface ISolver<TSudokuGameType, TCoordinationType>
where TSudokuGameType : ISudokuGame<TCoordinationType>
where TCoordinationType : ICoordination
{
    public ICollection<TSudokuGameType> Solve(TSudokuGameType sudokuGame, RequirementChain<TSudokuGameType, TCoordinationType> requirement);
}
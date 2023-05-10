using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;

namespace MokuSakura.Sudoku.Core.Solver;

public interface ISolver<TSudokuGameType, TCoordinationType>
where TCoordinationType : ICoordination
where TSudokuGameType : ISudokuGame<TCoordinationType>
{
    public TSudokuGameType Solve(TSudokuGameType sudokuGame, RequirementChain<TSudokuGameType, TCoordinationType> requirement);
}
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;

namespace MokuSakura.Sudoku.Core.Solver;

public class DfsSolver<TSudokuGameType, TCoordinationType> : ISolver<TSudokuGameType, TCoordinationType>
where TCoordinationType : ICoordination
where TSudokuGameType : ISudokuGame<TCoordinationType>
{
    public TSudokuGameType Solve(TSudokuGameType sudokuGame, RequirementChain<TSudokuGameType, TCoordinationType> requirement)
    {
        requirement.Init(sudokuGame);
        DfsBody(sudokuGame, requirement, 0);
        return sudokuGame;
    }

    private Boolean DfsBody(TSudokuGameType sudokuGame, RequirementChain<TSudokuGameType, TCoordinationType> requirement, Int32 n)
    {
        if (n >= sudokuGame.NumToFill)
        {
            return true;
        }


        if (sudokuGame.GetNum(n) != 0)
        {
            return DfsBody(sudokuGame, requirement, n + 1);
        }

        foreach (Int32 testNum in sudokuGame.AvailableSet)
        {
            if (!requirement.FitRequirement(sudokuGame, n, testNum))
            {
                continue;
            }

            sudokuGame.SetNum(n, testNum);
            requirement.Step(sudokuGame, n, testNum);
            if (DfsBody(sudokuGame, requirement, n + 1))
            {
                return true;
            }

            requirement.RollBack(sudokuGame, n);
            sudokuGame.SetNum(n, 0);
        }

        return false;
    }
}
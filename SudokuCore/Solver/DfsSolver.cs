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

    private bool DfsBody(TSudokuGameType sudokuGame, RequirementChain<TSudokuGameType, TCoordinationType> requirement, int n)
    {
        if (n >= sudokuGame.NumToFill)
        {
            return true;
        }

        TCoordinationType coordination = sudokuGame.MapIndexToCoordination(n);

        if (sudokuGame.GetNum(coordination) != 0)
        {
            return DfsBody(sudokuGame, requirement, n + 1);
        }

        foreach (int testNum in sudokuGame.AvailableSet)
        {
            if (!requirement.FitRequirement(sudokuGame, coordination, testNum))
            {
                continue;
            }

            sudokuGame.SetNum(coordination, testNum);
            requirement.Step(sudokuGame, coordination, testNum);
            if (DfsBody(sudokuGame, requirement, n + 1))
            {
                return true;
            }

            requirement.RollBack(sudokuGame, coordination);
            sudokuGame.SetNum(coordination, 0);
        }

        return false;
    }
}
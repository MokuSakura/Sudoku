using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;

namespace MokuSakura.Sudoku.Core.Solver;

public class DfsSolver : ISolver<SudokuGame, Coordinate>
{
    public ICollection<SudokuGame> Solve(SudokuGame sudokuGame, RequirementChain requirement)
    {
        requirement.Init(sudokuGame);
        List<SudokuGame> res = new List<SudokuGame>();
        DfsBody(sudokuGame, requirement, 0, res);
        return res;
    }

    private void DfsBody(SudokuGame sudokuGame, RequirementChain requirement, Int32 n, List<SudokuGame> res)
    {
        if (n >= sudokuGame.NumToFill)
        {
            res.Add(sudokuGame.Clone());
            return;
        }

        Coordinate coordinate = sudokuGame.MapIndexToCoordination(n);

        if (sudokuGame.GetNum(coordinate) != 0)
        {
            DfsBody(sudokuGame, requirement, n + 1, res);
            return;
        }

        foreach (Int32 testNum in sudokuGame.AvailableSet)
        {
            if (!requirement.FitRequirement(sudokuGame, coordinate, testNum))
            {
                continue;
            }

            sudokuGame.SetNum(coordinate, testNum);
            requirement.Step(sudokuGame, coordinate, testNum);
            DfsBody(sudokuGame, requirement, n + 1, res);
            requirement.RollBack(sudokuGame, coordinate);
            sudokuGame.SetNum(coordinate, 0);
        }
    }
}
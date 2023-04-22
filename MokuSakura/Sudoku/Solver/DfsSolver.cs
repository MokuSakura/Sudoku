using MokuSakura.Sudoku.Coordination;
using MokuSakura.Sudoku.Game;
using MokuSakura.Sudoku.Requirement;

namespace MokuSakura.Sudoku.Solver;

public class DfsSolver : ISolver
{
    public ICollection<ISudokuGame> Solve(ISudokuGame sudokuGame, IRequirement requirement)
    {
        requirement.Init(sudokuGame);
        ICollection<ISudokuGame> res = new List<ISudokuGame>();
        DfsBody(sudokuGame, requirement, 0, res);
        return res;
    }

    private void DfsBody(ISudokuGame sudokuGame, IRequirement requirement, Int32 n, ICollection<ISudokuGame> res)
    {
        if (n >= sudokuGame.NumToFill)
        {
            res.Add(sudokuGame.Clone());
            return;
        }

        ICoordination coordinate = sudokuGame.MapIndexToCoordination(n);

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
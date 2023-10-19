using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Setting;

namespace MokuSakura.Sudoku.Core.Requirement.Common;

public class RowRequirement : IRequirement<SudokuGame, Coordinate>
{
    protected bool[,] Cache { get; set; } = { };

    public bool FitRequirement(SudokuGame sudokuGame, Coordinate coordination, int num)
    {
        return !Cache[coordination.X, num];
    }

    public void Init(SudokuGame sudokuGame)
    {
        Cache = new bool[sudokuGame.RowNum, sudokuGame.AvailableSet.Max() + 1];
        for (int i = 0; i < sudokuGame.NumToFill; ++i)
        {
            Coordinate coordination = sudokuGame.MapIndexToCoordination(i);
            int num = sudokuGame.GetNum(coordination);
            int rowIdx = coordination.X;
            if (num != 0)
            {
                Cache[rowIdx, num] = true;
            }
        }
    }

    public void Step(SudokuGame sudokuGame, Coordinate coordination, int num)
    {
        Cache[coordination.X, num] = true;
    }

    public void Rollback(SudokuGame sudokuGame, Coordinate coordination)
    {
        Cache[coordination.X, sudokuGame.GetNum(coordination)] = false;
    }
}
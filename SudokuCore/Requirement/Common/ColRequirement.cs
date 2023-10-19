using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement.Common;

public class ColRequirement : IRequirement<SudokuGame, Coordinate>
{
    protected bool[,] Cache { get; set; } = { };

    public bool FitRequirement(SudokuGame sudokuGame, Coordinate coordination, int num)
    {
        return !Cache[coordination.Y, num];
    }

    public void Init(SudokuGame sudokuGame)
    {
        Cache = new bool[sudokuGame.ColNum, sudokuGame.AvailableSet.Max() + 1];
        for (int i = 0; i < sudokuGame.NumToFill; ++i)
        {
            Coordinate coordination = sudokuGame.MapIndexToCoordination(i);
            int num = sudokuGame.GetNum(coordination);
            int colIdx = coordination.Y;
            if (num != 0)
            {
                Cache[colIdx, num] = true;
            }
        }
    }

    public void Step(SudokuGame sudokuGame, Coordinate coordination, int num)
    {
        Cache[coordination.Y, num] = true;
    }

    public void Rollback(SudokuGame sudokuGame, Coordinate coordination)
    {
        Cache[coordination.Y, sudokuGame.GetNum(coordination)] = false;
    }
}
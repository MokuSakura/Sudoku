using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Setting;

namespace MokuSakura.Sudoku.Core.Requirement.Common;

public class RowRequirement : IRequirement<Object, SudokuGame, Coordinate>
{
    protected Boolean[,] Cache { get; set; } = { };

    public Boolean FitRequirement(SudokuGame sudokuGame, Coordinate coordination, Int32 num)
    {
        return !Cache[coordination.X, num];
    }

    public void Init(SudokuGame sudokuGame)
    {
        Cache = new Boolean[sudokuGame.RowNum, sudokuGame.AvailableSet.Max() + 1];
        for (Int32 i = 0; i < sudokuGame.NumToFill; ++i)
        {
            Coordinate coordination = sudokuGame.MapIndexToCoordination(i);
            Int32 num = sudokuGame.GetNum(coordination);
            Int32 rowIdx = coordination.X;
            if (num != 0)
            {
                Cache[rowIdx, num] = true;
            }
        }
    }

    public void Step(SudokuGame sudokuGame, Coordinate coordination, Int32 num)
    {
        Cache[coordination.X, num] = true;
    }

    public void RollBack(SudokuGame sudokuGame, Coordinate coordination)
    {
        Cache[coordination.X, sudokuGame.GetNum(coordination)] = false;
    }
}
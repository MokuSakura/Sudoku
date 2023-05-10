using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement.Common;

public class ColRequirement : AbstractSudokuRequirement
{
    protected Boolean[,] Cache { get; set; } = { };

    public override Boolean FitRequirement(SudokuGame sudokuGame, Coordinate coordination, Int32 num)
    {
        return !Cache[coordination.Y, num];
    }

    public override void Init(SudokuGame sudokuGame)
    {
        Cache = new Boolean[sudokuGame.ColNum, sudokuGame.AvailableSet.Max() + 1];
        for (Int32 i = 0; i < sudokuGame.NumToFill; ++i)
        {
            Coordinate coordination = sudokuGame.MapIndexToCoordination(i);
            Int32 num = sudokuGame.GetNum(coordination);
            Int32 colIdx = coordination.Y;
            if (num != 0)
            {
                Cache[colIdx, num] = true;
            }
        }
    }

    public override void Step(SudokuGame sudokuGame, Coordinate coordination, Int32 num)
    {
        Cache[coordination.Y, num] = true;
    }

    public override void Rollback(SudokuGame sudokuGame, Coordinate coordination)
    {
        Cache[coordination.Y, sudokuGame.GetNum(coordination)] = false;
    }
}
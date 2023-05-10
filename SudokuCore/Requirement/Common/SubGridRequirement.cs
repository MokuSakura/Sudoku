using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement.Common;

public class SubGridRequirement : IRequirement<SudokuGame, Coordinate>
{
    protected Boolean[,] Cache { get; set; } = { };

    public Boolean FitRequirement(SudokuGame sudokuGame, Coordinate coordination, Int32 num)
    {
        return !Cache[GetSubGridIdx(sudokuGame,coordination), num];
    }

    public void Init(SudokuGame sudokuGame)
    {
        Cache = new Boolean[sudokuGame.SubGridNum, sudokuGame.AvailableSet.Max() + 1];
        for (Int32 i = 0; i < sudokuGame.NumToFill; ++i)
        {
            Coordinate coordination = sudokuGame.MapIndexToCoordination(i);
            Int32 num = sudokuGame.GetNum(coordination);
            Int32 subGridIdx = GetSubGridIdx(sudokuGame,coordination);
            if (num != 0)
            {
                Cache[subGridIdx, num] = true;
            }
        }
    }

    public void Step(SudokuGame sudokuGame, Coordinate coordination, Int32 num)
    {
        Cache[GetSubGridIdx(sudokuGame,coordination), num] = true;
    }

    public void Rollback(SudokuGame sudokuGame, Coordinate coordination)
    {
        Cache[GetSubGridIdx(sudokuGame,coordination), sudokuGame.GetNum(coordination)] = false;
    }
    protected Int32 GetSubGridIdx(SudokuGame sudokuGame, Coordinate coordination)
    {
        return coordination.X / sudokuGame.SubGridSizeX * sudokuGame.SubGridSizeX + coordination.Y / sudokuGame.SubGridSizeY;
    }
}
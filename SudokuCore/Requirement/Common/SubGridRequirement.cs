using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement.Common;

public class SubGridRequirement : IRequirement<SudokuGame, Coordinate>
{
    protected bool[,] Cache { get; set; } = { };

    public bool FitRequirement(SudokuGame sudokuGame, Coordinate coordination, int num)
    {
        return !Cache[GetSubGridIdx(sudokuGame,coordination), num];
    }

    public void Init(SudokuGame sudokuGame)
    {
        Cache = new bool[sudokuGame.SubGridNum, sudokuGame.AvailableSet.Max() + 1];
        for (int i = 0; i < sudokuGame.NumToFill; ++i)
        {
            Coordinate coordination = sudokuGame.MapIndexToCoordination(i);
            int num = sudokuGame.GetNum(coordination);
            int subGridIdx = GetSubGridIdx(sudokuGame,coordination);
            if (num != 0)
            {
                Cache[subGridIdx, num] = true;
            }
        }
    }

    public void Step(SudokuGame sudokuGame, Coordinate coordination, int num)
    {
        Cache[GetSubGridIdx(sudokuGame,coordination), num] = true;
    }

    public void Rollback(SudokuGame sudokuGame, Coordinate coordination)
    {
        Cache[GetSubGridIdx(sudokuGame,coordination), sudokuGame.GetNum(coordination)] = false;
    }
    protected int GetSubGridIdx(SudokuGame sudokuGame, Coordinate coordination)
    {
        return coordination.X / sudokuGame.SubGridSizeX * sudokuGame.SubGridSizeX + coordination.Y / sudokuGame.SubGridSizeY;
    }
}
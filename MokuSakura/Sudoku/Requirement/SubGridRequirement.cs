using MokuSakura.Sudoku.Coordination;
using MokuSakura.Sudoku.Game;

namespace MokuSakura.Sudoku.Requirement;

public class SubGridRequirement : IRequirement
{
    protected Boolean[,] Cache { get; set; } = new Boolean[0, 0];

    public Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        return !Cache[sudokuGame.GetSubGridIdx(coordination), num];
    }

    public void Init(ISudokuGame sudokuGame)
    {
        Cache = new Boolean[sudokuGame.SubGridNum, sudokuGame.AvailableSet.Max() + 1];
        for (Int32 i = 0; i < sudokuGame.NumToFill; ++i)
        {
            ICoordination coordination = sudokuGame.MapIndexToCoordination(i);
            Int32 num = sudokuGame.GetNum(coordination);
            Int32 subGridIdx = sudokuGame.GetSubGridIdx(coordination);
            if (num != 0)
            {
                Cache[subGridIdx, num] = true;
            }
        }
    }

    public void Step(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        Cache[sudokuGame.GetSubGridIdx(coordination), num] = true;
    }

    public void RollBack(ISudokuGame sudokuGame, ICoordination coordination)
    {
        Cache[sudokuGame.GetSubGridIdx(coordination), sudokuGame.GetNum(coordination)] = false;
    }
}
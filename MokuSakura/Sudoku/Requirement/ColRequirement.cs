using MokuSakura.Sudoku.Coordination;
using MokuSakura.Sudoku.Game;

namespace MokuSakura.Sudoku.Requirement;

public class ColRequirement : IRequirement
{
    protected Boolean[,] Cache { get; set; } = { };

    public Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        return !Cache[coordination.Y, num];
    }

    public void Init(ISudokuGame sudokuGame)
    {
        Cache = new Boolean[sudokuGame.ColNum, sudokuGame.AvailableSet.Max() + 1];
        for (Int32 i = 0; i < sudokuGame.NumToFill; ++i)
        {
            ICoordination coordination = sudokuGame.MapIndexToCoordination(i);
            Int32 num = sudokuGame.GetNum(coordination);
            Int32 colIdx = coordination.Y;
            if (num != 0)
            {
                Cache[colIdx, num] = true;
            }
        }
    }

    public void Step(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        Cache[coordination.Y, num] = true;
    }

    public void RollBack(ISudokuGame sudokuGame, ICoordination coordination)
    {
        Cache[coordination.Y, sudokuGame.GetNum(coordination)] = false;
    }
}
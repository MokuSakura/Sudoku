using MokuSakura.Sudoku.Coordination;
using MokuSakura.Sudoku.Game;

namespace MokuSakura.Sudoku.Requirement;

public interface IRequirement
{
    public Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num);

    public void Init(ISudokuGame sudokuGame)
    {
    }

    public void Step(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
    }

    public void RollBack(ISudokuGame sudokuGame, ICoordination coordination)
    {
    }
}
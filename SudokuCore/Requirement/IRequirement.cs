using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public interface IRequirement
{

    public Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num);

    public void Configure(IDictionary<String, Object> configuration)
    {
    }

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
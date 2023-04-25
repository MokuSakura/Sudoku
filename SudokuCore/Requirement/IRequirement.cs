using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public interface IRequirement<in TConfigType> where TConfigType : class, new()
{

    public Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num);

    public void Configure(TConfigType configuration)
    {
    }

    // public sealed Object DefaultConfig => new TConfigType();
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

public abstract class GenericRequirement<TConfigType> : IRequirement<TConfigType> where TConfigType : class,new()
{
    public abstract Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num);

    public void Configure(TConfigType configuration)
    {
    }

    // public sealed Object DefaultConfig => new TConfigType();
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
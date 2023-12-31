using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public interface IRequirement<in TSudokuGameType, in TCoordinationType>
    // where TConfigType : class, new()
    where TSudokuGameType : ISudokuGame<TCoordinationType>
    where TCoordinationType : ICoordination
{
    public Boolean FitRequirement(TSudokuGameType sudokuGame, TCoordinationType coordination, Int32 num);

    // public void Configure(Object configuration)
    // {
    // }
    // public sealed Object DefaultConfig => new TConfigType();
    public void Init(TSudokuGameType sudokuGame)
    {
    }

    public void Step(TSudokuGameType sudokuGame, TCoordinationType coordination, Int32 num)
    {
    }

    public void Rollback(TSudokuGameType sudokuGame, TCoordinationType coordination)
    {
    }
}
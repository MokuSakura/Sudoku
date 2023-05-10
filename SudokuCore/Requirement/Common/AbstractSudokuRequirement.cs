using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement.Common;

public abstract class AbstractSudokuRequirement : IRequirement<SudokuGame, Coordinate>
{
    public abstract Boolean FitRequirement(SudokuGame sudokuGame, Coordinate coordinate, Int32 num);

    public virtual void Step(SudokuGame sudokuGame, Coordinate coordinate, Int32 num)
    {
    }

    public virtual void Rollback(SudokuGame sudokuGame, Coordinate coordinate)
    {
    }

    public Boolean FitRequirement(SudokuGame sudokuGame, Int32 idx, Int32 num)
    {
        return FitRequirement(sudokuGame, sudokuGame.MapIndexToCoordination(idx), num);
    }

    // public void Configure(Object configuration)
    // {
    //     Configure((TConfigType)configuration);
    // }

    public virtual void Init(SudokuGame sudokuGame)
    {
    }

    public void Step(SudokuGame sudokuGame, Int32 idx, Int32 num)
    {
        Step(sudokuGame, sudokuGame.MapIndexToCoordination(idx), num);
    }

    public void Rollback(SudokuGame sudokuGame, Int32 idx)
    {
        Rollback(sudokuGame, sudokuGame.MapIndexToCoordination(idx));
    }
}
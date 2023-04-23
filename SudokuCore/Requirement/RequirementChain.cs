using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public class RequirementChain : IRequirement
{
    public List<IRequirement> Requirements { get; init; }

    public RequirementChain(ICollection<IRequirement> requirements)
    {
        Requirements = new List<IRequirement>(requirements);
    }

    public Boolean FitRequirement(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        Boolean res = true;
        foreach (IRequirement requirement in Requirements)
        {
            res = res && requirement.FitRequirement(sudokuGame, coordination, num);
            if (!res)
            {
                break;
            }
        }

        return res;
    }

    public void Init(ISudokuGame sudokuGame)
    {
        foreach (IRequirement requirement in Requirements)
        {
            requirement.Init(sudokuGame);
        }
    }

    public void Step(ISudokuGame sudokuGame, ICoordination coordination, Int32 num)
    {
        foreach (IRequirement requirement in Requirements)
        {
            requirement.Step(sudokuGame, coordination, num);
        }
    }

    public void RollBack(ISudokuGame sudokuGame, ICoordination coordination)
    {
        foreach (IRequirement requirement in Requirements)
        {
            requirement.RollBack(sudokuGame, coordination);
        }
    }
}
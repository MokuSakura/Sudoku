using log4net;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public class RequirementChain<TSudokuGameType, TCoordinationType>
    where TCoordinationType : ICoordination
    where TSudokuGameType : ISudokuGame<TCoordinationType>
{
    private static ILog Log => LogManager.GetLogger(typeof(RequirementChain<TSudokuGameType, TCoordinationType>));

    protected List<IRequirement<TSudokuGameType, TCoordinationType>> Requirements2 { get; init; }

    public RequirementChain(ICollection<IRequirement<TSudokuGameType, TCoordinationType>> requirements)
    {
        Requirements2 = new List<IRequirement<TSudokuGameType, TCoordinationType>>(requirements);
    }

    public bool FitRequirement(TSudokuGameType sudokuGame, TCoordinationType coordination, int num)
    {
        bool res = true;

        foreach (var requirement in Requirements2)
        {
            res = res && requirement.FitRequirement(sudokuGame, coordination, num);
            if (!res)
            {
                break;
            }
        }

        return res;
    }

    public void Init(TSudokuGameType sudokuGame)
    {
        foreach (var requirement in Requirements2)
        {
            requirement.Init(sudokuGame);
        }
    }

    public void Step(TSudokuGameType sudokuGame, TCoordinationType coordination, int num)
    {
        foreach (var requirement in Requirements2)
        {
            requirement.Step(sudokuGame, coordination, num);
        }
    }

    public void RollBack(TSudokuGameType sudokuGame, TCoordinationType coordination)
    {
        foreach (var requirement in Requirements2)
        {
            requirement.Rollback(sudokuGame, coordination);
        }
    }
}

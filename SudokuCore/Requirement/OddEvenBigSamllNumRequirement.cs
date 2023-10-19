using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public class NumRequirementConfig
{
    public List<int> LeftRows { get; set; } = new();
    public List<int> RightRows { get; set; } = new();
    public List<int> UpCols { get; set; } = new();
    public List<int> DownCols { get; set; } = new();
    public int SmallNumMax { get; set; } = 4;
}

public abstract class AbstractNumRequirement : IRequirement<SudokuGame, Coordinate>, IConfigurable<NumRequirementConfig>
{
    public HashSet<int> LeftRows { get; set; } = new();
    public HashSet<int> RightRows { get; set; } = new();
    public HashSet<int> UpCols { get; set; } = new();
    public HashSet<int> DownCols { get; set; } = new();
    protected int AffectNum { get; set; } = 2;
    protected int SmallNumMax { get; set; }
    protected abstract bool FitNumRequirement(int num);

    public virtual bool FitRequirement(SudokuGame sudokuGame, Coordinate coordination, int num)
    {
        if (LeftRows.Contains(coordination.X) && coordination.Y < AffectNum 
            || RightRows.Contains(coordination.X) && coordination.Y > sudokuGame.ColNum - AffectNum - 1
            || UpCols.Contains(coordination.Y) && coordination.X < AffectNum
            || DownCols.Contains(coordination.Y) && coordination.X > sudokuGame.RowNum - AffectNum - 1)
        {
            return FitNumRequirement(num);
        }

        return true;
    }

    public virtual void Configure(NumRequirementConfig configuration)
    {
        LeftRows.UnionWith(configuration.LeftRows);
        RightRows.UnionWith(configuration.RightRows);
        UpCols.UnionWith(configuration.UpCols);
        DownCols.UnionWith(configuration.DownCols);
        SmallNumMax = configuration.SmallNumMax;
    }
}

public class OddNumRequirement : AbstractNumRequirement
{
    protected override bool FitNumRequirement(int num)
    {
        return (num & 1) == 1;
    }
}

public class EvenNumRequirement : AbstractNumRequirement
{
    protected override bool FitNumRequirement(int num)
    {
        return (num & 1) == 0;
    }
}

public class BigNumRequirement : AbstractNumRequirement
{
    protected override bool FitNumRequirement(int num)
    {
        return num > SmallNumMax;
    }
}

public class SmallNumRequirement : AbstractNumRequirement
{
    protected override bool FitNumRequirement(int num)
    {
        return num <= SmallNumMax;
    }
}
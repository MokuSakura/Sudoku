using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public class NumRequirementConfig
{
    public List<Int32> LeftRows { get; set; } = new();
    public List<Int32> RightRows { get; set; } = new();
    public List<Int32> UpCols { get; set; } = new();
    public List<Int32> DownCols { get; set; } = new();
    public Int32 SmallNumMax { get; set; } = 4;
}

public abstract class AbstractNumRequirement : IRequirement<SudokuGame, Coordinate>, IConfigurable<NumRequirementConfig>
{
    public HashSet<Int32> LeftRows { get; set; } = new();
    public HashSet<Int32> RightRows { get; set; } = new();
    public HashSet<Int32> UpCols { get; set; } = new();
    public HashSet<Int32> DownCols { get; set; } = new();
    protected Int32 AffectNum { get; set; } = 2;
    protected Int32 SmallNumMax { get; set; }
    protected abstract Boolean FitNumRequirement(Int32 num);

    public virtual Boolean FitRequirement(SudokuGame sudokuGame, Coordinate coordination, Int32 num)
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
    protected override Boolean FitNumRequirement(Int32 num)
    {
        return (num & 1) == 1;
    }
}

public class EvenNumRequirement : AbstractNumRequirement
{
    protected override Boolean FitNumRequirement(Int32 num)
    {
        return (num & 1) == 0;
    }
}

public class BigNumRequirement : AbstractNumRequirement
{
    protected override Boolean FitNumRequirement(Int32 num)
    {
        return num > SmallNumMax;
    }
}

public class SmallNumRequirement : AbstractNumRequirement
{
    protected override Boolean FitNumRequirement(Int32 num)
    {
        return num <= SmallNumMax;
    }
}
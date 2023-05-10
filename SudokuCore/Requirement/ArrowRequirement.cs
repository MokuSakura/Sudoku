using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement.Common;

namespace MokuSakura.Sudoku.Core.Requirement;

public class ArrowRequirement : AbstractSudokuRequirement,IConfigurable<ArrowConfig>
{
    protected Dictionary<Coordinate, ArrowCache> TargetToArrowDict { get; } = new();
    protected Dictionary<Coordinate, ArrowCache> PathToArrowDict { get; } = new();

    public override Boolean FitRequirement(SudokuGame sudokuGame, Coordinate coordinate, Int32 num)
    {
        if (TargetToArrowDict.TryGetValue(coordinate, out ArrowCache? arrow))
        {
            if (arrow.RemainToAdd == 0)
            {
                return arrow.CurrentSum == num;
            }

            return arrow.CurrentSum < num;
        }

        if (PathToArrowDict.TryGetValue(coordinate, out arrow))
        {
            Int32 sumTargetValue = sudokuGame.GetNum(arrow.SumTarget);
            if (sumTargetValue == 0)
            {
                return arrow.CurrentSum + num < 10;
            }

            if (arrow.RemainToAdd == 1)
            {
                return arrow.CurrentSum + num == sumTargetValue;
            }

            return arrow.CurrentSum + num < sumTargetValue;
        }

        return true;
    }

    public void Configure(ArrowConfig configuration)
    {
        TargetToArrowDict.EnsureCapacity(configuration.Arrows.Count);
        PathToArrowDict.EnsureCapacity(configuration.Arrows.Count);
        foreach (Arrow arrow in configuration.Arrows)
        {
            ArrowCache arrowCache = new()
            {
                SumTarget = arrow.SumTarget,
                CurrentSum = 0,
                SumPath = new List<Coordinate>(arrow.SumPath),
                RemainToAdd = arrow.SumPath.Count
            };
            TargetToArrowDict.Add(arrow.SumTarget, arrowCache);
            foreach (Coordinate coordinate in arrow.SumPath)
            {
                PathToArrowDict.Add(coordinate, arrowCache);
            }
        }
    }

    public override void Step(SudokuGame sudokuGame, Coordinate coordinate, Int32 num)
    {
        if (!PathToArrowDict.TryGetValue(coordinate, out ArrowCache? arrow))
        {
            return;
        }

        arrow.CurrentSum += num;
        --arrow.RemainToAdd;
    }

    public override void Rollback(SudokuGame sudokuGame, Coordinate coordinate)
    {
        if (!PathToArrowDict.TryGetValue(coordinate, out ArrowCache? arrow))
        {
            return;
        }

        arrow.CurrentSum -= sudokuGame.GetNum(coordinate);
        ++arrow.RemainToAdd;
    }
}

public class ArrowConfig
{
    public List<Arrow> Arrows { get; set; } = new();
}

public class Arrow
{
    public Coordinate SumTarget { get; set; } = new(0, 0);
    public List<Coordinate> SumPath { get; set; } = new();
}

public class ArrowCache
{
    public Coordinate SumTarget { get; set; } = new(0, 0);
    public List<Coordinate> SumPath { get; set; } = new();
    public Int32 CurrentSum { get; set; }
    public Int32 RemainToAdd { get; set; }
}
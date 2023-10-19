using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;

namespace MokuSakura.Sudoku.Core.Requirement;

public class FortressConfiguration
{
    public List<Coordinate> GreyCoordinates { get; set; } = new();
}
public class FortressRequirement : IRequirement<SudokuGame, Coordinate>, IConfigurable<FortressConfiguration>
{
    protected HashSet<Coordinate> GreyCoordinates { get; } = new();

    protected static HashSet<Coordinate> GetAdjacentCoordinates(int rowNum, int colNum, Coordinate coordination)
    {
        HashSet<Coordinate> res = new();
        if (coordination.X > 0)
        {
            res.Add(new Coordinate(coordination.X - 1, coordination.Y));
        }
        // Look down
        if (coordination.X < rowNum - 1)
        {
            res.Add(new Coordinate(coordination.X + 1, coordination.Y));
        }
        // Look left
        if (coordination.Y > 0)
        {
            res.Add(new Coordinate(coordination.X, coordination.Y - 1));
        }
        // Look right
        if (coordination.Y < colNum - 1)
        {
            res.Add(new Coordinate(coordination.X, coordination.Y + 1));
        }

        return res;
    }
    public bool FitRequirement(SudokuGame sudokuGame, Coordinate coordination, int num)
    {
        bool isGrey = GreyCoordinates.Contains(coordination);
        HashSet<Coordinate> adjacentCoordinates = GetAdjacentCoordinates(sudokuGame.RowNum, sudokuGame.ColNum, coordination);
        if (isGrey)
        {
            foreach (Coordinate adjacentCoordinate in adjacentCoordinates)
            {
                if (GreyCoordinates.Contains(adjacentCoordinate))
                {
                    continue;
                }

                int adjNum = sudokuGame.GetNum(adjacentCoordinate);
                if (num == 0)
                {
                    continue;
                }

                if (num < adjNum)
                {
                    return false;
                }
            }

            return true;
        }

        foreach (Coordinate adjacentCoordinate in adjacentCoordinates)
        {
            if (!GreyCoordinates.Contains(adjacentCoordinate))
            {
                continue;
            }

            int adjNum = sudokuGame.GetNum(adjacentCoordinate);
            if (adjNum == 0)
            {
                continue;
            }

            if (adjNum < num)
            {
                return false;
            }
        }

        return true;

    }

    public void Configure(FortressConfiguration configuration)
    {
        GreyCoordinates.UnionWith(configuration.GreyCoordinates);
    }
}
using Newtonsoft.Json;

namespace MokuSakura.Sudoku.Core.Coordination;

/// <summary>
/// 2D coordinate. Never try to new it. Instead using index to get it from ISudokuGame by calling ISudokuGame#MapIndexToCoordination
/// </summary>
public readonly struct Coordinate : ICoordination
{
    [JsonConstructor]
    public Coordinate(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public int X { get; }

    public int Y { get; }

    public override bool Equals(object? obj)
    {
        if (obj is Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Tuple.Create(X, Y).GetHashCode();
    }

    public override string ToString()
    {
        return $"Coordinate{{X={X}, Y={Y}}}";
    }

    public static bool operator ==(Coordinate left, Coordinate right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coordinate left, Coordinate right)
    {
        return !(left == right);
    }
}
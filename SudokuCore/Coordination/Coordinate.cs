namespace MokuSakura.Sudoku.Core.Coordination;

/// <summary>
/// 2D coordinate. Never try to new it. Instead using index to get it from ISudokuGame by calling ISudokuGame#MapIndexToCoordination
/// </summary>
public readonly struct Coordinate : ICoordination
{
    internal Coordinate(Int32 x, Int32 y)
    {
        this.X = x;
        this.Y = y;
    }

    public Int32 X { get; }

    public Int32 Y { get; }

    public override Boolean Equals(Object? obj)
    {
        if (obj is Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        return false;
    }

    public override Int32 GetHashCode()
    {
        return Tuple.Create(X, Y).GetHashCode();
    }

    public override String ToString()
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
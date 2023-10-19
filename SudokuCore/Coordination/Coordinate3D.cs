namespace MokuSakura.Sudoku.Core.Coordination;

public struct Coordinate3D : ICoordination3D
{
    public Coordinate3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    
}
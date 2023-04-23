namespace MokuSakura.Sudoku.Coordination;

public struct Coordinate3D : ICoordination3D
{
    public Coordinate3D(Int32 x, Int32 y, Int32 z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Int32 X { get; }
    public Int32 Y { get; }
    public Int32 Z { get; }
    
}
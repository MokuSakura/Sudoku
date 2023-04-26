using System.Collections.Immutable;
using System.Text;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Setting;

namespace MokuSakura.Sudoku.Core.Game;

public abstract class AbstractSudokuGame<TCoordinationType> : ISudokuGame<TCoordinationType>
where TCoordinationType : ICoordination
{
    public abstract Int32 RowNum { get; }
    public abstract Int32 ColNum { get; }
    public abstract Int32 SubGridNum { get; }
    public abstract Int32 SubGridSizeX { get; }
    public abstract Int32 SubGridSizeY { get; }
    public abstract ISet<Int32> AvailableSet { get; }
    public abstract Int32 NumToFill { get; }
    public abstract Int32 GetNum(TCoordinationType coordination);

    public abstract Int32 SetNum(TCoordinationType coordination, Int32 num);

    public abstract Int32 MapCoordinationToIndex(TCoordinationType coordination);

    public abstract TCoordinationType MapIndexToCoordination(Int32 idx);

    public abstract ISudokuGame<TCoordinationType> Clone();

    public abstract void PrintGameBoard(TextWriter writer);
}

public class SudokuGame : AbstractSudokuGame<Coordinate>
{
    public Int32[,] GameBoard { get; }
    public override Int32 RowNum => rowNum;
    public override Int32 ColNum => colNum;
    public override Int32 SubGridNum { get; }

    public override Int32 SubGridSizeX => subGridSizeX;
    public override Int32 SubGridSizeY => subGridSizeY;

    public override ISet<Int32> AvailableSet { get; }

    public override Int32 NumToFill => numToFill;
    private Int32 rowNum;
    private Int32 colNum;
    private Int32 subGridSizeX;
    private Int32 subGridSizeY;
    private Int32 numToFill;
    public SudokuGame(SudokuSetting sudokuSetting) : this(sudokuSetting, new Int32[sudokuSetting.GameBoardSizeX, sudokuSetting.GameBoardSizeY])
    {
    }

    public SudokuGame(SudokuSetting sudokuSetting, Int32[,] gameBoard)
    {
        GameBoard = gameBoard;
        AvailableSet = ImmutableHashSet<Int32>.Empty.Union(sudokuSetting.AvailableSet);
        rowNum = sudokuSetting.GameBoardSizeX;
        colNum = sudokuSetting.GameBoardSizeY;
        subGridSizeX = sudokuSetting.SubGridSizeX;
        subGridSizeY = sudokuSetting.SubGridSizeY;
        numToFill = rowNum * colNum;
        SubGridNum = numToFill / subGridSizeX / subGridSizeY;
    }

    public override SudokuGame Clone()
    {
        SudokuSetting setting = new()
        {
            AvailableSet = AvailableSet,
            GameBoardSizeX = RowNum,
            GameBoardSizeY = ColNum,
            SubGridSizeX = SubGridSizeX,
            SubGridSizeY = SubGridSizeY
        };
        SudokuGame res = new(setting);
        Array.Copy(GameBoard, res.GameBoard, GameBoard.Length);
        return res;
    }

    public override Int32 GetNum(Coordinate coordination)
    {
        return GameBoard[coordination.X, coordination.Y];
    }

    public override Int32 SetNum(Coordinate coordination, Int32 num)
    {
        Int32 preNum = GetNum(coordination);
        GameBoard[coordination.X, coordination.Y] = num;
        return preNum;
    }

    public Coordinate GetSubGridBeginCoordinate(Coordinate coordination)
    {
        Int32 resX = coordination.X / SubGridSizeX * SubGridSizeX;
        Int32 resY = coordination.Y / SubGridSizeY * SubGridSizeY;
        return new Coordinate(resX, resY);
    }

    public override Int32 MapCoordinationToIndex(Coordinate coordination)
    {
        return coordination.X * ColNum + coordination.Y;
    }

    public override Coordinate MapIndexToCoordination(Int32 idx)
    {
        return new Coordinate(idx / ColNum, idx % ColNum);
    }

    public override void PrintGameBoard(TextWriter writer)
    {
        StringBuilder sb = new();
        for (Int32 i = 0; i < GameBoard.GetLength(0); ++i)
        {
            for (Int32 j = 0; j < GameBoard.GetLength(1); ++j)
            {
                sb.Append('\t')
                    .Append(GameBoard[i, j]);
            }

            writer.WriteLine(sb);
            sb.Clear();
        }
    }
}
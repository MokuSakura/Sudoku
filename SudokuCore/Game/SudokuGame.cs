using System.Collections.Immutable;
using System.Text;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Setting;

namespace MokuSakura.Sudoku.Core.Game;

public class SudokuGame : ISudokuGame<Coordinate>
{
    public Int32[,] GameBoard { get; }
    public Int32 RowNum { get; }

    public Int32 ColNum { get; }

    public Int32 SubGridNum { get; }

    public Int32 SubGridSizeX { get; }

    public Int32 SubGridSizeY { get; }

    public ISet<Int32> AvailableSet { get; }

    public Int32 NumToFill { get; }

    public SudokuGame(SudokuSetting sudokuSetting) : this(sudokuSetting, new Int32[sudokuSetting.GameBoardSizeX, sudokuSetting.GameBoardSizeY])
    {
    }

    public SudokuGame(SudokuSetting sudokuSetting, Int32[,] gameBoard)
    {
        GameBoard = gameBoard;
        AvailableSet = ImmutableHashSet<Int32>.Empty.Union(sudokuSetting.AvailableSet);
        RowNum = sudokuSetting.GameBoardSizeX;
        ColNum = sudokuSetting.GameBoardSizeY;
        SubGridSizeX = sudokuSetting.SubGridSizeX;
        SubGridSizeY = sudokuSetting.SubGridSizeY;
        NumToFill = RowNum * ColNum;
        SubGridNum = NumToFill / SubGridSizeX / SubGridSizeY;
    }

    public Int32 GetNum(Int32 idx)
    {
        return GetNum(MapIndexToCoordination(idx));
    }

    public Int32 SetNum(Int32 idx, Int32 num)
    {
        return SetNum(MapIndexToCoordination(idx), num);
    }

    public Coordinate MapIndexToCoordination(Int32 idx)
    {
        return new Coordinate(idx / ColNum, idx % ColNum);
    }

    public void PrintGameBoard(TextWriter writer)
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

    public Int32 GetNum(Coordinate coordination)
    {
        return GameBoard[coordination.X, coordination.Y];
    }

    public Int32 SetNum(Coordinate coordination, Int32 num)
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

    public Int32 MapCoordinationToIndex(Coordinate coordination)
    {
        return coordination.X * ColNum + coordination.Y;
    }
}
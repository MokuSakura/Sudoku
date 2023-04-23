using System.Collections.Immutable;
using System.Text;
using MokuSakura.Sudoku.Coordination;
using MokuSakura.Sudoku.Setting;

namespace MokuSakura.Sudoku.Game;

public class SudokuGame : ISudokuGame
{
    public Int32[,] GameBoard { get; }
    public Int32 RowNum { get; }
    public Int32 ColNum { get; }
    public Int32 SubGridNum { get; }
    public Int32 SubGridSizeX { get; }
    public Int32 SubGridSizeY { get; }
    public ISet<Int32> AvailableSet { get; }
    public Int32 NumToFill { get; }

    public SudokuGame(SudokuSetting sudokuSetting)
    {
        GameBoard = new Int32[sudokuSetting.GameBoardSizeX, sudokuSetting.GameBoardSizeY];
        AvailableSet = ImmutableHashSet<Int32>.Empty.Union(sudokuSetting.AvailableSet);
        RowNum = sudokuSetting.GameBoardSizeX;
        ColNum = sudokuSetting.GameBoardSizeY;
        SubGridSizeX = sudokuSetting.SubGridSizeX;
        SubGridSizeY = sudokuSetting.SubGridSizeY;
        NumToFill = RowNum * ColNum;
        SubGridNum = NumToFill / SubGridSizeX / SubGridSizeY;
    }

    public ISudokuGame Clone()
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

    public Int32 GetNum(ICoordination coordination)
    {
        return GameBoard[coordination.X, coordination.Y];
    }

    public Int32 SetNum(ICoordination coordination, Int32 num)
    {
        Int32 preNum = GetNum(coordination);
        GameBoard[coordination.X, coordination.Y] = num;
        return preNum;
    }

    public ICoordination GetSubGridBeginCoordinate(ICoordination coordination)
    {
        Int32 resX = coordination.X / SubGridSizeX * SubGridSizeX;
        Int32 resY = coordination.Y / SubGridSizeY * SubGridSizeY;
        return new Coordinate(resX, resY);
    }

    public Int32 MapCoordinationToIndex(ICoordination coordination)
    {
        return coordination.X * ColNum + coordination.Y;
    }

    public ICoordination MapIndexToCoordination(Int32 idx)
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
}
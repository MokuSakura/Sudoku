using System.Collections.Immutable;
using System.Text;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Setting;

namespace MokuSakura.Sudoku.Core.Game;

public class SudokuGame : ISudokuGame<Coordinate>
{
    public int[,] GameBoard { get; }
    public int RowNum { get; }

    public int ColNum { get; }

    public int SubGridNum { get; }

    public int SubGridSizeX { get; }

    public int SubGridSizeY { get; }

    public ISet<int> AvailableSet { get; }

    public int NumToFill { get; }

    public SudokuGame(SudokuSetting sudokuSetting) : this(sudokuSetting, new int[sudokuSetting.GameBoardSizeX, sudokuSetting.GameBoardSizeY])
    {
    }

    public SudokuGame(SudokuSetting sudokuSetting, int[,] gameBoard)
    {
        GameBoard = gameBoard;
        AvailableSet = ImmutableHashSet<int>.Empty.Union(sudokuSetting.AvailableSet);
        RowNum = sudokuSetting.GameBoardSizeX;
        ColNum = sudokuSetting.GameBoardSizeY;
        SubGridSizeX = sudokuSetting.SubGridSizeX;
        SubGridSizeY = sudokuSetting.SubGridSizeY;
        NumToFill = RowNum * ColNum;
        SubGridNum = NumToFill / SubGridSizeX / SubGridSizeY;
    }

    public int GetNum(int idx)
    {
        return GetNum(MapIndexToCoordination(idx));
    }

    public int SetNum(int idx, int num)
    {
        return SetNum(MapIndexToCoordination(idx), num);
    }

    public Coordinate MapIndexToCoordination(int idx)
    {
        return new Coordinate(idx / ColNum, idx % ColNum);
    }

    public void PrintGameBoard(TextWriter writer)
    {
        StringBuilder sb = new();
        for (int i = 0; i < GameBoard.GetLength(0); ++i)
        {
            for (int j = 0; j < GameBoard.GetLength(1); ++j)
            {
                sb.Append('\t')
                    .Append(GameBoard[i, j]);
            }

            writer.WriteLine(sb);
            sb.Clear();
        }
    }

    public int GetNum(Coordinate coordination)
    {
        return GameBoard[coordination.X, coordination.Y];
    }

    public int SetNum(Coordinate coordination, int num)
    {
        int preNum = GetNum(coordination);
        GameBoard[coordination.X, coordination.Y] = num;
        return preNum;
    }

    public Coordinate GetSubGridBeginCoordinate(Coordinate coordination)
    {
        int resX = coordination.X / SubGridSizeX * SubGridSizeX;
        int resY = coordination.Y / SubGridSizeY * SubGridSizeY;
        return new Coordinate(resX, resY);
    }

    public int MapCoordinationToIndex(Coordinate coordination)
    {
        return coordination.X * ColNum + coordination.Y;
    }
}
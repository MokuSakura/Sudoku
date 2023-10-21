using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using log4net;
using Microsoft.Xaml.Behaviors;
using MokuSakura.Sudoku.Core.Coordination;
using SudokuWpf.Behaviors;
using SudokuWpf.ViewModel;
using Brushes = System.Windows.Media.Brushes;

namespace SudokuWpf.View;

public partial class GameBoardView : UserControl
{
    private static ILog Log { get; } = LogManager.GetLogger(typeof(GameBoardView));

    public int[,] GameBoard
    {
        get
        {
            int[,] res = new int[GameBoardSizeX, GameBoardSizeY];
            int[] board = ((GameBoardViewModel)DataContext).GameBoard;
            for (int i = 0; i < board.Length; ++i)
            {
                int row = i / GameBoardSizeY;
                int col = i % GameBoardSizeY;
                res[row, col] = board[i];
            }

            return res;
        }
    }

    public int GridSize { get; set; } = 40;
    public bool IsReadOnly { get; init; } = false;
    public int GameBoardSizeX { get; set; } = 9;

    public int GameBoardSizeY { get; set; } = 9;

    private int SubGridSizeX { get; set; } = 3;

    private int SubGridSizeY { get; set; } = 3;
    private Dictionary<GameBoardCellView, Coordinate> CellToCoordinate { get; } = new();

    public GameBoardView()
    {
        InitializeComponent();
    }

    public override void EndInit()
    {
        base.EndInit();
        RebuildGameBoard();
    }

    protected void RebuildGameBoard()
    {
        GameBoardGrid.ColumnDefinitions.Clear();
        GameBoardGrid.RowDefinitions.Clear();
        CellToCoordinate.Clear();
        GameBoardGrid.Children.Clear();
        DataContext = new GameBoardViewModel(new int[GameBoardSizeX * GameBoardSizeY]);
        for (int i = 0; i < GameBoardSizeX; ++i)
        {
            ColumnDefinition columnDefinition = new() { Width = new GridLength(GridSize, GridUnitType.Pixel) };
            GameBoardGrid.ColumnDefinitions.Add(columnDefinition);
        }

        for (int i = 0; i < GameBoardSizeY; ++i)
        {
            RowDefinition rowDefinition = new() { Height = new GridLength(GridSize, GridUnitType.Pixel) };
            GameBoardGrid.RowDefinitions.Add(rowDefinition);
        }

        for (int i = 0; i < GameBoardSizeX; ++i)
        {
            for (int j = 0; j < GameBoardSizeY; ++j)
            {
                GameBoardCellView gameBoardCellView = new();
                gameBoardCellView.SetBorderThickness(GetBoardDirectionAndThickness(i, j));
                gameBoardCellView.BorderClick += GameBoardCellViewOnBorderClick;
                gameBoardCellView.GridClick += GameBoardCellViewOnGridClick;
                int idx = i * GameBoardSizeY + j;
                Binding binding = new($"GameBoard[{idx}]")
                {
                    Source = DataContext
                };
                gameBoardCellView.SetBinding(GameBoardCellView.NumberValueProperty, binding);

                GameBoardGrid.Children.Add(gameBoardCellView);
                Grid.SetRow(gameBoardCellView, i);
                Grid.SetColumn(gameBoardCellView, j);

                CellToCoordinate.Add(gameBoardCellView, new Coordinate(i, j));
            }
        }
    }

    private void GameBoardCellViewOnGridClick(object sender, RoutedEventArgs e)
    {
        Log.Debug($"{GetCoordinateFromCell((GameBoardCellView)sender)}");
    }

    private void GameBoardCellViewOnBorderClick(object sender, RoutedEventArgs e, Direction direction)
    {
        Log.Debug($"{GetCoordinateFromCell((GameBoardCellView)sender)} {direction}");
    }

    private Thickness GetBoardDirectionAndThickness(int i, int j)
    {
        int left = 1, top = 1, right = 1, bottom = 1;
        if (j % SubGridSizeY == 0)
        {
            left = 4;
        }

        if (i % SubGridSizeX == 0)
        {
            top = 4;
        }

        if (i == GameBoardSizeX - 1)
        {
            bottom = 4;
        }

        if (j == GameBoardSizeY - 1)
        {
            right = 4;
        }

        return new Thickness(left, top, right, bottom);
        // return new Thickness(
        //     j % SubGridSizeY == 0 ? 4 : 1,
        //     i % SubGridSizeX == 0 ? 4 : 1,
        //     j % SubGridSizeY == SubGridSizeY - 1 ? 3 : 1,
        //     i % SubGridSizeX == SubGridSizeX - 1 ? 3 : 1
        // );
    }

    protected Coordinate GetCoordinateFromCell(GameBoardCellView sender) => CellToCoordinate[sender];
}
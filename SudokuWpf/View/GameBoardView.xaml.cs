using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using log4net;
using MokuSakura.Sudoku.Core.Coordination;
using SudokuWpf.Converters;

namespace SudokuWpf.View;

public partial class GameBoardView : UserControl
{
    private static ILog Log { get; } = LogManager.GetLogger(typeof(GameBoardView));

    public int[,] GameBoard
    {
        get
        {
            int[,] res = new int[GameBoardSizeX, GameBoardSizeY];
            int[] board = GameBoard1D;
            for (int i = 0; i < board.Length; ++i)
            {
                int row = i / GameBoardSizeY;
                int col = i % GameBoardSizeY;
                res[row, col] = board[i];
            }

            return res;
        }
        set
        {
            GameBoardSizeX = value.GetLength(0);
            GameBoardSizeY = value.GetLength(1);
            int[] board = new int[GameBoardSizeX * GameBoardSizeY];

            for (int i = 0; i < GameBoardSizeX; ++i)
            {
                for (int j = 0; j < GameBoardSizeY; ++j)
                {
                    board[j + i * GameBoardSizeY] = value[i, j];
                }
            }

            GameBoard1D = board;
        }
    }

    public static readonly DependencyProperty GameBoard1DProperty = DependencyProperty.Register(
        "GameBoard1D", typeof(int[]), typeof(GameBoardView), new PropertyMetadata(default(int[])));

    private int[] GameBoard1D
    {
        get => (int[])GetValue(GameBoard1DProperty);
        set => SetValue(GameBoard1DProperty, value);
    }
    public int GridSize { get; set; } = 40;

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        "IsReadOnly", typeof(bool), typeof(GameBoardView), new PropertyMetadata(default(bool)));

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly DependencyProperty GameBoardSizeXProperty = DependencyProperty.Register(
        "GameBoardSizeX", typeof(int), typeof(GameBoardView), new PropertyMetadata(9, GameBoardSizeChanged));

    public int GameBoardSizeX
    {
        get => (int)GetValue(GameBoardSizeXProperty);
        set => SetValue(GameBoardSizeXProperty, value);
    }

    public static readonly DependencyProperty GameBoardSizeYProperty = DependencyProperty.Register(
        "GameBoardSizeY", typeof(int), typeof(GameBoardView), new PropertyMetadata(9, GameBoardSizeChanged));

    public int GameBoardSizeY
    {
        get => (int)GetValue(GameBoardSizeYProperty);
        set => SetValue(GameBoardSizeYProperty, value);
    }

    public static readonly DependencyProperty SubGridSizeXProperty = DependencyProperty.Register(
        "SubGridSizeX", typeof(int), typeof(GameBoardView), new PropertyMetadata(3, GameBoardSizeChanged));

    public int SubGridSizeX
    {
        get => (int)GetValue(SubGridSizeXProperty);
        set => SetValue(SubGridSizeXProperty, value);
    }

    public static readonly DependencyProperty SubGridSizeYProperty = DependencyProperty.Register(
        "SubGridSizeY", typeof(int), typeof(GameBoardView), new PropertyMetadata(3, GameBoardSizeChanged));

    private static void GameBoardSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as GameBoardView)?.RebuildGameBoard();
    }

    public int SubGridSizeY
    {
        get => (int)GetValue(SubGridSizeYProperty);
        set => SetValue(SubGridSizeYProperty, value);
    }

    private Dictionary<GameBoardCellView, Coordinate> CellToCoordinate { get; } = new();
    private Dictionary<Coordinate, GameBoardCellView> CoordinateToCell { get; } = new();
    private Dictionary<TextBox, GameBoardCellView> TextBoxToCell { get; } = new();

    public GameBoardView()
    {
        InitializeComponent();
        RebuildGameBoard();
    }

    public void FocusNext(GameBoardCellView gameBoardCellView)
    {
        Coordinate coordinateFromCell = GetCoordinateFromCell(gameBoardCellView);
        int x, y;
        if (coordinateFromCell.Y == GameBoardSizeY - 1)
        {
            x = coordinateFromCell.X + 1;
            y = 0;
            if (x == GameBoardSizeX)
            {
                return;
            }
        }
        else
        {
            x = coordinateFromCell.X;
            y = coordinateFromCell.Y + 1;
        }

        GameBoardCellView cellToFocus = CoordinateToCell[new Coordinate(x, y)];
        cellToFocus.TextBox.Focus();
    }

    public void RebuildGameBoard()
    {
        GameBoardGrid.ColumnDefinitions.Clear();
        GameBoardGrid.RowDefinitions.Clear();
        CellToCoordinate.Clear();
        GameBoardGrid.Children.Clear();

        DataContext = this;
        GameBoard = new int[GameBoardSizeX, GameBoardSizeY];
        for (int i = 0; i < GameBoardSizeY; ++i)
        {
            ColumnDefinition columnDefinition = new() { Width = new GridLength(GridSize, GridUnitType.Pixel) };
            GameBoardGrid.ColumnDefinitions.Add(columnDefinition);
        }

        for (int i = 0; i < GameBoardSizeX; ++i)
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
                gameBoardCellView.SetBinding(GameBoardCellView.IsReadOnlyProperty, new Binding("IsReadOnly") { Source = this, Mode = BindingMode.OneWay });
                int idx = i * GameBoardSizeY + j;
                Binding binding = new($"GameBoard1D[{idx}]")
                {
                    Source = DataContext,
                    Mode = BindingMode.TwoWay,
                    Converter = new ZeroValueConverter()
                };
                gameBoardCellView.TextBox.SetBinding(TextBox.TextProperty, binding);

                GameBoardGrid.Children.Add(gameBoardCellView);
                Grid.SetRow(gameBoardCellView, i);
                Grid.SetColumn(gameBoardCellView, j);

                Coordinate coordinate = new(i, j);
                CellToCoordinate.Add(gameBoardCellView, coordinate);
                TextBoxToCell.Add(gameBoardCellView.TextBox, gameBoardCellView);
                CoordinateToCell.Add(coordinate, gameBoardCellView);
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
    }

    protected Coordinate GetCoordinateFromCell(GameBoardCellView sender) => CellToCoordinate[sender];

    private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        IInputElement focusedElement = Keyboard.FocusedElement;
        if (focusedElement is TextBox textBox && TextBoxToCell.TryGetValue(textBox, out GameBoardCellView? cellView))
        {
            FocusNext(cellView);
        }
    }
}
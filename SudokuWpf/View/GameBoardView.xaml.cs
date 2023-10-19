using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;
using Microsoft.Xaml.Behaviors;
using MokuSakura.Sudoku.Core.Coordination;
using SudokuWpf.Behaviors;

namespace SudokuWpf.View;

public partial class GameBoardView : UserControl
{
    public delegate void GridClickHandler(object sender, MouseButtonEventArgs e, Coordinate coordinate);

    private static ILog Log { get; } = LogManager.GetLogger(typeof(GameBoardView));

    public static readonly DependencyProperty GameBoardSizeXProperty = DependencyProperty.Register("GameBoardSizeX", typeof(int), typeof(GameBoardView));

    public int GameBoardSizeX
    {
        get => (int)GetValue(GameBoardSizeXProperty);
        set => SetValue(GameBoardSizeXProperty, value);
    }

    public static readonly DependencyProperty GameBoardSizeYProperty = DependencyProperty.Register("GameBoardSizeY", typeof(int), typeof(GameBoardView));

    public int GameBoardSizeY
    {
        get => (int)GetValue(GameBoardSizeYProperty);
        set => SetValue(GameBoardSizeYProperty, value);
    }

    public static readonly DependencyProperty SubGridSizeXProperty = DependencyProperty.Register(
        "SubGridSizeX", typeof(int), typeof(GameBoardView), new PropertyMetadata(default(int)));

    public int SubGridSizeX
    {
        get => (int)GetValue(SubGridSizeXProperty);
        set => SetValue(SubGridSizeXProperty, value);
    }

    public static readonly DependencyProperty SubGridSizeYProperty = DependencyProperty.Register(
        "SubGridSizeY", typeof(int), typeof(GameBoardView), new PropertyMetadata(default(int)));

    public int SubGridSizeY
    {
        get => (int)GetValue(SubGridSizeYProperty);
        set => SetValue(SubGridSizeYProperty, value);
    }

    public int[,] GameBoard { get; set; } = new int[0, 0];
    public int GirdSize { get; set; } = 40;
    public bool IsReadOnly { get; init; } = false;
    private Dictionary<TextBox, Coordinate> TextBoxToCoordinate { get; } = new();
    // private HashSet<GridClickHandler> GridClickHandlers { get; } = new();

    public event GridClickHandler? PreviewGridClick;

    public GameBoardView()
    {
        InitializeComponent();
    }


    protected void RebuildGameBoard()
    {
        GameBoardGrid.ColumnDefinitions.Clear();
        GameBoardGrid.RowDefinitions.Clear();
        TextBoxToCoordinate.Clear();
        GameBoardGrid.Children.Clear();

        GameBoard = new int[GameBoardSizeX, GameBoardSizeY];
        DataContext = GameBoard;
        for (int i = 0; i < GameBoardSizeX; ++i)
        {
            ColumnDefinition columnDefinition = new() { Width = new GridLength(GirdSize, GridUnitType.Auto) };
            GameBoardGrid.ColumnDefinitions.Add(columnDefinition);
        }

        for (int i = 0; i < GameBoardSizeY; ++i)
        {
            RowDefinition rowDefinition = new() { Height = new GridLength(GirdSize, GridUnitType.Auto) };
            GameBoardGrid.RowDefinitions.Add(rowDefinition);
        }

        for (int i = 0; i < GameBoardSizeX; ++i)
        {
            for (int j = 0; j < GameBoardSizeY; ++j)
            {
                TextBox textBox = new();
                textBox.PreviewMouseLeftButtonDown += TextBlockOnPreviewMouseDown;
                Interaction.GetBehaviors(textBox).Add(new TextBoxIntegerOnly());
                textBox.MaxWidth = GirdSize;
                textBox.Width = GirdSize;
                textBox.Height = GirdSize;
                textBox.TextAlignment = TextAlignment.Center;
                textBox.Margin = new Thickness(3, 3, 0, 0);
                textBox.IsReadOnly = IsReadOnly;
                GameBoardGrid.Children.Add(textBox);
                textBox.SetValue(Grid.RowProperty, j);
                textBox.SetValue(Grid.ColumnProperty, i);
                TextBoxToCoordinate.Add(textBox, new Coordinate(i, j));
            }
        }
    }

    protected Coordinate GetCoordinateFromSender(object sender) => TextBoxToCoordinate[(TextBox)sender];

    private void TextBlockOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        PreviewGridClick?.Invoke(sender, e, GetCoordinateFromSender(sender));
    }
}
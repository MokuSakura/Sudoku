using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;

namespace SudokuWpf.Controls;

public partial class GameBoardControl : UserControl
{
    public Int32 GameBoardSizeX { get; init; } = 9;
    public Int32 GameBoardSizeY { get; init; } = 9;
    public Int32 SubGridSizeX { get; init; } = 3;
    public Int32 SubGridSizeY { get; init; } = 3;
    public Int32[,] GameBoard { get; }
    public const Int32 GRID_LENGTH = 40;
    public Boolean IsReadOnly { get; init; } = false;
    private static ILog Log { get; } = LogManager.GetLogger(typeof(GameBoardControl));
    public GameBoardControl()
    {
        InitializeComponent();
        GameBoard = new Int32[GameBoardSizeX, GameBoardSizeY];
        for (Int32 i = 0; i < GameBoardSizeX; ++i)
        {
            ColumnDefinition columnDefinition = new() { Width = new GridLength(GRID_LENGTH, GridUnitType.Auto) };
            MainGrid.ColumnDefinitions.Add(columnDefinition);
        }

        for (Int32 i = 0; i < GameBoardSizeY; ++i)
        {
            RowDefinition rowDefinition = new() { Height = new GridLength(GRID_LENGTH, GridUnitType.Auto) };
            MainGrid.RowDefinitions.Add(rowDefinition);
        }

        for (Int32 i = 0; i < GameBoardSizeX; ++i)
        {
            for (Int32 j = 0; j < GameBoardSizeY; ++j)
            {
                TextBox textBlock = new TextBox();
                textBlock.MouseDown += TextBlockOnMouseDown;
                textBlock.TextInput += TextBlockOnTextInput;
                
                textBlock.MaxWidth = GRID_LENGTH;
                textBlock.MaxWidth = GRID_LENGTH;
                textBlock.Width = GRID_LENGTH;
                textBlock.Height = GRID_LENGTH;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.Margin = new Thickness(3, 3, 0, 0);
                MainGrid.Children.Add(textBlock);
                textBlock.SetValue(Grid.RowProperty, j);
                textBlock.SetValue(Grid.ColumnProperty, i);
            }
        }
    }

    private void TextBlockOnMouseDown(Object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
        Log.Debug(sender);
    }

    private void TextBlockOnTextInput(Object sender, TextCompositionEventArgs e)
    {
        if (IsReadOnly)
        {
            e.Handled = true;
            return;
        }
        e.Handled = false;
    }
}
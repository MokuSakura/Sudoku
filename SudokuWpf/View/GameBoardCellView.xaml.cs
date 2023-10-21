using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MokuSakura.Sudoku.Core.Coordination;

namespace SudokuWpf.View;

public partial class GameBoardCellView : UserControl
{
    public delegate void BorderClickHandler(object sender, RoutedEventArgs e, Direction direction);

    public event BorderClickHandler? BorderClick;
    public event RoutedEventHandler? GridClick;
    private Dictionary<object, Direction> SenderToDirection { get; } = new();

    public static readonly DependencyProperty NumberValueProperty = DependencyProperty.Register(
        "NumberValue", typeof(int), typeof(GameBoardCellView), new PropertyMetadata(default(int)));

    public int NumberValue
    {
        get => (int)GetValue(NumberValueProperty);
        set => SetValue(NumberValueProperty, value);
    }

    public GameBoardCellView()
    {
        InitializeComponent();
        DataContext = this;
        SenderToDirection.Add(LeftEdge, Direction.Left);
        SenderToDirection.Add(LeftTopCorner, Direction.LeftUp);
        SenderToDirection.Add(TopEdge, Direction.Up);
        SenderToDirection.Add(RightTopCorner, Direction.RightUp);
        SenderToDirection.Add(RightEdge, Direction.Right);
        SenderToDirection.Add(RightBottomCorner, Direction.RightDown);
        SenderToDirection.Add(BottomEdge, Direction.Down);
        SenderToDirection.Add(LeftBottomCorner, Direction.LeftDown);
    }

    public void SetBorderThickness(Thickness thickness)
    {
        LeftEdge.BorderThickness = new Thickness(thickness.Left, 0, 0, 0);
        LeftTopCorner.BorderThickness = new Thickness(thickness.Left, thickness.Top, 0, 0);
        TopEdge.BorderThickness = new Thickness(0, thickness.Top, 0, 0);
        RightTopCorner.BorderThickness = new Thickness(0, thickness.Top, thickness.Right, 0);
        RightEdge.BorderThickness = new Thickness(0, 0, thickness.Right, 0);
        RightBottomCorner.BorderThickness = new Thickness(0, 0, thickness.Right, thickness.Bottom);
        BottomEdge.BorderThickness = new Thickness(0, 0, 0, thickness.Bottom);
        LeftBottomCorner.BorderThickness = new Thickness(thickness.Left, 0, 0, thickness.Bottom);


        // if (direction.HasFlag(Direction.Left))
        // {
        //     LeftEdge.BorderThickness = new Thickness(thickness, 0, 0, 0);
        //     LeftUpCorner.BorderThickness = new Thickness(thickness, 0, 0, 0);
        //     LeftButtomCorner.BorderThickness = new Thickness(thickness, 0, 0, 0);
        // }
        //
        // if (direction.HasFlag(Direction.Up))
        // {
        //     TopEdge.BorderThickness = new Thickness(0, thickness, 0, 0);
        //     LeftUpCorner.BorderThickness = new Thickness(0, thickness, 0, 0);
        //     RightTopCorner.BorderThickness = new Thickness(0, thickness, 0, 0);
        // }
        //
        // if (direction.HasFlag(Direction.Right))
        // {
        //     RightEdge.BorderThickness = new Thickness(0, 0, thickness, 0);
        //     RightTopCorner.BorderThickness = new Thickness(0, 0, thickness, 0);
        //     RightBottomCorner.BorderThickness = new Thickness(0, 0, thickness, 0);
        // }
        //
        // if (direction.HasFlag(Direction.Down))
        // {
        //     BottomEdge.BorderThickness = new Thickness(0, 0, 0, thickness);
        //     LeftButtomCorner.BorderThickness = new Thickness(0, 0, 0, thickness);
        //     RightBottomCorner.BorderThickness = new Thickness(0, 0, 0, thickness);
        // }
    }

    private void ButtonOnClick(object sender, RoutedEventArgs e)
    {
        Direction direction = GetDirectionFromSender(sender);
        BorderClick?.Invoke(this, e, direction);
    }

    private Direction GetDirectionFromSender(object sender)
    {
        return SenderToDirection[sender];
    }

    private void TextBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        GridClick?.Invoke(this, e);
    }
}
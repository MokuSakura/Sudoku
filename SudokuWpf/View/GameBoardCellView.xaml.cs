using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace SudokuWpf.View;

public partial class GameBoardCellView : UserControl
{
    public delegate void BorderClickHandler(object sender, RoutedEventArgs e, Direction direction);

    public event BorderClickHandler? BorderClick;
    public event RoutedEventHandler? GridClick;
    private Dictionary<object, Direction> SenderToDirection { get; } = new();

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        "IsReadOnly", typeof(bool), typeof(GameBoardCellView), new PropertyMetadata(default(bool)));

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
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
        TextBox.SetBinding(TextBoxBase.IsReadOnlyProperty, new Binding("IsReadOnly") { Source = this, Mode = BindingMode.OneWay});
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

    private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        TextBox textBox = (sender as TextBox)!;
        // Fixes issue when clicking cut/copy/paste in context menu
        if (textBox.SelectionLength == 0) 
            textBox.SelectAll();
    }

    private void TextBox_LostMouseCapture(object sender, MouseEventArgs e)
    {
        TextBox textBox = (sender as TextBox)!;
        // If user highlights some text, don't override it
        if (textBox.SelectionLength == 0) 
            textBox.SelectAll();

        // further clicks will not select all
        textBox.LostMouseCapture -= TextBox_LostMouseCapture; 
    }

    private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        TextBox textBox = (sender as TextBox)!;
        // once we've left the TextBox, return the select all behavior
        textBox.LostMouseCapture += TextBox_LostMouseCapture;
    }
}
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SudokuWpf.Behaviors;

public class TextBoxIntegerOnly : Behavior<TextBox>
{
    #region DependencyProperties

    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register("MaxLength", typeof(int), typeof(TextBoxIntegerOnly),
            new FrameworkPropertyMetadata(Int32.MinValue));

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public static readonly DependencyProperty EmptyValueProperty =
        DependencyProperty.Register("EmptyValue", typeof(string), typeof(TextBoxIntegerOnly), null);

    public string EmptyValue
    {
        get => (string)GetValue(EmptyValueProperty);
        set => SetValue(EmptyValueProperty, value);
    }

    #endregion

    /// <summary>
    ///     Attach our behaviour. Add event handlers
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();
        InputMethod.SetIsInputMethodEnabled(AssociatedObject, false);
        AssociatedObject.PreviewTextInput += PreviewTextInputHandler;
        AssociatedObject.PreviewKeyDown += PreviewKeyDownHandler;
        DataObject.AddPastingHandler(AssociatedObject, PastingHandler);
    }

    /// <summary>
    ///     Deattach our behaviour. remove event handlers
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.PreviewTextInput -= PreviewTextInputHandler;
        AssociatedObject.PreviewKeyDown -= PreviewKeyDownHandler;
        DataObject.RemovePastingHandler(AssociatedObject, PastingHandler);
    }

    #region Event handlers [PRIVATE] --------------------------------------

    private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
    {
        string text;
        if (AssociatedObject.SelectionLength != 0)
        {
            text = AssociatedObject.Text.Remove(AssociatedObject.CaretIndex, AssociatedObject.SelectionLength).Insert(AssociatedObject.CaretIndex, e.Text);
        }
        else
        {
            text = AssociatedObject.Text.Insert(AssociatedObject.CaretIndex, e.Text);
        }

        // if (this.AssociatedObject.Text.Length < this.AssociatedObject.CaretIndex)
        //     text = this.AssociatedObject.Text;
        // else
        // {
        //     //  Remaining text after removing selected text.
        //     String remainingTextAfterRemoveSelection;
        //
        //     text = TreatSelectedText(out remainingTextAfterRemoveSelection)
        //         ? remainingTextAfterRemoveSelection.Insert(AssociatedObject.SelectionStart, e.Text)
        //         : AssociatedObject.Text.Insert(this.AssociatedObject.CaretIndex, e.Text);
        // }
        //
        e.Handled = !ValidateText(text);
    }

    /// <summary>
    ///     PreviewKeyDown event handler
    /// </summary>
    void PreviewKeyDownHandler(object sender, KeyEventArgs e)
    {
        if (String.IsNullOrEmpty(this.EmptyValue))
            return;

        string text = "";

        // Handle the Backspace key
        if (e.Key == Key.Back)
        {
            if (!this.TreatSelectedText(out text))
            {
                if (AssociatedObject.SelectionStart > 0)
                    text = this.AssociatedObject.Text.Remove(AssociatedObject.SelectionStart - 1, 1);
            }
        }
        // Handle the Delete key
        else if (e.Key == Key.Delete)
        {
            // If text was selected, delete it
            if (!this.TreatSelectedText(out text) && this.AssociatedObject.Text.Length > AssociatedObject.SelectionStart)
            {
                // Otherwise delete next symbol
                text = this.AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, 1);
            }
        }

        if (text == String.Empty)
        {
            this.AssociatedObject.Text = this.EmptyValue;
            if (e.Key == Key.Back)
                AssociatedObject.SelectionStart++;
            e.Handled = true;
        }
    }

    private void PastingHandler(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            string text = Convert.ToString(e.DataObject.GetData(DataFormats.Text))!;

            if (!ValidateText(text))
                e.CancelCommand();
        }
        else
            e.CancelCommand();
    }

    #endregion Event handlers [PRIVATE] -----------------------------------

    #region Auxiliary methods [PRIVATE] -----------------------------------

    /// <summary>
    ///     Validate certain text by our regular expression and text length conditions
    /// </summary>
    /// <param name="text"> Text for validation </param>
    /// <returns> True - valid, False - invalid </returns>
    private bool ValidateText(string text)
    {
        return Int32.TryParse(text, NumberStyles.Integer, null, out int _) && (MaxLength == Int32.MinValue || text.Length <= MaxLength);
    }

    /// <summary>
    ///     Handle text selection
    /// </summary>
    /// <returns>true if the character was successfully removed; otherwise, false. </returns>
    private bool TreatSelectedText(out string text)
    {
        text = "";
        if (AssociatedObject.SelectionLength <= 0)
            return false;

        var length = this.AssociatedObject.Text.Length;
        if (AssociatedObject.SelectionStart >= length)
            return true;

        if (AssociatedObject.SelectionStart + AssociatedObject.SelectionLength >= length)
            AssociatedObject.SelectionLength = length - AssociatedObject.SelectionStart;

        text = this.AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, AssociatedObject.SelectionLength);
        return true;
    }

    #endregion Auxiliary methods [PRIVATE] --------------------------------
}
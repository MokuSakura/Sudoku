using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;
using MokuSakura.Sudoku.Core.Requirement.Common;
using MokuSakura.Sudoku.Core.Setting;
using MokuSakura.Sudoku.Core.Solver;

namespace SudokuWpf.View;

public partial class SimpleGameView : UserControl
{

    public SimpleGameView()
    {
        InitializeComponent();
    }

    private void OnSolveButtonClick(object sender, RoutedEventArgs e)
    {
        DfsSolver<SudokuGame, Coordinate> solver = new();
        SudokuSetting setting = new();
        SudokuGame sudokuGame = new(setting, QuizView.GameBoard);
        RequirementChain<SudokuGame, Coordinate> requirementChain = new(new IRequirement<SudokuGame, Coordinate>[]
        {
            new RowRequirement(),
            new ColRequirement(),
            new SubGridRequirement()
        });
        DateTime t1 = DateTime.Now;
        SudokuGame res = solver.Solve(sudokuGame, requirementChain);
        DateTime t2 = DateTime.Now;
        AnswerView.GameBoard = res.GameBoard;
        TimeUsageBlock.Text = $"{(t2 - t1).TotalMilliseconds:F2} ms";
    }
}
using MokuSakura.Sudoku.Core.Coordination;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;
using MokuSakura.Sudoku.Core.Requirement.Common;
using MokuSakura.Sudoku.Core.Setting;
using MokuSakura.Sudoku.Core.Solver;
using MokuSakura.Sudoku.Core.Util;

namespace MokuSakura.SudokuConsole.Starter;

public class SimpleConsoleStarter : ISudokuStarter<SudokuGame, Coordinate>
{
    public String RegisterName => "SimpleConsoleStarter";

    public SudokuGame Run()
    {
        SudokuSetting setting = new();
        SudokuGame sudokuGame = new(setting);
        Scanner scanner = new(Console.OpenStandardInput());
        for (Int32 i = 0; i < sudokuGame.NumToFill; ++i)
        {
            Coordinate coordination = sudokuGame.MapIndexToCoordination(i);
            sudokuGame.SetNum(coordination, scanner.NextInt());
        }

        Console.Out.WriteLine("GameBoard: ");
        sudokuGame.PrintGameBoard(Console.Out);
        DfsSolver<SudokuGame, Coordinate> solver = new();
        RequirementChain<SudokuGame, Coordinate> requirementChain = new(new IRequirement<SudokuGame, Coordinate>[]
        {
            new RowRequirement(),
            new ColRequirement(),
            new SubGridRequirement()
        });
        DateTime t1 = DateTime.Now;
        SudokuGame sudokuGames = solver.Solve(sudokuGame, requirementChain);
        DateTime t2 = DateTime.Now;
        Console.Out.WriteLine($"Time used: {(t2 - t1).Milliseconds}ms");
        return sudokuGames;
    }
}
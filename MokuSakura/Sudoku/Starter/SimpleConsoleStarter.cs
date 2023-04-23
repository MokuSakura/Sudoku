using MokuSakura.Sudoku.Coordination;
using MokuSakura.Sudoku.Game;
using MokuSakura.Sudoku.Requirement;
using MokuSakura.Sudoku.Setting;
using MokuSakura.Sudoku.Solver;
using MokuSakura.Sudoku.Util;

namespace MokuSakura.Sudoku.Starter;

public class SimpleConsoleStarter : ISudokuStarter
{
    public String RegisterName => "SimpleConsoleStarter";

    public ICollection<ISudokuGame> Run()
    {
        SudokuSetting setting = new();
        ISudokuGame sudokuGame = new SudokuGame(setting);
        Scanner scanner = new(Console.OpenStandardInput());
        for (Int32 i = 0; i < sudokuGame.NumToFill; ++i)
        {
            ICoordination coordination = sudokuGame.MapIndexToCoordination(i);
            sudokuGame.SetNum(coordination, scanner.NextInt());
        }

        Console.Out.WriteLine("GameBoard: ");
        sudokuGame.PrintGameBoard(Console.Out);
        ISolver solver = new DfsSolver();
        RequirementChain requirementChain = new(new IRequirement[]
        {
            new RowRequirement(),
            new ColRequirement(),
            new SubGridRequirement()
        });
        DateTime t1 = DateTime.Now;
        ICollection<ISudokuGame> sudokuGames = solver.Solve(sudokuGame, requirementChain);
        DateTime t2 = DateTime.Now;
        if (sudokuGames.Count == 0)
        {
            Console.Error.WriteLine("No Solution");
        }

        foreach (ISudokuGame res in sudokuGames)
        {
            Console.Out.WriteLine("Res:");
            res.PrintGameBoard(Console.Out);
        }

        Console.Out.WriteLine($"Time used: {(t2 - t1).Milliseconds}ms");
        return sudokuGames;
    }
}
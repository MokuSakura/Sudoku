using System.Reflection;
using log4net;
using MokuSakura.Sudoku.Starter;

namespace MokuSakura.Sudoku;

public sealed class ConsoleApp
{
    private static ILog Log => LogManager.GetLogger(typeof(ConsoleApp));

    public static void Main(String[] args)
    {
        SudokuGeneralEntryPoint sudokuGeneralEntryPoint = SudokuGeneralEntryPoint.Instance;
        Log.Info("Starters info: ");

        foreach ((String key, ISudokuStarter value) in sudokuGeneralEntryPoint.SudokuStarters)
        {
            Log.Info($"{key}: {value.GetType().FullName}");
        }

        Log.Debug("Requirements info: ");
        foreach ((String key, Type requirementType) in sudokuGeneralEntryPoint.RequirementTypes)
        {
            Log.Debug($"{key}: {requirementType.FullName}");
        }

        Console.WriteLine("Input starter to use.");
        String starterName = Console.ReadLine() ?? "";
        sudokuGeneralEntryPoint.Run(starterName, new Dictionary<String, Object>());
    }
}
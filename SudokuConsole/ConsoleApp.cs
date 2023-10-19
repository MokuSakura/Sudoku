using log4net;
using MokuSakura.Sudoku.Core.Game;
using MokuSakura.Sudoku.Core.Requirement;
using MokuSakura.SudokuConsole.Starter;

namespace MokuSakura.SudokuConsole;

public sealed class ConsoleApp
{
    private static ILog Log => LogManager.GetLogger(typeof(ConsoleApp));

    private static IDictionary<string, Action> NamedActions => new Dictionary<string, Action>
    {
        { "json", RunJson },
    };

    public static void Main(string[] args)
    {
        SudokuRequirementReflectionUtils sudokuRequirementReflectionUtils = SudokuRequirementReflectionUtils.Instance;

        Log.Debug("Requirements info: ");
        foreach ((string key, SudokuRequirementReflectionUtils.RequirementTemplate requirementTemplate) in
                 sudokuRequirementReflectionUtils.RequirementTemplates)
        {
            Log.Debug($"{key}: {requirementTemplate.RequirementType.FullName}");
        }

        Console.WriteLine("Input starter to use.");
        string starterName = Console.ReadLine() ?? "";

        if (NamedActions.TryGetValue(starterName, out Action? value))
        {
            value();
        }
        else
        {
            Log.Error($"Cannot find starter {starterName}");
        }
    }

    private static void RunJson()
    {
        Log.Info("Input json file");
        string path = Console.ReadLine() ?? "";
        path = path.Trim('\"');
        if (!File.Exists(path))
        {
            Log.Error($"{path} is not a file");
            return;
        }

        string json;
        {
            using FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            TextReader textReader = new StreamReader(fileStream);
            json = textReader.ReadToEnd();
        }
        JsonSudokuStarter starter = new(json);
        SudokuGame sudokuGame = starter.Run(out long solveTime);
        sudokuGame.PrintGameBoard(Console.Out);
        Console.WriteLine($"Time used {solveTime}ms");
    }
}
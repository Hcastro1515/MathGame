using System.Text;
using Spectre.Console;

namespace MathGame;

public class Program
{
    private static readonly List<int> Scores = new();
    private static string? _playerName;
    private static DateTime _currentDate; 
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to your math game");
        Console.WriteLine("Please enter your name");
        _playerName = Console.ReadLine();
        _currentDate = DateTime.UtcNow;

        ShowMainMenu();
    }

    private static void ShowMainMenu()
    {
        var table = new Table().Centered();
        table.AddColumn(
            new TableColumn(
                    $"Hello {_playerName?.ToUpper()} it's {_currentDate.DayOfWeek}, What game would you like to play today: ")
                .Centered());
        table.AddRow(new Markup("[bold]Select a game:[/]").Centered());
        table.AddRow("Addition [green](A)[/]");
        table.AddRow("Subtraction [green](S)[/]");
        table.AddRow("Multiplication [green](M)[/]");
        table.AddRow("Division [green](D)[/]");
        table.AddRow("Quit the program [green](Q)[/]");

        AnsiConsole.Write(table);

        var selection = AnsiConsole.Ask<string>("Enter your choice: ");

        switch (selection.ToUpper())
        {
            case "A":
                PlayGame(GameType.Addition);
                break;
            case "S":
                PlayGame(GameType.Subtraction);
                break;
            case "M":
                PlayGame(GameType.Multiplication);
                break;
            case "D":
                PlayGame(GameType.Division);
                break;
            case "Q":
                PrintScores();
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                ShowMainMenu();
                break;
        }
    }


    private static void PlayGame(GameType gameType)
    {
        var score = 0;
        var random = new Random();
        var operation = GetOperationSymbol(gameType);

        Console.WriteLine($"{gameType} Game!");
        for (var i = 0; i < 5; i++)
        {
            var number1 = random.Next(1, 9);
            var number2 = random.Next(1, 9);
            Console.WriteLine($"{number1} {operation} {number2}");
            Console.Write("Enter your answer: ");
            var answer = Console.ReadLine();

            if (int.TryParse(answer, out var result) && result == GetOperationResult(gameType, number1, number2))
            {
                Console.WriteLine("Your answer is correct");
                score++;
            }
            else
            {
                Console.WriteLine("Your answer is wrong");
            }
        }

        Scores.Add(score);
        Console.WriteLine($"End of game: your score is {score}");
        Console.WriteLine("Would you like to keep playing? (y/n)");

        string? choice;
        do
        {
            choice = Console.ReadLine()?.Trim().ToLower();
        } while (choice != "y" && choice != "n");

        if (choice == "y")
        {
            ShowMainMenu();
        }
        else
        {
            PrintScores();
            Environment.Exit(0);
        }
    }

    private static char GetOperationSymbol(GameType gameType)
    {
        return gameType switch
        {
            GameType.Addition => '+',
            GameType.Subtraction => '-',
            GameType.Multiplication => '*',
            GameType.Division => '/',
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, "Invalid game type.")
        };
    }

    private static int GetOperationResult(GameType gameType, int number1, int number2)
    {
        return gameType switch
        {
            GameType.Addition => number1 + number2,
            GameType.Subtraction => number1 - number2,
            GameType.Multiplication => number1 * number2,
            GameType.Division => number1 / number2,
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, "Invalid game type.")
        };
    }

    private static void PrintScores()
    {
        Console.Write("Here are your scores: ");
        var sb = new StringBuilder();

        foreach (var score in Scores) sb.Append($"{score}, ");

        sb.Length -= 2; // Remove the last comma and space
        Console.WriteLine(sb);
    }

    private enum GameType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
}
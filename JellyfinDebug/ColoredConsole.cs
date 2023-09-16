using Colorful;
using System.Drawing;
using Console = System.Console;

namespace JellyfinDebug;

internal class ColoredConsole
{
	public static Color ErrorColor = Color.Red;
	public static Color WarningColor = Color.Yellow;
	public static Color SuccessColor = Color.Green;
	public static Color InfoColor = Color.Aqua;
	public static Color InfoColorAlternate = Color.MediumAquamarine;
	public static StyleSheet ColoredSheet;


	public static void SetupMarkup()
	{
		var styleSheet = new StyleSheet(Color.White);
		SetupMarkupPart(styleSheet, "Error", ErrorColor);
		SetupMarkupPart(styleSheet, "Warning", WarningColor);
		SetupMarkupPart(styleSheet, "warn", WarningColor);
		SetupMarkupPart(styleSheet, "Success", SuccessColor);
		SetupMarkupPart(styleSheet, "Info", InfoColor);
		SetupIconPart(styleSheet, "OK", "\u2714", SuccessColor);
		SetupIconPart(styleSheet, "ERR", "\u274C", ErrorColor);
		SetupIconPart(styleSheet, "INF", "!", InfoColor);
		SetupIconPart(styleSheet, "WARN", "!", WarningColor);
		SetupIconPart(styleSheet, "NOP", "-", WarningColor);
		ColoredSheet = styleSheet;

        System.Console.OutputEncoding = System.Text.Encoding.Default;
	}

	private static void SetupIconPart(StyleSheet styleSheet, string iconName, string unicodeIcon, Color color)
	{
		var target = $"(\\[{iconName}\\])";
		styleSheet.AddStyle(target, color, match => match.Replace($"[{iconName}]", unicodeIcon));
	}

	private static void SetupMarkupPart(StyleSheet styleSheet, string name, Color color)
	{
		var target = $@"<{name}>(.*?)<\/{name}>";
		var idLength = $"<{name}>".Length;

		string MatchHandler(string e)
		{
			if (e is null or "")
			{
				return e;
			}

			var nString = e.Remove(0, idLength);
			return nString.Remove(nString.Length - idLength - 1);
		}

		styleSheet.AddStyle(target, color, MatchHandler);
		styleSheet.AddStyle(target.ToLower(), color, MatchHandler);
	}

	public static bool Ask(string question)
	{
		ConsoleKeyInfo input;
		do
		{
			Write($"{question} [y|n]:");
			input = Console.ReadKey();
			Console.WriteLine();
		} while (input.Key is not ConsoleKey.Y and not ConsoleKey.N);

		return input.Key is ConsoleKey.Y;
	}

	public static int AskNumber(string question)
	{
		string input;
		int number;
		do
		{
			Write($"{question}: ");
			input = Console.ReadLine();
		} while (!int.TryParse(input, out number));

		return number;
	}

	public static string AskStringNonNull(string question)
	{
		string input;
		int number;
		do
		{
			Write($"{question}: ");
			input = Console.ReadLine();
		} while (input is null);

		return input;
	}

	public static void WriteLine(string text)
	{
		Write(text + Environment.NewLine);
	}

	public static void Write(string text)
	{
		var textAnnotator = new TextAnnotator(ColoredSheet);
		var keyValuePairs = textAnnotator.GetAnnotationMap(text);
		foreach (var keyValuePair in keyValuePairs)
		{
			var oldColor = Console.ForegroundColor;
			Console.ForegroundColor = keyValuePair.Value.ToNearestConsoleColor();
			Console.Write(keyValuePair.Key);
			Console.ForegroundColor = oldColor;
		}
	}

	public static string ReadLine()
	{
		return Console.ReadLine();
	}
}
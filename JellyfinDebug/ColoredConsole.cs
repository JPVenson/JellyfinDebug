using Colorful;
using System.Drawing;
using Console = Colorful.Console;

namespace JellyfinDebug;

internal class ColoredConsole
{
	public static Color ErrorColor = Color.Red;
	public static Color WarningColor = Color.Yellow;
	public static Color SuccessColor = Color.LawnGreen;
	public static Color InfoColor = Color.Aqua;
	public static Color InfoColorAlternate = Color.MediumAquamarine;
	public static StyleSheet ColoredSheet;


	public static void SetupMarkup()
	{
		var styleSheet = new StyleSheet(Color.White);
		SetupMarkupPart(styleSheet, "Error", ErrorColor);
		SetupMarkupPart(styleSheet, "Warning", WarningColor);
		SetupMarkupPart(styleSheet, "Success", SuccessColor);
		SetupMarkupPart(styleSheet, "Info", InfoColor);
		SetupIconPart(styleSheet, "OK", "\u2714", SuccessColor);
		SetupIconPart(styleSheet, "ERR", "\u274C", ErrorColor);
		ColoredSheet = styleSheet;
	}

	private static void SetupIconPart(StyleSheet styleSheet, string iconName, string unicodeIcon, Color color)
	{
		var target = $"(\\[{iconName}\\])";
		styleSheet.AddStyle(target, color, match => match.Replace(iconName, unicodeIcon));
	}

	private static void SetupMarkupPart(StyleSheet styleSheet, string name, Color color)
	{
		var target = $@"<{name}>(.*?)<\/{name}>";
		var idLength = $"<{name}>".Length;

		string MatchHandler(string e)
		{
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
			Console.Write($"{question} [y|n]:");
			input = Console.ReadKey();
			Console.WriteLine();
		} while (input.Key is not ConsoleKey.Y and not ConsoleKey.N);

		return input.Key is ConsoleKey.Y;
	}

	public static void WriteLine(string text)
	{
		Console.WriteLineStyled(text, ColoredSheet);
	}

	public static void Write(string text)
	{
		Console.WriteStyled(text, ColoredSheet);
	}

	public static string ReadLine()
	{
		return Console.ReadLine();
	}
}
// See https://aka.ms/new-console-template for more information

using JellyfinDebug;
using JellyfinDebug.MenuItems;
using Microsoft.Extensions.DependencyInjection;
using ServiceLocator.Discovery.Service;
using Console = JellyfinDebug.ColoredConsole;

ColoredConsole.SetupMarkup();

Console.WriteLine("Jellyfin Debugger");

var services = new DefaultServiceProviderFactory()
	.CreateBuilder(new ServiceCollection().UseServiceDiscovery().FromAppDomain().LocateServices())
	.BuildServiceProvider();
await using var scope = services.CreateAsyncScope();

var actions = scope.ServiceProvider.GetServices<IDebugAction>().ToArray();
for (int i = 0; i < actions.Length; i++)
{
	var action = actions[i];
	Console.WriteLine($"\t {i + 1} - {action.Name}");
}

int id = 1;
while (false)
{
	Console.Write("Choose your Action:");
	var input = Console.ReadLine();
	if (!int.TryParse(input, out id))
	{
		Console.WriteLine($"Could not parse input: <error>\"{input}\"</error>. Please input an number from the list.");
		continue;
	}

	if (id < 1 || id > actions.Count())
	{
		Console.WriteLine($"Input out of range. Min is 1 max is {actions.Length + 1}");
		continue;
	}

	break;
}

await actions[id - 1].Execute();
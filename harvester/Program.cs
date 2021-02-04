using Devisioona.Harvest.CLI.Commands;
using Devisioona.Harvest.CLI.Configuration;
using Devisioona.Harvest.CLI.HarvestApi;
using Microsoft.Extensions.Configuration;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Devisioona.Harvest.CLI
{
	internal class Program
	{
		private static CLISettings settings;

		internal static CLISettings Settings => settings;

		internal static HarvestClient GetHarvestClient() => new HarvestClient(Settings.Harvest);

		static int Main(string[] args)
		{
			IConfiguration config = new ConfigurationBuilder()
					  .AddJsonFile("appsettings.json", false, true)
					  .AddJsonFile("appsettings.local.json", true, true)
					  .Build();

			settings = new CLISettings();
			config.Bind(settings);

			var rootCommand = CreateCommands();

			return rootCommand.Invoke(args);

		}

		private static RootCommand CreateCommands()
		{
			var rootCommand = new RootCommand("Harvest API Command Line Interface")
			{
				CreateClientCommand(),
				CreateProjectCommand(),
				CreateMyProjectCommand(),
				CreateTimeEntryCommand(),
				CreateTimerCommand(),
				Me.GetCommand(),

			};

			rootCommand.AddGlobalOption(new Option<bool>(new string[] { "--output-json", "-oj" }, "Output JSON"));

			return rootCommand;
		}

		private static Command CreateClientCommand()
		{
			var cmd = new Command("client", "Client listing (for admins only)") {
				Client_List.GetCommand(),
			};
			cmd.AddAlias("cli");

			cmd.Handler = CommandHandler.Create(() =>
			{
				Console.WriteLine("Specify subcommand");
			});

			return cmd;
		}

		private static Command CreateTimerCommand()
		{
			var cmd = new Command("timer", "Timer commands") {
				Timer_Start.GetCommand(),
				Timer_Continue.GetCommand(),
			};

			cmd.Handler = CommandHandler.Create(() =>
			{
				Console.WriteLine("Specify subcommand");
			});

			return cmd;
		}

		private static Command CreateProjectCommand()
		{
			var cmd = new Command("project", "Project listing") {
				Project_List.GetCommand(),
			};
			cmd.AddAlias("proj");

			cmd.Handler = CommandHandler.Create(() =>
			{
				Console.WriteLine("Specify subcommand");
			});

			return cmd;
		}


		private static Command CreateMyProjectCommand()
		{
			var cmd = new Command("my-project", "My project assignment listing") {
				MyProject_List.GetCommand(),
			};
			cmd.AddAlias("myp");

			cmd.Handler = CommandHandler.Create(() =>
			{
				Console.WriteLine("Specify subcommand");
			});

			return cmd;
		}

		private static Command CreateTimeEntryCommand()
		{
			var cmd = new Command("timeentry", "Time Entry listing and manipulation") {
				TimeEntry_Show.GetCommand(),
				TimeEntry_List.GetCommand(),
				TimeEntry_Create.GetCommand(),
				TimeEntry_Update.GetCommand(),
				TimeEntry_Interactive.GetCommand(),
				TimeEntry_Copy.GetCommand(),
			};
			cmd.AddAlias("te");

			cmd.Handler = CommandHandler.Create(() =>
			{
				Console.WriteLine("Specify subcommand");
			});

			return cmd;
		}
	}
}

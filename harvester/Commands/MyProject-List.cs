using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class MyProject_List {
		public static Command GetCommand() {
			var show = new Command("list", "List projects")
			{
				new Option<string>(new string[] {"--search", "-s" }, "Search for substrings")
			};

			show.Handler = CommandHandler.Create<string, bool>(Execute);

			return show;
		}

		private static async Task Execute(string search, bool outputJson) {
			var client = Program.GetHarvestClient();

			var me = await client.GetCurrentUser();

			var resp = await client.GetAllProjectsAssignment(me.Id);

			if (outputJson) {
				Console.WriteLine(JsonConvert.SerializeObject(resp.ProjectAssignments, Formatting.Indented));
				return;
			}

			foreach (var p in resp.ProjectAssignments) {
				if (!string.IsNullOrEmpty(search) &&
					!p.Client.Name.Contains(search, StringComparison.OrdinalIgnoreCase) &&
					!p.Project.Name.Contains(search, StringComparison.OrdinalIgnoreCase)) {
					continue;
				}

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write($"{p.Client.Name}/{p.Client.Id}");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" / ");
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write($"{p.Project.Name}/{p.Project.Id}");
				Console.ResetColor();
				Console.WriteLine();

				foreach (var t in p.TaskAssignments) {
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write(" + ");
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.Write($"{t.Task.Name}/{t.Task.Id}");
					Console.ResetColor();
					Console.WriteLine();
				}
			}
		}
	}
}

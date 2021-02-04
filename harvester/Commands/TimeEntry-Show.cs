using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class TimeEntry_Show {
		public static Command GetCommand() {
			var id = new Option<long>(new string[] { "--id", "-id" }, "Id of time entry");

			var cmd = new Command("show", "Show time entry details")
			{
				id,
			};

			cmd.Handler = CommandHandler.Create<long>(Execute);

			return cmd;
		}

		private static async Task Execute(long id) {
			var client = Program.GetHarvestClient();

			var entry = await client.GetTimeEntry(id);

			Console.WriteLine(JsonConvert.SerializeObject(entry, Formatting.Indented));
		}
		public static void ShowTimeEntry(HarvestApi.Model.TimeEntry e) {
			ShowTimeEntry(e, false);
		}

		public static void ShowTimeEntry(HarvestApi.Model.TimeEntry e, bool includeIds) {
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write($"{e.SpentDate.ToShortDateString()}");
			if (includeIds) {
				Console.Write($"/{e.Id}");
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" / ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(e.Client.Name);
			if (includeIds) {
				Console.Write($"/{e.Client.Id}");
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" / ");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write(e.Project.Name);
			if (includeIds) {
				Console.Write($"/{e.Project.Id}");
			}

			if (e.Task != null) {
				Console.ForegroundColor = ConsoleColor.Magenta;
				if (includeIds) {
					Console.Write($" ({e.Task.Name}/{e.Task.Id})");
				}
				else {
					Console.Write($" ({e.Task.Name})");
				}
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" / ");
			//Console.ForegroundColor = ConsoleColor.Yellow;
			//Console.Write(e.User.Name);
			//Console.ForegroundColor = ConsoleColor.White;
			//Console.Write(" - ");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write(e.Hours + "h");
			if (e.BillableRate.HasValue) {
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(" @ ");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write(e.BillableRate.Value + "eur");
			}
			Console.ForegroundColor = ConsoleColor.White;
			if (e.Notes?.Contains("\n") == true) {
				Console.Write($" - {e.Notes.Substring(0, e.Notes.IndexOf("\n"))}...");
			}
			else {
				Console.Write($" - {e.Notes}");
			}
			Console.ResetColor();
			Console.WriteLine();
		}
	}
}

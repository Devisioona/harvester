using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class Timer_Start {
		public static Command GetCommand() {
			var id = new Option<long>(new string[] { "-id" }, "ID of time entry to base this upon");
			var project = new Option<long>(new string[] { "--projectId", "-p" }, "Project id of entry");
			var task = new Option<long>(new string[] { "--taskId", "-t" }, "Task id of entry");
			var notes = new Option<string>(new string[] { "--notes", "-n" }, "Notes of time entry");
			var interactive = new Option<bool>(new string[] { "--interactive", "-ia" }, "Interactively ask for project and task");

			var cmd = new Command("start", "Start a new timer")
			{
				id,
				project,
				task,
				notes,
				interactive
			};

			cmd.Handler = CommandHandler.Create<long?, long?, long?, string, bool>(Execute);

			return cmd;
		}

		private static async Task Execute(long? id, long? projectId, long? taskId, string notes, bool interactive) {
			var client = Program.GetHarvestClient();

			if (interactive) {
				var me = await client.GetCurrentUser();
				var selected = await TimeEntry_Interactive.SelectProjectAndTask(client, me);
				if (selected == null) {
					Console.WriteLine("Aborting.");
					return;
				}

				projectId = selected.ProjectId;
				taskId = selected.TaskId;
			}

			if (!ValidateInput(id, projectId, taskId)) {
				return;
			}

			TimeSpan finalElapsed = await MeasureTime();

			if (notes == null && interactive) {
				Console.Write("Notes: ");
				notes = Console.ReadLine();
			}

			if (id.HasValue) {
				var originalEntry = await client.GetTimeEntry(id.Value);
				var newEntry = await TimeEntry_Copy.CopyTimeEntry(client, originalEntry, (decimal)finalElapsed.TotalHours, notes);
				TimeEntry_Show.ShowTimeEntry(newEntry);
			}
			else {
				var newEntry = await client.CreateTimeEntry(projectId.Value, taskId.Value, (decimal)finalElapsed.TotalHours, notes);
				TimeEntry_Show.ShowTimeEntry(newEntry);
			}

		}

		public static async Task<TimeSpan> MeasureTime(decimal baseElapsed = 0.0m) {
			DateTime startTime = DateTime.Now;
			if (baseElapsed > 0.0m) {
				startTime = startTime.AddHours(-(double)baseElapsed);
				Console.WriteLine($"Continue work, previously logged {baseElapsed} hours -- press ESC to stop working");
			}
			else {
				Console.WriteLine($"Started at {startTime.ToString("t")} -- press ESC to stop working");
			}

			while (true) {
				await Task.Delay(1000);

				if (Console.KeyAvailable) {
					var key = Console.ReadKey();
					if (key.Key == ConsoleKey.Escape) {
						Console.Write("                                                                      \r");
						break;
					}
				}

				Console.Write("Working for ");
				Console.ForegroundColor = ConsoleColor.Yellow;

				var elapsed = DateTime.Now - startTime;
				PrintElapsed(elapsed);
				Console.Write("                                  \r");
				Console.ResetColor();
			}

			var endtime = DateTime.Now;
			var elapsedTotal = (int)((endtime - startTime).TotalMinutes);

			// round to nearest 15 minutes
			elapsedTotal += 15 - (elapsedTotal % 15);
			Console.Write("YYou worked for ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			var finalElapsed = TimeSpan.FromMinutes(elapsedTotal);
			PrintElapsed(finalElapsed);
			Console.WriteLine();
			return finalElapsed;
		}

		private static bool ValidateInput(long? id, long? projectId, long? taskId) {
			if (id.HasValue == false && (projectId.HasValue == false || taskId.HasValue == false)) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Either --id or both --projectId and --taskId must be specified!");
				Console.ResetColor();
				return false;
			}
			if (id.HasValue == true && (projectId.HasValue == true || taskId.HasValue == true)) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Either --id or both --projectId and --taskId must be specified!");
				Console.ResetColor();
				return false;
			}

			return true;
		}

		private static void PrintElapsed(TimeSpan elapsed) {
			if (elapsed.TotalHours >= 1.0) {
				Console.Write(elapsed.Hours);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(" hour(s) and ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(elapsed.Minutes);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(" minute(s)");
			}
			else {
				Console.Write(elapsed.Minutes);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(" minute(s)");
			}
		}
	}
}

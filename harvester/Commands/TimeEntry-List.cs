using Devisioona.Harvest.CLI.Helpers;
using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class TimeEntry_List {
		public static Command GetCommand() {
			var timerange = new Option<string>(new string[] { "--timerange", "-t" }, "Time range to use (today, thisweek, lastweek, thismonth, lastmonth)");
			var includeIds = new Option<bool>(new string[] { "--include-ids", "-iid" }, () => false, "Include IDs in printout");
			var search = new Option<string>(new string[] { "--search", "-s" }, "Search for substring in client/project/task/notes");
			var summary = new Option<bool>(new string[] { "--summary" }, "Summarize the hours for listed entries");

			var cmd = new Command("list", "List (search) time entries")
			{
				timerange,
				includeIds,
				search,
				summary,
			};
			cmd.AddAlias("ls");

			cmd.Handler = CommandHandler.Create<string, bool, bool, bool, string>(Execute);

			return cmd;
		}

		private static async Task Execute(string timerange, bool summary, bool includeIds, bool outputJson, string search) {
			var client = Program.GetHarvestClient();

			var me = await client.GetCurrentUser();

			DateTime from = DateTime.Today;
			DateTime until = DateTime.Today;

			if (timerange != null) {
				TimeRangeHelper.GetTimerangeFromString(timerange, out from, out until);
			}

			var entries = await client.GetTimeEntriesForUser(me.Id, from, until);

			var filtered =
				entries.TimeEntries;

			if (string.IsNullOrEmpty(search) == false) {
				filtered = filtered
					.Where(e => e.Project.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
							 || e.Client.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
							 || e.Task.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
							 || e.Notes.Contains(search, StringComparison.OrdinalIgnoreCase))
					.ToArray();
			}

			if (outputJson) {
				Console.WriteLine(JsonConvert.SerializeObject(filtered, Formatting.Indented));
			}
			else {
				decimal hours = 0.0m;
				foreach (var e in filtered) {
					TimeEntry_Show.ShowTimeEntry(e, includeIds);
					hours += e.Hours;
				}

				if (summary) {
					Console.WriteLine();
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write("Total: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write(hours.ToString());
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write(" hours");
					Console.ResetColor();
					Console.WriteLine();
				}
			}
		}
	}
}

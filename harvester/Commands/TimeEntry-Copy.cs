using Devisioona.Harvest.CLI.HarvestApi.Model;
using Devisioona.Harvest.CLI.Helpers;
using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class TimeEntry_Copy {
		public static Command GetCommand() {
			var id = new Option<long>(new string[] { "-id" }, "ID of time entry");
			var timerange = new Option<string>(new string[] { "--timerange", "-t" }, "Time range to use (today, thisweek, lastweek, thismonth, lastmonth)");
			var search = new Option<string>(new string[] { "--search", "-s" }, "Search for substring in client/project/task/notes");
			var hour = new Option<decimal>(new string[] { "--hours", "-h" }, "Override duration (hours) of copied entry");
			var notes = new Option<string>(new string[] { "--notes", "-n" }, "Override notes of copied time entry");

			var cmd = new Command("copy", "Copy time entries")
			{
				id,
				timerange,
				hour,
				search,
				notes,
			};
			cmd.AddAlias("cp");

			cmd.Handler = CommandHandler.Create<long?, decimal?, string, string, string>(Execute);

			return cmd;
		}

		private static async Task Execute(long? id, decimal? hours, string timerange, string search, string notes) {
			var client = Program.GetHarvestClient();

			DateTime from = DateTime.Today;
			DateTime until = DateTime.Today;

			if (timerange != null) {
				TimeRangeHelper.GetTimerangeFromString(timerange, out from, out until);
			}
			else {
				TimeRangeHelper.GetTimerangeFromString("last30days", out from, out until);
			}

			TimeEntry entryToCopy = null;
			if (id.HasValue) {
				entryToCopy = await client.GetTimeEntry(id.Value);
			}
			else {
				var me = await client.GetCurrentUser();
				var entries = await client.GetTimeEntriesForUser(me.Id, from, until);

				var filtered =
					entries.TimeEntries;

				if (string.IsNullOrEmpty(search) == false) {
					filtered = filtered
						.Where(e => e.Project.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
								 || e.Client.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
								 || e.Task.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
								 || (e.Notes != null && e.Notes.Contains(search, StringComparison.OrdinalIgnoreCase)))
						.ToArray();
				}

				filtered = filtered.ToArray();
				if (filtered.Length > 1) {
					entryToCopy = SelectionHelper.Select(filtered, TimeEntry_Show.ShowTimeEntry);
					if (entryToCopy == null) {
						Console.WriteLine("Aborting.");
						return;
					}
				}
				else {
					entryToCopy = filtered[0];
				}
			}

			TimeEntry newEntry = await CopyTimeEntry(client, entryToCopy, hours);

			TimeEntry_Show.ShowTimeEntry(newEntry);

		}

		public static async Task<TimeEntry> CopyTimeEntry(HarvestApi.HarvestClient client, TimeEntry entryToCopy, decimal? hour, string notes = null) {
			return await client.CreateTimeEntry(
				entryToCopy.Project.Id,
				entryToCopy.Task.Id,
				hour.HasValue ? hour.Value : entryToCopy.Hours,
				notes != null ? notes : entryToCopy.Notes);
		}
	}
}

using Devisioona.Harvest.CLI.HarvestApi.Model;
using Devisioona.Harvest.CLI.Helpers;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class Timer_Continue {
		public static Command GetCommand() {
			var id = new Option<long>(new string[] { "-id" }, "Id of time entry to continue");

			var cmd = new Command("continue", "Continue working on a task")
			{
				id,
			};

			cmd.Handler = CommandHandler.Create<long?>(Execute);

			return cmd;
		}

		private static async Task Execute(long? id) {
			var client = Program.GetHarvestClient();

			TimeEntry originalEntry;
			if (id.HasValue) {
				originalEntry = await client.GetTimeEntry(id.Value);
			}
			else {
				var me = await client.GetCurrentUser();
				DateTime from, until;
				TimeRangeHelper.GetTimerangeFromString("today", out from, out until);
				var todaysEntries = await client.GetTimeEntriesForUser(me.Id, from, until);
				originalEntry = SelectionHelper.Select(todaysEntries.TimeEntries, TimeEntry_Show.ShowTimeEntry);
				if (originalEntry == null) {
					Console.WriteLine("Aborting.");
					return;
				}
			}

			TimeSpan finalElapsed = await Timer_Start.MeasureTime(originalEntry.Hours);

			var updatedEntry = await client.UpdateTimeEntry(originalEntry.Id, null, null, (decimal)finalElapsed.TotalHours, null);
			TimeEntry_Show.ShowTimeEntry(updatedEntry);
		}
	}
}

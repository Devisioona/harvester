using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class TimeEntry_Update {
		public static Command GetCommand() {
			var id = new Option<string>(new string[] { "--id", "-id" }, "ID of time entry") { IsRequired = true };
			var notes = new Option<string>(new string[] { "--notes", "-n" }, "Notes of time entry");
			var project = new Option<long>(new string[] { "--projectId", "-p" }, "Project id of entry");
			var task = new Option<long>(new string[] { "--taskId", "-t" }, "Task id of entry");
			var hour = new Option<decimal>(new string[] { "--hours", "-h" }, "Duration (hours) of entry (1.75)");

			var cmd = new Command("update", "Update an existing time entry")
			{
				id,
				notes,
				project,
				task,
				hour
			};

			cmd.Handler = CommandHandler.Create<int, long?, long?, decimal?, string>(Execute);

			return cmd;
		}

		private static async Task Execute(int id, long? projectId, long? taskId, decimal? hours, string notes) {
			var client = Program.GetHarvestClient();

			var entry = await client.UpdateTimeEntry(id, projectId, taskId, hours, notes);

			TimeEntry_Show.ShowTimeEntry(entry);
		}

	}
}

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class TimeEntry_Create {
		public static Command GetCommand() {
			var project = new Option<long>(new string[] { "--projectId", "-p" }, "Project id of entry") { IsRequired = true };
			var task = new Option<long>(new string[] { "--taskId", "-t" }, "Task id of entry") { IsRequired = true };
			var hour = new Option<decimal>(new string[] { "--hours", "-h" }, "Duration (hours) of entry") { IsRequired = true };
			var notes = new Option<string>(new string[] { "--notes", "-n" }, "Notes of time entry");

			var cmd = new Command("create", "Create a new time entry")
			{
				notes,
				project,
				task,
				hour
			};

			cmd.Handler = CommandHandler.Create<long, long, decimal, string>(Execute);

			return cmd;
		}

		private static async Task Execute(long projectId, long taskId, decimal hours, string notes) {
			var client = Program.GetHarvestClient();

			var entry = await client.CreateTimeEntry(projectId, taskId, hours, notes);

			TimeEntry_Show.ShowTimeEntry(entry);
		}
	}
}

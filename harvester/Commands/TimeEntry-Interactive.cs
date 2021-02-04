using Devisioona.Harvest.CLI.Helpers;
using System;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Commands {
	public static class TimeEntry_Interactive {
		public static Command GetCommand() {
			var cmd = new Command("interactive", "Create new entry interactively") {
			};
			cmd.AddAlias("ia");

			cmd.Handler = CommandHandler.Create<long?, decimal?, string, string>(Execute);

			return cmd;
		}

		private static async Task Execute(long? id, decimal? hour, string timerange, string search) {
			var client = Program.GetHarvestClient();

			var me = await client.GetCurrentUser();

			var selected = await SelectProjectAndTask(client, me);
			if (selected == null) {
				Console.WriteLine("Aborting.");
				return;
			}

			decimal hours;
			while (true) {
				Console.Write("Hours: ");
				var hoursString = Console.ReadLine();
				if (decimal.TryParse(hoursString, out hours)) {
					break;
				}
			}

			Console.Write("Notes: ");
			string notes = Console.ReadLine();

			var entry = await client.CreateTimeEntry(selected.ProjectId, selected.TaskId, hours, notes);
			TimeEntry_Show.ShowTimeEntry(entry);
		}

		public static async Task<InteractiveProjectAndTaskSelectionResult> SelectProjectAndTask(
			HarvestApi.HarvestClient client,
			HarvestApi.Model.User me) {
			var myProjects = await client.GetAllProjectsAssignment(me.Id);

			var allClients = myProjects.ProjectAssignments
				.Select(pa => pa.Client)
				.Distinct()
				.OrderBy(c => c.Name)
				.ToArray();

			var selectedClient = SelectionHelper.Select(allClients, c => {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write(c.Name);
				Console.ResetColor();
				Console.WriteLine();
			});
			if (selectedClient == null) {
				Console.WriteLine("Aborting.");
				return null;
			}

			var projects = myProjects.ProjectAssignments
				.Where(pa => pa.Client.Id == selectedClient.Id)
				.Select(pa => pa.Project)
				.Distinct()
				.OrderBy(p => p.Name)
				.ToArray();
			var selectedProject = SelectionHelper.Select(projects, p => {
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(p.Name);
				Console.ResetColor();
				Console.WriteLine();
			});
			if (selectedProject == null) {
				return null;
			}

			var tasks = myProjects.ProjectAssignments
				.Where(pa => pa.Project.Id == selectedProject.Id)
				.SelectMany(pa => pa.TaskAssignments)
				.Distinct()
				.OrderBy(ta => ta.Task.Name)
				.ToArray();

			var selectedTask = SelectionHelper.Select(tasks, t => {
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.Write(t.Task.Name);
				Console.ResetColor();
				Console.WriteLine();
			})?.Task;
			if (selectedTask == null) {
				Console.WriteLine("Aborting.");
				return null;
			}

			return new InteractiveProjectAndTaskSelectionResult(selectedProject.Id, selectedTask.Id);
		}
	}

	public record InteractiveProjectAndTaskSelectionResult(long ProjectId, long TaskId);
}

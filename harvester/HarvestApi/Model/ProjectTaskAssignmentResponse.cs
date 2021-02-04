using Newtonsoft.Json;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class ProjectTaskAssignmentResponse : HarvestPagedResponse
	{
		[JsonProperty("task_assignments")]
		public ProjectTaskAssignment[] TaskAssignments { get; set; }
	}
}

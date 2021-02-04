using Newtonsoft.Json;
using System;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class ProjectAssignmentResponse : HarvestPagedResponse
	{
		[JsonProperty("project_assignments")]
		public ProjectAssignment[] ProjectAssignments { get; set; }
	}
}

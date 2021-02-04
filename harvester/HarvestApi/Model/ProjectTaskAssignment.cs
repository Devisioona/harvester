using Newtonsoft.Json;
using System;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class ProjectTaskAssignment : IEquatable<ProjectTaskAssignment>
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("project")]
		public ProjectTaskAssignmentProject Project { get; set; }

		[JsonProperty("task")]
		public ProjectTaskAssignmentTask Task { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("is_billable")]
		public bool IsBillable { get; set; }

		[JsonProperty("hourly_rate")]
		public decimal? HourlyRate { get; set; }

		[JsonProperty("budget")]
		public decimal? Budget { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTime UpdatedAt { get; set; }

		public bool Equals(ProjectTaskAssignment other)
		{
			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}

	public class ProjectTaskAssignmentProject
	{
		[JsonProperty("id")]
		public long Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
	}

	public class ProjectTaskAssignmentTask
	{
		[JsonProperty("id")]
		public long Id { get; set; }
		public string Name { get; set; }
	}
}

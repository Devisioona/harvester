using Newtonsoft.Json;
using System;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class ProjectAssignment
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("is_project_manager")]
		public bool IsProjectManager { get; set; }

		public ProjectAssignmentProject Project { get; set; }

		public ProjectAssignmentClient Client { get; set; }

		[JsonProperty("hourly_rate")]
		public decimal? HourlyRate { get; set; }

		[JsonProperty("budget")]
		public decimal? Budget { get; set; }

		[JsonProperty("task_assignments")]
		public ProjectTaskAssignment[] TaskAssignments { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTime UpdatedAt { get; set; }
	}

	public class ProjectAssignmentProject : IEquatable<ProjectAssignmentProject>
	{
		[JsonProperty("id")]
		public long Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }

		[JsonProperty("is_billable")]
		public bool IsBillable { get; set; }

		public bool Equals(ProjectAssignmentProject other)
		{
			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}

	public class ProjectAssignmentClient : IEquatable<ProjectAssignmentClient>
	{
		[JsonProperty("id")]
		public long Id { get; set; }
		public string Name { get; set; }
		public string Currency { get; set; }

		public bool Equals(ProjectAssignmentClient other)
		{
			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}		
	}
}

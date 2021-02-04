using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	[DebuggerDisplay("{Name}")]
	public class Project
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("client")]
		public ProjectClient Client { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("is_billable")]
		public bool IsBillable { get; set; }

		[JsonProperty("is_fixed_fee")]
		public bool IsFixedFee { get; set; }

		[JsonProperty("hourly_rate")]
		public decimal? HourlyRate { get; set; }

		[JsonProperty("budget")]
		public decimal? Budget { get; set; }

		[JsonProperty("fee")]
		public decimal? Fee { get; set; }

		[JsonProperty("notes")]
		public string Notes { get; set; }

		[JsonProperty("starts_on")]
		public DateTime? StartsOn { get; set; }

		[JsonProperty("ends_on")]
		public DateTime? EndsOn { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTime UpdatedAt { get; set; }

	}

	[DebuggerDisplay("{Name}")]
	public class ProjectClient
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}
}

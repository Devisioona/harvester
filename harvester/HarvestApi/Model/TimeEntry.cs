using Newtonsoft.Json;
using System;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class TimeEntry
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("notes")]
		public string Notes { get; set; }

		public TimeEntryClient Client { get; set; }

		public TimeEntryProject Project { get; set; }

		public TimeEntryTask Task { get; set; }

		public TimeEntryUser User { get; set; }

		[JsonProperty("spent_date")]
		public DateTime SpentDate { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTime UpdatedAt { get; set; }

		[JsonProperty("started_time")]
		public string StartedTime { get; set; }

		[JsonProperty("ended_time")]
		public string EndedTime { get; set; }

		public decimal Hours { get; set; }

		[JsonProperty("billable_rate")]
		public decimal? BillableRate { get; set; }

		[JsonProperty("cost_rate")]
		public decimal? CostRate { get; set; }

		[JsonProperty("is_locked")]
		public bool IsLocked { get; set; }

		[JsonProperty("is_billed")]
		public bool IsBilled { get; set; }

		[JsonProperty("is_closed")]
		public bool IsClosed { get; set; }

		[JsonProperty("is_running")]
		public bool IsRunning { get; set; }

		[JsonProperty("billable")]
		public bool IsBillable { get; set; }

		[JsonProperty("budgeted")]
		public bool IsBudgeted { get; set; }
	}

	public class TimeEntryUser
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}

	public class TimeEntryClient
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}

	public class TimeEntryProject
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}

	public class TimeEntryTask
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}

}

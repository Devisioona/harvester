using Newtonsoft.Json;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class TimeEntriesResponse : HarvestPagedResponse
	{
		[JsonProperty("time_entries")]
		public TimeEntry[] TimeEntries { get; set; }
	}
}

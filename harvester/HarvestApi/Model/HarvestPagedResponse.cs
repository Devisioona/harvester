using Newtonsoft.Json;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public abstract class HarvestPagedResponse
	{
		[JsonProperty("per_page")]
		public int PerPage { get; set; }

		[JsonProperty("total_pages")]
		public int TotalPages { get; set; }

		[JsonProperty("total_entries")]
		public int TotalEntries { get; set; }

		[JsonProperty("next_page")]
		public int? NextPage { get; set; }

		[JsonProperty("previous_page")]
		public int? PrevPage { get; set; }

		[JsonProperty("page")]
		public int Page { get; set; }
	}
}

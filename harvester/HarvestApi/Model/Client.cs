using Newtonsoft.Json;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class Client
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }
	}
}

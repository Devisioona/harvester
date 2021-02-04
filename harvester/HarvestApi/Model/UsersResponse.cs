using Newtonsoft.Json;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class UsersResponse : HarvestPagedResponse
	{
		[JsonProperty("users")]
		public User[] Users { get; set; }
	}
}

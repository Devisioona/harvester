using Newtonsoft.Json;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class ClientsResponse : HarvestPagedResponse
	{
		public Client[] Clients { get; set; }
	}
}

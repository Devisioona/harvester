using Newtonsoft.Json;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class ProjectsResponse : HarvestPagedResponse
	{
		[JsonProperty("projects")]
		public Project[] Projects { get; set; }
	}
}

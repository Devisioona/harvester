using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.HarvestApi.Model
{
	public class User
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		public string Email { get; set; }

		public string Timezone { get; set; }

		[JsonProperty("is_admin")]
		public bool IsAdmin { get; set; }
		
		[JsonProperty("is_project_manager")]
		public bool IsProjectManager { get; set; }
		
		[JsonProperty("is_contractor")]
		public bool IsContractor { get; set; }
		
		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("roles")]
		public string[] Roles { get; set; }

		[JsonProperty("weekly_capacity")]
		public int WeeklyCapacity { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTime UpdatedAt { get; set; }
	}
}

using Devisioona.Harvest.CLI.Configuration;
using Devisioona.Harvest.CLI.HarvestApi.Model;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.HarvestApi
{
	public class HarvestClient
	{
		private readonly HarvestConfiguration config;

		private const string HarvestApiUrl = "https://api.harvestapp.com/api/v2/";

		public HarvestClient(HarvestConfiguration configuration)
		{
			config = configuration;
		}

		internal Task<User> GetCurrentUser()
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment("users/me");

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<User>();
		}

		public Task<ClientsResponse> GetAllClients(int? page = null)
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment("clients");

			if (page.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("page", page.Value);
			}

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<ClientsResponse>();
		}

		public Task<Client> GetClient(long clientId)
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment($"clients/{clientId}");

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<Client>();
		}

		public Task<ProjectTaskAssignmentResponse> GetTasksForProject(long projectId, int? page)
		{
			var baseUrl = HarvestApiUrl
							.AppendPathSegment("projects")
							.AppendPathSegment(projectId.ToString())
							.AppendPathSegment("task_assignments");

			if (page.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("page", page.Value);
			}

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<ProjectTaskAssignmentResponse>();

		}

		private IFlurlRequest CreateRequest(Url url)
		{
			return url.WithHeaders(new
			{
				Authorization = $"Bearer {config.APIToken}",
				Harvest_Account_ID = config.AccountID,
				User_Agent = "Devisioona.Harvest.CLI"
			});
		}

		public Task<TimeEntriesResponse> GetTimeEntriesForUser(
			long userId,
			DateTime? startDate = null,
			DateTime? endDate = null,
			int? page = null,
			int? per_page = null)
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment("time_entries")
				.SetQueryParam("user_id", userId);
			if ( per_page.HasValue && per_page.Value > 1 && per_page.Value <= 100)
			{
				baseUrl = baseUrl.SetQueryParam("per_page", per_page.Value);
			}

			if (startDate.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("from", startDate.Value.ToString("yyyy-MM-dd"));
			}

			if (endDate.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("to", endDate.Value.ToString("yyyy-MM-dd"));
			}

			if (page.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("page", page.Value);
			}

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<TimeEntriesResponse>();
		}

		public Task<TimeEntry> GetTimeEntry(
			long entryId)
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment($"time_entries/{entryId}");

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<TimeEntry>();
		}

		public class UpdateTimeEntryObject
		{
			[JsonProperty("notes", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
			public string Notes { get; set; }

			[JsonProperty("hours", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
			public decimal? Hours { get; set; }

			[JsonProperty("project_id", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
			public long? ProjectId { get; set; }

			[JsonProperty("task_id", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
			public long? TaskId { get; set; }
		}

		public class CreateTimeEntryObject : UpdateTimeEntryObject
		{
			[JsonProperty("spent_date", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
			public string SpentDate { get; set; }

			public CreateTimeEntryObject()
			{
				SpentDate = DateTime.Today.ToString("yyyy-MM-dd");
			}
		}
		public async Task<TimeEntry> UpdateTimeEntry(long entryId, long? projectId, long? taskId, decimal? hours, string notes)
		{
			var baseUrl = HarvestApiUrl
							.AppendPathSegment($"time_entries/{entryId}")
							;
			var request = CreateRequest(baseUrl)
				.WithHeader("Content-Type", "application/json");

			var updateObject = new UpdateTimeEntryObject
			{
				Notes = notes,
				ProjectId = projectId,
				TaskId = taskId,
				Hours = hours,

			};

			var response = await request.PatchJsonAsync(updateObject);
			return await response.GetJsonAsync<TimeEntry>();
		}

		public async Task<TimeEntry> CreateTimeEntry(long projectId, long taskId, decimal hours, string notes)
		{
			var baseUrl = HarvestApiUrl
							.AppendPathSegment($"time_entries");
			var request = CreateRequest(baseUrl)
				.WithHeader("Content-Type", "application/json");

			var updateObject = new CreateTimeEntryObject
			{
				Notes = notes,
				ProjectId = projectId,
				TaskId = taskId,
				Hours = hours,
			};

			var response = await request.PostJsonAsync(updateObject);
			return await response.GetJsonAsync<TimeEntry>();
		}

		public Task<TimeEntriesResponse> GetTimeEntriesForClient(
			long clientId,
			DateTime? startDate = null,
			DateTime? endDate = null,
			DateTime? updatedSince = null,
			int? page = null)
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment("time_entries")
				.SetQueryParam("client_id", clientId);

			if (startDate.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("from", startDate.Value.ToString("yyyy-MM-dd"));
			}

			if (endDate.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("to", endDate.Value.ToString("yyyy-MM-dd"));
			}

			if (updatedSince.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("updated_since", endDate.Value.ToString("yyyy-MM-dd"));
			}

			if (page.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("page", page.Value);
			}

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<TimeEntriesResponse>();
		}

		public Task<TimeEntriesResponse> GetTimeEntriesForProject(
			long projectId,
			DateTime? startDate = null,
			DateTime? endDate = null,
			DateTime? updatedSince = null,
			int? page = null)
		{

			var baseUrl = HarvestApiUrl
				.AppendPathSegment("time_entries")
				.SetQueryParam("project_id", projectId);

			if (startDate.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("from", startDate.Value.ToString("yyyy-MM-dd"));
			}

			if (endDate.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("to", endDate.Value.ToString("yyyy-MM-dd"));
			}

			if (updatedSince.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("updated_since", endDate.Value.ToString("yyyy-MM-dd"));
			}

			if (page.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("page", page.Value);
			}

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<TimeEntriesResponse>();
		}

		public Task<ProjectAssignmentResponse> GetAllProjectsAssignment(long userId, int? page = null)
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment($"users/{userId}/project_assignments");

			if (page.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("page", page.Value);
			}

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<ProjectAssignmentResponse>();
		}

		public Task<ProjectsResponse> GetAllProjects(int? page = null, bool activeOnly = true)
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment("projects");

			if (activeOnly)
			{
				baseUrl = baseUrl.SetQueryParam("is_active", "true");
			}

			if (page.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("page", page.Value);
			}

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<ProjectsResponse>();
		}

		public Task<ProjectsResponse> GetProjectsForClient(long clientId, int? page = null)
		{
			var baseUrl = HarvestApiUrl
				.AppendPathSegment("projects")
				.SetQueryParam("client_id", clientId);

			if (page.HasValue)
			{
				baseUrl = baseUrl.SetQueryParam("page", page.Value);
			}

			var request = CreateRequest(baseUrl);

			return request.GetJsonAsync<ProjectsResponse>();
		}
	}
}

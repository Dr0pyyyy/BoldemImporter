using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BoldemImporter
{
	public class TokenManager
	{
		//ClientId and ClientSecret should be stored safely somewhere else, for example in vault
		private string ClientId = "78e2255844044faca45ba4d6cd86df10";
		private string ClientSecret = "e714fb1508c242f6800df06e254bdbaafe2e97341d0c444db58276c3d110bf33";
		private string GetTokenApiUrl = "https://api.boldem.cz/v1/oauth";
		private string RefreshTokenApiUrl = "https://api.boldem.cz/v1/oauth/refresh";
		private string ImportRecordApiUrl = "https://api.boldem.cz/v1/contacts-imports";

		private HttpClient Client { get; set; }
		private Token AccessToken { get; set; }

		public TokenManager()
		{
			Client = new HttpClient();
			AccessToken = GetAccessToken().GetAwaiter().GetResult();
		}

		public async Task<HttpResponseMessage> InsertUsers(List<User> contacts)
		{
			if (AccessToken.valid_to <= DateTime.Now)
				AccessToken = await RefreshToken();

			var requestBody = new
			{
				contacts = contacts,
				mailingListIds = new List<int> { 1391 },
				contactOverwriteMode = 1,
				preImportAction = 1
			};

			var json = JsonConvert.SerializeObject(requestBody);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken.access_token}");

			return await Client.PostAsync(ImportRecordApiUrl, content);
		}

		private async Task<Token> GetAccessToken()
		{
			var requestBody = new
			{
				client_id = ClientId,
				client_secret = ClientSecret
			};
			var json = JsonConvert.SerializeObject(requestBody);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await Client.PostAsync(GetTokenApiUrl, content);
			string responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<Token>(responseContent);
		}

		private async Task<Token> RefreshToken()
		{
			var requestBody = new
			{
				access_token = AccessToken.access_token,
				refresh_token = AccessToken.refresh_token
			};

			var json = JsonConvert.SerializeObject(requestBody);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await Client.PostAsync(RefreshTokenApiUrl, content);
			string responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<Token>(responseContent);
		}
	}
}
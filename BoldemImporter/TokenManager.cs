using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BoldemImporter
{
	public class TokenManager
	{
		//ClientId and ClientSecret should be stored safely somewhere else, for example in vault
		private string clientId = "78e2255844044faca45ba4d6cd86df10";
		private string clientSecret = "e714fb1508c242f6800df06e254bdbaafe2e97341d0c444db58276c3d110bf33";
		private string tokenApi = "https://api.boldem.cz/v1/oauth";

        public Token AccessToken { get; set; }
        
		public TokenManager()
        {
			AccessToken = GetAccessToken().GetAwaiter().GetResult();
		}

		//TODO: Add method RefreshToken()
        private async Task<Token> GetAccessToken()
		{
			using (HttpClient httpClient = new HttpClient())
			{
				var requestData = new
				{
					client_id = clientId,
					client_secret = clientSecret
				};
				var json = JsonConvert.SerializeObject(requestData);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await httpClient.PostAsync(tokenApi, content);
				string responseContent = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<Token>(responseContent);
			}
		}
	}
}
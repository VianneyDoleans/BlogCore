using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlogCoreAPI.FunctionalTests.Models;
using BlogCoreAPI.Models.DTOs.Account;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public class AccountHelper
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;


        public AccountHelper(HttpClient client, string baseUrl = "/account")
        {
            _baseUrl = baseUrl;
            _client = client;
        }

        public async Task<string> GetJwtAccessToken(AccountLoginDto accountLoginDto)
        {
            var json = JsonSerializer.Serialize(accountLoginDto);
            var httpResponse =
                await _client.PostAsync(_baseUrl + "/SignIn", new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var tokenApiResponseJson = await httpResponse.Content.ReadAsStringAsync();
            var tokenApiResponse = JsonSerializer.Deserialize<TokenApiResponse>(tokenApiResponseJson);
            return tokenApiResponse?.AccessToken;
        }
        
        public async Task<GetAccountDto> GetById(int id)
        {
            var httpGetResponse = await _client.GetAsync(_baseUrl + "/" + id);
            httpGetResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpGetResponse.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<GetAccountDto>(stringResponse);
            return entity;
        }

        public async Task<GetAccountDto> CreateAccount(AddAccountDto addAccountDto)
        {
            var json = JsonSerializer.Serialize(addAccountDto);
            var httpResponse =
                await _client.PostAsync(_baseUrl + "/SignUp", new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetAccountDto>(stringResponse);
        }

        public async Task DeleteAccount(int id)
        {
            var httpResponse =
                await _client.DeleteAsync(_baseUrl + "/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task UpdateAccount(UpdateAccountDto entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var httpResponse =
                await _client.PutAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
        }
    }
}

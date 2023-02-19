using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlogCoreAPI.FunctionalTests.Models;
using BlogCoreAPI.Models.DTOs.User;
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

        public async Task<string> GetJwtLoginToken(UserLoginDto userLoginDto)
        {
            var json = JsonSerializer.Serialize(userLoginDto);
            var httpResponse =
                await _client.PostAsync(_baseUrl + "/SignIn", new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var jsonWebTokenDto = JsonSerializer.Deserialize<JsonWebTokenDto>(await httpResponse.Content.ReadAsStringAsync());
            return jsonWebTokenDto?.Token;
        }

        public async Task<GetUserDto> CreateAccount(AddUserDto addUserDto)
        {
            var json = JsonSerializer.Serialize(addUserDto);
            var httpResponse =
                await _client.PostAsync(_baseUrl + "/SignUp", new StringContent(json, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetUserDto>(stringResponse);
        }
    }
}

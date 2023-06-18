using System.Net.Http.Json;
using BusinessLogic.Models;

namespace UnitTests
{
    public class LoginTests
    {
        private readonly HttpClient _httpClient;

        public LoginTests()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5052/");
        }

        [Fact]
        public async Task Test_ValidLogin()
        {
            // Login válido
            var loginModel = new LoginModel
            {
                Username = "johndoe",
                Password = "password123"
            };
            
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5052/api/Auth/token", loginModel);
            
            Assert.True(response.IsSuccessStatusCode);
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            Assert.NotNull(tokenResponse);
            Assert.NotNull(tokenResponse.Token);
        }

        [Fact]
        public async Task Test_InvalidLogin()
        {
            // Login inválido
            var loginModel = new LoginModel
            {
                Username = "invaliduser",
                Password = "invalidpassword"
            };
            
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5052/api/Auth/token", loginModel);

            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
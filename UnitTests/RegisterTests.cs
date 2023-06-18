using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessLogic.Models;
using Xunit;

namespace YourNamespace.Tests
{
    public class RegisterTests
    {
        private readonly HttpClient _httpClient;

        public RegisterTests()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5052/");
        }

        [Fact]
        public async Task Test_InvalidRegistration_PasswordMismatch()
        {
            // Arrange
            var registrationModel = new UserRegistrationModel
            {
                DisplayName = "John Doe",
                Username = "johndoe",
                Password = "invalidpassword"
            };
            // Act
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5052/api/Auth/register", registrationModel);

            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
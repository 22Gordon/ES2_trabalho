using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Xunit;

namespace UnitTests
{
    public class ProfileTests
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<AuthenticationStateProvider> _authenticationStateProviderMock;

        public ProfileTests()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5052/");

            _authenticationStateProviderMock = new Mock<AuthenticationStateProvider>();
            var authenticationState = new AuthenticationState(new ClaimsPrincipal());
            _authenticationStateProviderMock.Setup(m => m.GetAuthenticationStateAsync()).ReturnsAsync(authenticationState);
        }

        [Fact]
        public async Task Test_LoadUserProfile()
        {
            // Mock do perfil do usuário
            var userProfile = new UserEdit
            {
                Displayname = "John Doe",
                Username = "johndoe"
            };

            // Mock da resposta d HTTP para obter o perfil do user
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(userProfile)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var profileService = new ProfileService(httpClient, _authenticationStateProviderMock.Object);

            // Dar load do perfil
            var loadedUserProfile = await profileService.LoadUserProfile();

            // Assert
            Assert.NotNull(loadedUserProfile);
            Assert.Equal(userProfile.Displayname, loadedUserProfile.Displayname);
            Assert.Equal(userProfile.Username, loadedUserProfile.Username);
        }

        [Fact]
        public async Task Test_UpdateProfile_Success()
        {
            // Mock dos dados do perfil a serem atualizados
            var userEditModel = new UserEdit
            {
                Displayname = "New Name",
                Username = "newusername"
            };

            // Mock da resposta do HTTP de atualização do perfil
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var profileService = new ProfileService(httpClient, _authenticationStateProviderMock.Object);

            // Realizar a atualização do perfil
            var updateProfileResult = await profileService.UpdateProfile(userEditModel);

            // Assert
            Assert.True(updateProfileResult);
        }

        [Fact]
        public async Task Test_UpdateProfile_Failure()
        {
            // Mock dos dados do perfil a serem atualizados
            var userEditModel = new UserEdit
            {
                Displayname = "New Name",
                Username = "newusername"
            };

            // Mock da resposta do HTTP de atualização do perfil (errado)
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var profileService = new ProfileService(httpClient, _authenticationStateProviderMock.Object);

            // Realizar a atualização do perfil
            var updateProfileResult = await profileService.UpdateProfile(userEditModel);

            // Assert
            Assert.False(updateProfileResult);
        }
    }

    public class ProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public ProfileService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<UserEdit> LoadUserProfile()
        {
            // Obter o ID do usuário autenticado
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authenticationState.User.FindFirstValue("userId");

            // Fazer solicitação HTTP para obter o perfil do usuário
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/Users/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var userProfile = await response.Content.ReadFromJsonAsync<UserEdit>();
                return userProfile;
            }

            return null;
        }

        public async Task<bool> UpdateProfile(UserEdit userEditModel)
        {
            // Obter o ID do usuário autenticado
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authenticationState.User.FindFirstValue("userId");

            // Fazer solicitação HTTP para atualizar o perfil do usuário
            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5052/api/Users/{userId}", userEditModel);

            return response.IsSuccessStatusCode;
        }
    }
}

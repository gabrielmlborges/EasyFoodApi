using EasyFood.Controllers;
using EasyFood.DTOs.Auth;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EasyFood.Tests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Login_CredenciaisValidas_RetornaOkComToken()
    {
        var request = new LoginRequest("user@test.com", "senha123");
        var expectedResponse = new LoginResponse("token-jwt", DateTime.UtcNow.AddHours(2), "Gabriel", "user@test.com", "Cliente");

        _authServiceMock.Setup(s => s.LoginAsync(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Login(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, ok.Value);
    }

    [Fact]
    public async Task Login_CredenciaisInvalidas_PropagaExcecao()
    {
        var request = new LoginRequest("user@test.com", "senha-errada");

        _authServiceMock.Setup(s => s.LoginAsync(request))
            .ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas."));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _controller.Login(request));
    }
}

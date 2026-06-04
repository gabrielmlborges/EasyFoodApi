using System.Security.Claims;
using EasyFood.Controllers;
using EasyFood.DTOs.Usuarios;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EasyFood.Tests;

public class UsuariosControllerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
    private readonly UsuariosController _controller;

    public UsuariosControllerTests()
    {
        _controller = new UsuariosController(_usuarioServiceMock.Object)
        {
            ControllerContext = CriarContextoAdmin()
        };
    }

    private static ControllerContext CriarContextoAdmin()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    [Fact]
    public async Task Criar_RequestValido_Retorna201ComUsuario()
    {
        var request = new CriarUsuarioRequest("Gabriel", "gabriel@test.com", "senha123");
        var expectedResponse = new UsuarioResponse(1, "Gabriel", "gabriel@test.com", "Cliente", true);

        _usuarioServiceMock.Setup(s => s.CriarAsync(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Criar(request);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, created.StatusCode);
        Assert.Equal(expectedResponse, created.Value);
    }

    [Fact]
    public async Task Criar_EmailDuplicado_PropagaArgumentException()
    {
        var request = new CriarUsuarioRequest("Gabriel", "gabriel@test.com", "senha123");

        _usuarioServiceMock.Setup(s => s.CriarAsync(request))
            .ThrowsAsync(new ArgumentException("Email já cadastrado."));

        await Assert.ThrowsAsync<ArgumentException>(() => _controller.Criar(request));
    }

    [Fact]
    public async Task ObterPorId_UsuarioExistente_RetornaOk()
    {
        var expectedResponse = new UsuarioResponse(1, "Gabriel", "gabriel@test.com", "Admin", true);

        _usuarioServiceMock.Setup(s => s.ObterPorIdAsync(1)).ReturnsAsync(expectedResponse);

        var result = await _controller.ObterPorId(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, ok.Value);
    }

    [Fact]
    public async Task ObterPorId_UsuarioNaoEncontrado_PropagaKeyNotFoundException()
    {
        _usuarioServiceMock.Setup(s => s.ObterPorIdAsync(99))
            .ThrowsAsync(new KeyNotFoundException("Usuário não encontrado."));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.ObterPorId(99));
    }

    [Fact]
    public async Task Listar_RetornaOkComListaDeUsuarios()
    {
        var lista = new List<UsuarioResponse>
        {
            new(1, "Gabriel", "gabriel@test.com", "Admin", true),
            new(2, "Maria", "maria@test.com", "Cliente", true)
        };

        _usuarioServiceMock.Setup(s => s.ListarAsync()).ReturnsAsync(lista);

        var result = await _controller.Listar();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lista, ok.Value);
    }

    [Fact]
    public async Task Desativar_UsuarioExistente_RetornaNoContent()
    {
        _usuarioServiceMock.Setup(s => s.DesativarAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Desativar(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Desativar_UsuarioNaoEncontrado_PropagaKeyNotFoundException()
    {
        _usuarioServiceMock.Setup(s => s.DesativarAsync(99))
            .ThrowsAsync(new KeyNotFoundException("Usuário não encontrado."));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Desativar(99));
    }
}

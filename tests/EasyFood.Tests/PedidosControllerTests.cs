using System.Security.Claims;
using EasyFood.Controllers;
using EasyFood.DTOs.Pedidos;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EasyFood.Tests;

public class PedidosControllerTests
{
    private readonly Mock<IPedidoService> _pedidoServiceMock = new();
    private readonly PedidosController _controller;

    public PedidosControllerTests()
    {
        _controller = new PedidosController(_pedidoServiceMock.Object)
        {
            ControllerContext = CriarContextoCliente(usuarioId: 1)
        };
    }

    private static ControllerContext CriarContextoCliente(int usuarioId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuarioId.ToString()),
            new(ClaimTypes.Role, "Cliente")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    private static ControllerContext CriarContextoAdmin(int usuarioId = 1)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuarioId.ToString()),
            new(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    private static PedidoResponse BuildPedidoResponse(int id = 1, int usuarioId = 1) =>
        new(id, usuarioId, "Pendente", 39.90m, true, DateTime.UtcNow, DateTime.UtcNow,
            [new ItemPedidoResponse(1, "Pizza", 1, 39.90m, 39.90m)]);

    [Fact]
    public async Task Criar_RequestValido_Retorna201ComPedido()
    {
        var request = new CriarPedidoRequest([new ItemPedidoRequest(1, 1)]);
        var expectedResponse = BuildPedidoResponse();

        _pedidoServiceMock.Setup(s => s.CriarAsync(1, request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Criar(request);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, created.StatusCode);
        Assert.Equal(expectedResponse, created.Value);
    }

    [Fact]
    public async Task ObterPorId_PedidoProprioCliente_RetornaOk()
    {
        var expectedResponse = BuildPedidoResponse(id: 1, usuarioId: 1);

        _pedidoServiceMock.Setup(s => s.ObterPorIdAsync(1, 1, false)).ReturnsAsync(expectedResponse);

        var result = await _controller.ObterPorId(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, ok.Value);
    }

    [Fact]
    public async Task ObterPorId_PedidoNaoEncontrado_PropagaKeyNotFoundException()
    {
        _pedidoServiceMock.Setup(s => s.ObterPorIdAsync(99, 1, false))
            .ThrowsAsync(new KeyNotFoundException("Pedido não encontrado."));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.ObterPorId(99));
    }

    [Fact]
    public async Task Listar_RetornaOkComPedidosDoCliente()
    {
        var lista = new List<PedidoResponse> { BuildPedidoResponse() };

        _pedidoServiceMock.Setup(s => s.ListarAsync(1)).ReturnsAsync(lista);

        var result = await _controller.Listar();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lista, ok.Value);
    }

    [Fact]
    public async Task ListarTodos_Admin_RetornaOkComTodosOsPedidos()
    {
        _controller.ControllerContext = CriarContextoAdmin();

        var lista = new List<PedidoResponse>
        {
            BuildPedidoResponse(1, 1),
            BuildPedidoResponse(2, 2)
        };

        _pedidoServiceMock.Setup(s => s.ListarTodosAsync()).ReturnsAsync(lista);

        var result = await _controller.ListarTodos();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lista, ok.Value);
    }

    [Fact]
    public async Task AtualizarStatus_RequestValido_RetornaOkComPedidoAtualizado()
    {
        _controller.ControllerContext = CriarContextoAdmin();

        var request = new AtualizarStatusRequest("Entregue");
        var expectedResponse = BuildPedidoResponse() with { Status = "Entregue" };

        _pedidoServiceMock.Setup(s => s.AtualizarStatusAsync(1, request)).ReturnsAsync(expectedResponse);

        var result = await _controller.AtualizarStatus(1, request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, ok.Value);
    }

    [Fact]
    public async Task AtualizarStatus_PedidoNaoEncontrado_PropagaKeyNotFoundException()
    {
        _controller.ControllerContext = CriarContextoAdmin();

        var request = new AtualizarStatusRequest("Entregue");

        _pedidoServiceMock.Setup(s => s.AtualizarStatusAsync(99, request))
            .ThrowsAsync(new KeyNotFoundException("Pedido não encontrado."));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.AtualizarStatus(99, request));
    }
}

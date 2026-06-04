using EasyFood.Controllers;
using EasyFood.DTOs.Produtos;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EasyFood.Tests;

public class ProdutosControllerTests
{
    private readonly Mock<IProdutoService> _produtoServiceMock = new();
    private readonly ProdutosController _controller;

    public ProdutosControllerTests()
    {
        _controller = new ProdutosController(_produtoServiceMock.Object);
    }

    [Fact]
    public async Task Criar_RequestValido_Retorna201ComProduto()
    {
        var request = new CriarProdutoRequest("Pizza", "Deliciosa", "http://img.com/pizza.jpg", 39.90m);
        var expectedResponse = new ProdutoResponse(1, "Pizza", "Deliciosa", "http://img.com/pizza.jpg", 39.90m, true);

        _produtoServiceMock.Setup(s => s.CriarAsync(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Criar(request);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, created.StatusCode);
        Assert.Equal(expectedResponse, created.Value);
    }

    [Fact]
    public async Task ObterPorId_ProdutoExistente_RetornaOk()
    {
        var expectedResponse = new ProdutoResponse(1, "Pizza", "Deliciosa", "http://img.com/pizza.jpg", 39.90m, true);

        _produtoServiceMock.Setup(s => s.ObterPorIdAsync(1)).ReturnsAsync(expectedResponse);

        var result = await _controller.ObterPorId(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, ok.Value);
    }

    [Fact]
    public async Task ObterPorId_ProdutoNaoEncontrado_PropagaKeyNotFoundException()
    {
        _produtoServiceMock.Setup(s => s.ObterPorIdAsync(99))
            .ThrowsAsync(new KeyNotFoundException("Produto não encontrado."));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.ObterPorId(99));
    }

    [Fact]
    public async Task Listar_RetornaOkComListaDeProdutos()
    {
        var lista = new List<ProdutoResponse>
        {
            new(1, "Pizza", "Deliciosa", "", 39.90m, true),
            new(2, "Hamburguer", "Suculento", "", 25.00m, true)
        };

        _produtoServiceMock.Setup(s => s.ListarAsync(true)).ReturnsAsync(lista);

        var result = await _controller.Listar(apenasAtivos: true);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lista, ok.Value);
    }

    [Fact]
    public async Task Atualizar_RequestValido_RetornaOkComProdutoAtualizado()
    {
        var request = new AtualizarProdutoRequest("Pizza Premium", null, null, 49.90m, null);
        var expectedResponse = new ProdutoResponse(1, "Pizza Premium", "Deliciosa", "", 49.90m, true);

        _produtoServiceMock.Setup(s => s.AtualizarAsync(1, request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Atualizar(1, request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, ok.Value);
    }

    [Fact]
    public async Task Atualizar_ProdutoNaoEncontrado_PropagaKeyNotFoundException()
    {
        var request = new AtualizarProdutoRequest("Nome", null, null, null, null);

        _produtoServiceMock.Setup(s => s.AtualizarAsync(99, request))
            .ThrowsAsync(new KeyNotFoundException("Produto não encontrado."));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Atualizar(99, request));
    }
}

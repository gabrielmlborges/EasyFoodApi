using EasyFood.DTOs.Produtos;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyFood.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutosController(IProdutoService produtoService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoRequest request)
    {
        var response = await produtoService.CriarAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await produtoService.ObterPorIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] bool apenasAtivos = true)
    {
        var response = await produtoService.ListarAsync(apenasAtivos);
        return Ok(response);
    }

    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarProdutoRequest request)
    {
        var response = await produtoService.AtualizarAsync(id, request);
        return Ok(response);
    }
}

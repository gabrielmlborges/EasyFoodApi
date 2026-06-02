using EasyFood.DTOs.Produtos;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyFood.Controllers;

/// <summary>Gestão do catálogo de produtos.</summary>
[ApiController]
[Route("api/produtos")]
public class ProdutosController(IProdutoService produtoService) : ControllerBase
{
    /// <summary>Cria um novo produto. Requer role Admin.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoRequest request)
    {
        var response = await produtoService.CriarAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
    }

    /// <summary>Obtém produto por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await produtoService.ObterPorIdAsync(id);
        return Ok(response);
    }

    /// <summary>Lista produtos. Use apenasAtivos=false para incluir inativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProdutoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] bool apenasAtivos = true)
    {
        var response = await produtoService.ListarAsync(apenasAtivos);
        return Ok(response);
    }

    /// <summary>Atualiza parcialmente um produto. Requer role Admin.</summary>
    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarProdutoRequest request)
    {
        var response = await produtoService.AtualizarAsync(id, request);
        return Ok(response);
    }
}

using System.Security.Claims;
using EasyFood.DTOs.Pedidos;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyFood.Controllers;

/// <summary>Gestão de pedidos.</summary>
[ApiController]
[Route("api/pedidos")]
[Authorize]
public class PedidosController(IPedidoService pedidoService) : ControllerBase
{
    /// <summary>Cria um novo pedido para o usuário autenticado.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Criar([FromBody] CriarPedidoRequest request)
    {
        var usuarioId = ObterUsuarioId();
        var response = await pedidoService.CriarAsync(usuarioId, request);
        return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
    }

    /// <summary>Obtém um pedido por ID. Admin vê qualquer pedido; Cliente vê apenas os seus.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuarioId = ObterUsuarioId();
        var isAdmin = User.IsInRole("Admin");
        var response = await pedidoService.ObterPorIdAsync(id, usuarioId, isAdmin);
        return Ok(response);
    }

    /// <summary>Lista os pedidos do usuário autenticado.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Listar()
    {
        var usuarioId = ObterUsuarioId();
        var response = await pedidoService.ListarAsync(usuarioId);
        return Ok(response);
    }

    /// <summary>Lista todos os pedidos. Requer role Admin.</summary>
    [HttpGet("todos")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ListarTodos()
    {
        var response = await pedidoService.ListarTodosAsync();
        return Ok(response);
    }

    /// <summary>Atualiza o status de um pedido. Requer role Admin.</summary>
    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusRequest request)
    {
        var response = await pedidoService.AtualizarStatusAsync(id, request);
        return Ok(response);
    }

    private int ObterUsuarioId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("Token inválido."));
}

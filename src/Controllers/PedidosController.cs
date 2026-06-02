using System.Security.Claims;
using EasyFood.DTOs.Pedidos;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyFood.Controllers;

[ApiController]
[Route("api/pedidos")]
[Authorize]
public class PedidosController(IPedidoService pedidoService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPedidoRequest request)
    {
        var usuarioId = ObterUsuarioId();
        var response = await pedidoService.CriarAsync(usuarioId, request);
        return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuarioId = ObterUsuarioId();
        var isAdmin = User.IsInRole("Admin");
        var response = await pedidoService.ObterPorIdAsync(id, usuarioId, isAdmin);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var usuarioId = ObterUsuarioId();
        var isAdmin = User.IsInRole("Admin");
        var response = await pedidoService.ListarAsync(usuarioId, isAdmin);
        return Ok(response);
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Admin")]
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

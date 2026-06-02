using EasyFood.DTOs.Usuarios;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyFood.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize]
public class UsuariosController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request)
    {
        var response = await usuarioService.CriarAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await usuarioService.ObterPorIdAsync(id);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Listar()
    {
        var response = await usuarioService.ListarAsync();
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Desativar(int id)
    {
        await usuarioService.DesativarAsync(id);
        return NoContent();
    }
}

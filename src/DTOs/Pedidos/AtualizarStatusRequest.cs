using System.ComponentModel.DataAnnotations;

namespace EasyFood.DTOs.Pedidos;

public record AtualizarStatusRequest(
    [Required] string Status
);

# Projeto: MinhaAPI

## Stack
- ASP.NET Core 10 (controllers)
- Entity Framework Core com PostGreSQL (via Docker-compose)
- Autenticação via JWT (Bearer)

## Convenções de código
- Nomes de classes em PascalCase, variáveis em camelCase
- Sempre usar record types para DTOs
- Retornar `IActionResult` nas endpoints
- Nunca expor entidades do banco diretamente — usar DTOs
- Erros tratados com middleware global (não try/catch em todo lugar)

## Estrutura de pastas esperada
src/
  Controllers/
  Models/
  DTOs/
  Data/
  Services/

## O que NÃO fazer
- Não usar ViewBag, ViewData, nada de MVC views
- Não usar .NET 6 ou inferior
- Não colocar lógica de negócio em controllers

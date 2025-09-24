using Sprint1CSharp.Application.Common;

namespace Sprint1CSharp.Application.Dtos;

public record PatioDto(int Id, string Nome, string? Endereco, IEnumerable<LinkDto> Links);
public record CreatePatioDto(string Nome, string? Endereco);
public record UpdatePatioDto(string Nome, string? Endereco);
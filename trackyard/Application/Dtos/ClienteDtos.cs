using Sprint1CSharp.Application.Common;

namespace Sprint1CSharp.Application.Dtos{

    public record ClienteDto(int Id, string Nome, string CPF, string Email, string? Endereco, IEnumerable<LinkDto> Links);

    public class CreateClienteDto
    {
        public string Nome { get; set; } = null!;
        public string CPF  { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Endereco { get; set; }
    }

    public class UpdateClienteDto
    {
        public string Nome { get; set; } = null!;
        public string CPF  { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Endereco { get; set; }
    }
}
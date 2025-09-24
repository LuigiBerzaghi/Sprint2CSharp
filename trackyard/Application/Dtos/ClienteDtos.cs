namespace Sprint1CSharp.Application.Dtos{

    public class UpdateClienteDto
    {
        public string Nome { get; set; } = null!;
        public string CPF  { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Endereco { get; set; }
    }
}
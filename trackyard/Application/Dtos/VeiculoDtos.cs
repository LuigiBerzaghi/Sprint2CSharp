using Sprint1CSharp.Application.Common;

namespace Sprint1CSharp.Application.Dtos
{

    public record VeiculoDto(int Id, string Modelo, string Placa, string Cor, string Ano, int ClienteId, IEnumerable<LinkDto> Links);

    public class CreateVeiculoDto
    {
        public string Modelo { get; set; } = null!;
        public string Placa  { get; set; } = null!;
        public string Cor    { get; set; } = null!;
        public string Ano    { get; set; } = null!;
        public int ClienteId { get; set; }
    }

    public class UpdateVeiculoDto
    {
        public string Modelo { get; set; } = null!;
        public string Placa  { get; set; } = null!;
        public string Cor    { get; set; } = null!;
        public string Ano    { get; set; } = null!;
        public int? ClienteId { get; set; }
    }
}

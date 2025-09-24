using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint1CSharp.Models
{
    [Table("VEICULOS")]
    public class Veiculo
    {
        /// <summary>Nome completo do cliente.</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Modelo do veículo.</summary>
        [Required]
        public string? Modelo { get; set; }

        /// <summary>Placa do veículo.</summary>
        [Required]
        public string? Placa { get; set; }

         /// <summary>Cor do veículo.</summary>
        [Required]
        public string? Cor { get; set; }

        /// <summary>Ano do veículo.</summary>
        [Required]
        public string? Ano { get; set; }

        /// <summary>Dono do veículo.</summary>
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
    }
}

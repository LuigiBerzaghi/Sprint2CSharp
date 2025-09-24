using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint1CSharp.Models
{
    [Table("PATIOS")]
    public class Patio
    {
        /// <summary>ID do pátio.</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Nome do pátio.</summary>
        [Required, MaxLength(120)]
        public string? Nome { get; set; }

        /// <summary>Endereço do pátio.</summary>
        [Required, MaxLength(200)]
        public string? Endereco { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint1CSharp.Models
{
    [Table("CLIENTES")]
    public class Cliente
    {
        [Key]
        [BindRequired]              
        [Range(1, int.MaxValue)]
        public int Id { get; set; } 

         /// <summary>Nome completo do cliente.</summary>
        [Required] 
        public string? Nome { get; set; }

        /// <summary>CPF completo do cliente.</summary>
        [Required] 
        public string? CPF  { get; set; }

        /// <summary>Email completo do cliente.</summary>
        [Required, EmailAddress] 
        public string? Email { get; set; }

        /// <summary>Endereço completo do cliente.</summary>
        public string? Endereco { get; set; }
        
        /// <summary>Lista de veículos completa do cliente.</summary>
        public List<Veiculo> Veiculos { get; set; } = new();
    }
}

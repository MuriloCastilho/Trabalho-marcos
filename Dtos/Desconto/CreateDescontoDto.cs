using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Desconto
{
    public class DescontoCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string PrincipioAtivo { get; set; } = string.Empty;

        public bool Promocao { get; set; } = false;

        [Required]
        public float ValorDesconto { get; set; }

        [Required]
        public long MedicamentoId { get; set; }
    }
}

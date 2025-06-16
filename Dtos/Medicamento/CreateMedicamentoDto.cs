using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Medicamento
{
    public class CreateMedicamentoDto
    {
        [Required]
        [StringLength(100)]
        public string PrincipioAtivo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public DateOnly DataValidade { get; set; }

        public float Preco { get; set; }

        [Required]
        public string Quantidade { get; set; } = "0";

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        [Required]
        public long IndustriaId { get; set; }

        [Required]
        public long EstoqueId { get; set; }
    }
}

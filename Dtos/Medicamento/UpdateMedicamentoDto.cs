using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Medicamento
{
    public class UpdateMedicamentoDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PrincipioAtivo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public DateOnly DataValidade { get; set; }

        [Required]
        public string Quantidade { get; set; } = "0";
        public float Preco { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public long IndustriaId { get; set; }

        [Required]
        public long EstoqueId { get; set; }
        public float? ValorDesconto { get; set; } = 0;
        public bool Promocao { get; set; } = false;

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Entities
{
    public class Medicamento
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

        public float Preco { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        [Required]
        public string Quantidade { get; set; }

        public Desconto? Desconto { get; set; }

        [ForeignKey(nameof(Industria))]
        public long IndustriaId { get; set; }
        public Industria Industria { get; set; } = null!;

        [ForeignKey(nameof(Estoque))]
        public long EstoqueId { get; set; }
        public Estoque Estoque { get; set; } = null!;
    }
}

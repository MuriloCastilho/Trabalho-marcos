using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Entities;

public class Desconto
{
    [Required]
    public long Id {  get; set; }

    [Required]
    [StringLength(100)]
    public string PrincipioAtivo { get; set; } = string.Empty;

    public bool Promocao { get; set; } = false;

    [Required]
    public float ValorDesconto { get; set; }

    [ForeignKey(nameof(Medicamento))]
    public long MedicamentoId { get; set; }

    public Medicamento Medicamento { get; set; } = null!;
}

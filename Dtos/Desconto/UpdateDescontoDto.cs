using System.ComponentModel.DataAnnotations;

public class DescontoUpdateDTO
{
    [Required]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string PrincipioAtivo { get; set; } = string.Empty;

    public bool Promocao { get; set; } = false;

    [Required]
    public float ValorDesconto { get; set; }

    [Required]
    public long MedicamentoId { get; set; }
}


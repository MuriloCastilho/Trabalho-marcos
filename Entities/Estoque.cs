using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Estoque
{
    [Required]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [ForeignKey("Filial")]
    public long FilialId { get; set; }

    public Filial Filial { get; set; }
}

using System.ComponentModel.DataAnnotations;

public class Industria
{
    [Required]
    public long Id { get; set; }

    [StringLength(18)]
    public string CNPJ { get; set; }

    [StringLength(100)]
    public string Nome { get; set; }
}

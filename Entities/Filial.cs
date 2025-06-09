using System.ComponentModel.DataAnnotations;

public class Filial
{
    [Required]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    [StringLength(18)]
    public string CNPJ { get; set; }

    [Required]
    [StringLength(100)]
    public string Endereco { get; set; }
}

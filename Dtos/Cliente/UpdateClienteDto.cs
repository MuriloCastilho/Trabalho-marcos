using System.ComponentModel.DataAnnotations;

public class UpdateClienteDto
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    [StringLength(14)]
    public string CPF { get; set; }

    [Required]
    [StringLength(100)]
    public string Email { get; set; }
}

using System.ComponentModel.DataAnnotations;
using WebApplication2.Entities;

public class Cliente
{
    [Key]
    public long Id { get; set; }

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

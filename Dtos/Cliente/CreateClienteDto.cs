using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Cliente
{
    public class CreateClienteDto
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
}

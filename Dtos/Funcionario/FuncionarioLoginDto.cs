using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Funcionario
{
    public class FuncionarioLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}

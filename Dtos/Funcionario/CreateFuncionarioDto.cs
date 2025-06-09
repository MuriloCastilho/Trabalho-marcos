using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Funcionario
{
    public class CreateFuncionarioDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        public string CPF { get; set; }

        [Required]
        public float Salario { get; set; }

        public float? Comissao { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}

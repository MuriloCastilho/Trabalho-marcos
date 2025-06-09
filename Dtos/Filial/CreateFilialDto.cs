using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Filial
{
    public class CreateFilialDto
    {
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
}

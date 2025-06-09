using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Industria
{
    public class UpdateIndustriaDto
    {
        [Required]
        [StringLength(18)]
        public string CNPJ { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
    }
}

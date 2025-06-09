using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.Estoque
{
    public class UpdateEstoqueDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public long FilialId { get; set; }
    }
}

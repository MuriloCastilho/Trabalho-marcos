using System.ComponentModel.DataAnnotations;
using WebApplication4.Utils.Enums;

namespace WebApplication4.Dtos.Venda
{
    public class CreateVendaDto
    {
        [Required]
        [StringLength(100)]
        public string NomeProduto { get; set; } = string.Empty;

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public TipoPagamentoEnum TipoPagamento { get; set; }

        public float ValorTotal { get; set; }

        [Required]
        public long ClienteId { get; set; }

        [Required]
        public long MedicamentoId { get; set; }

        [Required]
        public long FuncionarioId { get; set; }
    }
}

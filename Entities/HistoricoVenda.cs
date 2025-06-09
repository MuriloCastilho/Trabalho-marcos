using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Entities
{
    public class HistoricoVenda
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long VendaId { get; set; }

        [ForeignKey(nameof(VendaId))]
        public Venda Venda { get; set; } = null!;

        [Required]
        public long FuncionarioId { get; set; }

        [ForeignKey(nameof(FuncionarioId))]
        public Funcionario Funcionario { get; set; } = null!;
    }
}

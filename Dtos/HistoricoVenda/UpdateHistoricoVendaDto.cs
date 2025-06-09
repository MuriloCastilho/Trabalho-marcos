using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dtos.HistoricoVenda
{
    public class UpdateHistoricoVendaDto
    {
        [Required]
        public long VendaId { get; set; }

        [Required]
        public long FuncionarioId { get; set; }
    }
}

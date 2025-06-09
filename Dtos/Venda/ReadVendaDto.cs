using WebApplication4.Utils.Enums;

namespace WebApplication4.Dtos.Venda
{
    public class ReadVendaDto
    {
        public long Id { get; set; }

        public string NomeProduto { get; set; } = string.Empty;

        public DateTime Data { get; set; }

        public TipoPagamentoEnum TipoPagamento { get; set; }

        public float ValorTotal { get; set; }

        public long ClienteId { get; set; }
        public string? ClienteNome { get; set; }

        public long MedicamentoId { get; set; }
        public string? MedicamentoNome { get; set; }

        public long FuncionarioId { get; set; }
        public string? FuncionarioNome { get; set; }
    }
}

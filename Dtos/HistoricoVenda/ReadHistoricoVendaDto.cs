namespace WebApplication4.Dtos.HistoricoVenda
{
    public class ReadHistoricoVendaDto
    {
        public long Id { get; set; }
        public long VendaId { get; set; }
        public long FuncionarioId { get; set; }
        public string? FuncionarioNome { get; set; }
    }
}

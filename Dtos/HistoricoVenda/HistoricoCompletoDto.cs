namespace WebApplication4.Dtos.HistoricoVenda
{
    public class HistoricoCompletoDto
    {
        public long VendaId { get; set; }
        public DateTime DataVenda { get; set; }
        public float ValorTotal { get; set; }
        public string NomeMedicamento { get; set; }
        public string PrincipioAtivo { get; set; }
        public string CPFCliente { get; set; }
        public string NomeFuncionario { get; set; }
    }
}

namespace WebApplication4.Dtos.Desconto
{
    public class ReadDescontoDto
    {
        public long Id { get; set; }

        public string PrincipioAtivo { get; set; } = string.Empty;

        public bool Promocao { get; set; }

        public float ValorDesconto { get; set; }

        public long MedicamentoId { get; set; }

        public string? NomeMedicamento { get; set; }
    }
}

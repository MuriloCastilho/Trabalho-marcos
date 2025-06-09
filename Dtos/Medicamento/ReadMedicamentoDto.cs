namespace WebApplication4.Dtos.Medicamento
{
    public class ReadMedicamentoDto
    {
        public long Id { get; set; }

        public string PrincipioAtivo { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public DateOnly DataValidade { get; set; }

        public float Preco { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public string Quantidade { get; set; } = "0";

        public long IndustriaId { get; set; }

        public string? NomeIndustria { get; set; }

        public long EstoqueId { get; set; }

        public string? NomeFilial { get; set; }

        public float? ValorDesconto { get; set; }
        public bool Promocao { get; set; }

    }
}

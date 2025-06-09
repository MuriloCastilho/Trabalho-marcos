namespace WebApplication4.Dtos.Estoque
{
    public class ReadEstoqueDto
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public long FilialId { get; set; }

        public string? NomeFilial { get; set; }
    }
}

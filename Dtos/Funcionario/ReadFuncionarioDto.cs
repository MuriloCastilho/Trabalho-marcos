namespace WebApplication4.Dtos.Funcionario
{
    public class ReadFuncionarioDto
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public string CPF { get; set; }

        public float Salario { get; set; }

        public float? Comissao { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication4.Dtos.Venda;
using WebApplication4.Utils.Enums;
using AutoMapper;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendaMedicamentoController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;
        private readonly IMapper _mapper;

        public VendaMedicamentoController(AbarateiraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("realizar-venda")]
        public async Task<IActionResult> RealizarVenda([FromBody] RealizarVendaDto dto)
        {
            var medicamento = await _context.Medicamentos
        .Include(m => m.Estoque)
        .Include(m => m.Desconto)
        .FirstOrDefaultAsync(m => m.Id == dto.MedicamentoId);

            if (medicamento == null)
                return BadRequest("Medicamento não encontrado.");

            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null)
                return BadRequest("Cliente não encontrado.");

            if (!int.TryParse(medicamento.Quantidade, out int quantidadeAtual))
                return BadRequest("Quantidade atual do medicamento inválida.");

            if (dto.QuantidadeVendida <= 0)
                return BadRequest("Quantidade vendida deve ser maior que zero.");

            if (dto.QuantidadeVendida > quantidadeAtual)
                return BadRequest($"Estoque insuficiente. Quantidade disponível: {quantidadeAtual}");

            int novaQuantidade = quantidadeAtual - dto.QuantidadeVendida;
            medicamento.Quantidade = novaQuantidade.ToString();

            float precoUnitario = medicamento.Preco;
            float descontoAplicado = 0;

            if (medicamento.Desconto != null && medicamento.Desconto.Promocao)
            {
                descontoAplicado = medicamento.Desconto.ValorDesconto;
                precoUnitario -= descontoAplicado;
                if (precoUnitario < 0) precoUnitario = 0;
            }

            float valorTotal = precoUnitario * dto.QuantidadeVendida;

            var venda = new Venda
            {
                NomeProduto = medicamento.Nome,
                ClienteId = dto.ClienteId,
                MedicamentoId = dto.MedicamentoId,
                TipoPagamento = (TipoPagamentoEnum)dto.TipoPagamento,
                Data = DateTime.Now,
                ValorTotal = valorTotal,
                FuncionarioId = dto.FuncionarioId
            };

            _context.Vendas.Add(venda);

            await _context.SaveChangesAsync();

            var historico = new WebApplication4.Entities.HistoricoVenda
            {
                VendaId = venda.Id,
                FuncionarioId = dto.FuncionarioId
            };
            _context.HistoricoVendas.Add(historico);

            await _context.SaveChangesAsync();

            var readVenda = _mapper.Map<ReadVendaDto>(venda);

            var resposta = new
            {
                sucesso = true,
                venda = readVenda,
                quantidadeRestante = novaQuantidade,
                precoUnitario = precoUnitario,
                descontoAplicado = descontoAplicado,
                valorTotal = valorTotal,
                alerta = novaQuantidade < 10 ? "Estoque baixo: menos de 10 unidades restantes." : null
            };

            return Ok(resposta);
        }
    }

    public class RealizarVendaDto
    {
        public long ClienteId { get; set; }
        public long MedicamentoId { get; set; }
        public int QuantidadeVendida { get; set; }
        public int TipoPagamento { get; set; }
        public long FuncionarioId { get; set; }
    }
}

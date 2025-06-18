using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication4.Dtos.HistoricoVenda;
using WebApplication4.Entities;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricoVendaController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;
        private readonly IMapper _mapper;

        public HistoricoVendaController(AbarateiraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadHistoricoVendaDto>>> GetHistoricoVendas()
        {
            var historicos = await _context.HistoricoVendas
                .Include(hv => hv.Venda)
                .Include(hv => hv.Funcionario)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ReadHistoricoVendaDto>>(historicos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadHistoricoVendaDto>> GetHistoricoVenda(long id)
        {
            var historico = await _context.HistoricoVendas
                .Include(hv => hv.Venda)
                .Include(hv => hv.Funcionario)
                .FirstOrDefaultAsync(hv => hv.Id == id);

            if (historico == null)
                return NotFound();

            return Ok(_mapper.Map<ReadHistoricoVendaDto>(historico));
        }

        [HttpPost]
        public async Task<ActionResult<ReadHistoricoVendaDto>> PostHistoricoVenda(CreateHistoricoVendaDto dto)
        {
            var vendaExiste = await _context.Vendas.AnyAsync(v => v.Id == dto.VendaId);
            var funcionarioExiste = await _context.Funcionarios.AnyAsync(f => f.Id == dto.FuncionarioId);
            if (!vendaExiste || !funcionarioExiste)
                return BadRequest("Venda ou Funcionário inválido.");

            var historico = _mapper.Map<HistoricoVenda>(dto);

            _context.HistoricoVendas.Add(historico);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<ReadHistoricoVendaDto>(historico);
            return CreatedAtAction(nameof(GetHistoricoVenda), new { id = historico.Id }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoricoVenda(long id, UpdateHistoricoVendaDto dto)
        {
            var historico = await _context.HistoricoVendas.FindAsync(id);
            if (historico == null)
                return NotFound();

            _mapper.Map(dto, historico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoricoVenda(long id)
        {
            var historico = await _context.HistoricoVendas.FindAsync(id);
            if (historico == null)
                return NotFound();

            _context.HistoricoVendas.Remove(historico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("completo")]
        public async Task<ActionResult<IEnumerable<HistoricoCompletoDto>>> GetHistoricoCompleto()
        {
            var historicos = await _context.HistoricoVendas
                .Include(h => h.Venda)
                    .ThenInclude(v => v.Medicamento)
                .Include(h => h.Venda)
                    .ThenInclude(v => v.Cliente)
                .Include(h => h.Funcionario)
                .ToListAsync();

            var result = historicos.Select(h => new HistoricoCompletoDto
            {
                VendaId = h.VendaId,
                DataVenda = h.Venda.Data,
                ValorTotal = h.Venda.ValorTotal,
                NomeMedicamento = h.Venda.Medicamento.Nome,
                PrincipioAtivo = h.Venda.Medicamento.PrincipioAtivo,
                CPFCliente = h.Venda.Cliente.CPF,
                NomeFuncionario = h.Funcionario.Nome
            });

            return Ok(result);
        }

        [HttpGet("completo/{id}")]
        public async Task<ActionResult<IEnumerable<HistoricoCompletoDto>>> GetHistoricoCompleto(long id)
        {
            var historico = await _context.HistoricoVendas
            .Include(h => h.Venda)
                .ThenInclude(v => v.Medicamento)
            .Include(h => h.Venda)
                .ThenInclude(v => v.Cliente)
            .Include(h => h.Funcionario)
            .FirstOrDefaultAsync(h => h.Id == id);

            if (historico == null)
                return NotFound("Histórico de venda não encontrado.");

            var result = new HistoricoCompletoDto
            {
                VendaId = historico.VendaId,
                DataVenda = historico.Venda.Data,
                ValorTotal = historico.Venda.ValorTotal,
                NomeMedicamento = historico.Venda.Medicamento.Nome,
                PrincipioAtivo = historico.Venda.Medicamento.PrincipioAtivo,
                CPFCliente = historico.Venda.Cliente.CPF,
                NomeFuncionario = historico.Funcionario.Nome
            };

            return Ok(result);
        }
    }
}

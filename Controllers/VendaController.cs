using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication4.Dtos.Venda;
using WebApplication4.Entities;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;
        private readonly IMapper _mapper;

        public VendaController(AbarateiraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadVendaDto>>> GetVendas()
        {
            var vendas = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Medicamento)
                .ToListAsync();

            var readDtos = _mapper.Map<List<ReadVendaDto>>(vendas);
            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadVendaDto>> GetVenda(long id)
        {
            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Medicamento)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null)
                return NotFound();

            var readDto = _mapper.Map<ReadVendaDto>(venda);
            return Ok(readDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReadVendaDto>> PostVenda(CreateVendaDto createDto)
        {
            // Validação de entidades
            var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == createDto.ClienteId);
            var medicamentoExists = await _context.Medicamentos.AnyAsync(m => m.Id == createDto.MedicamentoId);
            var funcionario = await _context.Funcionarios.FindAsync(createDto.FuncionarioId);

            if (!clienteExists || !medicamentoExists || funcionario == null)
                return BadRequest("Cliente, Medicamento ou Funcionário inválido.");

            // Mapeamento e comissão
            var venda = _mapper.Map<Venda>(createDto);
            float comissao = createDto.ValorTotal * 0.10f;
            funcionario.Comissao = (funcionario.Comissao ?? 0) + comissao;

            // Persistência da venda
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            // Histórico de venda
            var historico = new HistoricoVenda
            {
                FuncionarioId = funcionario.Id,
                VendaId = venda.Id
            };
            _context.HistoricoVendas.Add(historico);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<ReadVendaDto>(venda);
            return CreatedAtAction(nameof(GetVenda), new { id = venda.Id }, readDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenda(long id, UpdateVendaDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
                return NotFound();

            _mapper.Map(updateDto, venda);
            _context.Entry(venda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenda(long id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
                return NotFound();

            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendaExists(long id)
        {
            return _context.Vendas.Any(v => v.Id == id);
        }
    }
}
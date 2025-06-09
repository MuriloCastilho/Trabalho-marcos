using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Entities;
using WebApplication4.Dtos.Medicamento;
using AutoMapper;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicamentoController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;
        private readonly IMapper _mapper;

        public MedicamentoController(AbarateiraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadMedicamentoDto>>> GetMedicamentos()
        {
            var medicamentos = await _context.Medicamentos
                .Include(m => m.Desconto)
                .Include(m => m.Industria)
                .Include(m => m.Estoque)
                    .ThenInclude(e => e.Filial)
                .ToListAsync();

            return Ok(_mapper.Map<List<ReadMedicamentoDto>>(medicamentos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadMedicamentoDto>> GetMedicamento(long id)
        {
            var medicamento = await _context.Medicamentos
                .Include(m => m.Desconto)
                .Include(m => m.Industria)
                .Include(m => m.Estoque)
                    .ThenInclude(e => e.Filial)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicamento == null)
                return NotFound();

            return Ok(_mapper.Map<ReadMedicamentoDto>(medicamento));
        }

        [HttpPost]
        public async Task<ActionResult<ReadMedicamentoDto>> PostMedicamento(CreateMedicamentoDto dto)
        {
            var industriaExists = await _context.Industrias.AnyAsync(i => i.Id == dto.IndustriaId);
            var estoqueExists = await _context.Estoques.AnyAsync(e => e.Id == dto.EstoqueId);

            if (!industriaExists || !estoqueExists)
                return BadRequest("Industria ou Estoque inválido.");

            var medicamento = _mapper.Map<Medicamento>(dto);
            _context.Medicamentos.Add(medicamento);
            await _context.SaveChangesAsync();

            // Cria desconto com base no medicamento
            var desconto = new Desconto
            {
                MedicamentoId = medicamento.Id,
                PrincipioAtivo = medicamento.PrincipioAtivo,
                Promocao = dto.Promocao,
                ValorDesconto = dto.ValorDesconto ?? 0
            };
            _context.Descontos.Add(desconto);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<ReadMedicamentoDto>(medicamento);
            readDto.ValorDesconto = desconto.ValorDesconto;
            readDto.Promocao = desconto.Promocao;

            return CreatedAtAction(nameof(GetMedicamento), new { id = medicamento.Id }, readDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicamento(long id, UpdateMedicamentoDto dto)
        {
            var medicamento = await _context.Medicamentos
                .Include(m => m.Desconto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicamento == null)
                return NotFound();

            _mapper.Map(dto, medicamento);

            // Atualizar desconto
            if (medicamento.Desconto != null)
            {
                medicamento.Desconto.ValorDesconto = dto.ValorDesconto ?? medicamento.Desconto.ValorDesconto;
                medicamento.Desconto.Promocao = dto.Promocao;
                medicamento.Desconto.PrincipioAtivo = medicamento.PrincipioAtivo;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicamentoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicamento(long id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
                return NotFound();

            _context.Medicamentos.Remove(medicamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("buscar-por-principio")]
        public async Task<ActionResult<IEnumerable<ReadMedicamentoDto>>> BuscarPorPrincipioAtivo([FromQuery] string principioAtivo,
                                                                                                 [FromQuery] long? estoqueId)
        {
            var query = _context.Medicamentos
                .Include(m => m.Industria)
                .Include(m => m.Estoque).ThenInclude(e => e.Filial)
                .Include(m => m.Desconto)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(principioAtivo))
                query = query.Where(m => m.PrincipioAtivo.Contains(principioAtivo));

            if (estoqueId.HasValue)
                query = query.Where(m => m.EstoqueId == estoqueId.Value);

            var medicamentos = await query.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ReadMedicamentoDto>>(medicamentos));
        }

        [HttpGet("cliente-busca")]
        public async Task<ActionResult<IEnumerable<ReadMedicamentoDto>>> ClienteBuscarMedicamentos([FromQuery] string termo,
                                                                                                   [FromQuery] long? estoqueId)
        {
            var query = _context.Medicamentos
                .Include(m => m.Industria)
                .Include(m => m.Estoque).ThenInclude(e => e.Filial)
                .Include(m => m.Desconto)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(termo))
            {
                query = query.Where(m =>
                    m.PrincipioAtivo.Contains(termo) ||
                    m.Nome.Contains(termo));
            }

            if (estoqueId.HasValue)
                query = query.Where(m => m.EstoqueId == estoqueId.Value);

            var medicamentos = await query.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ReadMedicamentoDto>>(medicamentos));
        }

        private bool MedicamentoExists(long id)
        {
            return _context.Medicamentos.Any(m => m.Id == id);
        }
    }
}

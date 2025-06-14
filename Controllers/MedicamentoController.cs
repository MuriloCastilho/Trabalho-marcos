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

            dto.PrincipioAtivo = dto.PrincipioAtivo?.ToUpper();

            var medicamento = _mapper.Map<Medicamento>(dto);
            _context.Medicamentos.Add(medicamento);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<ReadMedicamentoDto>(medicamento);
            return CreatedAtAction(nameof(GetMedicamento), new { id = medicamento.Id }, readDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicamento(long id, UpdateMedicamentoDto dto)
        {
            var medicamento = await _context.Medicamentos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicamento == null)
                return NotFound();

            dto.PrincipioAtivo = dto.PrincipioAtivo?.ToUpper();

            _mapper.Map(dto, medicamento);

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

        [HttpGet("principios-ativos-unicos")]
        public async Task<ActionResult<IEnumerable<string>>> GetPrincipiosAtivosUnicos()
        {
            var principiosAtivos = await _context.Medicamentos
                .Select(m => m.PrincipioAtivo.ToUpper())
                .Distinct()
                .ToListAsync();

            return Ok(principiosAtivos);
        }

        [HttpGet("filtrar-por-principio")]
        public async Task<ActionResult<IEnumerable<string>>> FiltrarMedicamentosPorPrincipio([FromQuery] string principioAtivo)
        {
            if (string.IsNullOrWhiteSpace(principioAtivo))
                return BadRequest("O parâmetro 'principioAtivo' é obrigatório.");

            var medicamentos = await _context.Medicamentos
                .Where(m => m.PrincipioAtivo.ToUpper() == principioAtivo.ToUpper())
                .Select(m => m.Nome)
                .ToListAsync();

            return Ok(medicamentos);
        }


        private bool MedicamentoExists(long id)
        {
            return _context.Medicamentos.Any(m => m.Id == id);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication4.Dtos.Desconto;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescontoController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;
        private readonly IMapper _mapper;

        public DescontoController(AbarateiraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadDescontoDto>>> GetDescontos()
        {
            var descontos = await _context.Descontos
                .Include(d => d.Medicamento)
                .ToListAsync();

            var descontosDto = _mapper.Map<List<ReadDescontoDto>>(descontos);
            return Ok(descontosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadDescontoDto>> GetDesconto(long id)
        {
            var desconto = await _context.Descontos
                .Include(d => d.Medicamento)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (desconto == null)
                return NotFound();

            var descontoDto = _mapper.Map<ReadDescontoDto>(desconto);
            return Ok(descontoDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReadDescontoDto>> PostDesconto(DescontoCreateDTO createDto)
        {
            bool jaExiste = await _context.Descontos
                .AnyAsync(d => d.PrincipioAtivo == createDto.PrincipioAtivo);

            if (jaExiste)
                return BadRequest("Já existe um desconto para este princípio ativo.");

            var desconto = _mapper.Map<Desconto>(createDto);

            _context.Descontos.Add(desconto);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<ReadDescontoDto>(desconto);

            return CreatedAtAction(nameof(GetDesconto), new { id = desconto.Id }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesconto(long id, DescontoUpdateDTO updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var desconto = await _context.Descontos.FindAsync(id);
            if (desconto == null)
                return NotFound();

            // Verifica se outro desconto já existe com o mesmo principio ativo
            bool conflito = await _context.Descontos
                .AnyAsync(d => d.PrincipioAtivo == updateDto.PrincipioAtivo && d.Id != id);

            if (conflito)
                return BadRequest("Já existe outro desconto para este princípio ativo.");

            _mapper.Map(updateDto, desconto);

            _context.Entry(desconto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DescontoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesconto(long id)
        {
            var desconto = await _context.Descontos.FindAsync(id);
            if (desconto == null)
                return NotFound();

            _context.Descontos.Remove(desconto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DescontoExists(long id)
        {
            return _context.Descontos.Any(d => d.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication4.Dtos.Filial; // supondo que seus DTOs estejam aqui
using AutoMapper;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilialController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;
        private readonly IMapper _mapper;

        public FilialController(AbarateiraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Filial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadFilialDto>>> GetFiliais()
        {
            var filiais = await _context.Filiais.ToListAsync();
            var filiaisDto = _mapper.Map<List<ReadFilialDto>>(filiais);
            return Ok(filiaisDto);
        }

        // GET: api/Filial/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadFilialDto>> GetFilial(long id)
        {
            var filial = await _context.Filiais.FindAsync(id);

            if (filial == null)
                return NotFound();

            var filialDto = _mapper.Map<ReadFilialDto>(filial);
            return Ok(filialDto);
        }

        // POST: api/Filial
        [HttpPost]
        public async Task<ActionResult<ReadFilialDto>> PostFilial(CreateFilialDto dto)
        {
            var filial = _mapper.Map<Filial>(dto);

            _context.Filiais.Add(filial);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<ReadFilialDto>(filial);

            return CreatedAtAction(nameof(GetFilial), new { id = filial.Id }, readDto);
        }

        // PUT: api/Filial/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilial(long id, UpdateFilialDto dto)
        {
            if (!FilialExists(id))
                return NotFound();

            var filial = await _context.Filiais.FindAsync(id);
            if (filial == null)
                return NotFound();

            _mapper.Map(dto, filial);

            _context.Entry(filial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilialExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Filial/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilial(long id)
        {
            var filial = await _context.Filiais.FindAsync(id);
            if (filial == null)
                return NotFound();

            _context.Filiais.Remove(filial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FilialExists(long id)
        {
            return _context.Filiais.Any(f => f.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustriaController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;

        public IndustriaController(AbarateiraDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Industria>>> GetIndustrias()
        {
            return await _context.Industrias.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Industria>> GetIndustria(long id)
        {
            var industria = await _context.Industrias.FindAsync(id);

            if (industria == null)
                return NotFound();

            return industria;
        }

        [HttpPost]
        public async Task<ActionResult<Industria>> PostIndustria(Industria industria)
        {
            _context.Industrias.Add(industria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIndustria), new { id = industria.Id }, industria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndustria(long id, Industria industria)
        {
            if (id != industria.Id)
                return BadRequest();

            _context.Entry(industria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndustriaExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIndustria(long id)
        {
            var industria = await _context.Industrias.FindAsync(id);
            if (industria == null)
                return NotFound();

            _context.Industrias.Remove(industria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IndustriaExists(long id)
        {
            return _context.Industrias.Any(e => e.Id == id);
        }
    }
}

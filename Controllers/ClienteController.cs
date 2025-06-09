using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication4.Dtos.Cliente;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;
        private readonly IMapper _mapper;

        public ClienteController(AbarateiraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadClienteDto>>> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            var clientesDto = _mapper.Map<List<ReadClienteDto>>(clientes);
            return Ok(clientesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadClienteDto>> GetCliente(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            var dto = _mapper.Map<ReadClienteDto>(cliente);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ReadClienteDto>> PostCliente([FromBody] CreateClienteDto dto)
        {
            var cliente = _mapper.Map<Cliente>(dto);

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<ReadClienteDto>(cliente);

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(long id, [FromBody] UpdateClienteDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            _mapper.Map(dto, cliente);

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(long id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}

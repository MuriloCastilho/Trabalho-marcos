using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Entities;
using WebApplication4.Dtos.Funcionario;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;
        private readonly IMapper _mapper;

        public FuncionarioController(AbarateiraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadFuncionarioDto>>> GetFuncionarios()
        {
            var funcionarios = await _context.Funcionarios
                .Include(f => f.Vendas)
                .ToListAsync();

            var funcionariosDto = _mapper.Map<List<ReadFuncionarioDto>>(funcionarios);
            return Ok(funcionariosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadFuncionarioDto>> GetFuncionario(long id)
        {
            var funcionario = await _context.Funcionarios
                .Include(f => f.Vendas)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (funcionario == null)
                return NotFound();

            var funcionarioDto = _mapper.Map<ReadFuncionarioDto>(funcionario);
            return Ok(funcionarioDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReadFuncionarioDto>> PostFuncionario(CreateFuncionarioDto createDto)
        {
            // Exemplo de validação extra: CPF único
            var cpfExiste = await _context.Funcionarios.AnyAsync(f => f.CPF == createDto.CPF);
            if (cpfExiste)
                return BadRequest("CPF já cadastrado para outro funcionário.");

            var funcionario = _mapper.Map<Funcionario>(createDto);

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<ReadFuncionarioDto>(funcionario);

            return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.Id }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuncionario(long id, UpdateFuncionarioDto updateDto)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
                return NotFound();

            // Validar se o CPF não está duplicado em outro registro
            var cpfDuplicado = await _context.Funcionarios
                .AnyAsync(f => f.CPF == updateDto.CPF && f.Id != id);
            if (cpfDuplicado)
                return BadRequest("CPF já cadastrado para outro funcionário.");

            _mapper.Map(updateDto, funcionario);
            _context.Entry(funcionario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(long id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
                return NotFound();

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FuncionarioExists(long id)
        {
            return _context.Funcionarios.Any(f => f.Id == id);
        }
    }
}

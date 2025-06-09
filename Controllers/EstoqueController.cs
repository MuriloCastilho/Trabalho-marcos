using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication4.Dtos.Estoque;

[ApiController]
[Route("api/[controller]")]
public class EstoqueController : ControllerBase
{
    private readonly AbarateiraDbContext _context;
    private readonly IMapper _mapper;

    public EstoqueController(AbarateiraDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadEstoqueDto>>> GetEstoques()
    {
        var estoques = await _context.Estoques
            .Include(e => e.Filial)
            .ToListAsync();

        return Ok(_mapper.Map<List<ReadEstoqueDto>>(estoques));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReadEstoqueDto>> GetEstoque(long id)
    {
        var estoque = await _context.Estoques
            .Include(e => e.Filial)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (estoque == null)
            return NotFound();

        return Ok(_mapper.Map<ReadEstoqueDto>(estoque));
    }

    [HttpPost]
    public async Task<ActionResult<ReadEstoqueDto>> PostEstoque(CreateEstoqueDto dto)
    {
        var estoque = _mapper.Map<Estoque>(dto);
        _context.Estoques.Add(estoque);
        await _context.SaveChangesAsync();

        var readDto = _mapper.Map<ReadEstoqueDto>(estoque);
        return CreatedAtAction(nameof(GetEstoque), new { id = estoque.Id }, readDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEstoque(long id, UpdateEstoqueDto dto)
    {
        var estoque = await _context.Estoques.FindAsync(id);
        if (estoque == null)
            return NotFound();

        _mapper.Map(dto, estoque);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEstoque(long id)
    {
        var estoque = await _context.Estoques.FindAsync(id);
        if (estoque == null)
            return NotFound();

        _context.Estoques.Remove(estoque);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

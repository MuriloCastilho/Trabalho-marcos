using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication4.Dtos.Funcionario;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AbarateiraDbContext _context;

        public AuthController(AbarateiraDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] FuncionarioLoginDto loginDto)
        {
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Email == loginDto.Email);

            if (funcionario == null)
                return NotFound("Funcionário não encontrado.");

            if (funcionario.Senha != loginDto.Senha)
                return Unauthorized("Senha incorreta.");

            // Aqui você pode gerar token JWT ou só retornar dados do usuário.
            return Ok(new
            {
                funcionario = new { funcionario.Id, funcionario.Nome, funcionario.Email },
                mensagem = "Login realizado com sucesso!"
            });
        }
    }
}

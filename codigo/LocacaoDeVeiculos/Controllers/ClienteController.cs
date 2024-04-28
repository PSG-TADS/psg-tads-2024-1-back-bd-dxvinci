using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocacaoDeVeiculos.Data;
using LocacaoDeVeiculos.Models;

namespace LocacaoDeVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ClienteController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        // Recupera uma lista de todos os clientes cadastrados
        // Retorna um objeto JSON com a lista de clientes (200)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        // Recupera um cliente específico pelo ID
        // Retorna um objeto JSON com o cliente encontrado (200)
        // ou um erro 404 caso o cliente não seja encontrado
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Clientes/5
        // Atualiza um cliente específico pelo ID
        // Retorna NoContent (204) caso o cliente seja atualizado com sucesso
        // ou um erro 400 caso o ID do cliente não corresponda ao ID informado
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.ID)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Clientes
        // Cria um novo cliente
        // Retorna um objeto JSON com o cliente cadastrado (201)
        // ou um erro 400 caso o cliente não seja válido
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.ID }, cliente);
        }

        // DELETE: api/Clientes/5
        // Deleta um cliente específico pelo ID
        // Retorna NoContent (204) caso o cliente seja deletado com sucesso
        // ou um erro 404 caso o cliente não seja encontrado
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica se um cliente existe pelo ID
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ID == id);
        }
    }
}

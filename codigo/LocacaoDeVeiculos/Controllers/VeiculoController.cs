using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocacaoDeVeiculos.Data;
using LocacaoDeVeiculos.Models;
using System.Text.RegularExpressions;

namespace LocacaoDeVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public VeiculoController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Veiculos
        // Recupera uma lista de todos os veículos cadastrados
        // Retorna um objeto JSON com a lista de veículos (200)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            return await _context.Veiculos.ToListAsync();
        }

        // GET: api/Veiculos/5
        // Recupera um veículo específico pelo ID
        // Retorna um objeto JSON com o veículo encontrado (200)
        // ou um erro 404 caso o veículo não seja encontrado
        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
            {
                return NotFound();
            }

            return veiculo;
        }

        // PUT: api/Veiculos/5
        // Atualiza um veículo específico pelo ID
        // Retorna NoContent (204) caso o veículo seja atualizado com sucesso
        // ou um erro 400 caso o ID do veículo não corresponda ao ID informado
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.ID)
            {
                return BadRequest();
            }

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VeiculoExists(id))
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

        // POST: api/Veiculos
        // Cadastra um novo veículo
        // Retorna um objeto JSON com o veículo cadastrado (201)
        // ou um erro 400 caso o modelo do veículo não seja válido
        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await VeiculoExistsPorPlaca(veiculo.Placa))
            {
                return BadRequest("Já existe um veículo com essa placa cadastrado.");
            }

            if (!Regex.IsMatch(veiculo.Placa, @"^[A-Z]{3}-\d{4}$"))
            {
                return BadRequest("O formato da placa está incorreto.");
            }

            if (veiculo.Valor_Diaria <= 0)
            {
                return BadRequest("O valor da diária deve ser maior que zero.");
            }

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVeiculo", new { id = veiculo.ID }, veiculo);
        }

        // DELETE: api/Veiculos/5
        // Deleta um veículo específico pelo ID
        // Retorna NoContent (204) caso o veículo seja deletado com sucesso
        // ou um erro 404 caso o veículo não seja encontrado
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica se já existe um veículo cadastrado com o ID informado
        private async Task<bool> VeiculoExists(int id)
        {
            return await _context.Veiculos.AnyAsync(e => e.ID == id);
        }

        // Verifica se já existe um veículo cadastrado com a placa informada
        private async Task<bool> VeiculoExistsPorPlaca(string placa)
        {
            return await _context.Veiculos.AnyAsync(v => v.Placa == placa);
        }
    }
}

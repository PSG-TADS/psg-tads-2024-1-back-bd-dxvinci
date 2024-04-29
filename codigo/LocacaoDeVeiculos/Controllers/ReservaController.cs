using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocacaoDeVeiculos.Data;
using LocacaoDeVeiculos.Models;

namespace LocacaoDeVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ReservaController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Reservas
        // Recupera uma lista de todas as reservas cadastradas
        // Retorna um objeto JSON com a lista de reservas (200)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas.ToListAsync();
        }

        // GET: api/Reservas/5
        // Recupera uma reserva específica pelo ID
        // Retorna um objeto JSON com a reserva encontrada (200)
        // ou um erro 404 caso a reserva não seja encontrada
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }

        // PUT: api/Reservas/5
        // Atualiza uma reserva específica pelo ID
        // Retorna NoContent (204) caso a reserva seja atualizada com sucesso
        // ou um erro 400 caso o ID da reserva não corresponda ao ID informado
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.ID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.IsDefined(typeof(StatusReserva), reserva.Status))
            {
                return BadRequest("Status de reserva inválido.");
            }

            try
            {
                _context.Entry(reserva).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        // POST: api/Reservas
        // Cria uma nova reserva
        // Retorna um objeto JSON com a reserva criada (201)
        // ou um erro 400 caso os dados da reserva sejam inválidos
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cliente = await _context.Clientes.FindAsync(reserva.ClienteID);
            var veiculo = await _context.Veiculos.FindAsync(reserva.VeiculoID);

            if (cliente == null || veiculo == null)
            {
                return BadRequest("Cliente ou Veículo não encontrados.");
            }

            if (reserva.Data_Final <= reserva.Data_Inicio)
            {
                return BadRequest("Data fianl da reserva não pode ser anterior à data atual.");
            }

            if (await VeiculoReservado(reserva.VeiculoID, reserva.Data_Inicio, reserva.Data_Final))
            {
                return BadRequest("Veículo indisponível no período selecionado.");
            }

            if (!Enum.IsDefined(typeof(StatusReserva), reserva.Status))
            {
                return BadRequest("Status de reserva inválido.");
            }

            var diasDeLocacao = (reserva.Data_Final - reserva.Data_Inicio).Days + 1;
            reserva.Valor = diasDeLocacao * veiculo.Valor_Diaria;

            var novaReserva = new Reserva
            {
                ClienteID = reserva.ClienteID,
                VeiculoID = reserva.VeiculoID,
                Data_Inicio = reserva.Data_Inicio,
                Data_Final = reserva.Data_Final,
                Valor = reserva.Valor,
                Status = reserva.Status
            };

            _context.Reservas.Add(novaReserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserva", new { id = novaReserva.ID }, novaReserva);
        }

        // DELETE: api/Reservas/5
        // Deleta uma reserva específica pelo ID
        // Retorna NoContent (204) caso a reserva seja deletada com sucesso
        // ou um erro 404 caso a reserva não seja encontrada
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Reservas/cliente/5
        // Recupera uma lista de todas as reservas de um cliente específico
        // Retorna um objeto JSON com a lista de reservas (200)
        // ou um erro 404 caso o cliente não seja encontrado
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasDoUsuario(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                return NotFound("Cliente com o ID informado não encontrado.");
            }

            var reservas = await _context.Reservas
              .Where(r => r.ClienteID == clienteId)
              .ToListAsync();

            if (reservas.Count == 0)
            {
                return NotFound("Nenhuma reserva encontrada para o cliente informado.");
            }

            return reservas;
        }


        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.ID == id);
        }

        // Método para verificar se o veículo já não está reservado em um determinado período
        private async Task<bool> VeiculoReservado(int veiculoID, DateTime dataInicio, DateTime dataFinal)
        {
            return await _context.Reservas.AnyAsync(r =>
                r.VeiculoID == veiculoID &&
                (dataInicio < r.Data_Final && dataFinal > r.Data_Inicio));
        }
    }
}

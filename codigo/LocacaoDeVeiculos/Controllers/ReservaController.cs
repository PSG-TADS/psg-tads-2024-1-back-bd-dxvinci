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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas.ToListAsync();
        }

        // GET: api/Reservas/5
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.ID)
            {
                return BadRequest();
            }

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
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

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.ID == id);
        }
    }
}

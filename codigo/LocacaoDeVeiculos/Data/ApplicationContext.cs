using LocacaoDeVeiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace LocacaoDeVeiculos.Data

{
    public class ApplicationContext : DbContext
    {

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=LocadoraDB;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}

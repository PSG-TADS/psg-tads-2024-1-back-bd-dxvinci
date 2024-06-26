﻿using LocacaoDeVeiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace LocacaoDeVeiculos.Data

{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=LocadoraDB;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reserva>()
                .Property(p => p.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (StatusReserva)Enum.Parse(typeof(StatusReserva), v));
        }
    }
}

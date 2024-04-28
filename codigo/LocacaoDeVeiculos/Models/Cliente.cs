using System.ComponentModel.DataAnnotations;

namespace LocacaoDeVeiculos.Models
{
    public class Cliente
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Endereco { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }
        public ICollection<Reserva> Reservas { get; set; }

        public Cliente()
        {
               Reservas = new List<Reserva>();
        }

    }
}

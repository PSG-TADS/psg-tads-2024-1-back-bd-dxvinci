using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocacaoDeVeiculos.Models
{
    public class Reserva
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int IDCliente { get; set; }

        [Required]
        [ForeignKey("Veiculo")]
        public int IDVeiculo { get; set; }

        [Required]
        public DateTime Data_Inicio { get; set; }

        [Required]
        public DateTime Data_Final { get; set; }

        public decimal Valor { get; set; }

        public string Status { get; set; }

    }
}

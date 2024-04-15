using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocacaoDeVeiculos.Models
{
    public class Reserva
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Cliente")]
        public int IDCliente { get; set; }

        [ForeignKey("Veiculo")]
        public int IDVeiculo { get; set; }

        public DateTime Data_Inicio { get; set; }

        public DateTime Data_Final { get; set; }

        public decimal Valor { get; set; }

        public string Status { get; set; }

    }
}

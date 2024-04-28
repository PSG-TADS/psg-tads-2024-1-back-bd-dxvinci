using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocacaoDeVeiculos.Models
{
    public class Reserva
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Clientes")]
        public int ClienteID { get; set; }

        [Required]
        [ForeignKey("Veiculos")]
        public int VeiculoID { get; set; }

        [Required]
        public DateTime Data_Inicio { get; set; }

        [Required]
        public DateTime Data_Final { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal? Valor { get; set; }

        [EnumDataType(typeof(StatusReserva))]
        [Column(TypeName = "nvarchar(20)")]
        public StatusReserva? Status { get; set; }

    }
}

﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocacaoDeVeiculos.Models
{
    public class Reserva
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteID { get; set; }

        [Required]
        [ForeignKey("Veiculo")]
        public int VeiculoID { get; set; }

        [Required]
        public DateTime Data_Inicio { get; set; }

        [Required]
        public DateTime Data_Final { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Valor { get; set; }

        public StatusReserva Status { get; set; }

    }
}

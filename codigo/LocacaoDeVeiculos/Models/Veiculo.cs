using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocacaoDeVeiculos.Models
{
    public class Veiculo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public string Marca { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Valor_Diaria { get; set; }

    }
}

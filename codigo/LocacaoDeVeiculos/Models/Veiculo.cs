using System.ComponentModel.DataAnnotations;

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
        public decimal Valor_Diaria { get; set; }

    }
}

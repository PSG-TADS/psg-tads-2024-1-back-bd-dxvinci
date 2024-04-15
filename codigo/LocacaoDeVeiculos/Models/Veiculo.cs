using System.ComponentModel.DataAnnotations;

namespace LocacaoDeVeiculos.Models
{
    public class Veiculo
    {
        [Key]
        public int ID { get; set; }

        public string Modelo { get; set; }

        public string Marca { get; set; }

        public string Placa { get; set; }

        public string Tipo { get; set; }

        public decimal Valor_Diaria { get; set; }

    }
}

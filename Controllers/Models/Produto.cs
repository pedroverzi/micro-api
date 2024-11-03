using System.ComponentModel.DataAnnotations;

namespace CRUDCadastroDeProdutos.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Nome { get; set; }

        [Required]
        public decimal PrecoCusto { get; set; }

        [Required]
        public decimal PrecoVenda { get; set; }

        [Required]
        public int Quantidade { get; set; }
    }
}
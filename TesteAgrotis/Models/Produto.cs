using System.ComponentModel.DataAnnotations;

namespace TesteAgrotis.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        //public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal PesoLiquido { get; set; }
        public decimal PrecoUnitario { get; set; }
        public DateTime DtCreate { get; set; }
        public DateTime? DtDeleted { get; set; }
        public bool Ativo { get; set; }
    }
}

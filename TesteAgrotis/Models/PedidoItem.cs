using System.ComponentModel.DataAnnotations;

namespace TesteAgrotis.Models
{
    public class PedidoItem
    {
        [Key]
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public string Quantidade { get; set; }
        public string PesoTotal { get; set; }
        public string ValorTotal { get; set; }

        public DateTime DtCreate { get; set; }
        public DateTime? DtDeleted { get; set; }
        public bool Ativo { get; set; }

        public Pedido Pedido { get; set; }
        public Produto Produto { get; set; }
    }
}
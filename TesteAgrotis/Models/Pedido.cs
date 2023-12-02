using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteAgrotis.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        public DateTime DataEmissao { get; set; }
        public int ClienteId { get; set; }
        public string PrecoTotalPedido { get; set; }
        public string PesoTotalPedido { get; set; }
        public string? Observacao { get; set; }

        public DateTime DtCreate { get; set; }
        public DateTime? DtDeleted { get; set; }
        public bool Ativo { get; set; }
        public Cliente Cliente { get; set; }

        public ICollection<PedidoItem> Itens { get; set; }

    }
}
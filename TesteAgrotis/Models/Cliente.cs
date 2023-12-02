using System.ComponentModel.DataAnnotations;

namespace TesteAgrotis.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CodigoIBGE { get; set; }
        public DateTime DtCreate { get; set; }
        public DateTime? DtDeleted { get; set; }
        public bool Ativo { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using TesteAgrotis.Models;

namespace TesteAgrotis
{
    public class Contexto : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoItem> PedidoItem { get; set; }

        // Construtor que recebe as opções do DbContext
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pedido>()
                .HasOne(pedido => pedido.Cliente)
                .WithMany()
                .HasForeignKey(pedido => pedido.ClienteId);


            modelBuilder.Entity<Pedido>()
                .HasMany(pedido => pedido.Itens) // Um pedido tem muitos itens
                .WithOne(item => item.Pedido) // Um item pertence a um pedido
                .HasForeignKey(item => item.PedidoId); // Chave estrangeira em PedidoItem referenciando Pedido
        }
    }
}

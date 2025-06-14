using Microsoft.EntityFrameworkCore;
using WebApplication2.Entities;
using WebApplication4.Entities;

namespace WebApplication2.Data
{
    public class AbarateiraDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Desconto> Descontos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<HistoricoVenda> HistoricoVendas { get; set; }
        public DbSet<Industria> Industrias { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        public AbarateiraDbContext(DbContextOptions<AbarateiraDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medicamento>()
                .HasAlternateKey(m => m.PrincipioAtivo);

            // Configuração para evitar cascata na tabela HistoricoVenda
            modelBuilder.Entity<HistoricoVenda>()
                .HasOne(hv => hv.Venda)
                .WithMany() // Se tiver coleção, substitua por .WithMany(v => v.HistoricoVendas)
                .HasForeignKey(hv => hv.VendaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoricoVenda>()
                .HasOne(hv => hv.Funcionario)
                .WithMany() // Se tiver coleção, substitua por .WithMany(f => f.HistoricoVendas)
                .HasForeignKey(hv => hv.FuncionarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}

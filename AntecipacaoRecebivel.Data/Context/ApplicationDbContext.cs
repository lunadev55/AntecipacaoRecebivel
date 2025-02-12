using AntecipacaoRecebivel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntecipacaoRecebivel.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<NotaFiscal> NotasFiscais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Empresa>()
                .HasMany(c => c.NotasFiscais)
                .WithOne(i => i.Empresa)
                .HasForeignKey(i => i.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

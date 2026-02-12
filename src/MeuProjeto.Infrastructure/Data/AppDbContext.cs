using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MeuProjeto.Domain.Entities;
using MeuProjeto.Domain.Interfaces;

namespace MeuProjeto.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>, IUnitOfWork
{
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Projeto> Projetos => Set<Projeto>();
    public DbSet<HistoricoStatus> HistoricoStatus => Set<HistoricoStatus>();
    public DbSet<ArquivoProjeto> ArquivosProjeto => Set<ArquivoProjeto>();
    public DbSet<Parcela> Parcelas => Set<Parcela>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public async Task<int> SalvarAsync(CancellationToken ct = default) => await SaveChangesAsync(ct);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Empresa>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Slug).IsUnique();
            e.Property(x => x.Nome).HasMaxLength(200).IsRequired();
            e.Property(x => x.Slug).HasMaxLength(50).IsRequired();
            e.Property(x => x.CorPrimaria).HasMaxLength(10);
            e.Property(x => x.CorSecundaria).HasMaxLength(10);
        });

        builder.Entity<Cliente>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.CodigoAcesso).IsUnique();
            e.Property(x => x.Nome).HasMaxLength(200).IsRequired();
            e.Property(x => x.CodigoAcesso).HasMaxLength(20).IsRequired();
            e.HasMany(x => x.Projetos).WithOne(p => p.Cliente).HasForeignKey(p => p.ClienteId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Projeto>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.CodigoPublico).IsUnique();
            e.Property(x => x.Titulo).HasMaxLength(300).IsRequired();
            e.Property(x => x.CodigoPublico).HasMaxLength(30).IsRequired();
            e.Property(x => x.ValorTotal).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.Empresa).WithMany(emp => emp.Projetos).HasForeignKey(x => x.EmpresaId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.Arquivos).WithOne().HasForeignKey(a => a.ProjetoId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Historico).WithOne().HasForeignKey(h => h.ProjetoId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Parcelas).WithOne().HasForeignKey(p => p.ProjetoId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Parcela>(e =>
        {
            e.Property(x => x.Valor).HasColumnType("decimal(18,2)");
        });

        builder.Entity<ArquivoProjeto>(e =>
        {
            e.Property(x => x.Nome).HasMaxLength(300);
            e.Property(x => x.Url).HasMaxLength(500);
        });

        builder.Entity<HistoricoStatus>(e =>
        {
            e.Property(x => x.Observacao).HasMaxLength(500);
        });
    }
}

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Loja> Lojas { get; set; }
    public DbSet<Segmento> Segmentos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=revsch.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>()
            .HasOne(p => p.Loja)
            .WithMany(l => l.Produtos)
            .HasForeignKey(p => p.LojaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Produto>()
            .HasOne(p => p.Segmento)
            .WithMany(s => s.Produtos)
            .HasForeignKey(p => p.SegmentoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Segmento>()
            .HasOne(s => s.Categoria)
            .WithMany(c => c.Segmentos)
            .HasForeignKey(s => s.CategoriaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Avaliacao>()
            .HasOne(a => a.Produto)
            .WithMany(p => p.Avaliacoes)
            .HasForeignKey(a => a.ProdutoId);
        
        modelBuilder.Entity<Loja>()
            .HasMany(l => l.Produtos)
            .WithOne(p => p.Loja)
            .OnDelete(DeleteBehavior.Cascade);
    }



}
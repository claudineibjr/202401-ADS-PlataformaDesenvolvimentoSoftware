using Microsoft.EntityFrameworkCore;
using Aula16APIFilmes.Models;

namespace Aula16APIFilmes.Database;

public partial class MeusFilmesDbContext : DbContext
{

    public DbSet<Filme> Filmes { get; set; }

    public MeusFilmesDbContext()
    {
    }

    public MeusFilmesDbContext(DbContextOptions<MeusFilmesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

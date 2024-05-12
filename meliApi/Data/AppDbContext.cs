using meliApi.Entidades;
using meliApi.Entidades.meliApi.Entidades;
using Microsoft.EntityFrameworkCore;

namespace meliApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }

        // Agrega un DbSet para la entidad Root que representa la estructura del JSON
        public DbSet<Token> Token { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}

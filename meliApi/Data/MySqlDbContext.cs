using meliApi.Entidades.meliApi.Entidades;
using meliApi.Entidades;
using Microsoft.EntityFrameworkCore;

namespace meliApi.Data
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
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

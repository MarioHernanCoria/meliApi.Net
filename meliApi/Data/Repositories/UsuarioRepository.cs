using meliApi.Data.Repositories.Interfaces;
using meliApi.Entidades;
using meliApi.Entidades.meliApi.Entidades;

namespace meliApi.Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }
    }
}

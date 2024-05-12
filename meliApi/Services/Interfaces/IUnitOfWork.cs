using meliApi.Data.Repositories;

namespace meliApi.Services.Interfaces
{
    public interface IUnitOfWork
    {
        public UsuarioRepository UsuarioRepository { get; }
        Task<int> Complete();
    }
}

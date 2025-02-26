using Lojinha.Domain;

namespace Lojinha.Aplicacao.Interfaces
{
    public interface IUsuarioService
    {
        public List<Usuario> ListUsuario();
        public Task<Usuario> GetUsuario(int id);
        public Task<bool> UpdateUsuario(Usuario usuario);
        public Task<bool> SaveUsuario(Usuario usuario);
        public Task<bool> DeleteUsuario(int id);
    }
}

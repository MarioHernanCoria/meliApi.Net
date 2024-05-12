using meliApi.Entidades.meliApi.Entidades;
using System.ComponentModel.DataAnnotations;

namespace meliApi.Entidades
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int UsuarioId { get; set; }
        public string Scope { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}

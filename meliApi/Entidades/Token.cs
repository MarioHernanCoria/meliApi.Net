using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "datetime")]
        public DateTime ExpirationDate { get; set; }
    }
}

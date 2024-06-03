namespace meliApi.Entidades
{
    public class UsuarioAutorizado
    {
        public string User_Id { get; set; }
        public string App_Id { get; set; }
        public DateTime Date_Created { get; set; }
        public DateTime Date_Updated { get; set; }
        public List<string> Scopes { get; set; }  
    }
}

namespace PracticaS13_WEB.Models
{
    public class PrincipalModel
    {
        public long IdCompra { get; set; }
        public double Precio { get; set; }
        public double Saldo { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
    }
}

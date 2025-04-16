namespace PracticaS13_WEB.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? MensajeError { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

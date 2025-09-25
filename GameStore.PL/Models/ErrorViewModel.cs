namespace GameStore.PL.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrWhiteSpace(RequestId);

        
        public string? Path { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public int? StatusCode { get; set; }

    }

}
                
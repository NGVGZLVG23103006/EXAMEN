using Microsoft.AspNetCore.Mvc.RazorPages;

public class ErrorModelo : PageModel
{
    public string ErrorMessage { get; set; }
    public string RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet()
    {
        RequestId = HttpContext.TraceIdentifier;

        // Recupera el mensaje de error desde el contexto
        ErrorMessage = HttpContext.Items["ErrorMessage"] as string;
    }
}

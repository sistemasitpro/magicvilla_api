using System.Net;

namespace MagicVilla_API.Models.Dto
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            ErrorMensaje = new List<String>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<String>? ErrorMensaje { get; set; }
        public object? Data { get; set; }
        public int TotalPaginas { get; set; } = 0;
    }
}

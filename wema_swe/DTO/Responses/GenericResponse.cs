using System.Text.Json;

namespace wema_swe.DTO.Responses
{
    public class GenericResponse<T> where T : class
    {
        public T Data { get; set; }
        public int HttpStatusCode { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

using LSS.WebApp.MVC.Extensions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LSS.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected StringContent ObterConteudo(object dado)
        {
           return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json"
                );
        }
        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }
        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                    throw new CustomHttpRequestException(response.StatusCode);
                case 403:
                    throw new CustomHttpRequestException(response.StatusCode);
                case 404:
                    throw new CustomHttpRequestException(response.StatusCode);
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);
                case 400:
                    return false;

            }
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}

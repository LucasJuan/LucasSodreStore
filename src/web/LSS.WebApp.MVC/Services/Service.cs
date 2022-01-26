using LSS.WebApp.MVC.Extensions;
using System.Net.Http;

namespace LSS.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                    throw new CustomHttpRequestException(response.StatusCode);
                case 403:

                case 404:

                case 500:
                case 400:
                    return false;

            }
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}

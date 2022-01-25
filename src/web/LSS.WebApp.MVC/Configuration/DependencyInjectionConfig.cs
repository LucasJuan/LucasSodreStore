using LSS.WebApp.MVC.Extensions;
using LSS.WebApp.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace LSS.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
            // singleton pois é acesso ao próprio contexto
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}

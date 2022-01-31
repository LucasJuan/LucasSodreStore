using LSS.WebApp.MVC.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace LSS.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = ObterConteudo(usuarioLogin);

            var response = await _httpClient.PostAsync("https://localhost:44394/api/Identidade/autenticar", loginContent);

        
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            };

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registronContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync("https://localhost:44394/api/Identidade/nova-conta", registronContent);

            // como está usando o system.text.json precisa serializar as options para ficar igual as propriedades

           

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            };

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);

        }
    }
}
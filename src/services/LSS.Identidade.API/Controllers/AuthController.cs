using LSS.Identidade.API.Extensions;
using LSS.Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LSS.Identidade.API.Controllers
{
    [ApiController]
    [Route("api/Identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }
        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
      
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true

            };
            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
            if (result.Succeeded)
            {
                return CustomResponse(GerarJwt(user.Email));
            }

            foreach (var item in result.Errors)
            {
                AdicionarErroProcessamento(item.Description);
            }
            return CustomResponse();
        }
        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
           
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

            if (result.Succeeded) return CustomResponse(await GerarJwt(usuarioLogin.Email));

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuario temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou senha incorretos");
            return CustomResponse();

        }

        private async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            ClaimsIdentity identityClaims = await ObterClaimsUsuario(user, claims);

            string encodedToken = CodificarToken(identityClaims);

            return ObterTokenResposta(user, claims, encodedToken); ;
        }

        private UsuarioRespostaLogin ObterTokenResposta(IdentityUser user, IList<Claim> claims, string encodedToken)
        {
            return new UsuarioRespostaLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token); 
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(IdentityUser user, IList<Claim> claims)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var item in userRoles)
            {
                claims.Add(new Claim("role", item));
            }
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);
            return identityClaims;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}

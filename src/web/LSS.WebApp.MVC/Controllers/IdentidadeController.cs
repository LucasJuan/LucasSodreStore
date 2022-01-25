using LSS.WebApp.MVC.Models;
using LSS.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LSS.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
    {
        private readonly IAutenticacaoService _autenticacaoService;
        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }
        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            var resposta = await _autenticacaoService.Registro(usuarioRegistro);

            if (!ModelState.IsValid) return View(usuarioRegistro);

            //API - Registro

            //if (false) return View(usuarioRegistro);

            // Realizar Login na API
            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
        {

            if (!ModelState.IsValid) return View(usuarioLogin);

            //API - Login
            var resposta = await _autenticacaoService.Login(usuarioLogin);

            //if (false) return View(usuarioLogin);

            // Realizar Login na APP
            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            return RedirectToAction("Index", "Home");
        }
        private async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            var token = ObtenTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.AccessToken));
            // lista dentro da lista
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        private static JwtSecurityToken ObtenTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}

using LSS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSS.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
    {
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
            if (!ModelState.IsValid) return View(usuarioRegistro);

            //API - Registro

            if (false) return View(usuarioRegistro);

            // Realizar Login na API

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

            if (false) return View(usuarioLogin);

            // Realizar Login na API

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}

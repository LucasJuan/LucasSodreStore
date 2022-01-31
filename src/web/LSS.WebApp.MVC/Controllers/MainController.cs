using LSS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSS.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
       protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if(resposta != null && resposta.Errors != null)
            {
                foreach (var item in resposta.Errors.Mensagens)
                {
                    ModelState.AddModelError(string.Empty, item);
                }
                return true;
            }
            return false;
        }
    }
}

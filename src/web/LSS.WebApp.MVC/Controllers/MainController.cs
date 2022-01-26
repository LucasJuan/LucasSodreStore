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
            if(resposta != null && resposta.Errors.Messages.Any())
            {
                return true;
            }
            return false;
        }
    }
}

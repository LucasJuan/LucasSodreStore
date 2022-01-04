﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSS.Identidade.API.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected ICollection<string> Erros = new List<string>();
       protected ActionResult CustomResponse(object result = null)
        {
            if(OperacaoValida())
            {
                return Ok(result);

            }
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray()}
            }));

        }
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var item in erros)
            {
                AdicionarErroProcessamento(item.ErrorMessage);
            }
            return CustomResponse();

        }
        protected bool OperacaoValida()
        {
            return !Erros.Any();
        }
        protected void AdicionarErroProcessamento(string erro)
        {
            Erros.Add(erro);
        }
        protected void LimparErrosProcessamento(string erro)
        {
            Erros.Clear();
        }
    }
}

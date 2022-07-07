﻿using AutoMapper;
using Demo.Api.Services.Intefaces;
using Demo.Api.Services.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.Api.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    private readonly INotificador _notificador;
    protected readonly IMapper _mapper;

    protected MainController(IMapper mapper,
                             INotificador notificador)
    {
        _mapper = mapper;
        _notificador = notificador;
    }

    protected bool OperacaoEhValida()
            => !_notificador.TemNotificacao();

    protected IActionResult CustomResponse(object result = null)
    {
        if (OperacaoEhValida())
        {
            return Ok(new
            {
                success = true,
                data = result
            });
        }

        return BadRequest(new
        {
            success = false,
            errors = _notificador.ObterNotificacoes().Select(e => e.Mensagem)
        });
    }

    protected IActionResult CustomResponse(ModelStateDictionary modalState)
    {
        if (modalState.IsValid is false)
            NotificarErroModalInvalida(modalState);

        return CustomResponse();
    }

    protected void NotificarErroModalInvalida(ModelStateDictionary modalState)
    {
        var erros = modalState.Values.SelectMany(e => e.Errors);

        foreach (var erro in erros)
        {
            var errorMessage = erro.Exception is null ? erro.ErrorMessage : erro.Exception.Message;

            NotificarErro(errorMessage);
        }
    }

    protected void NotificarErro(string mensagem)
        => _notificador.Handle(new Notificacao(mensagem));
}



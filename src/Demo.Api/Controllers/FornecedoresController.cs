using AutoMapper;
using Demo.Api.Extensions;
using Demo.Api.Services.Intefaces;
using Demo.Api.Services.Intefaces.Repository;
using Demo.Api.Services.Intefaces.Services;
using Demo.Api.Services.Models;
using Demo.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class FornecedoresController : MainController
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly IFornecedorService _fornecedorService;

    public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                  IEnderecoRepository enderecoRepository,
                                  IFornecedorService fornecedorService,
                                  INotificador notificador,
                                  IUser aspNetUser,
                                  IMapper mapper)
                                  : base(notificador, aspNetUser, mapper)
    {
        _fornecedorRepository = fornecedorRepository;
        _enderecoRepository = enderecoRepository;
        _fornecedorService = fornecedorService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos() => Ok(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

        if (fornecedor is null) return NotFound();

        return Ok(fornecedor);
    }

    [ClaimsAuthorize("Fornecedor", "Adicionar")]
    [HttpPost]
    public async Task<IActionResult> Adicionar(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);
    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
    {
        if (id != fornecedorViewModel.Id)
        {
            NotificarErro("O id informado não é o mesmo que foi passado na query");

            return CustomResponse(fornecedorViewModel);
        }

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);
    }


    [ClaimsAuthorize("Fornecedor", "Excluir")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var fornecedorViewModel = await ObterForcenedorEndereco(id);

        if (fornecedorViewModel is null) return NotFound();

        await _fornecedorService.Remover(id);

        return CustomResponse();
    }

    [HttpGet("obter-endereco/{id:guid}")]
    public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id) => _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("atualizar-endereco/{id:guid}")]
    public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
    {
        if (id != enderecoViewModel.Id)
        {
            NotificarErro("O id informado não é o mesmo que foi passado na query");

            return CustomResponse(enderecoViewModel);
        }

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _enderecoRepository.Atualizar(_mapper.Map<Endereco>(enderecoViewModel));

        return CustomResponse(enderecoViewModel);
    }

    private async Task<IActionResult> ObterForcenedorEndereco(Guid id)
               => Ok(_mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id)));
}

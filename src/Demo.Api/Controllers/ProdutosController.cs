using AutoMapper;
using Demo.Api.Services.Intefaces;
using Demo.Api.Services.Models;
using Demo.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[Route("api/[controller]")]
public class ProdutosController : MainController
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoRepository produtoRepository,
                              IProdutoService produtoService,
                              INotificador notificador,
                              IMapper mapper) : base(mapper, notificador)
    {
        _produtoRepository = produtoRepository;
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
        => CustomResponse(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos()));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var produtoViewModel = await ObterProduto(id);

        if (produtoViewModel is null) return NotFound();

        return CustomResponse(produtoViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar(ProdutoViewModel produtoViewModel)
    {
        if (ModelState.IsValid is false) return CustomResponse(ModelState);

        var imagemPrefixo = Guid.NewGuid() + "_";

        if (!await UploadArquivo(produtoViewModel.ImagemUpload, imagemPrefixo))
            return CustomResponse(produtoViewModel);

        produtoViewModel.Imagem = imagemPrefixo + produtoViewModel.ImagemUpload.FileName;
        await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

        return CustomResponse(produtoViewModel);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
    {
        if (id != produtoViewModel.Id)
        {
            NotificarErro("O id informado não é o mesmo que foi passado na query");

            CustomResponse(produtoViewModel);
        }

        var produtoAtualizacao = await ObterProduto(id);

        produtoViewModel.Imagem = produtoAtualizacao.Imagem;

        if (ModelState.IsValid is false) return CustomResponse(ModelState);

        if (produtoViewModel.ImagemUpload is null)
        {
            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;

            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
                return CustomResponse(ModelState);

            produtoAtualizacao.Imagem = imagemNome;
        }

        produtoAtualizacao.Nome = produtoViewModel.Nome;
        produtoAtualizacao.Descricao = produtoViewModel.Descricao;
        produtoAtualizacao.Valor = produtoViewModel.Valor;
        produtoAtualizacao.Ativo = produtoViewModel.Ativo;

        await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

        return CustomResponse(produtoViewModel);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var produtoViewModel = await ObterProduto(id);

        if (produtoViewModel is null) return NotFound();

        await _produtoService.Remover(id);

        return CustomResponse();
    }

    private async Task<ProdutoViewModel> ObterProduto(Guid id)
        => _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));

    private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
    {
        if (arquivo is null || arquivo.Length == 0)
        {
            NotificarErro("Forneça uma imagem para este produto!");
            return false;
        }

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPrefixo + arquivo.FileName);

        if (System.IO.File.Exists(path))
        {
            NotificarErro("Já existe um arquivo com este nome!");
            return false;
        }

        using (var stream = new FileStream(path, FileMode.Create))
            await arquivo.CopyToAsync(stream);

        return true;
    }
}

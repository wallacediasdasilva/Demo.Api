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

        if(produtoViewModel is null) return NotFound();

        return CustomResponse(produtoViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar(ProdutoViewModel produtoViewModel)
    {
        if (ModelState.IsValid is false) return CustomResponse(ModelState);

        var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;

        if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            return CustomResponse(produtoViewModel);

        produtoViewModel.Imagem = imagemNome;
        await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

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

    private bool UploadArquivo(string arquivo, string imgNome)
    {
        if (string.IsNullOrEmpty(arquivo))
        {
            NotificarErro("Forneça uma imagem para este produto!");
            return false;
        }

        var imageDataByteArray = Convert.FromBase64String(arquivo);

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgNome);

        if (System.IO.File.Exists(filePath))
        {
            NotificarErro("Já existe um arquivo com este nome!");
            return false;
        }

        System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

        return true;
    }
}

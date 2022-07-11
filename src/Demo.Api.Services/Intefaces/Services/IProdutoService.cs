using Demo.Api.Services.Models;

namespace Demo.Api.Services.Intefaces.Services;

public interface IProdutoService : IDisposable
{
    Task Adicionar(Produto produto);
    Task Atualizar(Produto produto);
    Task Remover(Guid id);
}

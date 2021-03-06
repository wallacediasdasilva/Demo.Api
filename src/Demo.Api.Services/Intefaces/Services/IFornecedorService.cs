using Demo.Api.Services.Models;

namespace Demo.Api.Services.Intefaces.Services;

public interface IFornecedorService : IDisposable
{
    Task Adicionar(Fornecedor fornecedor);
    Task Atualizar(Fornecedor fornecedor);
    Task Remover(Guid id);
    Task AtualizarEndereco(Endereco endereco);
}

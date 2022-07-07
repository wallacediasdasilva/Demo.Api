using Demo.Api.Services.Models;

namespace Demo.Api.Services.Intefaces;

public interface IFornecedorRepository : IRepository<Fornecedor>
{
    Task<Fornecedor> ObterFornecedorEndereco(Guid id);
    Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);
}

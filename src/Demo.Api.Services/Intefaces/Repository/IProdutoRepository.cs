using Demo.Api.Services.Models;

namespace Demo.Api.Services.Intefaces.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);
    Task<IEnumerable<Produto>> ObterProdutosFornecedores();
    Task<Produto> ObterProdutoFornecedor(Guid id);
}

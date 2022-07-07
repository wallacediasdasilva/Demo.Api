using Demo.Api.Data.Context;
using Demo.Api.Services.Intefaces;
using Demo.Api.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Data.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(MeuDbContext context) : base(context) { }

    public async Task<Produto> ObterProdutoFornecedor(Guid id)
        => await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        => await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor)
            .OrderBy(p => p.Nome).ToListAsync();

    public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        => await Buscar(p => p.FornecedorId == fornecedorId);
}

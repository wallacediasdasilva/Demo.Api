using Demo.Api.Data.Context;
using Demo.Api.Services.Intefaces;
using Demo.Api.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Data.Repository;

public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(MeuDbContext context) : base(context)
    {
    }

    public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        => await Db.Fornecedores.AsNoTracking()
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        => await Db.Fornecedores.AsNoTracking()
            .Include(c => c.Produtos)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);
}

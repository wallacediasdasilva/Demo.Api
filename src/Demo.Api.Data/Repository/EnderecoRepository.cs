using Demo.Api.Data.Context;
using Demo.Api.Services.Intefaces;
using Demo.Api.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Data.Repository;

public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(MeuDbContext context) : base(context) { }

    public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
     => await Db.Enderecos.AsNoTracking()
            .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
}

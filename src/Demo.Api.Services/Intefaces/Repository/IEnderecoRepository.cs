using Demo.Api.Services.Models;

namespace Demo.Api.Services.Intefaces.Repository;

public interface IEnderecoRepository : IRepository<Endereco>
{
    Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
}

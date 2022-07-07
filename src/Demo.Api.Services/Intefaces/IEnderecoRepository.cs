using Demo.Api.Services.Models;

namespace Demo.Api.Services.Intefaces;

public interface IEnderecoRepository : IRepository<Endereco>
{
    Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
}

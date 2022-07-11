using Demo.Api.Data.Context;
using Demo.Api.Data.Repository;
using Demo.Api.Extensions;
using Demo.Api.Services.Intefaces;
using Demo.Api.Services.Intefaces.Repository;
using Demo.Api.Services.Intefaces.Services;
using Demo.Api.Services.Notificacoes;
using Demo.Api.Services.Services;

namespace Demo.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        services.AddScoped<MeuDbContext>();

        #region [ SERVICES ]
        services.AddScoped<IFornecedorService, FornecedorService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<INotificador, Notificador>();
        #endregion [ SERVICES ]

        #region [ REPOSITORY ]
        services.AddScoped<IFornecedorRepository, FornecedorRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        #endregion [ REPOSITORY ]

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUser, AspNetUser>();

        return services;
    }
}

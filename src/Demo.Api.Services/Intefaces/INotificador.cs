using Demo.Api.Services.Notificacoes;

namespace Demo.Api.Services.Intefaces;

public interface INotificador
{
    bool TemNotificacao();
    List<Notificacao> ObterNotificacoes();
    void Handle(Notificacao notificacao);
}

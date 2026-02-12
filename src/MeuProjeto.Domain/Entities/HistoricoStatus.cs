using MeuProjeto.Domain.Common;
using MeuProjeto.Domain.Enums;

namespace MeuProjeto.Domain.Entities;

public class HistoricoStatus : BaseEntity
{
    public Guid ProjetoId { get; private set; }
    public StatusProjeto Status { get; private set; }
    public string? Observacao { get; private set; }

    private HistoricoStatus() { }

    public static HistoricoStatus Criar(Guid projetoId, StatusProjeto status, string? observacao = null)
    {
        return new HistoricoStatus
        {
            ProjetoId = projetoId,
            Status = status,
            Observacao = observacao?.Trim()
        };
    }

    public string StatusTexto => Status switch
    {
        StatusProjeto.Orcamento => "Orçamento Enviado",
        StatusProjeto.Aprovado => "Projeto Aprovado",
        StatusProjeto.EmProducao => "Em Produção",
        StatusProjeto.ProntoEntrega => "Pronto para Entrega",
        StatusProjeto.Entregue => "Entregue",
        StatusProjeto.Instalado => "Instalado",
        StatusProjeto.Concluido => "Concluído",
        StatusProjeto.Cancelado => "Cancelado",
        _ => "Desconhecido"
    };
}

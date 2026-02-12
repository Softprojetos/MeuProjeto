using MeuProjeto.Domain.Common;
using MeuProjeto.Domain.Enums;

namespace MeuProjeto.Domain.Entities;

public class ArquivoProjeto : BaseEntity
{
    public Guid ProjetoId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;
    public TipoArquivo Tipo { get; private set; }

    private ArquivoProjeto() { }

    public static Result<ArquivoProjeto> Criar(Guid projetoId, string nome, string url, TipoArquivo tipo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Falha<ArquivoProjeto>("Nome do arquivo é obrigatório.");

        if (string.IsNullOrWhiteSpace(url))
            return Result.Falha<ArquivoProjeto>("URL do arquivo é obrigatória.");

        return Result.Ok(new ArquivoProjeto
        {
            ProjetoId = projetoId,
            Nome = nome.Trim(),
            Url = url.Trim(),
            Tipo = tipo
        });
    }
}

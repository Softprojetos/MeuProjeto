using MeuProjeto.Domain.Common;
using MeuProjeto.Domain.Enums;

namespace MeuProjeto.Domain.Entities;

public class Projeto : BaseEntity
{
    public Guid EmpresaId { get; private set; }
    public Guid ClienteId { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public StatusProjeto Status { get; private set; } = StatusProjeto.Orcamento;
    public decimal ValorTotal { get; private set; }
    public DateTime? PrevisaoEntrega { get; private set; }
    public DateTime? AprovadoEm { get; private set; }
    public DateTime? ConcluidoEm { get; private set; }
    public string CodigoPublico { get; private set; } = string.Empty;

    public Cliente? Cliente { get; private set; }
    public Empresa? Empresa { get; private set; }

    private readonly List<ArquivoProjeto> _arquivos = [];
    public IReadOnlyCollection<ArquivoProjeto> Arquivos => _arquivos.AsReadOnly();

    private readonly List<HistoricoStatus> _historico = [];
    public IReadOnlyCollection<HistoricoStatus> Historico => _historico.AsReadOnly();

    private readonly List<Parcela> _parcelas = [];
    public IReadOnlyCollection<Parcela> Parcelas => _parcelas.AsReadOnly();

    private Projeto() { }

    public static Result<Projeto> Criar(Guid empresaId, Guid clienteId, string titulo, decimal valorTotal, DateTime? previsaoEntrega = null)
    {
        if (empresaId == Guid.Empty)
            return Result.Falha<Projeto>("Empresa é obrigatória.");

        if (clienteId == Guid.Empty)
            return Result.Falha<Projeto>("Cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Falha<Projeto>("Título do projeto é obrigatório.");

        if (valorTotal < 0)
            return Result.Falha<Projeto>("Valor total não pode ser negativo.");

        var projeto = new Projeto
        {
            EmpresaId = empresaId,
            ClienteId = clienteId,
            Titulo = titulo.Trim(),
            ValorTotal = valorTotal,
            PrevisaoEntrega = previsaoEntrega,
            CodigoPublico = GerarCodigoPublico()
        };

        projeto._historico.Add(HistoricoStatus.Criar(projeto.Id, StatusProjeto.Orcamento, "Projeto criado."));

        return Result.Ok(projeto);
    }

    public Result AvancarStatus(string? observacao = null)
    {
        if (Status == StatusProjeto.Cancelado)
            return Result.Falha("Projeto cancelado não pode avançar.");

        if (Status == StatusProjeto.Concluido)
            return Result.Falha("Projeto já está concluído.");

        var novoStatus = Status switch
        {
            StatusProjeto.Orcamento => StatusProjeto.Aprovado,
            StatusProjeto.Aprovado => StatusProjeto.EmProducao,
            StatusProjeto.EmProducao => StatusProjeto.ProntoEntrega,
            StatusProjeto.ProntoEntrega => StatusProjeto.Entregue,
            StatusProjeto.Entregue => StatusProjeto.Instalado,
            StatusProjeto.Instalado => StatusProjeto.Concluido,
            _ => Status
        };

        if (novoStatus == StatusProjeto.Aprovado)
            AprovadoEm = DateTime.UtcNow;

        if (novoStatus == StatusProjeto.Concluido)
            ConcluidoEm = DateTime.UtcNow;

        Status = novoStatus;
        _historico.Add(HistoricoStatus.Criar(Id, novoStatus, observacao));
        MarcarAtualizado();

        return Result.Ok();
    }

    public Result Cancelar(string motivo)
    {
        if (Status == StatusProjeto.Concluido)
            return Result.Falha("Projeto concluído não pode ser cancelado.");

        Status = StatusProjeto.Cancelado;
        _historico.Add(HistoricoStatus.Criar(Id, StatusProjeto.Cancelado, motivo));
        MarcarAtualizado();

        return Result.Ok();
    }

    public void AtualizarDados(string titulo, string? descricao, decimal valorTotal, DateTime? previsaoEntrega)
    {
        if (!string.IsNullOrWhiteSpace(titulo)) Titulo = titulo.Trim();
        Descricao = descricao?.Trim();
        ValorTotal = valorTotal;
        PrevisaoEntrega = previsaoEntrega;
        MarcarAtualizado();
    }

    public Result AdicionarArquivo(string nome, string url, TipoArquivo tipo)
    {
        var arquivo = ArquivoProjeto.Criar(Id, nome, url, tipo);
        if (arquivo.Falhou) return Result.Falha(arquivo.Erro!);
        _arquivos.Add(arquivo.Valor!);
        MarcarAtualizado();
        return Result.Ok();
    }

    public Result AdicionarParcela(int numero, decimal valor, DateTime vencimento)
    {
        if (_parcelas.Any(p => p.Numero == numero))
            return Result.Falha($"Parcela {numero} já existe.");

        var parcela = Parcela.Criar(Id, numero, valor, vencimento);
        if (parcela.Falhou) return Result.Falha(parcela.Erro!);
        _parcelas.Add(parcela.Valor!);
        MarcarAtualizado();
        return Result.Ok();
    }

    public decimal ValorPago => _parcelas.Where(p => p.PagoEm.HasValue).Sum(p => p.Valor);
    public decimal ValorRestante => ValorTotal - ValorPago;
    public int PercentualProgresso => Status switch
    {
        StatusProjeto.Orcamento => 0,
        StatusProjeto.Aprovado => 15,
        StatusProjeto.EmProducao => 45,
        StatusProjeto.ProntoEntrega => 70,
        StatusProjeto.Entregue => 85,
        StatusProjeto.Instalado => 95,
        StatusProjeto.Concluido => 100,
        StatusProjeto.Cancelado => 0,
        _ => 0
    };

    private static string GerarCodigoPublico()
    {
        return $"PRJ-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString()[..6].ToUpperInvariant()}";
    }
}

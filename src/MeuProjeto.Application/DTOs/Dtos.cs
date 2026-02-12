using MeuProjeto.Domain.Enums;

namespace MeuProjeto.Application.DTOs;

// === Empresa ===
public record CriarEmpresaDto(string Nome, string Slug, string? Telefone);

public record AtualizarEmpresaDto(string Nome, string? Telefone, string? WhatsApp, string? LogoUrl);

public record EmpresaDto(
    Guid Id, string Nome, string Slug, string? LogoUrl,
    string? Telefone, string? WhatsApp,
    string CorPrimaria, string CorSecundaria,
    PlanoAssinatura Plano, bool TrialExpirado, DateTime CriadoEm);

// === Cliente ===
public record CriarClienteDto(string Nome, string? Email, string? Telefone);

public record ClienteDto(
    Guid Id, string Nome, string? Email, string? Telefone,
    string? WhatsApp, string CodigoAcesso, int TotalProjetos, DateTime CriadoEm);

// === Projeto ===
public record CriarProjetoDto(
    Guid ClienteId, string Titulo, string? Descricao,
    decimal ValorTotal, DateTime? PrevisaoEntrega);

public record ProjetoResumoDto(
    Guid Id, string CodigoPublico, string Titulo, string ClienteNome,
    StatusProjeto Status, decimal ValorTotal, decimal ValorPago,
    int PercentualProgresso, DateTime? PrevisaoEntrega, DateTime CriadoEm);

public record ProjetoDetalheDto(
    Guid Id, string CodigoPublico, string Titulo, string? Descricao,
    StatusProjeto Status, decimal ValorTotal, decimal ValorPago, decimal ValorRestante,
    int PercentualProgresso, DateTime? PrevisaoEntrega,
    DateTime? AprovadoEm, DateTime? ConcluidoEm, DateTime CriadoEm,
    ClienteDto Cliente,
    List<HistoricoStatusDto> Historico,
    List<ArquivoDto> Arquivos,
    List<ParcelaDto> Parcelas);

// === Portal Público (o que o cliente final vê) ===
public record ProjetoPublicoDto(
    string CodigoPublico, string Titulo, string? Descricao,
    string StatusTexto, int PercentualProgresso,
    DateTime? PrevisaoEntrega, decimal ValorTotal, decimal ValorPago,
    List<HistoricoStatusDto> Historico,
    List<ArquivoDto> Imagens,
    List<ParcelaPublicaDto> Parcelas,
    EmpresaPublicaDto Empresa);

public record EmpresaPublicaDto(
    string Nome, string? LogoUrl, string? WhatsApp,
    string CorPrimaria, string CorSecundaria);

// === Sub-DTOs ===
public record HistoricoStatusDto(string Status, string? Observacao, DateTime Data);

public record ArquivoDto(Guid Id, string Nome, string Url, TipoArquivo Tipo, DateTime CriadoEm);

public record ParcelaDto(Guid Id, int Numero, decimal Valor, DateTime Vencimento, DateTime? PagoEm, bool Atrasada);

public record ParcelaPublicaDto(int Numero, decimal Valor, DateTime Vencimento, bool Pago, bool Atrasada);

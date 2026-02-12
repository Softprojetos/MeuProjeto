using MeuProjeto.Application.DTOs;
using MeuProjeto.Domain.Entities;

namespace MeuProjeto.Application.Services;

public static class MappingExtensions
{
    public static EmpresaDto ToDto(this Empresa e) => new(
        e.Id, e.Nome, e.Slug, e.LogoUrl,
        e.Telefone, e.WhatsApp,
        e.CorPrimaria, e.CorSecundaria,
        e.Plano, e.TrialExpirado, e.CriadoEm);

    public static EmpresaPublicaDto ToPublicDto(this Empresa e) => new(
        e.Nome, e.LogoUrl, e.WhatsApp,
        e.CorPrimaria, e.CorSecundaria);

    public static ClienteDto ToDto(this Cliente c) => new(
        c.Id, c.Nome, c.Email, c.Telefone,
        c.WhatsApp, c.CodigoAcesso,
        c.Projetos.Count, c.CriadoEm);

    public static ProjetoResumoDto ToResumoDto(this Projeto p) => new(
        p.Id, p.CodigoPublico, p.Titulo,
        p.Cliente?.Nome ?? "—",
        p.Status, p.ValorTotal, p.ValorPago,
        p.PercentualProgresso, p.PrevisaoEntrega, p.CriadoEm);

    public static ProjetoDetalheDto ToDetalheDto(this Projeto p) => new(
        p.Id, p.CodigoPublico, p.Titulo, p.Descricao,
        p.Status, p.ValorTotal, p.ValorPago, p.ValorRestante,
        p.PercentualProgresso, p.PrevisaoEntrega,
        p.AprovadoEm, p.ConcluidoEm, p.CriadoEm,
        p.Cliente!.ToDto(),
        p.Historico.OrderByDescending(h => h.CriadoEm)
            .Select(h => new HistoricoStatusDto(h.StatusTexto, h.Observacao, h.CriadoEm)).ToList(),
        p.Arquivos.Select(a => new ArquivoDto(a.Id, a.Nome, a.Url, a.Tipo, a.CriadoEm)).ToList(),
        p.Parcelas.OrderBy(pc => pc.Numero)
            .Select(pc => new ParcelaDto(pc.Id, pc.Numero, pc.Valor, pc.Vencimento, pc.PagoEm, pc.Atrasada)).ToList());

    public static ProjetoPublicoDto ToPublicoDto(this Projeto p) => new(
        p.CodigoPublico, p.Titulo, p.Descricao,
        p.Historico.OrderByDescending(h => h.CriadoEm).FirstOrDefault()?.StatusTexto ?? "—",
        p.PercentualProgresso,
        p.PrevisaoEntrega, p.ValorTotal, p.ValorPago,
        p.Historico.OrderByDescending(h => h.CriadoEm)
            .Select(h => new HistoricoStatusDto(h.StatusTexto, h.Observacao, h.CriadoEm)).ToList(),
        p.Arquivos.Where(a => a.Tipo == Domain.Enums.TipoArquivo.ImagemProjeto || a.Tipo == Domain.Enums.TipoArquivo.ImagemProducao)
            .Select(a => new ArquivoDto(a.Id, a.Nome, a.Url, a.Tipo, a.CriadoEm)).ToList(),
        p.Parcelas.OrderBy(pc => pc.Numero)
            .Select(pc => new ParcelaPublicaDto(pc.Numero, pc.Valor, pc.Vencimento, pc.PagoEm.HasValue, pc.Atrasada)).ToList(),
        p.Empresa!.ToPublicDto());
}

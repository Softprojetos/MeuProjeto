using MeuProjeto.Domain.Common;
using MeuProjeto.Domain.Enums;

namespace MeuProjeto.Domain.Entities;

public class Empresa : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? LogoUrl { get; private set; }
    public string? Telefone { get; private set; }
    public string? WhatsApp { get; private set; }
    public string CorPrimaria { get; private set; } = "#2563EB";
    public string CorSecundaria { get; private set; } = "#1E40AF";
    public PlanoAssinatura Plano { get; private set; } = PlanoAssinatura.Trial;
    public DateTime? TrialExpiraEm { get; private set; }
    public bool Ativo { get; private set; } = true;

    private readonly List<Projeto> _projetos = [];
    public IReadOnlyCollection<Projeto> Projetos => _projetos.AsReadOnly();

    private Empresa() { }

    public static Result<Empresa> Criar(string nome, string slug, string? telefone = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Falha<Empresa>("Nome da empresa é obrigatório.");

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Falha<Empresa>("Slug é obrigatório.");

        slug = slug.ToLowerInvariant().Trim();

        if (slug.Length < 3 || slug.Length > 50)
            return Result.Falha<Empresa>("Slug deve ter entre 3 e 50 caracteres.");

        if (!System.Text.RegularExpressions.Regex.IsMatch(slug, @"^[a-z0-9\-]+$"))
            return Result.Falha<Empresa>("Slug deve conter apenas letras minúsculas, números e hífens.");

        var empresa = new Empresa
        {
            Nome = nome.Trim(),
            Slug = slug,
            Telefone = telefone?.Trim(),
            TrialExpiraEm = DateTime.UtcNow.AddDays(30)
        };

        return Result.Ok(empresa);
    }

    public void AtualizarDados(string nome, string? telefone, string? whatsApp, string? logoUrl)
    {
        if (!string.IsNullOrWhiteSpace(nome)) Nome = nome.Trim();
        Telefone = telefone?.Trim();
        WhatsApp = whatsApp?.Trim();
        LogoUrl = logoUrl?.Trim();
        MarcarAtualizado();
    }

    public void PersonalizarCores(string corPrimaria, string corSecundaria)
    {
        CorPrimaria = corPrimaria;
        CorSecundaria = corSecundaria;
        MarcarAtualizado();
    }

    public Result AtivarPlano(PlanoAssinatura plano)
    {
        Plano = plano;
        TrialExpiraEm = null;
        MarcarAtualizado();
        return Result.Ok();
    }

    public bool TrialExpirado => Plano == PlanoAssinatura.Trial
        && TrialExpiraEm.HasValue
        && DateTime.UtcNow > TrialExpiraEm.Value;
}

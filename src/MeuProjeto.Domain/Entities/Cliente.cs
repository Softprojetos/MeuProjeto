using MeuProjeto.Domain.Common;

namespace MeuProjeto.Domain.Entities;

public class Cliente : BaseEntity
{
    public Guid EmpresaId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Telefone { get; private set; }
    public string? WhatsApp { get; private set; }
    public string CodigoAcesso { get; private set; } = string.Empty;

    private readonly List<Projeto> _projetos = [];
    public IReadOnlyCollection<Projeto> Projetos => _projetos.AsReadOnly();

    private Cliente() { }

    public static Result<Cliente> Criar(Guid empresaId, string nome, string? email = null, string? telefone = null)
    {
        if (empresaId == Guid.Empty)
            return Result.Falha<Cliente>("Empresa é obrigatória.");

        if (string.IsNullOrWhiteSpace(nome))
            return Result.Falha<Cliente>("Nome do cliente é obrigatório.");

        var cliente = new Cliente
        {
            EmpresaId = empresaId,
            Nome = nome.Trim(),
            Email = email?.Trim(),
            Telefone = telefone?.Trim(),
            WhatsApp = telefone?.Trim(),
            CodigoAcesso = GerarCodigoAcesso()
        };

        return Result.Ok(cliente);
    }

    public void AtualizarDados(string nome, string? email, string? telefone, string? whatsApp)
    {
        if (!string.IsNullOrWhiteSpace(nome)) Nome = nome.Trim();
        Email = email?.Trim();
        Telefone = telefone?.Trim();
        WhatsApp = whatsApp?.Trim();
        MarcarAtualizado();
    }

    public void RegenerarCodigoAcesso()
    {
        CodigoAcesso = GerarCodigoAcesso();
        MarcarAtualizado();
    }

    private static string GerarCodigoAcesso()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "")
            .Replace("+", "")
            .Replace("=", "")[..12]
            .ToUpperInvariant();
    }
}

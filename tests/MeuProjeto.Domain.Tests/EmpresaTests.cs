using MeuProjeto.Domain.Entities;
using MeuProjeto.Domain.Enums;

namespace MeuProjeto.Domain.Tests;

public class EmpresaTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveRetornarSucesso()
    {
        var result = Empresa.Criar("Minha Marcenaria", "minha-marcenaria", "11999999999");

        Assert.True(result.Sucesso);
        Assert.NotNull(result.Valor);
        Assert.Equal("Minha Marcenaria", result.Valor!.Nome);
        Assert.Equal("minha-marcenaria", result.Valor.Slug);
        Assert.Equal(PlanoAssinatura.Trial, result.Valor.Plano);
        Assert.NotNull(result.Valor.TrialExpiraEm);
    }

    [Fact]
    public void Criar_SemNome_DeveRetornarFalha()
    {
        var result = Empresa.Criar("", "minha-marcenaria");

        Assert.True(result.Falhou);
        Assert.Contains("Nome", result.Erro);
    }

    [Fact]
    public void Criar_SlugInvalido_DeveRetornarFalha()
    {
        var result = Empresa.Criar("Teste", "slug com espaço");

        Assert.True(result.Falhou);
        Assert.Contains("Slug", result.Erro);
    }

    [Fact]
    public void Criar_SlugCurto_DeveRetornarFalha()
    {
        var result = Empresa.Criar("Teste", "ab");

        Assert.True(result.Falhou);
    }

    [Fact]
    public void TrialExpirado_DentroDoTrial_DeveRetornarFalso()
    {
        var result = Empresa.Criar("Teste", "teste-empresa");

        Assert.False(result.Valor!.TrialExpirado);
    }

    [Fact]
    public void AtivarPlano_DeveRemoverTrialExpiraEm()
    {
        var empresa = Empresa.Criar("Teste", "teste-empresa").Valor!;

        empresa.AtivarPlano(PlanoAssinatura.Profissional);

        Assert.Equal(PlanoAssinatura.Profissional, empresa.Plano);
        Assert.Null(empresa.TrialExpiraEm);
    }
}

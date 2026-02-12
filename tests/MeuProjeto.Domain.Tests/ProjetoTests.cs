using MeuProjeto.Domain.Entities;
using MeuProjeto.Domain.Enums;

namespace MeuProjeto.Domain.Tests;

public class ProjetoTests
{
    private static Projeto CriarProjetoValido()
    {
        return Projeto.Criar(Guid.NewGuid(), Guid.NewGuid(), "Cozinha Planejada", 15000m).Valor!;
    }

    [Fact]
    public void Criar_ComDadosValidos_DeveRetornarSucesso()
    {
        var empresaId = Guid.NewGuid();
        var clienteId = Guid.NewGuid();

        var result = Projeto.Criar(empresaId, clienteId, "Cozinha Planejada", 15000m);

        Assert.True(result.Sucesso);
        Assert.Equal("Cozinha Planejada", result.Valor!.Titulo);
        Assert.Equal(15000m, result.Valor.ValorTotal);
        Assert.Equal(StatusProjeto.Orcamento, result.Valor.Status);
        Assert.Single(result.Valor.Historico);
        Assert.StartsWith("PRJ-", result.Valor.CodigoPublico);
    }

    [Fact]
    public void Criar_SemTitulo_DeveRetornarFalha()
    {
        var result = Projeto.Criar(Guid.NewGuid(), Guid.NewGuid(), "", 15000m);

        Assert.True(result.Falhou);
    }

    [Fact]
    public void Criar_ValorNegativo_DeveRetornarFalha()
    {
        var result = Projeto.Criar(Guid.NewGuid(), Guid.NewGuid(), "Teste", -100m);

        Assert.True(result.Falhou);
    }

    [Fact]
    public void AvancarStatus_DeOrcamentoParaAprovado_DeveFuncionar()
    {
        var projeto = CriarProjetoValido();

        var result = projeto.AvancarStatus("Cliente aprovou");

        Assert.True(result.Sucesso);
        Assert.Equal(StatusProjeto.Aprovado, projeto.Status);
        Assert.NotNull(projeto.AprovadoEm);
        Assert.Equal(2, projeto.Historico.Count);
    }

    [Fact]
    public void AvancarStatus_FluxoCompleto_DevePassarPorTodosOsStatus()
    {
        var projeto = CriarProjetoValido();

        projeto.AvancarStatus(); // Aprovado
        Assert.Equal(StatusProjeto.Aprovado, projeto.Status);

        projeto.AvancarStatus(); // Em Produção
        Assert.Equal(StatusProjeto.EmProducao, projeto.Status);

        projeto.AvancarStatus(); // Pronto Entrega
        Assert.Equal(StatusProjeto.ProntoEntrega, projeto.Status);

        projeto.AvancarStatus(); // Entregue
        Assert.Equal(StatusProjeto.Entregue, projeto.Status);

        projeto.AvancarStatus(); // Instalado
        Assert.Equal(StatusProjeto.Instalado, projeto.Status);

        projeto.AvancarStatus(); // Concluído
        Assert.Equal(StatusProjeto.Concluido, projeto.Status);
        Assert.NotNull(projeto.ConcluidoEm);
        Assert.Equal(100, projeto.PercentualProgresso);
    }

    [Fact]
    public void AvancarStatus_ProjetoCancelado_DeveRetornarFalha()
    {
        var projeto = CriarProjetoValido();
        projeto.Cancelar("Desistiu");

        var result = projeto.AvancarStatus();

        Assert.True(result.Falhou);
    }

    [Fact]
    public void AvancarStatus_ProjetoConcluido_DeveRetornarFalha()
    {
        var projeto = CriarProjetoValido();
        for (int i = 0; i < 6; i++) projeto.AvancarStatus();

        var result = projeto.AvancarStatus();

        Assert.True(result.Falhou);
    }

    [Fact]
    public void Cancelar_ProjetoAtivo_DeveFuncionar()
    {
        var projeto = CriarProjetoValido();

        var result = projeto.Cancelar("Cliente desistiu");

        Assert.True(result.Sucesso);
        Assert.Equal(StatusProjeto.Cancelado, projeto.Status);
    }

    [Fact]
    public void Cancelar_ProjetoConcluido_DeveRetornarFalha()
    {
        var projeto = CriarProjetoValido();
        for (int i = 0; i < 6; i++) projeto.AvancarStatus();

        var result = projeto.Cancelar("Motivo");

        Assert.True(result.Falhou);
    }

    [Fact]
    public void AdicionarParcela_DeveCalcularValorPago()
    {
        var projeto = CriarProjetoValido();
        projeto.AdicionarParcela(1, 5000m, DateTime.UtcNow.AddDays(30));
        projeto.AdicionarParcela(2, 5000m, DateTime.UtcNow.AddDays(60));
        projeto.AdicionarParcela(3, 5000m, DateTime.UtcNow.AddDays(90));

        Assert.Equal(3, projeto.Parcelas.Count);
        Assert.Equal(0, projeto.ValorPago);
        Assert.Equal(15000m, projeto.ValorRestante);
    }

    [Fact]
    public void AdicionarParcela_NumeroDuplicado_DeveRetornarFalha()
    {
        var projeto = CriarProjetoValido();
        projeto.AdicionarParcela(1, 5000m, DateTime.UtcNow);

        var result = projeto.AdicionarParcela(1, 5000m, DateTime.UtcNow);

        Assert.True(result.Falhou);
    }

    [Fact]
    public void PercentualProgresso_DeveRefletirStatus()
    {
        var projeto = CriarProjetoValido();
        Assert.Equal(0, projeto.PercentualProgresso);

        projeto.AvancarStatus();
        Assert.Equal(15, projeto.PercentualProgresso);

        projeto.AvancarStatus();
        Assert.Equal(45, projeto.PercentualProgresso);
    }
}

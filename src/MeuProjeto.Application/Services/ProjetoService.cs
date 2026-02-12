using MeuProjeto.Application.DTOs;
using MeuProjeto.Domain.Common;
using MeuProjeto.Domain.Entities;
using MeuProjeto.Domain.Enums;
using MeuProjeto.Domain.Interfaces;

namespace MeuProjeto.Application.Services;

public class ProjetoService
{
    private readonly IProjetoRepository _projetoRepo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IUnitOfWork _uow;

    public ProjetoService(
        IProjetoRepository projetoRepo,
        IClienteRepository clienteRepo,
        IUnitOfWork uow)
    {
        _projetoRepo = projetoRepo;
        _clienteRepo = clienteRepo;
        _uow = uow;
    }

    public async Task<Result<ProjetoResumoDto>> CriarAsync(Guid empresaId, CriarProjetoDto dto, CancellationToken ct = default)
    {
        var cliente = await _clienteRepo.ObterPorIdAsync(dto.ClienteId, ct);
        if (cliente is null) return Result.Falha<ProjetoResumoDto>("Cliente não encontrado.");
        if (cliente.EmpresaId != empresaId) return Result.Falha<ProjetoResumoDto>("Cliente não pertence a esta empresa.");

        var result = Projeto.Criar(empresaId, dto.ClienteId, dto.Titulo, dto.ValorTotal, dto.PrevisaoEntrega);
        if (result.Falhou) return Result.Falha<ProjetoResumoDto>(result.Erro!);

        await _projetoRepo.AdicionarAsync(result.Valor!, ct);
        await _uow.SalvarAsync(ct);

        return Result.Ok(result.Valor!.ToResumoDto());
    }

    public async Task<Result<List<ProjetoResumoDto>>> ListarPorEmpresaAsync(Guid empresaId, CancellationToken ct = default)
    {
        var projetos = await _projetoRepo.ListarPorEmpresaAsync(empresaId, ct);
        return Result.Ok(projetos.Select(p => p.ToResumoDto()).ToList());
    }

    public async Task<Result<ProjetoDetalheDto>> ObterDetalheAsync(Guid projetoId, Guid empresaId, CancellationToken ct = default)
    {
        var projeto = await _projetoRepo.ObterPorIdAsync(projetoId, ct);
        if (projeto is null) return Result.Falha<ProjetoDetalheDto>("Projeto não encontrado.");
        if (projeto.EmpresaId != empresaId) return Result.Falha<ProjetoDetalheDto>("Projeto não pertence a esta empresa.");
        return Result.Ok(projeto.ToDetalheDto());
    }

    public async Task<Result> AvancarStatusAsync(Guid projetoId, Guid empresaId, string? observacao = null, CancellationToken ct = default)
    {
        var projeto = await _projetoRepo.ObterPorIdAsync(projetoId, ct);
        if (projeto is null) return Result.Falha("Projeto não encontrado.");
        if (projeto.EmpresaId != empresaId) return Result.Falha("Projeto não pertence a esta empresa.");

        var result = projeto.AvancarStatus(observacao);
        if (result.Falhou) return result;

        _projetoRepo.Atualizar(projeto);
        await _uow.SalvarAsync(ct);
        return Result.Ok();
    }

    public async Task<Result> AdicionarArquivoAsync(Guid projetoId, Guid empresaId, string nome, string url, TipoArquivo tipo, CancellationToken ct = default)
    {
        var projeto = await _projetoRepo.ObterPorIdAsync(projetoId, ct);
        if (projeto is null) return Result.Falha("Projeto não encontrado.");
        if (projeto.EmpresaId != empresaId) return Result.Falha("Projeto não pertence a esta empresa.");

        var result = projeto.AdicionarArquivo(nome, url, tipo);
        if (result.Falhou) return result;

        _projetoRepo.Atualizar(projeto);
        await _uow.SalvarAsync(ct);
        return Result.Ok();
    }

    public async Task<Result> MarcarParcelaPagaAsync(Guid projetoId, Guid empresaId, Guid parcelaId, CancellationToken ct = default)
    {
        var projeto = await _projetoRepo.ObterPorIdAsync(projetoId, ct);
        if (projeto is null) return Result.Falha("Projeto não encontrado.");
        if (projeto.EmpresaId != empresaId) return Result.Falha("Projeto não pertence a esta empresa.");

        var parcela = projeto.Parcelas.FirstOrDefault(p => p.Id == parcelaId);
        if (parcela is null) return Result.Falha("Parcela não encontrada.");

        var result = parcela.MarcarPago();
        if (result.Falhou) return result;

        _projetoRepo.Atualizar(projeto);
        await _uow.SalvarAsync(ct);
        return Result.Ok();
    }

    // === Portal Público (sem autenticação) ===
    public async Task<Result<ProjetoPublicoDto>> ObterPublicoAsync(string codigoPublico, CancellationToken ct = default)
    {
        var projeto = await _projetoRepo.ObterPorCodigoPublicoAsync(codigoPublico, ct);
        if (projeto is null) return Result.Falha<ProjetoPublicoDto>("Projeto não encontrado.");
        return Result.Ok(projeto.ToPublicoDto());
    }
}

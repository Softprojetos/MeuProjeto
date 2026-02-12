using MeuProjeto.Application.DTOs;
using MeuProjeto.Domain.Common;
using MeuProjeto.Domain.Entities;
using MeuProjeto.Domain.Interfaces;

namespace MeuProjeto.Application.Services;

public class EmpresaService
{
    private readonly IEmpresaRepository _empresaRepo;
    private readonly IUnitOfWork _uow;

    public EmpresaService(IEmpresaRepository empresaRepo, IUnitOfWork uow)
    {
        _empresaRepo = empresaRepo;
        _uow = uow;
    }

    public async Task<Result<EmpresaDto>> CriarAsync(CriarEmpresaDto dto, CancellationToken ct = default)
    {
        if (await _empresaRepo.SlugExisteAsync(dto.Slug, ct))
            return Result.Falha<EmpresaDto>("Esse slug já está em uso.");

        var result = Empresa.Criar(dto.Nome, dto.Slug, dto.Telefone);
        if (result.Falhou) return Result.Falha<EmpresaDto>(result.Erro!);

        await _empresaRepo.AdicionarAsync(result.Valor!, ct);
        await _uow.SalvarAsync(ct);

        return Result.Ok(result.Valor!.ToDto());
    }

    public async Task<Result<EmpresaDto>> ObterPorSlugAsync(string slug, CancellationToken ct = default)
    {
        var empresa = await _empresaRepo.ObterPorSlugAsync(slug, ct);
        if (empresa is null) return Result.Falha<EmpresaDto>("Empresa não encontrada.");
        return Result.Ok(empresa.ToDto());
    }

    public async Task<Result<EmpresaDto>> AtualizarAsync(Guid id, AtualizarEmpresaDto dto, CancellationToken ct = default)
    {
        var empresa = await _empresaRepo.ObterPorIdAsync(id, ct);
        if (empresa is null) return Result.Falha<EmpresaDto>("Empresa não encontrada.");

        empresa.AtualizarDados(dto.Nome, dto.Telefone, dto.WhatsApp, dto.LogoUrl);
        _empresaRepo.Atualizar(empresa);
        await _uow.SalvarAsync(ct);

        return Result.Ok(empresa.ToDto());
    }
}

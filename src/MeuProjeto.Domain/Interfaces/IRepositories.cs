using MeuProjeto.Domain.Entities;

namespace MeuProjeto.Domain.Interfaces;

public interface IEmpresaRepository
{
    Task<Empresa?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Empresa?> ObterPorSlugAsync(string slug, CancellationToken ct = default);
    Task<bool> SlugExisteAsync(string slug, CancellationToken ct = default);
    Task AdicionarAsync(Empresa empresa, CancellationToken ct = default);
    void Atualizar(Empresa empresa);
}

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Cliente?> ObterPorCodigoAcessoAsync(string codigoAcesso, CancellationToken ct = default);
    Task<List<Cliente>> ListarPorEmpresaAsync(Guid empresaId, CancellationToken ct = default);
    Task AdicionarAsync(Cliente cliente, CancellationToken ct = default);
    void Atualizar(Cliente cliente);
}

public interface IProjetoRepository
{
    Task<Projeto?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Projeto?> ObterPorCodigoPublicoAsync(string codigoPublico, CancellationToken ct = default);
    Task<List<Projeto>> ListarPorEmpresaAsync(Guid empresaId, CancellationToken ct = default);
    Task<List<Projeto>> ListarPorClienteAsync(Guid clienteId, CancellationToken ct = default);
    Task AdicionarAsync(Projeto projeto, CancellationToken ct = default);
    void Atualizar(Projeto projeto);
}

public interface IUnitOfWork
{
    Task<int> SalvarAsync(CancellationToken ct = default);
}

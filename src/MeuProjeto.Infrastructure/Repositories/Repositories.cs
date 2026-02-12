using Microsoft.EntityFrameworkCore;
using MeuProjeto.Domain.Entities;
using MeuProjeto.Domain.Interfaces;
using MeuProjeto.Infrastructure.Data;

namespace MeuProjeto.Infrastructure.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly AppDbContext _db;
    public EmpresaRepository(AppDbContext db) => _db = db;

    public async Task<Empresa?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Empresas.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<Empresa?> ObterPorSlugAsync(string slug, CancellationToken ct = default)
        => await _db.Empresas.FirstOrDefaultAsync(e => e.Slug == slug, ct);

    public async Task<bool> SlugExisteAsync(string slug, CancellationToken ct = default)
        => await _db.Empresas.AnyAsync(e => e.Slug == slug, ct);

    public async Task AdicionarAsync(Empresa empresa, CancellationToken ct = default)
        => await _db.Empresas.AddAsync(empresa, ct);

    public void Atualizar(Empresa empresa)
        => _db.Empresas.Update(empresa);
}

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _db;
    public ClienteRepository(AppDbContext db) => _db = db;

    public async Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Clientes.Include(c => c.Projetos).FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Cliente?> ObterPorCodigoAcessoAsync(string codigoAcesso, CancellationToken ct = default)
        => await _db.Clientes.Include(c => c.Projetos).FirstOrDefaultAsync(c => c.CodigoAcesso == codigoAcesso, ct);

    public async Task<List<Cliente>> ListarPorEmpresaAsync(Guid empresaId, CancellationToken ct = default)
        => await _db.Clientes.Include(c => c.Projetos).Where(c => c.EmpresaId == empresaId).OrderBy(c => c.Nome).ToListAsync(ct);

    public async Task AdicionarAsync(Cliente cliente, CancellationToken ct = default)
        => await _db.Clientes.AddAsync(cliente, ct);

    public void Atualizar(Cliente cliente)
        => _db.Clientes.Update(cliente);
}

public class ProjetoRepository : IProjetoRepository
{
    private readonly AppDbContext _db;
    public ProjetoRepository(AppDbContext db) => _db = db;

    private IQueryable<Projeto> ProjetoCompleto => _db.Projetos
        .Include(p => p.Cliente)
        .Include(p => p.Empresa)
        .Include(p => p.Arquivos)
        .Include(p => p.Historico)
        .Include(p => p.Parcelas);

    public async Task<Projeto?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => await ProjetoCompleto.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Projeto?> ObterPorCodigoPublicoAsync(string codigoPublico, CancellationToken ct = default)
        => await ProjetoCompleto.FirstOrDefaultAsync(p => p.CodigoPublico == codigoPublico, ct);

    public async Task<List<Projeto>> ListarPorEmpresaAsync(Guid empresaId, CancellationToken ct = default)
        => await ProjetoCompleto.Where(p => p.EmpresaId == empresaId).OrderByDescending(p => p.CriadoEm).ToListAsync(ct);

    public async Task<List<Projeto>> ListarPorClienteAsync(Guid clienteId, CancellationToken ct = default)
        => await ProjetoCompleto.Where(p => p.ClienteId == clienteId).OrderByDescending(p => p.CriadoEm).ToListAsync(ct);

    public async Task AdicionarAsync(Projeto projeto, CancellationToken ct = default)
        => await _db.Projetos.AddAsync(projeto, ct);

    public void Atualizar(Projeto projeto)
        => _db.Projetos.Update(projeto);
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MeuProjeto.Application.Services;
using MeuProjeto.Domain.Interfaces;
using MeuProjeto.Infrastructure.Data;
using MeuProjeto.Infrastructure.Repositories;

namespace MeuProjeto.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IProjetoRepository, ProjetoRepository>();

        services.AddScoped<EmpresaService>();
        services.AddScoped<ProjetoService>();

        return services;
    }

    public static IServiceCollection AddInfrastructureInMemory(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("MeuProjetoDb"));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IProjetoRepository, ProjetoRepository>();

        services.AddScoped<EmpresaService>();
        services.AddScoped<ProjetoService>();

        return services;
    }
}

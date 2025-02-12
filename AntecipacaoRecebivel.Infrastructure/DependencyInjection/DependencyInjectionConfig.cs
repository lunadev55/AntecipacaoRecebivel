using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.Application.Services;
using AntecipacaoRecebivel.Data.Repositories;
using AntecipacaoRecebivel.Infrastructure.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace AntecipacaoRecebivel.Infrastructure.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();

            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<INotaFiscalService, NotaFiscalService>();
            services.AddScoped<IAntecipacaoRecebivelService, AntecipacaoRecebivelService>();

            return services;
        }
    }
}
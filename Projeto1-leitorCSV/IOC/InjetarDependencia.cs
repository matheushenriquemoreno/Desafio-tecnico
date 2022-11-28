using System.Runtime.CompilerServices;
using Application.Services;
using Application.Services.Interfaces;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace IOC
{
    public static class InjetarDependencia
    {
        public static void RegistrarDependencias(this IServiceCollection service)
        {
            service.AddTransient<IServicoArquivosCSV, ServicoArquivosCSV>();
            service.AddTransient<IServicoFuncionario, ServicoFuncionario>();
            service.AddTransient<RepositoryDepartamentoCSV, RepositoryDepartamentoCSV>();
            service.AddTransient<IServicoDepartamento, ServicoDepartamento>();
        }
    }
}
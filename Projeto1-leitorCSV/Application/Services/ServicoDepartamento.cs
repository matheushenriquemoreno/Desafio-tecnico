using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTO;
using Application.Services.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;

namespace Application.Services
{
    public class ServicoDepartamento : IServicoDepartamento
    {
        private readonly RepositoryDepartamentoCSV repositoryDepartamento;
        private readonly IServicoFuncionario serviceFuncionario;
        private readonly IServicoArquivosCSV servicoArquivos;

        public ServicoDepartamento(RepositoryDepartamentoCSV repositoryDepartamento,
            IServicoFuncionario serviceFuncionario,
            IServicoArquivosCSV servicoArquivos)
        {
            this.repositoryDepartamento = repositoryDepartamento;
            this.serviceFuncionario = serviceFuncionario;
            this.servicoArquivos = servicoArquivos;
        }

        public async Task<List<DepartamentoConsolidadoDTO>> BuscarDepartamentosConsolidado(string caminhoPastaArquivos)
        {
            List<DepartamentoConsolidadoDTO> departamentos = new();

            var arquivos = servicoArquivos.ObterTodosArquivosCSVDeUmDiretorio(caminhoPastaArquivos);

            var result = arquivos.Select(async caminhoArquivo =>
            {
                departamentos.Add(await BuscarDepartamentoConsolidado(caminhoArquivo));
            }).ToArray();

            await Task.WhenAll(result);

            return departamentos;

        }

        public async Task<DepartamentoConsolidadoDTO> BuscarDepartamentoConsolidado(string caminho)
        {
            var departamento = await repositoryDepartamento.BuscarDepartamento(caminho);

            var dadosFuncionario = await serviceFuncionario.ConsolidaDadosFuncionariosDepartamento(departamento);

            return await RetornaDepartamentoComDadosConsolidados(departamento, dadosFuncionario);
        }

        private async Task<DepartamentoConsolidadoDTO> RetornaDepartamentoComDadosConsolidados(Departamento departamento, List<FuncionarioDTO> dadosFuncionario)
        {
            var totalDescontoDepartamento = await CalcularTotalDescontosDepartamento(dadosFuncionario, departamento.Funcionarios);
            var totalHorasExtraDepartamento = await CalculaTotalHotasExtraDepartamento(dadosFuncionario, departamento.Funcionarios);
            var totalPagarDepartamento = Math.Round(dadosFuncionario.Select(x => x.TotalReceber).Sum(), 2);

            var departamentoDTO = new DepartamentoConsolidadoDTO()
            {
                AnoVirgencia = departamento.AnoVirgencia,
                Departameto = departamento.Departameto,
                MesVirgencia = departamento.MesVirgencia,
                TotalDescontos = totalDescontoDepartamento,
                TotalPagar = totalPagarDepartamento,
                TotalExtras = totalHorasExtraDepartamento,
                Funcionarios = dadosFuncionario,

            };

            return departamentoDTO;
        }

        private async Task<double> CalculaTotalHotasExtraDepartamento(List<FuncionarioDTO> dadosFuncionario, List<Funcionario> funcionarios)
        {
            double quantidadeAPagar = 0;

            var result = dadosFuncionario.Select(async funcionario =>
            {
                if (funcionario.HorasExtras > 0)
                {
                    var valor = funcionarios.Where(x => x.Codigo == funcionario.Codigo).First().ValorHora;

                    quantidadeAPagar += funcionario.HorasExtras * valor;
                }
            });

            await Task.WhenAll(result);

            return quantidadeAPagar;
        }

        private async Task<double> CalcularTotalDescontosDepartamento(List<FuncionarioDTO> dadosFuncionario, List<Funcionario> funcionarios)
        {
            double quantidadeAPagar = 0;

            var result = dadosFuncionario.Select(async funcionario =>
            {
                if (funcionario.HorasDebito > 0)
                {
                    var valor = funcionarios.Where(x => x.Codigo == funcionario.Codigo).First().ValorHora;

                    quantidadeAPagar += funcionario.HorasDebito * valor;
                }
            });

            await Task.WhenAll(result);

            return quantidadeAPagar;
        }
    }
}

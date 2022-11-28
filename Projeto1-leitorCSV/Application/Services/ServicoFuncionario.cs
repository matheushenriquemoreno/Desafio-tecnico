using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Services.Interfaces;
using Domain.Entities;
using Utils.ExtensionMethods;

namespace Application.Services
{
    public class ServicoFuncionario : IServicoFuncionario
    {
        public int HorasATrabalhar { get; private set; }

        public int DiasATrabalhar { get; private set; }

        private int CargaHorariaEstimada { get { return HorasATrabalhar * DiasATrabalhar; } }

        public ServicoFuncionario()
        {
            /// <summary>
            /// Peguei o padrão informado 8 horas ao dia
            /// </summary>
            HorasATrabalhar = 9;

            /// <summary>
            /// Peguei o padrão informado 20 dias trabalhados
            /// </summary>
            DiasATrabalhar = 20;
        }

        public void SetarQuantidadeDeDiasATrabalhar(int dias)
        {
            DiasATrabalhar = dias;
        }
        public void SetarQuantidadeDeHorasATrabalhar(int horas)
        {
            HorasATrabalhar = horas;
        }

        public async Task<List<FuncionarioDTO>> ConsolidaDadosFuncionariosDepartamento(Departamento departamento)
        {
            var tarefas = departamento.Funcionarios.Select(async x =>
            {
                return await ConsolidaDadosFuncionario(x);
            });

            var result = Task.WhenAll(tarefas).Result.ToList();

            return result;
        }

        private async Task<FuncionarioDTO> ConsolidaDadosFuncionario(Funcionario funcionario)
        {
            FuncionarioDTO consolidado = new();

            consolidado.Codigo = funcionario.Codigo;
            consolidado.Nome = funcionario.Nome;
            consolidado.DiasTrabalhados = await CalculaDiasTrabalhados(funcionario);
            consolidado.HorasExtras = await CalculaHorasExtraFuncionario(funcionario);
            consolidado.HorasDebito = await CalculaHoraDebitoDeHorasFuncionario(funcionario);
            consolidado.TotalReceber = await CalculaTotalReceberFuncionario(funcionario, funcionario.ValorHora);
            consolidado.DiasFalta = await CalculaDiasFalta(consolidado.HorasDebito);
            consolidado.DiasExtras = await CalculaDiasExtras(consolidado.HorasExtras);

            return consolidado;
        }

        private async Task<int> CalculaDiasTrabalhados(Funcionario funcionario)
        {
            var dias = funcionario.QuantidadeHorasTrabalhadas() / HorasATrabalhar;

            if (funcionario.QuantidadeHorasTrabalhadas() == CargaHorariaEstimada)
                    return DiasATrabalhar;

            return (int)Math.Floor(dias);
        }

        private async Task<int> CalculaDiasFalta(double debitos)
        {
            var dias = debitos / HorasATrabalhar;

            return ((int)Math.Ceiling(dias));
        }
        private async Task<int> CalculaDiasExtras(double horasExtras)
        {
            var dias = horasExtras / HorasATrabalhar;

            return ((int)Math.Floor(dias));
        }
        private async Task<double> CalculaHorasExtraFuncionario(Funcionario funcionario)
        {

            if (funcionario.QuantidadeHorasTrabalhadas() <= CargaHorariaEstimada)
                return 0;

            var horasExtra = funcionario.QuantidadeHorasTrabalhadas() - CargaHorariaEstimada;

            return horasExtra;
        }
        private async Task<double> CalculaHoraDebitoDeHorasFuncionario(Funcionario funcionario)
        {

            if (funcionario.QuantidadeHorasTrabalhadas() >= CargaHorariaEstimada)
                return 0;

            var horas = CargaHorariaEstimada - funcionario.QuantidadeHorasTrabalhadas();

            return horas;
        }
        private async Task<double> CalculaTotalReceberFuncionario(Funcionario funcionario, double valorHora)
        {
            var valorAReceber = funcionario.QuantidadeHorasTrabalhadas() * valorHora;

            return Math.Round(valorAReceber, 2);
        }
    }
}

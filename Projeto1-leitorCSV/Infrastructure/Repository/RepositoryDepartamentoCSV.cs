using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper;
using Infrastructure.DTOS;
using System.Runtime.CompilerServices;
using Domain.Entities;
using Infrastructure.MapCSV;
using System.Drawing;

namespace Infrastructure.Repository
{
    public class RepositoryDepartamentoCSV
    {
        public RepositoryDepartamentoCSV()
        {

        }

        /// <summary>
        /// Faz a conecao com a biblioteca CSVHelper que vai ler o arquivo e envia os dados para action tratar esses dados
        /// </summary>
        /// <param name="arquivo"></param>
        /// <param name="action"></param>
        private void ConecaoComOsDados(string arquivo, Action<DadosUsuarioCSVDTO> action)
        {
            var config = new CsvConfiguration(new CultureInfo("pt-BR"))
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                Encoding = Encoding.UTF8
            };

            using (var reader = new StreamReader(arquivo, Encoding.UTF8))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<DadosUsuarioCSVDTOMap>();

                while (csv.Read())
                {
                    var item = csv.GetRecord<DadosUsuarioCSVDTO>();
                    action(item);
                }
            }
        }

        public async Task<Departamento> BuscarDepartamento(string arquivo)
        {
            Departamento departamento = CriaDepartamento(arquivo);
            var listaUsuarios = new List<Funcionario>();

            ConecaoComOsDados(arquivo, async (usuario) =>
            {
                listaUsuarios = await RetornaFuncionarios(listaUsuarios, usuario);
            });

            departamento.AdicionarFuncionarios(listaUsuarios);

            return departamento;
        }

        private Departamento CriaDepartamento(string caminho)
        {
            var arquivo = new FileInfo(caminho);

            var nomeAquivo = arquivo.Name.Contains("-") ? arquivo.Name.Split("-") : Array.Empty<string>();

            if (nomeAquivo.Length >= 2)
            {
                var nomeDepartamento = nomeAquivo[0];
                var mesvirgencia = nomeAquivo[1];
                var anoVirgencia = nomeAquivo[2].Replace(arquivo.Extension, "");
                return new Departamento(nomeDepartamento, mesvirgencia, anoVirgencia);
            }
            return new Departamento(arquivo.Name.Replace(arquivo.Extension, ""), "", "");
        }

        private async Task<List<Funcionario>> RetornaFuncionarios(List<Funcionario> listaUsuarios, DadosUsuarioCSVDTO usuarioCSV)
        {
            Funcionario funcionario = null;

            if (await VerificaUsuarioJaEstaNalista(listaUsuarios, usuarioCSV))
                funcionario = listaUsuarios.Where(u => u.Codigo == usuarioCSV.Codigo).FirstOrDefault();
            else
            {
                funcionario = new Funcionario(usuarioCSV.Nome, usuarioCSV.Codigo, ConverterValorHora(usuarioCSV.ValorHora));
                listaUsuarios.Add(funcionario);
            }

            var diaTrabalhado = await MontaDiaTrabalhadoDoFuncionario(usuarioCSV);

            funcionario.AdicionarDiaTrabalhado(diaTrabalhado);

            return listaUsuarios;
        }

        private double ConverterValorHora(string valorHora)
        {
            var valorFormatado = valorHora.Replace("R$", "").Replace(", ", ",");

            double valor = 0;
            double.TryParse(valorFormatado, out valor);

            return valor;
        }

        private async Task<DiaTrabalhado> MontaDiaTrabalhadoDoFuncionario(DadosUsuarioCSVDTO usuarioCSV)
        {
            var dia = DateTime.Parse(usuarioCSV.Data);

            var horaEntrada = TimeSpan.Parse(usuarioCSV.Entrada);
            var horaSaida = TimeSpan.Parse(usuarioCSV.Saida);

            var horasAlmocoSeparada = usuarioCSV.Almoco.Split('-');

            if (ExisteHorarioDeAlmoco(usuarioCSV))
            {
                var horaSaidaAlmoco = TimeSpan.Parse(horasAlmocoSeparada[0].Trim());
                var horaVoltouAlmoco = TimeSpan.Parse(horasAlmocoSeparada[1].Trim());
                return new DiaTrabalhado(dia, horaEntrada, horaSaida, horaSaidaAlmoco, horaVoltouAlmoco);
            }

            return new DiaTrabalhado(dia, horaEntrada, horaSaida, null, null);
        }

        /// <summary>
        /// Pode acontecer do colaborador trabalhar somente parte da tarde ou da manha, fazendo assim nao ter horario de almoco.
        /// </summary>
        /// <param name="usuarioCSV"></param>
        /// <returns></returns>
        private bool ExisteHorarioDeAlmoco(DadosUsuarioCSVDTO usuarioCSV)
        {
            return usuarioCSV.Almoco.Split('-').Length > 1;
        }

        private async Task<bool> VerificaUsuarioJaEstaNalista(List<Funcionario> listaUsuarios, DadosUsuarioCSVDTO usuarioCSV)
        {
            return listaUsuarios.Where(u => u.Codigo == usuarioCSV.Codigo).Any();
        }

    }
}

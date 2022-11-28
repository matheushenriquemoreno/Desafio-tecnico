using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Application.DTO;
using Application.Services.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Application.Services
{
    public class ServicoArquivosCSV : IServicoArquivosCSV
    {
        public ServicoArquivosCSV()
        {

        }

        public bool VerificaSeArquivoExiste(string caminho)
        {
            return File.Exists(caminho) && caminho.EndsWith(".csv");
        }

        public bool VerificaSeDiretorioExiste(string caminho)
        {
            return Directory.Exists(caminho);
        }

        public string[] ObterTodosArquivosCSVDeUmDiretorio(string caminhoAbsoluto)
        {
            if (!Directory.Exists(caminhoAbsoluto)) return new string[] { };

            var files = Directory.GetFiles(caminhoAbsoluto).Where(x => x.EndsWith(".csv")).ToArray();

            return files;
        }

        public async Task AdicionarDepartamentosConsolidadosEmArquivoJson(List<DepartamentoConsolidadoDTO> departamentos, string caminho)
        {
           var json = JsonConvert.SerializeObject(departamentos, Formatting.Indented);

            byte[] dados = new UTF8Encoding().GetBytes(json);

            var arquivo = new FileInfo(caminho);

            await EscreverJsonNoArquivo(arquivo.FullName, "Departamentos", dados);
        }

        public async Task AdicionarDepartamentoConsolidadoEmArquivoJson(DepartamentoConsolidadoDTO departamento, string caminho)
        {
            var json = JsonConvert.SerializeObject(departamento, Formatting.Indented);

            byte[] dados = new UTF8Encoding().GetBytes(json);

            var arquivo = new FileInfo(caminho);

            await EscreverJsonNoArquivo(arquivo.Directory.FullName, departamento.Departameto, dados);
        }

        private async Task EscreverJsonNoArquivo(string diretorioArquivo, string nomeArquivoCriar, byte[] dados)
        {
            var diretorioJson = diretorioArquivo + "\\Departamentos-Json";

            Directory.CreateDirectory(diretorioJson);

            var arquivoJson = diretorioJson + $"\\{nomeArquivoCriar}-{DateTime.Now.ToString("dd-MM-yyyy-H-m-s")}.json";

            using (FileStream fs = File.Create(arquivoJson))
            {
                await fs.WriteAsync(dados, 0, dados.Length);
            }
        }
    }
}

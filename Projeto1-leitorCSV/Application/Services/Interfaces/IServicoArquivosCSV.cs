using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Services.Interfaces
{
    public interface IServicoArquivosCSV
    {
        bool VerificaSeArquivoCSVExiste(string caminho);

        bool VerificaSeDiretorioExiste(string caminho);

        string[] ObterTodosArquivosCSVDeUmDiretorio(string caminhoAbsoluto);

        Task AdicionarDepartamentosConsolidadosEmArquivoJson(List<DepartamentoConsolidadoDTO> departamentos, string caminho);

        Task AdicionarDepartamentoConsolidadoEmArquivoJson(DepartamentoConsolidadoDTO departamento, string caminho);

    }
}

using System.Diagnostics;
using Application.DTO;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ServicoWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly IServicoArquivosCSV servicoArquivos;
        private readonly IServicoDepartamento servicoDepartamento;

        public HomeController(IServicoArquivosCSV servicoArquivos, IServicoDepartamento servicoDepartamento)
        {
            this.servicoArquivos = servicoArquivos;
            this.servicoDepartamento = servicoDepartamento;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProcessarVariosArquivos(string pastaArquivos)
        {
            if (!servicoArquivos.VerificaSeDiretorioExiste(pastaArquivos)) 
            {
                return BadRequest(new { sucess = false, menssage = "Diretorio informado não existente!" });
            }

            if (servicoArquivos.ObterTodosArquivosCSVDeUmDiretorio(pastaArquivos).Length == 0)
            {
                return BadRequest(new { sucess = false, menssage = "Diretorio informado existente, mas sem nenhum arquivo '.csv'" });
            }

            try
            {
                var departamentos = await servicoDepartamento.BuscarDepartamentosConsolidado(pastaArquivos);

                await servicoArquivos.AdicionarDepartamentosConsolidadosEmArquivoJson(departamentos, pastaArquivos);

                var quantidadeDepartamento = departamentos.Count();

                return Json(new { sucess = true, menssage = $"Foram processados {quantidadeDepartamento} departamentos, que se encontram dentro da pasta de origem informada, na pasta 'Departamentos-Json'." });
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, menssage = "Houve um erro por parte do processamento, Certifique que os arquivos estão no formato correto!" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProcessarUmArquivo(string caminhoArquivo)
        {
            if (!servicoArquivos.VerificaSeArquivoCSVExiste(caminhoArquivo))
            {
                return BadRequest(new { sucess = false, menssage = "Arquivo informado não existente!" });
            }

            try
            {
                var departamento = await servicoDepartamento.BuscarDepartamentoConsolidado(caminhoArquivo);

                await servicoArquivos.AdicionarDepartamentoConsolidadoEmArquivoJson( departamento  , caminhoArquivo);

                return Json(new { sucess = true, menssage = $"O Departamento: {departamento.Departameto}, foi processado com sucesso, o resultado se encontra dentro da pasta de origem informada, na pasta 'Departamentos-Json'." });
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, menssage = "Houve um erro por parte do processamento, Certifique que os arquivos estão no formato correto!" });
            }
        }
    }
}
/* Sistema desenvolvido somente para testes rapidos */

using Application.DTO;
using Application.Services;
using Infrastructure.Repository;

var caminhoArquivo = @"C:\Users\mathe\Downloads\Teste de Dev (geral todos os níveis) (1)\exemplo de arquivo de entrada-abril-2022.csv";

var caminhoPastaArquivos = @"C:\Users\mathe\Downloads\Teste de Dev (geral todos os níveis) (1)\varios-arquivos";

var servicoFuncionario = new ServicoFuncionario();
var repositoryDepartamento = new RepositoryDepartamentoCSV();
var servicoArquivos = new ServicoArquivosCSV();
var servicodepartamento = new ServicoDepartamento(repositoryDepartamento, servicoFuncionario, servicoArquivos);

try
{
    CaminhoComVariosArquivos();
    CaminhoArquivo();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

async Task CaminhoComVariosArquivos()
{
    List<DepartamentoConsolidadoDTO> departamentos = new();

    var arquivos = servicoArquivos.ObterTodosArquivosCSVDeUmDiretorio(caminhoPastaArquivos);

   var result = arquivos.Select(async caminhoArquivo =>
    {
        departamentos.Add(await servicodepartamento.BuscarDepartamentoConsolidado(caminhoArquivo));
    }).ToArray();

    await Task.WhenAll(result);

    await servicoArquivos.AdicionarDepartamentosConsolidadosEmArquivoJson(departamentos, caminhoPastaArquivos);
}


async Task CaminhoArquivo()
{
    var departamento = await servicodepartamento.BuscarDepartamentoConsolidado(caminhoArquivo);

    await servicoArquivos.AdicionarDepartamentoConsolidadoEmArquivoJson(departamento, caminhoArquivo);
}
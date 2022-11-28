let RequisicioesEnviadas = [];

AdicionarEvento("#close-modal", 'click', () => {
    $('.modal').modal('hide')
})

AdicionarEvento("#form-dados", "submit", async (e) => {
    e.preventDefault();
    if (CaminhoArquivoEstaVazio())
        return false;


    var idInputMarcado = document.querySelector('input[name="exampleRadios"]:checked').id;

    const LerVariosArquivos = idInputMarcado === "ler-pasta-arquivos"
    const LerUnicoArquivo = idInputMarcado === "ler-somente-arquivo"
    let PastaOuCaminho = document.querySelector("#caminho-pasta").value

    PastaOuCaminho = PastaOuCaminho.replace(`'\'`, '\\');

    let retorno = null;

    if (LerVariosArquivos) {
        retorno = await EnviarVariosArquivos(PastaOuCaminho);
    }

    if (LerUnicoArquivo) {
        retorno = await EnviarDadosArquivo(PastaOuCaminho);
    }

    adicionarRequisicaoEnviada(LerVariosArquivos ? 'Ler varios Arquivos' : "Ler unico Arquivo", PastaOuCaminho, retorno.sucess);
    atualizaTabelaDados();
    ModalRetornoInformativo(retorno)
})

AdicionarEvento("#limpar", "click", (e) => {
    e.preventDefault();
    document.querySelector("#caminho-pasta").value = "";
})

function ModalRetornoInformativo(retorno) {
    document.querySelector(".modal-title").innerHTML = ''
    document.querySelector("#textoRetorno").innerHTML = ''

    if (retorno.sucess) {
        document.querySelector(".modal-title").innerHTML = "Sucesso ao processar os dados."
        document.querySelector("#textoRetorno").innerHTML = retorno.menssage
        document.querySelector("#textoRetorno").classList.remove("text-danger")
    } else {
        document.querySelector(".modal-title").innerHTML = "Houve um erro no caminho ou pasta informada."
        document.querySelector("#textoRetorno").innerHTML = retorno.menssage
        document.querySelector("#textoRetorno").classList.add("text-danger")
    }

    $('.modal').modal('show')
}

function CaminhoArquivoEstaVazio() {
    if (document.querySelector("#caminho-pasta").value === '') {
        document.querySelector("#Validainput").innerHTML = "Caminho não pode ser vazio"
        return true;
    } else {
        document.querySelector("#Validainput").innerHTML = ""
    }
    return false
}

function adicionarRequisicaoEnviada(tipo, caminho, sucess) {
    RequisicioesEnviadas.push({ tipo, caminho, sucess })
}

function atualizaTabelaDados() {
    document.querySelectorAll('tbody').forEach(elemento => elemento.remove())

    let tbody = document.createElement("tbody")

    RequisicioesEnviadas.forEach((dado, index) => {

        let tr = document.createElement("tr")

        let id = document.createElement("th")
        id.innerHTML = index + 1
        tr.appendChild(id)

        let tipo = document.createElement("td")
        tipo.innerHTML = dado.tipo
        tr.appendChild(tipo)

        let caminhoArquivo = document.createElement("td")
        caminhoArquivo.innerHTML = dado.caminho
        tr.appendChild(caminhoArquivo)

        let processoRealizado = document.createElement("td")
        processoRealizado.innerHTML = dado.sucess ? "Sim" : "Não"
        tr.appendChild(processoRealizado)

        tbody.appendChild(tr);
    })

    document.querySelector(".dadosRequisicoes").appendChild(tbody)
}

async function EnviarDadosArquivo(caminho) {
    const url = `https://localhost:7161/Home/ProcessarUmArquivo?caminhoArquivo=${caminho}`

    let consulta = await fetch(url)

    return await consulta.json();
}

async function EnviarVariosArquivos(pastasComArquivos) {
    const url = `https://localhost:7161/Home/ProcessarVariosArquivos?pastaArquivos=${pastasComArquivos}`

    let consulta = await fetch(url)

    return await consulta.json();
}

function AdicionarEvento(tag, evento, calback) {
    return document.querySelector(tag).addEventListener(evento, calback);
}

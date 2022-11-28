using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Services.Interfaces
{
    public interface IServicoDepartamento
    {
         Task<List<DepartamentoConsolidadoDTO>> BuscarDepartamentosConsolidado(string caminhoPastaArquivos);

         Task<DepartamentoConsolidadoDTO> BuscarDepartamentoConsolidado(string caminho);
    }
}

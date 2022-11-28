using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IServicoFuncionario
    {
        int HorasATrabalhar { get; }

        int DiasATrabalhar { get; }

        void SetarQuantidadeDeDiasATrabalhar(int dias);
        void SetarQuantidadeDeHorasATrabalhar(int dias);

        Task<List<FuncionarioDTO>> ConsolidaDadosFuncionariosDepartamento(Departamento departamento);
    }
}

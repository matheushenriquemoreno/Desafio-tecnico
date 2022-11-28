using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class DepartamentoConsolidadoDTO
    {
        public string Departameto { get;  set; }
        public string MesVirgencia { get;  set; }
        public string AnoVirgencia { get;  set; }
        public double TotalPagar { get;  set; }
        public double TotalDescontos { get;  set; }
        public double TotalExtras { get;  set; }
        public List<FuncionarioDTO> Funcionarios { get; set; }

        public DepartamentoConsolidadoDTO()
        {
            Funcionarios = new List<FuncionarioDTO>();
        }
    }
}

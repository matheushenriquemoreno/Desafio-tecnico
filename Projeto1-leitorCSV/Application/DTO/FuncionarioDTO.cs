using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class FuncionarioDTO
    {
        public string Nome { get;  set; }
        public int Codigo { get;  set; }
        public double TotalReceber { get; set; }
        public double HorasExtras { get; set; }
        public double HorasDebito { get; set; }
        public int DiasFalta { get; set; }
        public int DiasExtras { get; set; }
        public int DiasTrabalhados { get; set; }

        public FuncionarioDTO()
        {

        }

        public bool FuncionarioFaltou()
        {
            return DiasFalta > 0;
        }
    }
}

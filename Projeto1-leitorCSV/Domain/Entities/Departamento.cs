using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Departamento
    {
        public string Departameto { get; private set; }
        public string MesVirgencia { get; private set; }
        public string AnoVirgencia { get; private set; }
        public decimal TotalPagar { get; private set; }
        public decimal TotalDescontos { get; private set; }
        public decimal TotalExtras { get; private set; }

        public List<Funcionario> Funcionarios { get; private set; }

        public Departamento(string departameto, string mesVirgencia, string anoVirgencia)
        {
            Departameto = departameto;
            MesVirgencia = mesVirgencia;
            AnoVirgencia = anoVirgencia;
            Funcionarios = new List<Funcionario>();
        }

        public void AdicionarFuncionarios(List<Funcionario> funcionarios)
        {
            this.Funcionarios = funcionarios;
        }
    }
}

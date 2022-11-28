using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Funcionario
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }
        public double ValorHora { get; private set; }
        public List<DiaTrabalhado> DiasTrabalhados { get; set; }

        public Funcionario(string nome, int codigo, double valorHora)
        {
            Nome = nome;
            Codigo = codigo;
            ValorHora = valorHora;
            DiasTrabalhados = new List<DiaTrabalhado>();
        }

        public void AdicionarDiaTrabalhado(DiaTrabalhado dia)
        {
            DiasTrabalhados.Add(dia);
        }

        public double QuantidadeHorasTrabalhadas()
        {
            return DiasTrabalhados.Select(dia => dia.HorasTrabalhadas().TotalHours).Sum();
        }
    }
}

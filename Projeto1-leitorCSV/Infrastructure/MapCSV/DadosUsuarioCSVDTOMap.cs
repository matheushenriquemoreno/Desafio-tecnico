using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Infrastructure.DTOS;

namespace Infrastructure.MapCSV
{
    internal class DadosUsuarioCSVDTOMap : ClassMap<DadosUsuarioCSVDTO>
    {
        public DadosUsuarioCSVDTOMap()
        {
            Map(x => x.Codigo).Index(0);
            Map(x => x.Nome).Index(1).Convert((args) =>
            {
                var nome = args.Row.GetField(1);

                return Convert.ToString(nome, new CultureInfo("pt-BR"));
            });
            Map(x => x.ValorHora).Index(2);
            Map(x => x.Data).Index(3);
            Map(x => x.Entrada).Index(4);
            Map(x => x.Saida).Index(5);
            Map(x => x.Almoco).Index(6);
        }
    }
}

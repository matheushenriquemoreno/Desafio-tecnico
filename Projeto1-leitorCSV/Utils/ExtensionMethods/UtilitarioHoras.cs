using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ExtensionMethods
{
    public static class UtilitarioHoras
    {

        public static double TranformaMinutosEmHoras(this int minutos)
        {
            return minutos / 60;
        }

        public static int TranformaHoraEmMinutos(this double horas)
        {
            return (int)(60 * horas);
        }

        public static int TranformaHoraEmMinutos(this int horas)
        {
            return (int)(60 * horas);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DiaTrabalhado
    {
        public DateTime DiaDoTrabalho { get; private set; }

        public TimeSpan HoraEntrada { get; private set; }

        public TimeSpan HoraSaida { get; private set; }

        public TimeSpan? HoraSaidaAlmoco { get; private set; }

        public TimeSpan? HoraVoltouAlmoco { get; private set; }

        public DiaTrabalhado(DateTime diaDoTrabalho, TimeSpan horaEntrada, TimeSpan horaSaida, TimeSpan? horaSaidaAlmoco, TimeSpan? horaVoltouAlmoco)
        {
            DiaDoTrabalho = diaDoTrabalho;
            HoraEntrada = horaEntrada;
            HoraSaida = horaSaida;
            HoraSaidaAlmoco = horaSaidaAlmoco;
            HoraVoltouAlmoco = horaVoltouAlmoco;
        }

        public TimeSpan IntervaloAlmoco()
        {
            if (!HoraSaidaAlmoco.HasValue && !HoraVoltouAlmoco.HasValue)
                return new TimeSpan(0, 0, 0);

            return HoraSaidaAlmoco.Value.Subtract(HoraVoltouAlmoco.Value);
        }

        public TimeSpan HorasTrabalhadas()
        {
            var horasTrabalhadasSemDescontarAlmoco = HoraSaida.Subtract(HoraEntrada);

            var horasTrabalhadas = horasTrabalhadasSemDescontarAlmoco + IntervaloAlmoco();

            return horasTrabalhadas;
        }

        public bool PossuiDebitoHora(int quantidadeHoraATrabalhar)
        {
            return HorasTrabalhadas().TotalHours < quantidadeHoraATrabalhar;
        }


    }
}

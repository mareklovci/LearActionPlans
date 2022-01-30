using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class KontrolaEfektivnostiViewModel
    {
        public int Id { get; set; }
        public DateTime PuvodniDatum { get; set; }
        public DateTime DatumOdstraneni { get; set; }
        public string Poznamka { get; set; }

        public static KontrolaEfektivnostiViewModel PuvodniTerminyEfektivnost(DateTime puvodniDatum, DateTime datumOdstraneni, string poznamka)
        {
            var kontrolaEfektivnostiViewModel = new KontrolaEfektivnostiViewModel
            {
                PuvodniDatum = puvodniDatum,
                DatumOdstraneni = datumOdstraneni,
                Poznamka = poznamka
            };

            return kontrolaEfektivnostiViewModel;
        }

        public static IEnumerable<KontrolaEfektivnostiViewModel> PuvodniTerminyEfektivnost(int bodAPId)
        {
            var kontrolaEfektivnosti = KontrolaEfektivnostiDataMapper.GetKontrolaEfektivnostiBodAPId(bodAPId).ToList();

            var query = from kE in kontrolaEfektivnosti
                        select PuvodniTerminyEfektivnost(kE.PuvodniDatum, kE.OdstranitDatum, kE.Poznamka);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                {
                    yield return q;
                }
            }
        }

    }
}

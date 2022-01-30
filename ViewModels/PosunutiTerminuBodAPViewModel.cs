using System;
using System.Collections.Generic;
using System.Linq;
using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class PosunutiTerminuBodAPViewModel
    {
        //akce
        public int Id { get; set; }
        public byte ZamitnutiTerminu { get; set; }
        public byte ZmenaTerminu { get; set; }

        //ukonceniBodAP
        public int UkonceniBodAPId { get; set; }
        public int BodAPId { get; set; }
        public DateTime DatumUkonceni { get; set; }
        public string Poznamka { get; set; }
        public string Odpoved { get; set; }
        public byte StavZadosti { get; set; }
        public byte StavObjektu { get; set; }

        public static PosunutiTerminuBodAPViewModel UkonceniBodAP(int ukonceniBodAPId, int bodAPId, DateTime datumUkonceni, string poznamka, string odpoved, byte stavZadosti, byte stavObjektu)
        {
            var posunutiTerminuAkceViewModel = new PosunutiTerminuBodAPViewModel
            {
                UkonceniBodAPId = ukonceniBodAPId,
                BodAPId = bodAPId,
                DatumUkonceni = datumUkonceni,
                Poznamka = poznamka,
                Odpoved = odpoved,
                StavZadosti = stavZadosti,
                StavObjektu = stavObjektu
            };

            return posunutiTerminuAkceViewModel;
        }

        public static PosunutiTerminuBodAPViewModel ZbyvajiciTerminy(int bodAPId, byte zamitnutiTerminu, byte zmenaTerminu)
        {
            var posunutiTerminuAkceViewModel = new PosunutiTerminuBodAPViewModel
            {
                Id = bodAPId,
                ZamitnutiTerminu = zamitnutiTerminu,
                ZmenaTerminu = zmenaTerminu
            };

            return posunutiTerminuAkceViewModel;
        }

        public static PosunutiTerminuBodAPViewModel ZavritPrvniTermin(int ukonceniBodAPId)
        {
            var posunutiTerminuAkceViewModel = new PosunutiTerminuBodAPViewModel
            {
                UkonceniBodAPId = ukonceniBodAPId
            };

            return posunutiTerminuAkceViewModel;
        }

        public static IEnumerable<PosunutiTerminuBodAPViewModel> GetUkonceniBodAP(int bodAPId)
        {
            var ukonceni = UkonceniBodAPDataMapper.GetUkonceniBodAPId(bodAPId).ToList();

            var query = ukonceni.Select(u => UkonceniBodAP(u.Id, u.BodAPId, u.DatumUkonceni, u.Poznamka, u.Odpoved,
                u.StavZadosti, u.StavObjektuUkonceni)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<PosunutiTerminuBodAPViewModel> GetZbyvajiciTerminy(int bodAPId)
        {
            var bodAP = BodAPDataMapper.GetZbyvajiciTerminyBodAPId(bodAPId).ToList();

            var query = bodAP.Select(b => ZbyvajiciTerminy(b.Id, b.ZamitnutiTerminu, b.ZmenaTerminu)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<PosunutiTerminuBodAPViewModel> GetZavritPrvniTermin(int bodAPId)
        {
            var ukonceni = UkonceniBodAPDataMapper.GetUkonceniBodAPId(bodAPId).ToList();

            var query = ukonceni.Where(u => u.BodAPId == bodAPId && u.StavZadosti == 1)
                .Select(u => ZavritPrvniTermin(u.Id)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }
    }
}

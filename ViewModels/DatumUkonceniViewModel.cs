using System.Collections.Generic;
using System.Linq;

using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class DatumUkonceniViewModel
    {
        //Start Zaměstnanci
        public int ZamestnanecId { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }

        public static DatumUkonceniViewModel Zamestnanec(int zamestnanecId, string jmeno, string prijmeni)
        {
            var datumUkonceniViewModel = new DatumUkonceniViewModel
            {
                ZamestnanecId = zamestnanecId,
                Jmeno = jmeno,
                Prijmeni = prijmeni
            };

            return datumUkonceniViewModel;
        }

        public static IEnumerable<DatumUkonceniViewModel> GetZamestnanec(int idZam)
        {
            var zamestnanci = ZamestnanciDataMapper.GetZamestnanciAll().ToList();

            var query = from z in zamestnanci
                        where z.Id == idZam
                        select Zamestnanec(z.Id, z.Jmeno, z.Prijmeni);

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

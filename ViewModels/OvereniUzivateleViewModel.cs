using System.Collections.Generic;
using System.Linq;

using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class OvereniUzivateleViewModel
    {
        public int ZadavatelId { get; set; }
        public string PrihlasovaciJmeno { get; set; }
        public bool Admin { get; set; }

        public static OvereniUzivateleViewModel Zadavatel(int zadavatelId, string prihlasovaciJmeno, bool admin)
        {
            var overeniUzivateleViewModel = new OvereniUzivateleViewModel
            {
                ZadavatelId = zadavatelId,
                PrihlasovaciJmeno = prihlasovaciJmeno,
                Admin = admin
            };
            return overeniUzivateleViewModel;
        }

        public static IEnumerable<OvereniUzivateleViewModel> GetZadavatelLogin(string login)
        {
            var zadavatel = ZamestnanciDataMapper.GetZadavatelLogin(login).ToList();

            var query = from z in zadavatel
                        select OvereniUzivateleViewModel.Zadavatel(z.Id, z.PrihlasovaciJmeno, z.AdminAP);

            if (query.Count() == 0)
            {
                yield break;
            }
            else
            {
                foreach (var q in query)
                    yield return q;
            }
        }
    }
}

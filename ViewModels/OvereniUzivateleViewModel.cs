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
            var zadavatel = EmployeeRepository.GetZadavatelLogin(login).ToList();

            var query = zadavatel.Select(z => Zadavatel(z.Id, z.PrihlasovaciJmeno, z.AdminAP)).ToList();

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

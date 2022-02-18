using System.Collections.Generic;
using System.Linq;
using LearActionPlans.Repositories;

namespace LearActionPlans.ViewModels
{
    public class NewActionPlanViewModel
    {
        public int ProjectId { get; private set; }
        public string ProjectName { get; private set; }
        public int CustomerId { get; private set; }
        public string CustomerName { get; private set; }

        public static NewActionPlanViewModel Project(int projectId, string name)
        {
            var actionPlanViewModel = new NewActionPlanViewModel {ProjectId = projectId, ProjectName = name};
            return actionPlanViewModel;
        }

        private static NewActionPlanViewModel Customer(int customerId, string name)
        {
            var actionPlanViewModel = new NewActionPlanViewModel {CustomerId = customerId, CustomerName = name};
            return actionPlanViewModel;
        }

        public static IEnumerable<NewActionPlanViewModel> GetProjects()
        {
            var projects = ProjektyDataMapper.GetProjektyAll().ToList();

            var query = projects.Where(p => p.StavObjektu == 1)
                .OrderBy(p => p.Nazev)
                .Select(p => Project(p.Id, p.Nazev)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }

        public static IEnumerable<NewActionPlanViewModel> GetCustomers()
        {
            var customers = CustomerRepository.GetZakazniciAll().ToList();

            var query = customers.Where(z => z.StavObjektu == 1)
                .OrderBy(z => z.Nazev)
                .Select(z => Customer(z.Id, z.Nazev)).ToList();

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

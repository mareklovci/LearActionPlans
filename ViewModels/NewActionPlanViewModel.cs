using System.Collections.Generic;
using System.Linq;
using LearActionPlans.DataMappers;

namespace LearActionPlans.ViewModels
{
    public class NewActionPlanViewModel
    {
        public int EmployeeId { get; private set; }
        public string EmployeeName { get; private set; }
        public int ProjectId { get; private set; }
        public string ProjectName { get; private set; }
        public int CustomerId { get; private set; }
        public string CustomerName { get; private set; }

        private static NewActionPlanViewModel Employee(int employeeId, string name)
        {
            var actionPlanViewModel = new NewActionPlanViewModel {EmployeeId = employeeId, EmployeeName = name};
            return actionPlanViewModel;
        }

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

        public static IEnumerable<NewActionPlanViewModel> GetEmployees()
        {
            var employees = EmployeeRepository.GetZamestnanciAll().ToList();

            var query = employees.Where(z => z.StavObjektu == 1)
                .OrderBy(z => z.Prijmeni)
                .ThenBy(z => z.Jmeno)
                .Select(z => Employee(z.Id, z.Prijmeni + " " + z.Jmeno)).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
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
            var customers = ZakazniciDataMapper.GetZakazniciAll().ToList();

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

        public static int GetLastActionPlanNumber(int rok)
        {
            var lastActionPlanNumber = AkcniPlanyDataMapper.GetPosledniCisloAP(rok);
            return lastActionPlanNumber;
        }
    }
}

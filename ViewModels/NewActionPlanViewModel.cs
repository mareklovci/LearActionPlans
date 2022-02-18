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

        public static NewActionPlanViewModel Customer(int customerId, string name)
        {
            var actionPlanViewModel = new NewActionPlanViewModel {CustomerId = customerId, CustomerName = name};
            return actionPlanViewModel;
        }
    }
}

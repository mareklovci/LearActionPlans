namespace LearActionPlans.Utilities
{
    public class ArgumentOptions
    {
        public bool RunWithoutParameters { get; set; }

        /// <summary>
        /// Number of Action Plan, but it is actually a string
        /// </summary>
        public string ActionPlanNumber { get; set; }

        public int ActionPlanId { get; set; }

        public int ActionPlanPointId { get; set; }

        public int ActionEndId { get; set; }

        public int ActionOwnerId { get; set; }
    }
}

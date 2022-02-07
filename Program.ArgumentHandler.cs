using System;
using LearActionPlans.Utilities;

namespace LearActionPlans
{
    internal static partial class Program
    {
        /// <summary>
        /// Parse arguments
        /// </summary>
        /// <returns>Parsed Arguments object</returns>
        private static ArgumentOptions ParseArguments()
        {
            var arguments = new ArgumentOptions
            {
                // The first parameter is always the name of the program
                RunWithoutParameters = args.Length <= 1
            };

            if (arguments.RunWithoutParameters)
            {
                return arguments;
            }

            var param = args[1].Split('?');
            var values = param[1].Split('&');

            arguments.ActionPlanNumber = Convert.ToString(values[0]).Replace("%20", " ");
            arguments.ActionPlanId = Convert.ToInt32(values[1]);
            arguments.ActionPlanPointId = Convert.ToInt32(values[2]);
            arguments.ActionEndId = Convert.ToInt32(values[3]);
            arguments.ActionOwnerId = Convert.ToInt32(values[4]);

            return arguments;
        }
    }
}

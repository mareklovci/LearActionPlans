using System;
using LearActionPlans.Utilities;

namespace LearActionPlans
{
    internal static partial class Program
    {
        /// <summary>
        /// Write new entries into the Windows Registry
        /// </summary>
        private static void ModifySystemRegistry()
        {
            var learAP = config.GetSection($"{ConfigOptions.Components}:LearActionPlans").Value;
            var learConfirmation = config.GetSection($"{ConfigOptions.Components}:LearConfirmation").Value;

            try
            {
                Helper.RegisterMyProtocol("LearActionPlans", learAP);
                Helper.RegisterMyProtocol("LearAPConfirmation", learConfirmation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

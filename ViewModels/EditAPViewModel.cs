using System.Collections.Generic;
using System.Linq;
using LearActionPlans.Repositories;

namespace LearActionPlans.ViewModels
{
    public class EditAPViewModel
    {
        //Start Projekty
        public int ProjektId { get; set; }
        public string NazevProjektu { get; set; }
        //End Projekty

        public static EditAPViewModel Projekt(int projektId, string nazev)
        {
            var editAPViewModel = new EditAPViewModel {ProjektId = projektId, NazevProjektu = nazev};
            return editAPViewModel;
        }
    }
}

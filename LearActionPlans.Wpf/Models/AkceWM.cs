//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LearActionPlans.Wpf.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AkceWM
    {
        public int AkceWMID { get; set; }
        public int BodAPID { get; set; }
        public string NapravnaOpatreni { get; set; }
        public int OdpovednaOsoba1 { get; set; }
        public Nullable<int> OdpovednaOsoba2 { get; set; }
        public Nullable<System.DateTime> KontrolaEfektivnosti { get; set; }
        public Nullable<int> OddeleniID { get; set; }
        public string Priloha { get; set; }
        public bool Storno { get; set; }
    }
}

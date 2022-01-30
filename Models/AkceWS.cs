using System;
using System.Collections.Generic;

namespace LearActionPlans.Models
{
    public class AkceWS : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public string NapravnaOpatreni { get; set; }
        public int OdpovednaOsoba1Id { get; set; }
        public int? OdpovednaOsoba2Id { get; set; }
        public List<DatumUkonceniAkceWS> UkonceniBodu { get; set; }
        public DateTime? KontrolaEfektivnosti { get; set; }
        public int? Oddeleni_Id { get; set; }
        public string Priloha { get; set; }
        public bool Storno { get; set; }

        public AkceWS()
        {
            this.Id = 0;
            this.NapravnaOpatreni = null;
            this.OdpovednaOsoba1Id = 0;
            this.OdpovednaOsoba2Id = null;
            this.KontrolaEfektivnosti = null;
            this.Oddeleni_Id = 0;
            this.Priloha = null;
            this.Storno = false;
        }

        public AkceWS(int id,
            string napravnaOpatreni,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            //List<DatumUkonceniBoduAPWS> ukonceniBodu,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string priloha,
            bool storno)
        {
            this.Id = id;
            this.NapravnaOpatreni = napravnaOpatreni;
            this.OdpovednaOsoba1Id = odpovednaOsoba1Id;
            this.OdpovednaOsoba2Id = odpovednaOsoba2Id;
            this.KontrolaEfektivnosti = kontrolaEfektivnosti;
            this.Oddeleni_Id = oddeleniId;
            this.Priloha = priloha;
            this.Storno = storno;
        }

        public AkceWS(string napravnaOpatreni,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string priloha,
            bool storno)
        {
            this.NapravnaOpatreni = napravnaOpatreni;
            this.OdpovednaOsoba1Id = odpovednaOsoba1Id;
            this.OdpovednaOsoba2Id = odpovednaOsoba2Id;
            this.UkonceniBodu = new List<DatumUkonceniAkceWS>();
            this.KontrolaEfektivnosti = kontrolaEfektivnosti;
            this.Oddeleni_Id = oddeleniId;
            this.Priloha = priloha;
            this.Storno = storno;
        }
    }
}

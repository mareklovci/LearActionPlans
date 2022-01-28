using System;
using System.Collections.Generic;

namespace LearActionPlans.Models
{
    public class AkceWM : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public string NapravnaOpatreni { get; set; }
        public int OdpovednaOsoba1Id { get; set; }
        public int? OdpovednaOsoba2Id { get; set; }
        public List<DatumUkonceniAkceWM> UkonceniBodu { get; set; }
        public DateTime? KontrolaEfektivnosti { get; set; }
        public int? Oddeleni_Id { get; set; }
        public string Priloha { get; set; }
        public int Typ { get; set; }
        public bool Storno { get; set; }

        public AkceWM()
        {
            Id = 0;
            NapravnaOpatreni = null;
            OdpovednaOsoba1Id = 0;
            OdpovednaOsoba2Id = null;
            KontrolaEfektivnosti = null;
            Oddeleni_Id = 0;
            Priloha = null;
            Storno = false;
        }

        public AkceWM(int id,
            string napravnaOpatreni,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            //List<DatumUkonceniBoduAPWM> ukonceniBodu,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string priloha,
            bool storno)
        {
            Id = id;
            NapravnaOpatreni = napravnaOpatreni;
            OdpovednaOsoba1Id = odpovednaOsoba1Id;
            OdpovednaOsoba2Id = odpovednaOsoba2Id;
            KontrolaEfektivnosti = kontrolaEfektivnosti;
            Oddeleni_Id = oddeleniId;
            Priloha = priloha;
            Storno = storno;
        }

        public AkceWM(string napravnaOpatreni,
            int odpovednaOsoba1Id,
            int? odpovednaOsoba2Id,
            DateTime? kontrolaEfektivnosti,
            int? oddeleniId,
            string priloha,
            bool storno)
        {
            NapravnaOpatreni = napravnaOpatreni;
            OdpovednaOsoba1Id = odpovednaOsoba1Id;
            OdpovednaOsoba2Id = odpovednaOsoba2Id;
            UkonceniBodu = new List<DatumUkonceniAkceWM>();
            KontrolaEfektivnosti = kontrolaEfektivnosti;
            Oddeleni_Id = oddeleniId;
            Priloha = priloha;
            Storno = storno;
        }
    }
}

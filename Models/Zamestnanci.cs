namespace LearActionPlans.Models
{
    public class Zamestnanci : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string PrihlasovaciJmeno { get; set; }
        public string Email { get; set; }
        public bool AdminAP { get; set; }
        public int OddeleniId { get; set; }
        public byte StavObjektu { get; set; }

        public Zamestnanci(int id, string prihlasovaciJmeno, bool adminAP)
        {
            Id = id;
            PrihlasovaciJmeno = prihlasovaciJmeno;
            AdminAP = adminAP;
        }

        public Zamestnanci(int id, string jmeno, string prijmeni, string prihlasovaciJmeno, byte stavObjektu)
        {
            Id = id;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            PrihlasovaciJmeno = prihlasovaciJmeno;
            StavObjektu = stavObjektu;
        }

        public Zamestnanci(int id, string jmeno, string prijmeni, string prihlasovaciJmeno, string email, bool adminAP, int oddeleniId, byte stavObjektu)
        {
            Id = id;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            PrihlasovaciJmeno = prihlasovaciJmeno;
            Email = email;
            AdminAP = adminAP;
            OddeleniId = oddeleniId;
            StavObjektu = stavObjektu;
        }
    }

}

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
            this.Id = id;
            this.PrihlasovaciJmeno = prihlasovaciJmeno;
            this.AdminAP = adminAP;
        }

        public Zamestnanci(int id, string jmeno, string prijmeni, string emailTo)
        {
            this.Id = id;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.Email = emailTo;
        }

        public Zamestnanci(int id, string jmeno, string prijmeni, string prihlasovaciJmeno, byte stavObjektu)
        {
            this.Id = id;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.PrihlasovaciJmeno = prihlasovaciJmeno;
            this.StavObjektu = stavObjektu;
        }

        public Zamestnanci(int id, string jmeno, string prijmeni, string prihlasovaciJmeno, string email, bool adminAP, int oddeleniId, byte stavObjektu)
        {
            this.Id = id;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.PrihlasovaciJmeno = prihlasovaciJmeno;
            this.Email = email;
            this.AdminAP = adminAP;
            this.OddeleniId = oddeleniId;
            this.StavObjektu = stavObjektu;
        }
    }

}

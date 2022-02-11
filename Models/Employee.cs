namespace LearActionPlans.Models
{
    public class Emploee
    {
        public string CeleJmeno { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public int ZamestnanecId { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool AdminAP { get; set; }
        public int OddeleniId { get; set; }
        public byte StavObjektu { get; set; }

        public Emploee(string celeJmeno, string jmeno, string prijmeni, int zamestnanecId, string login,
            string email, bool adminAP, int oddeleniId, byte stavObjektu)
        {
            this.CeleJmeno = celeJmeno;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.ZamestnanecId = zamestnanecId;
            this.Login = login;
            this.Email = email;
            this.AdminAP = adminAP;
            this.OddeleniId = oddeleniId;
            this.StavObjektu = stavObjektu;
        }
    }
}

// ReSharper disable IdentifierTypo

using System.Linq;

namespace LearActionPlans.Wpf.Models
{
    public partial class Akce
    {
        public Oddeleni Oddeleni => GetDepartment();
        public UkonceniAkce UkonceniAkce => GetDealine();

        private Oddeleni GetDepartment()
        {
            using (var context = new LearDataAllEntities())
            {
                var query = (from z in context.Oddeleni
                    where z.OddeleniID == OddeleniID
                    select z).FirstOrDefault();
                return query;
            }
        }

        private UkonceniAkce GetDealine()
        {
            using (var context = new LearDataAllEntities())
            {
                var query = (from z in context.UkonceniAkce
                    where z.AkceID == AkceID
                    orderby z.DatumUkonceni
                    select z).FirstOrDefault();
                return query;
            }
        }
    }
}
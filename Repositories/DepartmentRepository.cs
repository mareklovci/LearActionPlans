using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LearActionPlans.Models;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.Repositories
{
    public class DepartmentRepository
    {
        private readonly string connectionString;

        public DepartmentRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor) =>
            this.connectionString = optionsMonitor.CurrentValue.LearDataAll;

        public IEnumerable<Oddeleni> GetOddeleniAll()
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM Oddeleni";

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    yield return ConstructOddeleniAll(reader);
                }
            }
            else
            {
                yield return null;
            }
        }

        private static Oddeleni ConstructOddeleniAll(IDataRecord readerData)
        {
            var id = (int)readerData["OddeleniID"];
            var nazev = (string)readerData["Nazev"];
            var stavObjektu = (byte)readerData["StavObjektu"];

            return new Oddeleni(id, nazev, stavObjektu);
        }

        public IEnumerable<Oddeleni> GetOddeleniOriginallyViewModel()
        {
            var oddeleni = this.GetOddeleniAll().ToList();

            var query = oddeleni.OrderBy(o => o.Nazev).ToList();

            if (!query.Any())
            {
                yield break;
            }

            foreach (var q in query)
            {
                yield return q;
            }
        }
    }
}

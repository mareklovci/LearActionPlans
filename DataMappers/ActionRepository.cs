using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LearActionPlans.Models;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.DataMappers
{
    public class ActionRepository
    {
        private readonly ConnectionStringsOptions optionsMonitor;
        private readonly string connectionString;

        public ActionRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor)
        {
            this.optionsMonitor = optionsMonitor.CurrentValue;
            this.connectionString = this.optionsMonitor.LearDataAll;
        }

        public IEnumerable<Akce> GetAkceAll()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = "SELECT * FROM Akce WHERE StavObjektu = @stavObjektu";
            command.Parameters.AddWithValue("@stavObjektu", 1);

            var reader = command.ExecuteReader();

            if (reader == null)
            {
                yield break;
            }

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructAkceAll(reader);
            }
        }

        private static Akce ConstructAkceAll(IDataRecord readerData)
        {
            var id = Convert.ToInt32(readerData["AkceID"]);
            var bodAPId = Convert.ToInt32(readerData["BodAPID"]);
            var stavObjektu = Convert.ToByte(readerData["StavObjektu"]);

            return new Akce(id, bodAPId, stavObjektu);
        }
    }
}

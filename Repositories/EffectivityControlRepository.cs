using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LearActionPlans.Models;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.Repositories
{
    public class EffectivityControlRepository
    {
        private readonly string connectionString;

        public EffectivityControlRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor) =>
            this.connectionString = optionsMonitor.CurrentValue.LearDataAll;

        public IEnumerable<KontrolaEfektivnosti> GetKontrolaEfektivnostiBodAPId(int bodAPId)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText =
                $"SELECT * FROM OdstranitKontrolaEfektivnosti WHERE BodAPID = @bodAPId ORDER BY OdstranitKontrolaEfektivnostiID";
            command.Parameters.AddWithValue("@bodAPId", bodAPId);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructKontrolaEfektivnostiAll(reader);
            }
        }

        private static KontrolaEfektivnosti ConstructKontrolaEfektivnostiAll(IDataRecord readerData)
        {
            var puvodniDatum = Convert.ToDateTime(readerData["KontrolaEfektivnosti"]);
            var odstranitDatum = Convert.ToDateTime(readerData["OdstranitDatum"]);
            var poznamka = Convert.ToString(readerData["Poznamka"]);

            return new KontrolaEfektivnosti(puvodniDatum, odstranitDatum, poznamka);
        }
    }
}

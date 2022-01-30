using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using LearActionPlans.Models;
using LearActionPlans.Utilities;

namespace LearActionPlans.DataMappers
{
    public class KontrolaEfektivnostiDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ActionPlansEntity"].ConnectionString;

        public static IEnumerable<KontrolaEfektivnosti> GetKontrolaEfektivnostiBodAPId(int bodAPId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM OdstranitKontrolaEfektivnosti WHERE BodAPID = @bodAPId ORDER BY OdstranitKontrolaEfektivnostiID";
            command.Parameters.AddWithValue("@bodAPId", bodAPId);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    yield return ConstructKontrolaEfektivnostiAll(reader);
                }
            }
            else
            {
                yield break;
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

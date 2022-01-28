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
    public static class UkonceniAPDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["AkcniPlanyEntity"].ConnectionString;

        public static IEnumerable<UkonceniAP> GetUkonceniAP(int apId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText = $"SELECT * FROM UkonceniAP WHERE AkcniPlanID = @apId ORDER BY UkonceniAPID DESC";
                    command.Parameters.AddWithValue("@apId", apId);

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructUkonceniAP(reader);
                    }
                    else
                        yield break;
                }
            }
        }

        private static UkonceniAP ConstructUkonceniAP(IDataRecord readerData)
        {
            var id = Convert.ToInt32(readerData["UkonceniAPID"]);
            var apId = Convert.ToInt32(readerData["AkcniPlanID"]);
            var datumUkonceniAP = Convert.ToDateTime(readerData["DatumUkonceni"]);
            var poznamka = Convert.ToString(readerData["Poznamka"]);

            return new UkonceniAP(id, apId, datumUkonceniAP, poznamka);
        }
    }
}

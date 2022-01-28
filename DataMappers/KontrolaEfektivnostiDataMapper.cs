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
            ConfigurationManager.ConnectionStrings["AkcniPlanyEntity"].ConnectionString;

        public static IEnumerable<KontrolaEfektivnosti> GetKontrolaEfektivnostiBodAPId(int bodAPId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText = $"SELECT * FROM OdstranitKontrolaEfektivnosti WHERE BodAPID = @bodAPId ORDER BY OdstranitKontrolaEfektivnostiID";
                    command.Parameters.AddWithValue("@bodAPId", bodAPId);

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructKontrolaEfektivnostiAll(reader);
                    }
                    else
                        yield break;
                }
            }
        }

        private static KontrolaEfektivnosti ConstructKontrolaEfektivnostiAll(IDataRecord readerData)
        {
            DateTime puvodniDatum = Convert.ToDateTime(readerData["KontrolaEfektivnosti"]);
            DateTime odstranitDatum = Convert.ToDateTime(readerData["OdstranitDatum"]);
            string poznamka = Convert.ToString(readerData["Poznamka"]);

            return new KontrolaEfektivnosti(puvodniDatum, odstranitDatum, poznamka);
        }
    }
}

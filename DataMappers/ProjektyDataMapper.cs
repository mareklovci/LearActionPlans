using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using LearActionPlans.Models;

namespace LearActionPlans.DataMappers
{
    public static class ProjektyDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ActionPlansEntity"].ConnectionString;

        public static IEnumerable<Projekty> GetProjektyAll()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM Projekt";

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructProjekt(reader);
            }
        }

        private static Projekty ConstructProjekt(IDataRecord reader)
        {
            var id = (int)reader["ProjektID"];
            var nazev = (string)reader["Nazev"];
            var stavObjektu = (byte)reader["StavObjektu"];

            return new Projekty(id, nazev, stavObjektu);
        }
    }
}

using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LearActionPlans.Models;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.Repositories
{
    public class ProjectRepository
    {
        private readonly string connectionString;

        public ProjectRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor) =>
            this.connectionString = optionsMonitor.CurrentValue.LearDataAll;

        public IEnumerable<Projekty> GetProjektyAll()
        {
            using var connection = new SqlConnection(this.connectionString);
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
                yield return CreateProject(reader);
            }
        }

        private static Projekty CreateProject(IDataRecord reader)
        {
            var id = (int)reader["ProjektID"];
            var nazev = (string)reader["Nazev"];
            var stavObjektu = (byte)reader["StavObjektu"];

            return new Projekty(id, nazev, stavObjektu);
        }
    }
}

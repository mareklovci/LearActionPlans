using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using LearActionPlans.Models;
using System;

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

        public static void InsertProjekt(string nazev)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"INSERT INTO Projekt (Nazev, StavObjektu) VALUES" +
                                      $"(@nazev, @stavObjektu)";
                command.Parameters.AddWithValue("@nazev", nazev);
                command.Parameters.AddWithValue("@stavObjektu", 1);

                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                //Došlo k problému při práci s databází.
                //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void UpdateProjekt(int projektId, string nazev, byte stavObjektu)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = $"UPDATE Projekt SET Nazev = @nazev, StavObjektu = @stavObjektu " +
                                      $"WHERE ProjektID = @projektId";

                command.Parameters.AddWithValue("@nazev", nazev);
                command.Parameters.AddWithValue("@stavObjektu", stavObjektu);
                command.Parameters.AddWithValue("@projektId", projektId);

                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                //Došlo k problému při práci s databází.
                //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

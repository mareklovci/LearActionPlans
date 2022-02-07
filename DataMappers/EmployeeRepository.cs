using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using LearActionPlans.Models;
using System;
using System.Windows.Forms;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.DataMappers
{
    public class EmployeeRepository
    {
        private readonly ConnectionStringsOptions optionsMonitor;
        private readonly string connectionString;

        public EmployeeRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor)
        {
            this.optionsMonitor = optionsMonitor.CurrentValue;
            this.connectionString = this.optionsMonitor.LearDataAll;
        }

        public IEnumerable<Zamestnanci> GetZamestnanciAll()
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM Zamestnanec";

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructZamestnanec(reader);
            }
        }

        private static Zamestnanci ConstructZamestnanec(IDataRecord reader)
        {
            var id = (int)reader["ZamestnanecID"];
            var jmeno = (string)reader["Jmeno"];
            var prijmeni = (string)reader["Prijmeni"];
            var prihlasovaciJmeno = (string)reader["PrihlasovaciJmeno"];
            var email = (string)reader["Email"];
            var adminAP = (bool)reader["AdminAP"];
            var oddeleniId = (int)reader["OddeleniId"];
            var stavObjektu = (byte)reader["StavObjektu"];

            return new Zamestnanci(id, jmeno, prijmeni, prihlasovaciJmeno, email, adminAP, oddeleniId, stavObjektu);
        }

        public IEnumerable<Zamestnanci> GetOdpovednyPracovnikId(int odpovednyPracovnikId)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM Zamestnanec WHERE ZamestnanecId = @odpovednyPracovnikId";
            command.Parameters.AddWithValue("@odpovednyPracovnikId", odpovednyPracovnikId);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructOdpovednyPracovnik(reader);
            }
        }
        private static Zamestnanci ConstructOdpovednyPracovnik(IDataRecord reader)
        {
            var id = (int)reader["ZamestnanecID"];
            var jmeno = (string)reader["Jmeno"];
            var prijmeni = (string)reader["Prijmeni"];
            var email = (string)reader["Email"];

            return new Zamestnanci(id, jmeno, prijmeni, email);
        }

        //private static Zamestnanci ConstructZadavatel(IDataRecord reader)
        //{
        //    var id = (int)reader["ZamestnanecID"];
        //    var prihlasovaciJmeno = (string)reader["PrihlasovaciJmeno"];
        //    var admin = (bool)reader["AdminAP"];

        //    return new Zamestnanci(id, prihlasovaciJmeno, admin);
        //}

        public IEnumerable<Zamestnanci> GetZadavatelLogin(string login)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM Zamestnanec WHERE PrihlasovaciJmeno = @prihlasovaciJmeno";
            command.Parameters.AddWithValue("@prihlasovaciJmeno", login);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructZadavatel(reader);
            }
        }

        private static Zamestnanci ConstructZadavatel(IDataRecord reader)
        {
            var id = (int)reader["ZamestnanecID"];
            var prihlasovaciJmeno = (string)reader["PrihlasovaciJmeno"];
            var admin = (bool)reader["AdminAP"];

            return new Zamestnanci(id, prihlasovaciJmeno, admin);
        }

        public bool InsertZamestnanec(string jmeno, string prijmeni, string prihlasovaciJmeno, int oddeleniId, string email, bool adminAP, byte stavObjektu)
        {
            try
            {
                using var connection = new SqlConnection(this.connectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"INSERT INTO Zamestnanec (Jmeno, Prijmeni, PrihlasovaciJmeno, OddeleniID, Email, AdminAP, StavObjektu)" +
                                      $" VALUES (@jmeno, @prijmeni, @prihlasovaciJmeno, @oddeleniId, @email, @adminAP, @stavObjektu)";

                command.Parameters.AddWithValue("@jmeno", jmeno);
                command.Parameters.AddWithValue("@prijmeni", prijmeni);
                command.Parameters.AddWithValue("@prihlasovaciJmeno", prihlasovaciJmeno);
                command.Parameters.AddWithValue("@OddeleniId", oddeleniId);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@adminAP", adminAP);
                command.Parameters.AddWithValue("@stavObjektu", stavObjektu);

                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
                //Došlo k problému při práci s databází.
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }
        }

        public bool UpdateZamestnanec(int zamestnanecId, string jmeno, string prijmeni, string prihlasovaciJmeno, int oddeleniId, string email, bool adminAP, byte stavObjektu)
        {
            try
            {
                using var connection = new SqlConnection(this.connectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = $"UPDATE Zamestnanec SET Jmeno = @jmeno, " +
                                      $"Prijmeni = @prijmeni, " +
                                      $"PrihlasovaciJmeno = @prihlasovaciJmeno, " +
                                      $"OddeleniID = @oddeleniId, " +
                                      $"Email = @email, " +
                                      $"AdminAP = @adminAP, " +
                                      $"StavObjektu = @stavObjektu " +
                                      $"WHERE ZamestnanecID = @zamestnanecId";

                command.Parameters.AddWithValue("@zamestnanecId", zamestnanecId);
                command.Parameters.AddWithValue("@jmeno", jmeno);
                command.Parameters.AddWithValue("@prijmeni", prijmeni);
                command.Parameters.AddWithValue("@prihlasovaciJmeno", prihlasovaciJmeno);
                command.Parameters.AddWithValue("@oddeleniId", oddeleniId);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@adminAP", adminAP);
                command.Parameters.AddWithValue("@stavObjektu", stavObjektu);

                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
                //Došlo k problému při práci s databází.
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }
        }
    }
}

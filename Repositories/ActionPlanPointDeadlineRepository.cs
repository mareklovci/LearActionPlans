using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LearActionPlans.Models;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.Repositories
{
    public class ActionPlanPointDeadlineRepository
    {
        private readonly string connectionString;

        public ActionPlanPointDeadlineRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor) =>
            this.connectionString = optionsMonitor.CurrentValue.LearDataAll;

        public IEnumerable<UkonceniBodAP> GetUkonceniAkceAll(int apId)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText =
                $"SELECT KontrolaEfektivnosti FROM BodAP WHERE BodAP.AkcniPlanID = @apId AND BodAP.StavObjektu = @stavObjektuBodAP";
            command.Parameters.AddWithValue("@apId", apId);
            command.Parameters.AddWithValue("@stavObjektuBodAP", 1);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructUkonceniAkceAll(reader);
            }
        }

        private static UkonceniBodAP ConstructUkonceniAkceAll(IDataRecord readerData)
        {
            var kontrolaEfektivnosti = DatabaseReader.ConvertDateTime(readerData, "KontrolaEfektivnosti");

            return new UkonceniBodAP(kontrolaEfektivnosti);
        }

        public IEnumerable<UkonceniBodAP> GetUkonceniBodAPId(int bodAPId)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM UkonceniBodAP WHERE BodAPID = @bodAPId ORDER BY UkonceniBodAPID";
            command.Parameters.AddWithValue("@bodAPId", bodAPId);

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
                yield return this.ConstructUkonceniBodAP(reader);
            }
        }

        private UkonceniBodAP ConstructUkonceniBodAP(IDataRecord readerData)
        {
            var id = Convert.ToInt32(readerData["UkonceniBodAPID"]);
            var bodAPId = Convert.ToInt32(readerData["BodAPID"]);
            var ukonceniBodAP = Convert.ToDateTime(readerData["DatumUkonceni"]);
            var poznamka = DatabaseReader.ConvertString(readerData, "Poznamka");
            var odpoved = DatabaseReader.ConvertString(readerData, "Odpoved");
            var stavZadosti = Convert.ToByte(readerData["StavZadosti"]);
            var stavObjektu = Convert.ToByte(readerData["StavObjektu"]);

            return new UkonceniBodAP(id, bodAPId, ukonceniBodAP, poznamka, odpoved, stavZadosti, stavObjektu, true);
        }

        public int InsertUkonceniBodAP(int bodAPId, DateTime datumUkonceni, string poznamka)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText =
                $"INSERT INTO UkonceniBodAP (BodAPID, DatumUkonceni, Poznamka, StavZadosti, StavObjektu) OUTPUT INSERTED.UkonceniBodAPID VALUES" +
                $"(@bodAPId, @datumUkonceni, @poznamka, @stavZadosti, @stavObjektu)";
            command.Parameters.AddWithValue("@bodAPId", bodAPId);
            command.Parameters.AddWithValue("@datumUkonceni", datumUkonceni);
            if (string.IsNullOrWhiteSpace(poznamka))
            {
                command.Parameters.AddWithValue("@poznamka", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@poznamka", poznamka);
            }

            command.Parameters.AddWithValue("@stavZadosti", 3);
            command.Parameters.AddWithValue("@stavObjektu", 1);

            return (int)command.ExecuteScalar();
            ;
        }

        public void UpdateBodAPZmenaTerminu(int bodAPId, byte zmenaTerminu)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();
            ;
            using var commandAkce = connection.CreateCommand();
            commandAkce.CommandType = CommandType.Text;
            commandAkce.CommandText = $"UPDATE BodAP SET ZmenaTerminu = @zmenaTerminu WHERE BodAPID = @bodAPId";
            commandAkce.Parameters.AddWithValue("@bodAPId", bodAPId);
            commandAkce.Parameters.AddWithValue("@zmenaTerminu", zmenaTerminu - 1);
            commandAkce.ExecuteNonQuery();
        }

        public void UpdateUkonceniBodAP(int ukonceniBodAPId, string poznamka)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var commandAkce = connection.CreateCommand();
            commandAkce.CommandType = CommandType.Text;
            commandAkce.CommandText =
                $"UPDATE UkonceniBodAP SET Poznamka = @poznamka WHERE UkonceniBodAPID = @ukonceniBodAPId";
            commandAkce.Parameters.AddWithValue("@ukonceniBodAPId", ukonceniBodAPId);
            commandAkce.Parameters.AddWithValue("@poznamka", poznamka);
            commandAkce.ExecuteNonQuery();
        }

        public void UpdatePrvniTermin(int ukonceniBodAPId)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var commandAkce = connection.CreateCommand();
            commandAkce.CommandType = CommandType.Text;
            commandAkce.CommandText =
                $"UPDATE UkonceniBodAP SET StavZadosti = @stavZadosti WHERE UkonceniBodAPID = @ukonceniBodAPId";
            commandAkce.Parameters.AddWithValue("@stavZadosti", 2);
            commandAkce.Parameters.AddWithValue("@ukonceniBodAPId", ukonceniBodAPId);
            commandAkce.ExecuteNonQuery();
        }

        public IEnumerable<UkonceniBodAP> GetZavritPrvniTermin(int bodAPId)
        {
            var ukonceni = this.GetUkonceniBodAPId(bodAPId);

            var query = ukonceni.Where(u => u.BodAPId == bodAPId && u.StavZadosti == 1).ToList();

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

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

using LearActionPlans.Models;
using LearActionPlans.Utilities;
using LearActionPlans.Views;

namespace LearActionPlans.DataMappers
{
    public class UkonceniBodAPDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ActionPlansEntity"].ConnectionString;

        //public static IEnumerable<UkonceniAkce> GetUkonceniAkceAll()
        //{
        //    using (var connection = new SqlConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandType = CommandType.Text;

        //            command.CommandText = $"SELECT * FROM UkonceniAkce WHERE StavObjektu = @stavObjektu";
        //            command.Parameters.AddWithValue("@stavObjektu", 1);

        //            var reader = command.ExecuteReader();

        //            if (reader == null)
        //                yield break;

        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                    yield return ConstructUkonceniAkceAll(reader);
        //            }
        //        }
        //    }
        //}

        public static IEnumerable<UkonceniBodAP> GetUkonceniAkceAll(int apId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            //command.CommandText = $"SELECT Akce.KontrolaEfektivnosti AS KontrolaEfektivnosti " +
            //    $"FROM Akce INNER JOIN BodAP ON Akce.BodAPID = BodAP.BodAPID " +
            //    $"WHERE BodAP.AkcniPlanID = @apId AND BodAP.StavObjektu = @stavObjektuBodAP AND Akce.StavObjektu = @stavObjektuAkce";
            //command.Parameters.AddWithValue("@apId", apId);
            //command.Parameters.AddWithValue("@stavObjektuBodAP", 1);
            //command.Parameters.AddWithValue("@stavObjektuAkce", 1);

            command.CommandText = $"SELECT KontrolaEfektivnosti FROM BodAP WHERE BodAP.AkcniPlanID = @apId AND BodAP.StavObjektu = @stavObjektuBodAP AND Akce.StavObjektu = @stavObjektuAkce";
            command.Parameters.AddWithValue("@apId", apId);
            command.Parameters.AddWithValue("@stavObjektuBodAP", 1);
            command.Parameters.AddWithValue("@stavObjektuAkce", 1);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    yield return ConstructUkonceniAkceAll(reader);
                }
            }
            else
            {
                yield break;
            }
        }

        private static UkonceniBodAP ConstructUkonceniAkceAll(IDataRecord readerData)
        {
            var kontrolaEfektivnosti = DatabaseReader.ConvertDateTime(readerData, "KontrolaEfektivnosti");

            return new UkonceniBodAP(kontrolaEfektivnosti);
        }

        public static IEnumerable<UkonceniBodAP> GetUkonceniBodAPId(int bodAPId)
        {
            using var connection = new SqlConnection(ConnectionString);
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

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    yield return ConstructUkonceniBodAP(reader);
                }
            }
        }

        private static UkonceniBodAP ConstructUkonceniBodAP(IDataRecord readerData)
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

        public static int InsertUkonceniBodAP(int bodAPId, DateTime datumUkonceni, string poznamka)
        {
            var id = 0;

            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"INSERT INTO UkonceniBodAP (BodAPID, DatumUkonceni, Poznamka, StavZadosti, StavObjektu) OUTPUT INSERTED.UkonceniBodAPID VALUES" +
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
            //if (FormMain.VlastnikAP == true)
            //{
            //    //vlastník AP si rovnou potvrdí nový termín
            //    command.Parameters.AddWithValue("@stavZadosti", 4);
            //}
            //else
            //{
            //    command.Parameters.AddWithValue("@stavZadosti", 3);
            //}

            command.Parameters.AddWithValue("@stavObjektu", 1);
            id = (int)command.ExecuteScalar();
            //command.ExecuteNonQuery();

            return id;
        }
        public static void UpdateBodAPZmenaTerminu(int bodAPId, byte zmenaTerminu)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            ;
            using var commandAkce = connection.CreateCommand();
            commandAkce.CommandType = CommandType.Text;
            commandAkce.CommandText = $"UPDATE BodAP SET ZmenaTerminu = @zmenaTerminu WHERE BodAPID = @bodAPId";
            commandAkce.Parameters.AddWithValue("@bodAPId", bodAPId);
            commandAkce.Parameters.AddWithValue("@zmenaTerminu", zmenaTerminu - 1);
            commandAkce.ExecuteNonQuery();
        }

        public static void UpdateUkonceniBodAP(int ukonceniBodAPId, string poznamka)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var commandAkce = connection.CreateCommand();
            commandAkce.CommandType = CommandType.Text;
            commandAkce.CommandText = $"UPDATE UkonceniBodAP SET Poznamka = @poznamka WHERE UkonceniBodAPID = @ukonceniBodAPId";
            commandAkce.Parameters.AddWithValue("@ukonceniBodAPId", ukonceniBodAPId);
            commandAkce.Parameters.AddWithValue("@poznamka", poznamka);
            commandAkce.ExecuteNonQuery();
        }

        public static void UpdatePrvniTermin(int ukonceniBodAPId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var commandAkce = connection.CreateCommand();
            commandAkce.CommandType = CommandType.Text;
            commandAkce.CommandText = $"UPDATE UkonceniBodAP SET StavZadosti = @stavZadosti WHERE UkonceniBodAPID = @ukonceniBodAPId";
            commandAkce.Parameters.AddWithValue("@stavZadosti", 2);
            commandAkce.Parameters.AddWithValue("@ukonceniBodAPId", ukonceniBodAPId);
            commandAkce.ExecuteNonQuery();
        }
    }
}

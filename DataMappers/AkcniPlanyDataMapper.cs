using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Collections.Generic;

using LearActionPlans.Models;
using LearActionPlans.Views;
using LearActionPlans.Utilities;

namespace LearActionPlans.DataMappers
{
    public static class AkcniPlanyDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ActionPlansEntity"].ConnectionString;

        public static IEnumerable<AkcniPlany> GetAPAll()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"SELECT * FROM AkcniPlan ORDER BY DatumZalozeni, CisloAP";

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructAllAP(reader);
            }
        }

        public static IEnumerable<AkcniPlany> GetAPId(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
            command.CommandText = $"SELECT * FROM AkcniPlan WHERE AkcniPlanID = @apId";
            command.Parameters.AddWithValue("@apId", id);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructAllAP(reader);
            }
        }

        public static IEnumerable<AkcniPlany> GetPocetTerminuAP(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
            command.CommandText = $"SELECT AkcniPlanID, ZmenaTerminu FROM AkcniPlan WHERE AkcniPlanID = @apId";
            command.Parameters.AddWithValue("@apId", id);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructZmenaTerminuAP(reader);
            }
        }

        private static AkcniPlany ConstructZmenaTerminuAP(IDataRecord readerData)
        {
            var id = Convert.ToInt32(readerData["AkcniPlanID"]);
            var zmenaTerminuAP = Convert.ToByte(readerData["ZmenaTerminu"]);

            return new AkcniPlany(id, zmenaTerminuAP);
        }

        public static IEnumerable<AkcniPlany> GetZnovuOtevritAP(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT AkcniPlanID, UzavreniAP, ZnovuOtevrit, DuvodZnovuotevreni FROM AkcniPlan WHERE AkcniPlanID = @apId";
            command.Parameters.AddWithValue("@apId", id);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructZnovuOtevreniAP(reader);
            }
        }

        private static AkcniPlany ConstructZnovuOtevreniAP(IDataRecord readerData)
        {
            var id = Convert.ToInt32(readerData["AkcniPlanID"]);
            var znovuOtevritAP = Convert.ToByte(readerData["ZnovuOtevrit"]);
            var uzavreniAP = DatabaseReader.ConvertDateTime(readerData, "UzavreniAP");
            var duvod = Convert.ToString(readerData["DuvodZnovuotevreni"]);

            return new AkcniPlany(id, znovuOtevritAP, uzavreniAP, duvod);
        }

        public static int GetPosledniCisloAP(int rok)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = $"SELECT TOP 1 CisloAP FROM AkcniPlan WHERE YEAR(DatumZalozeni) = @rok";
                command.Parameters.AddWithValue("@rok", rok);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    using var connectionUniqueNumber = new SqlConnection(ConnectionString);
                    if (connectionUniqueNumber.State == ConnectionState.Closed)
                    {
                        connectionUniqueNumber.Open();
                    }

                    using var commandUniqueNumber = connectionUniqueNumber.CreateCommand();
                    commandUniqueNumber.CommandType = CommandType.Text;
                    commandUniqueNumber.CommandText = $"SELECT NEXT VALUE FOR dbo.NumberAP";
                    var valSeq = (int)commandUniqueNumber.ExecuteScalar();

                    connectionUniqueNumber.Close();

                    return valSeq;
                }

                //když nenajde číslo AP jedna, tak zresetuje čítač
                using (var connectionResetNumber = new SqlConnection(ConnectionString))
                {
                    if (connectionResetNumber.State == ConnectionState.Closed)
                    {
                        connectionResetNumber.Open();
                    }

                    using (var commandResetNumber = connectionResetNumber.CreateCommand())
                    {
                        commandResetNumber.CommandType = CommandType.Text;
                        commandResetNumber.CommandText = $"ALTER SEQUENCE NumberAP RESTART WITH 1";
                        commandResetNumber.ExecuteScalar();

                        connectionResetNumber.Close();
                    }
                }

                using (var connectionUniqueNumber = new SqlConnection(ConnectionString))
                {
                    if (connectionUniqueNumber.State == ConnectionState.Closed)
                    {
                        connectionUniqueNumber.Open();
                    }

                    using (var commandUniqueNumber = connectionUniqueNumber.CreateCommand())
                    {
                        commandUniqueNumber.CommandType = CommandType.Text;
                        commandUniqueNumber.CommandText = $"SELECT NEXT VALUE FOR dbo.NumberAP";
                        var valSeq = (int)commandUniqueNumber.ExecuteScalar();

                        connectionUniqueNumber.Close();
                        return valSeq;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //Problém s databází.
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return -1;
        }

        private static AkcniPlany ConstructAllAP(IDataRecord readerData)
        {
            var id = Convert.ToInt32(readerData["AkcniPlanID"]);
            var datumZalozeni = Convert.ToDateTime(readerData["DatumZalozeni"]);
            var cisloAP = Convert.ToInt32(readerData["CisloAP"]);
            var zadavatel1Id = Convert.ToInt32(readerData["Zadavatel1ID"]);
            var zadavatel2Id = DatabaseReader.ConvertInteger(readerData, "Zadavatel2ID");
            var tema = Convert.ToString(readerData["Tema"]);
            var projektId = DatabaseReader.ConvertInteger(readerData, "ProjektID");
            var zakaznikId = Convert.ToInt32(readerData["ZakaznikID"]);
            var typAP = Convert.ToByte(readerData["TypAP"]);
            var stavObjektu = Convert.ToByte(readerData["StavObjektu"]);
            var uzavreniAP = DatabaseReader.ConvertDateTime(readerData, "UzavreniAP");

            return new AkcniPlany(id, datumZalozeni, cisloAP, zadavatel1Id, zadavatel2Id, tema, projektId, zakaznikId, typAP, stavObjektu, uzavreniAP);
        }

        private static int ConstructPosledniCisloAP(IDataRecord readerData)
        {
            var noveCisloAP = (int)readerData["CisloAP"];

            return noveCisloAP;
        }

        public static int InsertAP(FormNovyAkcniPlan.AkcniPlanTmp akcniPlany)
        {
            var idZaznamu = 0;

            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"INSERT INTO AkcniPlan (DatumZalozeni, CisloAP, Zadavatel1ID, Zadavatel2ID, Tema, ProjektID, ZakaznikID, TypAP, ZmenaTerminu, UzavreniAP, ZnovuOtevrit, DuvodZnovuotevreni, StavObjektu)" +
                                      $"output INSERTED.AkcniPlanID VALUES" +
                                      $"(@datumZalozeni, @cisloAP, @zadavatel1Id, @zadavatel2Id, @tema, @projektId, @zakaznikId, @typAP, @zmenaTerminu, @uzavreniAP, @znovuOtevrit, @duvodZnovuotevreni, @stavObjektu)";
                command.Parameters.AddWithValue("@datumZalozeni", DateTime.Now);
                command.Parameters.AddWithValue("@cisloAP", akcniPlany.CisloAP);
                //command.Parameters.AddWithValue("@rok", akcniPlany.Rok);
                command.Parameters.AddWithValue("@zadavatel1Id", akcniPlany.Zadavatel1Id);
                if (akcniPlany.Zadavatel2Id == null)
                {
                    command.Parameters.AddWithValue("@zadavatel2Id", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@zadavatel2Id", akcniPlany.Zadavatel2Id);
                }

                command.Parameters.AddWithValue("@tema", akcniPlany.Tema);
                if (akcniPlany.ProjektId == null)
                {
                    command.Parameters.AddWithValue("@projektId", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@projektId", akcniPlany.ProjektId);
                }

                command.Parameters.AddWithValue("@zakaznikId", akcniPlany.ZakaznikId);
                command.Parameters.AddWithValue("@typAP", akcniPlany.TypAP);
                command.Parameters.AddWithValue("@ZmenaTerminu", 3);
                command.Parameters.AddWithValue("@UzavreniAP", DBNull.Value);
                command.Parameters.AddWithValue("@ZnovuOtevrit", 1);
                command.Parameters.AddWithValue("@DuvodZnovuotevreni", DBNull.Value);
                command.Parameters.AddWithValue("@stavObjektu", 1);

                idZaznamu = Convert.ToInt32(command.ExecuteScalar());

                if (idZaznamu > 0)
                {
                    using var commandDatum = connection.CreateCommand();
                    commandDatum.CommandType = CommandType.Text;
                    commandDatum.CommandText = $"INSERT INTO UkonceniAP (AkcniPlanID, DatumUkonceni, Poznamka) VALUES (@akcniPlanId, @datumUkonceni, @poznamka)";
                    commandDatum.Parameters.AddWithValue("@akcniPlanId", idZaznamu);
                    commandDatum.Parameters.AddWithValue("@datumUkonceni", akcniPlany.DatumUkonceni);
                    if (akcniPlany.Poznamka == null)
                    {
                        commandDatum.Parameters.AddWithValue("@poznamka", DBNull.Value);
                    }
                    else
                    {
                        commandDatum.Parameters.AddWithValue("@poznamka", akcniPlany.Poznamka);
                    }

                    commandDatum.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                //Došlo k problému při práci s databází.
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
            return idZaznamu;
        }

        public static void UpdateAP(int akcniPlanID, int zadavatel1ID, int? zadavatel2ID, string tema, int? projektID, int zakaznikID)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"UPDATE AkcniPlan SET Zadavatel1ID = @zadavatel1ID, Zadavatel2ID = @zadavatel2ID," +
                                      $" Tema = @tema, ZakaznikId = @zakaznikID, ProjektID = @projektID WHERE AkcniPlanID = @akcniPlanID";

                command.Parameters.AddWithValue("@akcniPlanID", akcniPlanID);
                command.Parameters.AddWithValue("@zadavatel1ID", zadavatel1ID);
                if (zadavatel2ID == null)
                {
                    command.Parameters.AddWithValue("@zadavatel2ID", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@zadavatel2ID", zadavatel2ID);
                }

                command.Parameters.AddWithValue("@tema", tema);
                command.Parameters.AddWithValue("@zakaznikID", zakaznikID);
                if (projektID == null)
                {
                    command.Parameters.AddWithValue("@projektID", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@projektID", projektID);
                }

                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                // Došlo k problému při práci s databází.
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void ZmenaTerminuAP(int apId, int zmenaTerminu, DateTime datumUkonceni, string poznamka)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = $"INSERT INTO UkonceniAP (AkcniPlanID, DatumUkonceni, Poznamka)" +
                            $" VALUES (@akcniPlanId, @datumUkonceni, @poznamka)";

                        command.Parameters.AddWithValue("@akcniPlanID", apId);
                        command.Parameters.AddWithValue("@datumUkonceni", datumUkonceni);
                        if (poznamka == string.Empty)
                        {
                            command.Parameters.AddWithValue("@poznamka", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@poznamka", poznamka);
                        }

                        command.ExecuteNonQuery();
                    }
                }
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = $"INSERT INTO UkonceniAP (AkcniPlanID, DatumUkonceni, Poznamka)" +
                            $" VALUES" +
                            $"(@akcniPlanId, @datumZalozeni, @poznamka)";
                        command.CommandText = $"UPDATE AkcniPlan SET ZmenaTerminu = @zmenaTerminu WHERE AkcniPlanID = @akcniPlanID";

                        command.Parameters.AddWithValue("@akcniPlanID", apId);
                        command.Parameters.AddWithValue("@zmenaTerminu", zmenaTerminu);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Došlo k problému při práci s databází.
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void UpdateUkonceniAP(int apId)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = $"UPDATE AkcniPlan SET UzavreniAP = @uzavreniAP WHERE AkcniPlanID = @akcniPlanID";

                command.Parameters.AddWithValue("@akcniPlanID", apId);
                command.Parameters.AddWithValue("@uzavreniAP", DateTime.Now);

                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                //Došlo k problému při práci s databází.
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void UpdateZnovuOtevritAP(int apId, string duvod)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = $"UPDATE AkcniPlan SET UzavreniAP = @uzavreniAP, ZnovuOtevrit = @znovuOtevrit, DuvodZnovuotevreni = @duvodZnovuotevreni WHERE AkcniPlanID = @akcniPlanID";

                command.Parameters.AddWithValue("@akcniPlanID", apId);
                command.Parameters.AddWithValue("@uzavreniAP", DBNull.Value);
                command.Parameters.AddWithValue("@znovuOtevrit", 0);
                command.Parameters.AddWithValue("@duvodZnovuotevreni", duvod);

                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                //Došlo k problému při práci s databází.
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

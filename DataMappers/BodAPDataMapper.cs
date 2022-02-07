using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;
using LearActionPlans.Models;
using LearActionPlans.Utilities;

namespace LearActionPlans.DataMappers
{
    public static partial class BodAPDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ActionPlansEntity"].ConnectionString;

        public static IEnumerable<BodAP> GetBodyAPAll()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
            command.CommandText = $"SELECT * FROM BodAP";

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    yield return ConstructBodyAP(reader);
                }
            }
        }

        public static IEnumerable<BodAP> GetBodId(int bodAPId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
            command.CommandText = $"SELECT * FROM BodAP WHERE BodAPID = @bodAPId";
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
                yield return ConstructBodyAP(reader);
            }
        }

        public static IEnumerable<BodAP> GetBodyIdAP(int aPId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
            command.CommandText = $"SELECT * FROM BodAP WHERE AkcniPlanID = @APId";
            command.Parameters.AddWithValue("@APId", aPId);

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
                yield return ConstructBodyAP(reader);
            }
        }

        private static BodAP ConstructBodyAP(IDataRecord readerData)
        {
            var id = (int)readerData["BodAPID"];
            var akcniPlanId = (int)readerData["AkcniPlanID"];
            var cisloBoduAP = (int)readerData["CisloBoduAP"];
            var datumZalozeni = (DateTime)readerData["DatumZalozeni"];
            var odkazNaNormu = DatabaseReader.ConvertString(readerData, "OdkazNaNormu");
            var hodnoceniNeshody = DatabaseReader.ConvertString(readerData, "HodnoceniNeshody");
            var popisProblemu = DatabaseReader.ConvertString(readerData, "PopisProblemu");
            var skutecnaPricinaWM = DatabaseReader.ConvertString(readerData, "SkutecnaPricinaWM");
            var napravnaOpatreniWM = DatabaseReader.ConvertString(readerData, "NapravnaOpatreniWM");
            var skutecnaPricinaWS = DatabaseReader.ConvertString(readerData, "SkutecnaPricinaWS");
            var napravnaOpatreniWS = DatabaseReader.ConvertString(readerData, "NapravnaOpatreniWS");

            var odpovednaOsoba1Id = (int)readerData["OdpovednaOsoba1ID"];
            var odpovednaOsoba2Id = DatabaseReader.ConvertInteger(readerData, "OdpovednaOsoba2ID");
            var kontrolaEfektivnosti = DatabaseReader.ConvertDateTime(readerData, "KontrolaEfektivnosti");
            var oddeleniId = Convert.ToInt32(readerData["OddeleniID"]);
            var priloha = DatabaseReader.ConvertString(readerData, "Priloha");
            var zamitnutiTerminu = (byte)readerData["ZamitnutiTerminu"];
            var zmenaTerminu = (byte)readerData["ZmenaTerminu"];

            var znovuOtevrit = (bool)readerData["ZnovuOtevrit"];
            var emailOdeslan = (bool)readerData["EmailOdeslan"];
            var stavObjektu = (byte)readerData["StavObjektu"];

            return new BodAP(id, akcniPlanId, cisloBoduAP, datumZalozeni, odkazNaNormu, hodnoceniNeshody, popisProblemu,
                skutecnaPricinaWM, napravnaOpatreniWM, skutecnaPricinaWS, napravnaOpatreniWS, odpovednaOsoba1Id, odpovednaOsoba2Id,
                kontrolaEfektivnosti, oddeleniId, priloha, zamitnutiTerminu, zmenaTerminu,
                znovuOtevrit, true, emailOdeslan, stavObjektu);
        }

        public static IEnumerable<UkonceniBodAP> GetUkonceniAkceZadost()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM UkonceniAkce WHERE StavZadosti = @stavZadosti";
            command.Parameters.AddWithValue("@stavZadosti", 3);

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
                yield return ConstructUkonceniAkceZadost(reader);
            }
        }

        private static UkonceniBodAP ConstructUkonceniAkceZadost(IDataRecord readerData)
        {
            var id = Convert.ToInt32(readerData["UkonceniAkceID"]);
            var akceId = Convert.ToInt32(readerData["AkceID"]);
            var ukonceniAkce = Convert.ToDateTime(readerData["DatumUkonceni"]);
            var poznamka = DatabaseReader.ConvertString(readerData, "Poznamka");
            var odpoved = DatabaseReader.ConvertString(readerData, "Odpoved");
            var stavZadosti = Convert.ToByte(readerData["StavZadosti"]);
            var stavObjektu = Convert.ToByte(readerData["StavObjektu"]);

            return new UkonceniBodAP(id, akceId, ukonceniAkce, poznamka, odpoved, stavZadosti, stavObjektu);
        }

        public static IEnumerable<UkonceniBodAP> GetUkonceniBodAP(int bodAPId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM UkonceniBodAP WHERE BodAPID = @bodAPId";
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

        //public static void InsertBodyAP(FormNovyAkcniPlan.AkcniPlanTmp akcniPlany)
        //public static void InsertUpdateBodyAP(int idAP)
        public static int InsertUpdateBodAP(BodAP ulozitBodAP)
        {
            var idZaznamu = 0;

            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                if (ulozitBodAP.BodUlozen == false)
                {
                    idZaznamu = InsertBodAP(ulozitBodAP, connection);
                }
                else
                {
                    UpdateBodAP(ulozitBodAP, connection);
                }
                connection.Close();
            }
            catch (Exception)
            {
                //Došlo k problému při práci s databází.
                //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return idZaznamu;
        }

        private static void UpdateBodAP(BodAP bodAP, SqlConnection connection)
        {
            using var command = connection.CreateCommand();
            //bodAP byl uložen
            //aktualizovat bodAP
            command.CommandType = CommandType.Text;
            command.CommandText = $"UPDATE BodAP SET " +
                                  $"OdkazNaNormu = @odkazNaNormu, " +
                                  $"HodnoceniNeshody = @hodnoceniNeshody, " +
                                  $"PopisProblemu = @popisProblemu, " +
                                  $"SkutecnaPricinaWM = @skutecnaPricinaWM, " +
                                  $"NapravnaOpatreniWM = @napravnaOpatreniWM, " +
                                  $"SkutecnaPricinaWS = @skutecnaPricinaWS, " +
                                  $"NapravnaOpatreniWS = @napravnaOpatreniWS, " +
                                  $"OdpovednaOsoba1ID = @odpovednaOsoba1Id, " +
                                  $"OdpovednaOsoba2ID = @odpovednaOsoba2Id, " +
                                  $"KontrolaEfektivnosti = @kontrolaEfektivnosti, " +
                                  $"OddeleniID = @oddeleniId, " +
                                  $"Priloha = @priloha" +
                                  $" WHERE BodAPID = @bodAPId";
            command.Parameters.AddWithValue("@bodAPId", bodAP.Id);
            if (string.IsNullOrWhiteSpace(bodAP.OdkazNaNormu))
            {
                command.Parameters.AddWithValue("@odkazNaNormu", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@odkazNaNormu", bodAP.OdkazNaNormu);
            }

            if (string.IsNullOrWhiteSpace(bodAP.HodnoceniNeshody))
            {
                command.Parameters.AddWithValue("@hodnoceniNeshody", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@hodnoceniNeshody", bodAP.HodnoceniNeshody);
            }

            command.Parameters.AddWithValue("@popisProblemu", bodAP.PopisProblemu);

            command.Parameters.AddWithValue("@skutecnaPricinaWM", bodAP.SkutecnaPricinaWM);
            command.Parameters.AddWithValue("@napravnaOpatreniWM", bodAP.NapravnaOpatreniWM);
            command.Parameters.AddWithValue("@skutecnaPricinaWS", bodAP.SkutecnaPricinaWS);
            command.Parameters.AddWithValue("@napravnaOpatreniWS", bodAP.NapravnaOpatreniWS);

            command.Parameters.AddWithValue("@odpovednaOsoba1Id", bodAP.OdpovednaOsoba1Id);

            if (bodAP.OdpovednaOsoba2Id == null)
            {
                command.Parameters.AddWithValue("@odpovednaOsoba2Id", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@odpovednaOsoba2Id", bodAP.OdpovednaOsoba2Id);
            }

            if (bodAP.KontrolaEfektivnosti == null)
            {
                command.Parameters.AddWithValue("@kontrolaEfektivnosti", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@kontrolaEfektivnosti", bodAP.KontrolaEfektivnosti);
            }

            command.Parameters.AddWithValue("@oddeleniId", bodAP.OddeleniId);

            if (string.IsNullOrWhiteSpace(bodAP.Priloha))
            {
                command.Parameters.AddWithValue("@priloha", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@priloha", bodAP.Priloha);
            }

            command.ExecuteNonQuery();
        }

        public static void UpdateKontrolaEfektivity(int bodAPId, DateTime datumEfektivita)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (var commandAkce = connection.CreateCommand())
                {
                    commandAkce.CommandType = CommandType.Text;
                    commandAkce.CommandText = $"UPDATE BodAP SET KontrolaEfektivnosti = @kontrolaEfektivnosti WHERE BodAPID = @bodAPId";
                    commandAkce.Parameters.AddWithValue("@bodAPId", bodAPId);
                    commandAkce.Parameters.AddWithValue("@kontrolaEfektivnosti", datumEfektivita);

                    commandAkce.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception)
            {
                //Došlo k problému při práci s databází.
                //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void RemoveKontrolaEfektivity(int bodAPId, DateTime? puvodniDatum, string poznamka)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (var commandAkce = connection.CreateCommand())
                {
                    commandAkce.CommandType = CommandType.Text;
                    commandAkce.CommandText = $"INSERT INTO OdstranitKontrolaEfektivnosti (BodAPID, KontrolaEfektivnosti, OdstranitDatum, Poznamka, StavObjektu) " +
                                              $"VALUES (@bodAPId, @kontrolaEfektivnosti, @odstranitDatum, @poznamka, @stavObjektu)";
                    commandAkce.Parameters.AddWithValue("@bodAPId", bodAPId);
                    commandAkce.Parameters.AddWithValue("@kontrolaEfektivnosti", puvodniDatum);
                    commandAkce.Parameters.AddWithValue("@odstranitDatum", Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    commandAkce.Parameters.AddWithValue("@poznamka", poznamka);
                    commandAkce.Parameters.AddWithValue("@stavObjektu", 1);

                    commandAkce.ExecuteNonQuery();
                }


                using (var commandAkce = connection.CreateCommand())
                {
                    commandAkce.CommandType = CommandType.Text;
                    commandAkce.CommandText = $"UPDATE BodAP SET KontrolaEfektivnosti = @kontrolaEfektivnosti WHERE BodAPID = @bodAPId";
                    commandAkce.Parameters.AddWithValue("@bodAPId", bodAPId);
                    commandAkce.Parameters.AddWithValue("@kontrolaEfektivnosti", DBNull.Value);

                    commandAkce.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception)
            {
                //Došlo k problému při práci s databází.
                //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //zjistí počet zbývajících
        public static IEnumerable<BodAP> GetZbyvajiciTerminyBodAPId(int bodAPId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM BodAP WHERE BodAPID = @bodAPId";
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
                yield return ConstructBodAP(reader);
            }
        }

        private static BodAP ConstructBodAP(IDataRecord readerData)
        {
            var bodAPid = Convert.ToInt32(readerData["BodAPID"]);
            var zamitnutiTerminu = Convert.ToByte(readerData["ZamitnutiTerminu"]);
            var zmenaTerminu = Convert.ToByte(readerData["ZmenaTerminu"]);

            return new BodAP(bodAPid, zamitnutiTerminu, zmenaTerminu);
        }
    }
}

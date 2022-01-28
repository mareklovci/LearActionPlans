using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;

using LearActionPlans.Models;
using LearActionPlans.Views;
using LearActionPlans.Utilities;

namespace LearActionPlans.DataMappers
{
    public static class BodAPDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["AkcniPlanyEntity"].ConnectionString;

        public static IEnumerable<BodAP> GetBodyAPAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
                    command.CommandText = $"SELECT * FROM BodAP";

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructBodyAP(reader);
                    }
                }
            }
        }

        public static IEnumerable<BodAP> GetBodId(int bodAPId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
                    command.CommandText = $"SELECT * FROM BodAP WHERE BodAPID = @bodAPId";
                    command.Parameters.AddWithValue("@bodAPId", bodAPId);

                    var reader = command.ExecuteReader();

                    if (reader == null)
                        yield break;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructBodyAP(reader);
                    }
                }
            }
        }

        public static IEnumerable<BodAP> GetBodyIdAP(int aPId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
                    command.CommandText = $"SELECT * FROM BodAP WHERE AkcniPlanID = @APId";
                    command.Parameters.AddWithValue("@APId", aPId);

                    var reader = command.ExecuteReader();

                    if (reader == null)
                        yield break;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructBodyAP(reader);
                    }
                }
            }
        }

        private static BodAP ConstructBodyAP(IDataRecord readerData)
        {
            int id = (int)readerData["BodAPID"];
            int akcniPlanId = (int)readerData["AkcniPlanID"];
            int cisloBoduAP = (int)readerData["CisloBoduAP"];
            DateTime datumZalozeni = (DateTime)readerData["DatumZalozeni"];
            string odkazNaNormu = DatabaseReader.ConvertString(readerData, "OdkazNaNormu");
            string hodnoceniNeshody = DatabaseReader.ConvertString(readerData, "HodnoceniNeshody");
            string popisProblemu = DatabaseReader.ConvertString(readerData, "PopisProblemu");
            string skutecnaPricinaWM = DatabaseReader.ConvertString(readerData, "SkutecnaPricinaWM");
            string napravnaOpatreniWM = DatabaseReader.ConvertString(readerData, "NapravnaOpatreniWM");
            string skutecnaPricinaWS = DatabaseReader.ConvertString(readerData, "SkutecnaPricinaWS");
            string napravnaOpatreniWS = DatabaseReader.ConvertString(readerData, "NapravnaOpatreniWS");

            int odpovednaOsoba1Id = (int)readerData["OdpovednaOsoba1ID"];
            int? odpovednaOsoba2Id = DatabaseReader.ConvertInteger(readerData, "OdpovednaOsoba2ID");
            DateTime? kontrolaEfektivnosti = DatabaseReader.ConvertDateTime(readerData, "KontrolaEfektivnosti");
            int? oddeleniId = DatabaseReader.ConvertInteger(readerData, "OddeleniID");
            string priloha = DatabaseReader.ConvertString(readerData, "Priloha");
            byte zamitnutiTerminu = (byte)readerData["ZamitnutiTerminu"];
            byte zmenaTerminu = (byte)readerData["ZmenaTerminu"];

            //List<Akce> typAkce = new List<Akce>();
            bool znovuOtevrit = (bool)readerData["ZnovuOtevrit"];
            byte stavObjektu = (byte)readerData["StavObjektu"];

            return new BodAP(id, akcniPlanId, cisloBoduAP, datumZalozeni, odkazNaNormu, hodnoceniNeshody, popisProblemu, 
                skutecnaPricinaWM, napravnaOpatreniWM, skutecnaPricinaWS, napravnaOpatreniWS, odpovednaOsoba1Id, odpovednaOsoba2Id, kontrolaEfektivnosti, oddeleniId, priloha, zamitnutiTerminu, zmenaTerminu,
                znovuOtevrit, true, stavObjektu);
        }

        //public static IEnumerable<Akce> GetUkonceniBodAPId(int bodAPId)
        //{
        //    using (var connection = new SqlConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandType = CommandType.Text;

        //            command.CommandText = $"SELECT * FROM UkonceniBodAP WHERE BodAPID = @bodAPId";
        //            command.Parameters.AddWithValue("@bodAPId", bodAPId);

        //            var reader = command.ExecuteReader();

        //            if (reader == null)
        //                yield break;

        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                    yield return ConstructAkce(reader);
        //            }
        //        }
        //    }
        //}

        public static IEnumerable<UkonceniBodAP> GetUkonceniAkceZadost()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText = $"SELECT * FROM UkonceniAkce WHERE StavZadosti = @stavZadosti";
                    command.Parameters.AddWithValue("@stavZadosti", 3);

                    var reader = command.ExecuteReader();

                    if (reader == null)
                        yield break;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructUkonceniAkceZadost(reader);
                    }
                }
            }
        }

        //public static IEnumerable<Akce> GetAkceAll()
        //{
        //    using (var connection = new SqlConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandType = CommandType.Text;

        //            command.CommandText = $"SELECT * FROM Akce";

        //            var reader = command.ExecuteReader();

        //            if (reader == null)
        //                yield break;

        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                    yield return ConstructAkceZadost(reader);
        //            }
        //        }
        //    }
        //}
        //private static Akce ConstructAkceZadost(IDataRecord readerData)
        //{
        //    int id = Convert.ToInt32(readerData["AkceID"]);
        //    int bodyAPId = Convert.ToInt32(readerData["BodAPID"]);
        //    string napravnaOpatreni = DatabaseReader.ConvertString(readerData, "NapravnaOpatreni");
        //    int odpovednaOsoba1 = Convert.ToInt32(readerData["OdpovednaOsoba1"]);
        //    int? odpovednaOsoba2 = DatabaseReader.ConvertInteger(readerData, "OdpovednaOsoba2");
        //    List<UkonceniBodAP> ukonceniAkce = new List<UkonceniBodAP>();
        //    DateTime? kontrolaEfektivnosti = DatabaseReader.ConvertDateTime(readerData, "KontrolaEfektivnosti");
        //    int? oddeleniId = DatabaseReader.ConvertInteger(readerData, "OddeleniId");
        //    string priloha = DatabaseReader.ConvertString(readerData, "Priloha");
        //    byte typ = Convert.ToByte(readerData["Typ"]);
        //    byte stavObjektu = Convert.ToByte(readerData["StavObjektu"]);

        //    return new Akce(id, bodyAPId, napravnaOpatreni, odpovednaOsoba1, odpovednaOsoba2, ukonceniAkce, kontrolaEfektivnosti, oddeleniId, priloha, typ, stavObjektu);
        //}

        private static UkonceniBodAP ConstructUkonceniAkceZadost(IDataRecord readerData)
        {
            int id = Convert.ToInt32(readerData["UkonceniAkceID"]);
            int akceId = Convert.ToInt32(readerData["AkceID"]);
            DateTime ukonceniAkce = Convert.ToDateTime(readerData["DatumUkonceni"]);
            string poznamka = DatabaseReader.ConvertString(readerData, "Poznamka");
            string odpoved = DatabaseReader.ConvertString(readerData, "Odpoved");
            byte stavZadosti = Convert.ToByte(readerData["StavZadosti"]);
            byte stavObjektu = Convert.ToByte(readerData["StavObjektu"]);

            return new UkonceniBodAP(id, akceId, ukonceniAkce, poznamka, odpoved, stavZadosti, stavObjektu);
        }

        //private static Akce ConstructAkce(IDataRecord readerData)
        //{
        //    int id = Convert.ToInt32(readerData["AkceID"]);
        //    int bodyAPId = Convert.ToInt32(readerData["BodAPID"]);
        //    string napravnaOpatreni = DatabaseReader.ConvertString(readerData, "NapravnaOpatreni");
        //    int odpovednaOsoba1 = Convert.ToInt32(readerData["OdpovednaOsoba1"]);
        //    int? odpovednaOsoba2 = DatabaseReader.ConvertInteger(readerData, "OdpovednaOsoba2");
        //    List<UkonceniBodAP> ukonceniAkce = new List<UkonceniBodAP>();
        //    DateTime? kontrolaEfektivnosti = DatabaseReader.ConvertDateTime(readerData, "KontrolaEfektivnosti");
        //    int? oddeleniId = DatabaseReader.ConvertInteger(readerData, "OddeleniId");
        //    string priloha = DatabaseReader.ConvertString(readerData, "Priloha");
        //    byte typ = Convert.ToByte(readerData["Typ"]);
        //    bool reopen = Convert.ToBoolean(readerData["ZnovuOtevrit"]);
        //    byte stavObjektu = Convert.ToByte(readerData["StavObjektu"]);

        //    return new Akce(id, bodyAPId, napravnaOpatreni, odpovednaOsoba1, odpovednaOsoba2, ukonceniAkce, kontrolaEfektivnosti, oddeleniId, priloha, typ, stavObjektu, true, reopen);
        //}

        public static IEnumerable<UkonceniBodAP> GetUkonceniBodAP(int bodAPId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText = $"SELECT * FROM UkonceniBodAP WHERE BodAPID = @bodAPId";
                    command.Parameters.AddWithValue("@bodAPId", bodAPId);

                    var reader = command.ExecuteReader();

                    if (reader == null)
                        yield break;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructUkonceniBodAP(reader);
                    }
                }
            }
        }

        private static UkonceniBodAP ConstructUkonceniBodAP(IDataRecord readerData)
        {
            int id = Convert.ToInt32(readerData["UkonceniBodAPID"]);
            int bodAPId = Convert.ToInt32(readerData["BodAPID"]);
            DateTime ukonceniBodAP = Convert.ToDateTime(readerData["DatumUkonceni"]);
            string poznamka = DatabaseReader.ConvertString(readerData, "Poznamka");
            string odpoved = DatabaseReader.ConvertString(readerData, "Odpoved");
            byte stavZadosti = Convert.ToByte(readerData["StavZadosti"]);
            byte stavObjektu = Convert.ToByte(readerData["StavObjektu"]);

            return new UkonceniBodAP(id, bodAPId, ukonceniBodAP, poznamka, odpoved, stavZadosti, stavObjektu, true);
        }

        //public static void InsertBodyAP(FormNovyAkcniPlan.AkcniPlanTmp akcniPlany)
        //public static void InsertUpdateBodyAP(int idAP)
        public static int InsertUpdateBodAP(BodAP ulozitBodAP)
        {
            int idZaznamu = 0;

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    if (ulozitBodAP.BodUlozen == false)
                    {
                        idZaznamu = InsertBodAP(ulozitBodAP, connection);

                        //foreach (var akce in ulozitBodAP.TypAkce)
                        //{
                        //    InsertAkce(idZaznamu, akce, connection);
                        //}
                    }
                    else
                    {
                        UpdateBodAP(ulozitBodAP, connection);

                        //foreach (Akce akce in ulozitBodAP.TypAkce)
                        //{
                        //    if (akce.AkceUlozena == true)
                        //    {
                        //        if (akce.StavObjektuAkce == 1)
                        //            UpdateAkce(akce.Id, akce, connection);
                        //    }
                        //    else
                        //    {
                        //        idZaznamu = ulozitBodAP.Id;
                        //        InsertAkce(idZaznamu, akce, connection);
                        //    }
                        //}
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //Došlo k problému při práci s databází.
                //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return idZaznamu;
        }

        private static int InsertBodAP(BodAP bodAP, SqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                //bodAP ještě nebyl uložen a tak bude proveden pouze Insert
                //------------------------------------------------------------------------------------------
                command.CommandType = CommandType.Text;
                command.CommandText = $"INSERT INTO BodAP (AkcniPlanID, CisloBoduAP, DatumZalozeni, OdkazNaNormu, HodnoceniNeshody, PopisProblemu, " +
                    $"SkutecnaPricinaWM, NapravnaOpatreniWM, SkutecnaPricinaWS, NapravnaOpatreniWS, OdpovednaOsoba1ID, OdpovednaOsoba2ID, " +
                    $"KontrolaEfektivnosti, OddeleniID, Priloha, ZnovuOtevrit, StavObjektu) output INSERTED.BodAPID VALUES" +
                    $"(@APId, @cisloBoduAP, @datumZalozeni, @odkazNaNormu, @hodnoceniNeshody, @popisProblemu, " +
                    $"@skutecnaPricinaWM, @napravnaOpatreniWM, @skutecnaPricinaWS, @napravnaOpatreniWS, " +
                    $"@odpovednaOsoba1Id, @odpovednaOsoba2Id, @kontrolaEfektivnosti, @oddeleniId,  @priloha, @znovuOtevrit, @stavObjektu)";
                command.Parameters.AddWithValue("@APId", bodAP.AkcniPlanId);
                command.Parameters.AddWithValue("@cisloBoduAP", bodAP.CisloBoduAP);
                command.Parameters.AddWithValue("@datumZalozeni", DateTime.Now);
                if (string.IsNullOrWhiteSpace(bodAP.OdkazNaNormu))
                    command.Parameters.AddWithValue("@odkazNaNormu", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@odkazNaNormu", bodAP.OdkazNaNormu);
                if (string.IsNullOrWhiteSpace(bodAP.HodnoceniNeshody))
                    command.Parameters.AddWithValue("@hodnoceniNeshody", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@hodnoceniNeshody", bodAP.HodnoceniNeshody);
                command.Parameters.AddWithValue("@popisProblemu", bodAP.PopisProblemu);

                command.Parameters.AddWithValue("@skutecnaPricinaWM", bodAP.SkutecnaPricinaWM);
                command.Parameters.AddWithValue("@napravnaOpatreniWM", bodAP.NapravnaOpatreniWM);
                command.Parameters.AddWithValue("@skutecnaPricinaWS", bodAP.SkutecnaPricinaWS);
                command.Parameters.AddWithValue("@napravnaOpatreniWS", bodAP.NapravnaOpatreniWS);

                command.Parameters.AddWithValue("@odpovednaOsoba1Id", bodAP.OdpovednaOsoba1Id);

                if (bodAP.OdpovednaOsoba2Id == null)
                    command.Parameters.AddWithValue("@odpovednaOsoba2Id", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@odpovednaOsoba2Id", bodAP.OdpovednaOsoba2Id);

                if (bodAP.KontrolaEfektivnosti == null)
                    command.Parameters.AddWithValue("@kontrolaEfektivnosti", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@kontrolaEfektivnosti", bodAP.KontrolaEfektivnosti);

                if (bodAP.OddeleniId == null)
                    command.Parameters.AddWithValue("@oddeleniId", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@oddeleniId", bodAP.OddeleniId);

                if (string.IsNullOrWhiteSpace(bodAP.Priloha))
                    command.Parameters.AddWithValue("@priloha", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@priloha", bodAP.Priloha);

                command.Parameters.AddWithValue("@znovuOtevrit", bodAP.ZnovuOtevrit);
                command.Parameters.AddWithValue("@stavObjektu", 1);

                int idZaznamu = Convert.ToInt32(command.ExecuteScalar());

                if (bodAP.DatumUkonceni == null) { }
                else
                {
                    using (var commandUkonceniDatum = connection.CreateCommand())
                    {
                        commandUkonceniDatum.CommandType = CommandType.Text;
                        commandUkonceniDatum.CommandText = $"INSERT INTO UkonceniBodAP (BodAPID, DatumUkonceni, Poznamka) VALUES" +
                            $"(@bodAPId, @datumUkonceni, @poznamka)";
                        commandUkonceniDatum.Parameters.AddWithValue("@bodAPId", idZaznamu);
                        commandUkonceniDatum.Parameters.AddWithValue("@datumUkonceni", bodAP.DatumUkonceni);
                        if (string.IsNullOrWhiteSpace(bodAP.UkonceniPoznamka))
                            commandUkonceniDatum.Parameters.AddWithValue("@poznamka", DBNull.Value);
                        else
                            commandUkonceniDatum.Parameters.AddWithValue("@poznamka", bodAP.UkonceniPoznamka);

                        commandUkonceniDatum.ExecuteNonQuery();
                    }
                }
                //foreach (var ukonceni in bodAP.UkonceniBodAP)
                //{
                //    using (var commandUkonceniDatum = connection.CreateCommand())
                //    {
                //        commandUkonceniDatum.CommandType = CommandType.Text;
                //        commandUkonceniDatum.CommandText = $"INSERT INTO UkonceniBodAP (BodAPID, DatumUkonceni, Poznamka) VALUES" +
                //            $"(@bodAPId, @datumUkonceni, @poznamka)";
                //        commandUkonceniDatum.Parameters.AddWithValue("@bodAPId", idZaznamu);
                //        commandUkonceniDatum.Parameters.AddWithValue("@datumUkonceni", ukonceni.DatumUkonceni);
                //        if (string.IsNullOrWhiteSpace(ukonceni.Poznamka))
                //            commandUkonceniDatum.Parameters.AddWithValue("@poznamka", DBNull.Value);
                //        else
                //            commandUkonceniDatum.Parameters.AddWithValue("@poznamka", ukonceni.Poznamka);

                //        commandUkonceniDatum.ExecuteNonQuery();
                //    }
                //}

                return idZaznamu;
            }
        }

        private static void UpdateBodAP(BodAP bodAP, SqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
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
                    command.Parameters.AddWithValue("@odkazNaNormu", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@odkazNaNormu", bodAP.OdkazNaNormu);
                if (string.IsNullOrWhiteSpace(bodAP.HodnoceniNeshody))
                    command.Parameters.AddWithValue("@hodnoceniNeshody", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@hodnoceniNeshody", bodAP.HodnoceniNeshody);
                command.Parameters.AddWithValue("@popisProblemu", bodAP.PopisProblemu);

                command.Parameters.AddWithValue("@skutecnaPricinaWM", bodAP.SkutecnaPricinaWM);
                command.Parameters.AddWithValue("@napravnaOpatreniWM", bodAP.NapravnaOpatreniWM);
                command.Parameters.AddWithValue("@skutecnaPricinaWS", bodAP.SkutecnaPricinaWS);
                command.Parameters.AddWithValue("@napravnaOpatreniWS", bodAP.NapravnaOpatreniWS);

                command.Parameters.AddWithValue("@odpovednaOsoba1Id", bodAP.OdpovednaOsoba1Id);

                if (bodAP.OdpovednaOsoba2Id == null)
                    command.Parameters.AddWithValue("@odpovednaOsoba2Id", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@odpovednaOsoba2Id", bodAP.OdpovednaOsoba2Id);

                if (bodAP.KontrolaEfektivnosti == null)
                    command.Parameters.AddWithValue("@kontrolaEfektivnosti", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@kontrolaEfektivnosti", bodAP.KontrolaEfektivnosti);

                if (bodAP.OddeleniId == null)
                    command.Parameters.AddWithValue("@oddeleniId", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@oddeleniId", bodAP.OddeleniId);

                if (string.IsNullOrWhiteSpace(bodAP.Priloha))
                    command.Parameters.AddWithValue("@priloha", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@priloha", bodAP.Priloha);
                command.ExecuteNonQuery();
            }
        }

        //private static void InsertAkce(int idZaznamu, Akce akce, SqlConnection connection)
        //{
        //    //int idAkce = 0;

        //    using (var commandAkce = connection.CreateCommand())
        //    {
        //        commandAkce.CommandType = CommandType.Text;
        //        commandAkce.CommandText = $"INSERT INTO Akce (BodAPID, SkutecnaPricina, NapravnaOpatreni, Typ, StavObjektu) output INSERTED.AkceID VALUES" +
        //            $"(@bodAPId, @skutecnaPricina, @napravnaOpatreni, @typ, @stavObjektu)";
        //        commandAkce.Parameters.AddWithValue("@bodAPId", idZaznamu);
        //        commandAkce.Parameters.AddWithValue("@skutecnaPricina", akce.SkutecnaPricina);
        //        commandAkce.Parameters.AddWithValue("@napravnaOpatreni", akce.NapravnaOpatreni);

        //        commandAkce.Parameters.AddWithValue("@typ", akce.Typ);
        //        commandAkce.Parameters.AddWithValue("@stavObjektu", 1);

        //        commandAkce.ExecuteNonQuery();
        //    }
        //}

        //public static void UpdateAkce(int akceId, Akce akce, SqlConnection connection)
        //{
        //    using (var commandAkce = connection.CreateCommand())
        //    {
        //        commandAkce.CommandType = CommandType.Text;
        //        commandAkce.CommandText = $"UPDATE Akce SET " +
        //            $"SkutecnaPricina = @skutecnaPricina," +
        //            $" NapravnaOpatreni = @napravnaOpatreni," +
        //            $" StavObjektu = @stavObjektu " +
        //            $"WHERE AkceID = @akceId";
        //        commandAkce.Parameters.AddWithValue("@akceId", akceId);
        //        commandAkce.Parameters.AddWithValue("@skutecnaPricina", akce.SkutecnaPricina);
        //        commandAkce.Parameters.AddWithValue("@napravnaOpatreni", akce.NapravnaOpatreni);

        //        commandAkce.Parameters.AddWithValue("@stavObjektu", akce.StavObjektuAkce);
        //        //WM = 1
        //        commandAkce.ExecuteNonQuery();
        //    }
        //}

        public static void UpdateKontrolaEfektivity(int bodAPId, DateTime datumEfektivita)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
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
            }
            catch (Exception ex)
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
                using (var connection = new SqlConnection(ConnectionString))
                {
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
            }
            catch (Exception ex)
            {
                //Došlo k problému při práci s databází.
                //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //zjistí počet zbývajících 
        public static IEnumerable<BodAP> GetZbyvajiciTerminyBodAPId(int bodAPId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText = $"SELECT * FROM BodAP WHERE BodAPID = @bodAPId";
                    command.Parameters.AddWithValue("@bodAPId", bodAPId);

                    var reader = command.ExecuteReader();

                    if (reader == null)
                        yield break;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructBodAP(reader);
                    }
                }
            }
        }

        private static BodAP ConstructBodAP(IDataRecord readerData)
        {
            int bodAPid = Convert.ToInt32(readerData["BodAPID"]);
            byte zamitnutiTerminu = Convert.ToByte(readerData["ZamitnutiTerminu"]);
            byte zmenaTerminu = Convert.ToByte(readerData["ZmenaTerminu"]);

            return new BodAP(bodAPid, zamitnutiTerminu, zmenaTerminu);
        }
    }
}

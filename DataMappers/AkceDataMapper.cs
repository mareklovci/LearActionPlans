using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using LearActionPlans.Models;
using LearActionPlans.Views;

namespace LearActionPlans.DataMappers
{
    public class AkceDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["AkcniPlanyEntity"].ConnectionString;

        public static IEnumerable<Akce> GetAkceAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText = $"SELECT * FROM Akce WHERE StavObjektu = @stavObjektu";
                    command.Parameters.AddWithValue("@stavObjektu", 1);

                    var reader = command.ExecuteReader();

                    if (reader == null)
                        yield break;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructAkceAll(reader);
                    }
                }
            }
        }

        private static Akce ConstructAkceAll(IDataRecord readerData)
        {
            int id = Convert.ToInt32(readerData["AkceID"]);
            int bodAPId = Convert.ToInt32(readerData["BodAPID"]);
            byte stavObjektu = Convert.ToByte(readerData["StavObjektu"]);

            return new Akce(id, bodAPId, stavObjektu);
        }

        //public static void UpdateAkce(int cisloRadkyDGVBody, List<int> opraveneAkce)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(ConnectionString))
        //        {
        //            connection.Open();

        //            foreach (Akce akce in FormPrehledBoduAP.bodyAP[cisloRadkyDGVBody].TypAkce)
        //            {
        //                //budou se aktualizovat poute záznamy, které patří jednomu majiteli
        //                if (opraveneAkce.Contains(akce.Id))
        //                {
        //                    BodAPDataMapper.UpdateAkce(akce.Id, akce, connection);
        //                }
        //            }
        //            connection.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Došlo k problému při práci s databází.
        //        //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
    }
}

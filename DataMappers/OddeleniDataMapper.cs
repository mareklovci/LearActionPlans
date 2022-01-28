using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using LearActionPlans.Models;

namespace LearActionPlans.DataMappers
{
    public static class OddeleniDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["OddeleniEntity"].ConnectionString;

        public static IEnumerable<Oddeleni> GetOddeleniAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    //command.CommandText = $"SELECT akcniPlan_Id, MAX(cisloAP) AS maxCislo FROM AkcniPlany WHERE rok = @rok GROUP BY akcniPlan_Id";
                    command.CommandText = $"SELECT * FROM Oddeleni";

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            yield return ConstructOddeleniAll(reader);
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.ToString());
            //    //Problém s databází.
            //    MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private static Oddeleni ConstructOddeleniAll(IDataRecord readerData)
        {
            int id = (int)readerData["OddeleniID"];
            string nazev = (string)readerData["Nazev"];
            byte stavObjektu = (byte)readerData["StavObjektu"];

            return new Oddeleni(id, nazev, stavObjektu);
        }

    }
}

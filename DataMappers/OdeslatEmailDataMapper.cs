using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LearActionPlans.DataMappers
{
    public static partial class OdeslatEmailDataMapper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ActionPlansEntity"].ConnectionString;

        public static void InsertEmail(string emailTo, string predmet, string zprava, List<int>odeslaneEmaily)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (var commandAkce = connection.CreateCommand())
                {
                    commandAkce.CommandType = CommandType.Text;
                    commandAkce.CommandText = $"INSERT INTO OdeslatEmail (EmailKomu, Predmet, Zprava) " +
                                              $"VALUES (@emailKomu, @predmet, @zprava)";
                    commandAkce.Parameters.AddWithValue("@emailKomu", emailTo);
                    commandAkce.Parameters.AddWithValue("@predmet", predmet);
                    commandAkce.Parameters.AddWithValue("@zprava", zprava);

                    commandAkce.ExecuteNonQuery();
                }

                // body AP, které byly odeslány, byly nastaveny jako odeslané
                foreach (var jedenBodAP in odeslaneEmaily)
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = $"UPDATE BodAP SET EmailOdeslan = @emailOdeslan " +
                                          $"WHERE BodAPID = @bodAPId";

                    command.Parameters.AddWithValue("@emailOdeslan", 1);
                    command.Parameters.AddWithValue("@bodAPId", jedenBodAP);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                //Došlo k problému při práci s databází.
                //MessageBox.Show(ex.ToString(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Database problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}

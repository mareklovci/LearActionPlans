using System;
using System.Data;
using System.Data.SqlClient;
using LearActionPlans.Models;

namespace LearActionPlans.Repositories
{
    public partial class ActionPlanPointRepository
    {
        private int InsertBodAP(BodAP bodAP, SqlConnection connection)
        {
            using var command = connection.CreateCommand();
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

            if (string.IsNullOrEmpty(bodAP.SkutecnaPricinaWM))
            {
                command.Parameters.AddWithValue("@skutecnaPricinaWM", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@skutecnaPricinaWM", bodAP.SkutecnaPricinaWM);
            }
            if (string.IsNullOrEmpty(bodAP.NapravnaOpatreniWM))
            {
                command.Parameters.AddWithValue("@napravnaOpatreniWM", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@napravnaOpatreniWM", bodAP.NapravnaOpatreniWM);
            }
            if (string.IsNullOrEmpty(bodAP.SkutecnaPricinaWS))
            {
                command.Parameters.AddWithValue("@skutecnaPricinaWS", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@skutecnaPricinaWS", bodAP.SkutecnaPricinaWS);
            }
            if (string.IsNullOrEmpty(bodAP.NapravnaOpatreniWS))
            {
                command.Parameters.AddWithValue("@napravnaOpatreniWS", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@napravnaOpatreniWS", bodAP.NapravnaOpatreniWS);
            }

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

            command.Parameters.AddWithValue("@znovuOtevrit", bodAP.ZnovuOtevrit);
            command.Parameters.AddWithValue("@stavObjektu", 1);

            var idZaznamu = Convert.ToInt32(command.ExecuteScalar());

            if (bodAP.DatumUkonceni == null) { }
            else
            {
                using var commandUkonceniDatum = connection.CreateCommand();
                commandUkonceniDatum.CommandType = CommandType.Text;
                commandUkonceniDatum.CommandText = $"INSERT INTO UkonceniBodAP (BodAPID, DatumUkonceni, Poznamka) VALUES" +
                                                   $"(@bodAPId, @datumUkonceni, @poznamka)";
                commandUkonceniDatum.Parameters.AddWithValue("@bodAPId", idZaznamu);
                commandUkonceniDatum.Parameters.AddWithValue("@datumUkonceni", bodAP.DatumUkonceni);
                if (string.IsNullOrWhiteSpace(bodAP.UkonceniPoznamka))
                {
                    commandUkonceniDatum.Parameters.AddWithValue("@poznamka", DBNull.Value);
                }
                else
                {
                    commandUkonceniDatum.Parameters.AddWithValue("@poznamka", bodAP.UkonceniPoznamka);
                }

                commandUkonceniDatum.ExecuteNonQuery();
            }

            return idZaznamu;
        }
    }
}

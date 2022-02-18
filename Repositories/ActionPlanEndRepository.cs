using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LearActionPlans.Interfaces;
using LearActionPlans.Models;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.Repositories
{
    public class ActionPlanEndRepository : IGenericRepository<UkonceniAP>
    {
        private readonly string connectionString;

        public ActionPlanEndRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor) =>
            this.connectionString = optionsMonitor.CurrentValue.LearDataAll;

        public IEnumerable<UkonceniAP> GetUkonceniAP(int apId)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM UkonceniAP WHERE AkcniPlanID = @apId ORDER BY UkonceniAPID DESC";
            command.Parameters.AddWithValue("@apId", apId);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return this.ConstructUkonceniAP(reader);
            }
        }

        private UkonceniAP ConstructUkonceniAP(IDataRecord readerData)
        {
            var id = Convert.ToInt32(readerData["UkonceniAPID"]);
            var apId = Convert.ToInt32(readerData["AkcniPlanID"]);
            var datumUkonceniAP = Convert.ToDateTime(readerData["DatumUkonceni"]);
            var poznamka = Convert.ToString(readerData["Poznamka"]);

            return new UkonceniAP(id, apId, datumUkonceniAP, poznamka);
        }

        public IEnumerable<UkonceniAP> GetAll() => throw new NotImplementedException();

        public UkonceniAP GetById(int id) => throw new NotImplementedException();

        public void Insert(UkonceniAP obj) => throw new NotImplementedException();

        public void Update(UkonceniAP obj) => throw new NotImplementedException();

        public void Delete(int id) => throw new NotImplementedException();

        public void Save() => throw new NotImplementedException();
    }
}

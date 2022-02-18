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
    public partial class ActionPlanRepository : IGenericRepository<AkcniPlany>
    {
        private readonly string connectionString;

        public ActionPlanRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor) =>
            this.connectionString = optionsMonitor.CurrentValue.LearDataAll;

        public IEnumerable<AkcniPlany> GetAll()
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM AkcniPlan ORDER BY DatumZalozeni, CisloAP";

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructAllAP(reader);
            }
        }

        public AkcniPlany GetById(int id) => throw new NotImplementedException();

        public void Insert(AkcniPlany obj) => throw new NotImplementedException();

        public void Update(AkcniPlany obj) => throw new NotImplementedException();

        public void Delete(int id) => throw new NotImplementedException();

        public void Save() => throw new NotImplementedException();
    }
}

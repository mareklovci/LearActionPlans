using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LearActionPlans.Interfaces;
using LearActionPlans.Models;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.Repositories
{
    public class CustomerRepository : IGenericRepository<Zakaznici>
    {
        private readonly string connectionString;

        public CustomerRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor) =>
            this.connectionString = optionsMonitor.CurrentValue.LearDataAll;

        public IEnumerable<Zakaznici> GetAll()
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = "SELECT * FROM Zakaznik";

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return CreateCustomer(reader);
            }
        }

        public Zakaznici GetById(int id) => throw new System.NotImplementedException();

        public void Insert(Zakaznici obj) => throw new System.NotImplementedException();

        public void Update(Zakaznici obj) => throw new System.NotImplementedException();

        public void Delete(int id) => throw new System.NotImplementedException();

        public void Save() => throw new System.NotImplementedException();

        private static Zakaznici CreateCustomer(IDataRecord reader)
        {
            var id = (int)reader["ZakaznikID"];
            var name = (string)reader["Nazev"];
            var objectState = (byte)reader["StavObjektu"];

            return new Zakaznici(id, name, objectState);
        }
    }
}

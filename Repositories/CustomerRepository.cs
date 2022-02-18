using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LearActionPlans.Interfaces;
using LearActionPlans.Models;

namespace LearActionPlans.Repositories
{
    public class CustomerRepository : IGenericRepository<Zakaznici>
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ActionPlansEntity"].ConnectionString;

        public static IEnumerable<Zakaznici> GetZakazniciAll()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            command.CommandText = $"SELECT * FROM Zakaznik";

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                yield break;
            }

            while (reader.Read())
            {
                yield return ConstructZakaznik(reader);
            }
        }

        private static Zakaznici ConstructZakaznik(IDataRecord reader)
        {
            var id = (int)reader["ZakaznikID"];
            var nazev = (string)reader["Nazev"];
            var stavObjektu = (byte)reader["StavObjektu"];

            return new Zakaznici(id, nazev, stavObjektu);
        }

        public IEnumerable<Zakaznici> GetAll() => throw new System.NotImplementedException();

        public Zakaznici GetById(int id) => throw new System.NotImplementedException();

        public void Insert(Zakaznici obj) => throw new System.NotImplementedException();

        public void Update(Zakaznici obj) => throw new System.NotImplementedException();

        public void Delete(int id) => throw new System.NotImplementedException();

        public void Save() => throw new System.NotImplementedException();
    }
}

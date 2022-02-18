using System.Collections.Generic;

namespace LearActionPlans.Interfaces
{
    public interface IGenericRepository <T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(int id);
        void Save();
    }
}

using System;
using System.Collections.Generic;
using LearActionPlans.Interfaces;
using LearActionPlans.Models;
using LearActionPlans.Utilities;
using Microsoft.Extensions.Options;

namespace LearActionPlans.Repositories
{
    public partial class ActionPlanPointRepository : IGenericRepository<BodAP>
    {
        private readonly string connectionString;

        public ActionPlanPointRepository(IOptionsMonitor<ConnectionStringsOptions> optionsMonitor) =>
            this.connectionString = optionsMonitor.CurrentValue.LearDataAll;

        public IEnumerable<BodAP> GetAll() => throw new NotImplementedException();

        public BodAP GetById(int id) => throw new NotImplementedException();

        public void Insert(BodAP obj) => throw new NotImplementedException();

        public void Update(BodAP obj) => throw new NotImplementedException();

        public void Delete(int id) => throw new NotImplementedException();

        public void Save() => throw new NotImplementedException();
    }
}

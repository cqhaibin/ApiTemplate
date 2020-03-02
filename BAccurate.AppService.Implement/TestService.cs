using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.AppServices;
using BAccurate.Models.Test;
using SAM.Framework.Result;
using SAM.Framework;
using BAccurate.Repository.Freesql;
using BAccurate.Repository.Freesql.Entities;

namespace BAccurate.AppService.Implement
{
    public class TestService : ITestService
    {
        private BAccurateContext context;
        private IFMapper mapper;

        public TestService(BAccurateContext context, IFMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public ResultListInfo<TestInfo> GetAll(QueryTest query)
        {
            var entites = this.context.testDb.Select.ToList();
            return new ResultListInfo<TestInfo>()
            {
                List = this.mapper.Map<List<TestInfo>>(entites)
            };
        }

        public ResultInfo Remove(int id)
        {
            this.context.testDb.Remove(m => m.Id == id);
            this.context.SaveChanges();
            return new ResultInfo();
        }

        public ResultInfo Save(TestInfo test)
        {
            this.context.testDb.Add(this.mapper.Map<TestEntity>(test));
            this.context.SaveChanges();
            return new ResultInfo();
        }
    }
}

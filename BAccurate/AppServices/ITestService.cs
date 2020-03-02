using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Models.Test;
using SAM.Framework.Result;
using SAM.Framework;

namespace BAccurate.AppServices
{
    public interface ITestService : ITransient
    {
        ResultListInfo<TestInfo> GetAll(QueryTest query);

        ResultInfo Save(TestInfo test);

        ResultInfo Remove(int id);
    }
}

using BAccurate.Models.Resource;
using SAM.Framework;
using SAM.Framework.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.AppServices
{
    public interface IResourceAppService : ITransient
    {
        ResultDataInfo<string> Save(ResourceInfo info);

        ResultInfo Remove(string id);

        ResultListInfo<ResourceInfo> GetAllList();
    }
}

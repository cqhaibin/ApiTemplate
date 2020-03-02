using System;
using BAccurate.Models.S_Staffinfo;
using SAM.Framework;
using SAM.Framework.Result;

namespace BAccurate.AppServices
{
    public interface IS_StaffinfoService : ITransient
    {
        ResultListOfPageInfo<S_StaffinfoInfo> GetAll(S_StaffinfoQuery query);

        ResultInfo Save(S_StaffinfoInfo info);

        ResultInfo Remove(Int64 id);
    }
}

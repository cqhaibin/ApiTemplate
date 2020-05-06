using BAccurate.AppServices;
using BAccurate.Models.S_Staffinfo;
using SAM.Framework.Result;
using System.Web.Http;
using System;

namespace ConsoleWebApi.Controllers
{
    public class S_StaffinfoController : BaseController
    {
        private IS_StaffinfoService s_StaffinfoService;
        public S_StaffinfoController(IS_StaffinfoService s_StaffinfoService)
        {
            this.s_StaffinfoService = s_StaffinfoService;
        }

        [HttpPost]
        public ResultInfo PostSave([FromBody] S_StaffinfoInfo info)
        {
            return this.s_StaffinfoService.Save(info);
        }

        [HttpGet]
        public ResultListOfPageInfo<S_StaffinfoInfo> GetAll([FromUri]S_StaffinfoQuery query)
        {
            return this.s_StaffinfoService.GetAll(query);
        }
		
		[HttpDelete]
        public ResultInfo Remove([FromUri]Int64 id)
        {
            return this.s_StaffinfoService.Remove(id);
        }
    }
}

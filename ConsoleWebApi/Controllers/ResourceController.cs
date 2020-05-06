using BAccurate.AppServices;
using BAccurate.Models.Resource;
using SAM.Framework.Result;
using System.Web.Http;

namespace ConsoleWebApi.Controllers
{
    public class ResourceController : BaseController
    {
        private IResourceAppService resourceService;
        public ResourceController(IResourceAppService resourceService)
        {
            this.resourceService = resourceService;
        }

        [HttpPost]
        public ResultDataInfo<string> Save([FromBody]ResourceInfo info)
        {
            return this.resourceService.Save(info);
        }

        [HttpGet]
        public ResultListInfo<ResourceInfo> GetAllResources()
        {
            return this.resourceService.GetAllList();
        }

        [HttpGet]
        public ResultInfo Remove([FromUri]string id)
        {
            return this.resourceService.Remove(id);
        }
    }
}

using BAccurate.AppServices;
using BAccurate.Models.Role;
using SAM.Framework.Result;
using System.Web.Http;

namespace BAccurate.WebApi.Controllers
{
    public class RoleController : ApiController
    {
        private IRoleAppService roleService;
        public RoleController(IRoleAppService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet]
        public ResultListInfo<RoleInfo> GetAll()
        {
            return this.roleService.GetAllRoles();
        }
        
        [HttpGet]
        public ResultDataInfo<RoleDetailInfo> Get([FromUri]int roleId)
        {
            return this.roleService.Get(roleId);
        }

        [HttpPost]
        public ResultInfo PostSave([FromBody]RoleParam param)
        {
            return this.roleService.Save(param);
        }

        [HttpDelete]
        public ResultInfo DeleteRole([FromUri]int id)
        {
            return this.roleService.Remove(id);
        }
    }
}

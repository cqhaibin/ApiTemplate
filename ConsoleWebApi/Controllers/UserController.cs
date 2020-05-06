using BAccurate.AppServices;
using BAccurate.Models.User;
using SAM.Framework.Result;
using System.Web.Http;

namespace ConsoleWebApi.Controllers
{
    public class UserController : BaseController
    {
        private IUserAppService userService;
        public UserController(IUserAppService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ResultListInfo<UserInfo> GetAll()
        {
            return this.userService.GetAllUsers();
        }

        [HttpGet]
        public ResultDataInfo<UserDetailInfo> Get([FromUri]int userId)
        {
            return this.userService.Get(userId);
        }

        [HttpPost]
        public ResultInfo PostSave([FromBody]UserParam param)
        {
            return this.userService.Save(param);
        }

        [HttpDelete]
        public ResultInfo DeleteUser([FromUri]int id)
        {
            return this.userService.Remove(id);
        }
    }
}

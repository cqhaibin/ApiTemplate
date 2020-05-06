using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SAM.Framework.Result;
using BAccurate.Models.Auth;
using BAccurate.AppServices;

namespace ConsoleWebApi.Controllers
{
    public class AuthController : BaseController
    {
        private IAuthAppService authAppService;
        public AuthController(IAuthAppService authAppService)
        {
            this.authAppService = authAppService;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ResultDataInfo<LoginResultInfo> PostLogin([FromBody]LoginInfo info)
        {
            return this.authAppService.Login(info, Utils.Common.GetClientInfo(Request), Utils.Common.GetToken(Request));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResultInfo LoginOut()
        {
            return this.authAppService.Logout(Utils.Common.GetToken(Request));
        }

        [HttpGet]
        [AllowAnonymous]
        public ResultInfo Verify()
        {
            return this.authAppService.Verify(Utils.Common.GetToken(Request));
        }

        [HttpGet]
        [AllowAnonymous]
        public ResultInfo Verify([FromUri]string res)
        {
            return this.authAppService.Verify(Utils.Common.GetToken(Request), res);
        }

        [HttpGet]
        public ResultListInfo<MenuInfo> GetMenus()
        {
            return this.authAppService.GetMenus(Utils.Common.GetToken(Request));
        }

        [HttpGet]
        public ResultDataInfo<IdentityInfo> GetIdentity()
        {
            return this.authAppService.GetIdentity(Utils.Common.GetToken(Request));
        }

        [HttpGet]
        public ResultDataInfo<IdentityInfo> GetIdentity([FromUri]string resCode)
        {
            return this.authAppService.GetIdentity(Utils.Common.GetToken(Request), resCode);
        }

        [HttpGet]
        public ResultDataInfo<bool> ReLoad()
        {
            return this.authAppService.ReLoadCache();
        }
    }
}
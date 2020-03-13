using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SAM.Framework.Result;
using BAccurate.Models.Auth;
using BAccurate.AppServices;

namespace BAccurate.WebApi.Controllers
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
        public ResultDataInfo<LoginResultInfo> PostLogin([FromBody]LoginInfo info)
        {
            return this.authAppService.Login(info, Utils.Common.GetClientInfo(), Utils.Common.GetToken());
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResultInfo LoginOut()
        {
            return this.authAppService.Logout(Utils.Common.GetToken());
        }
    }
}
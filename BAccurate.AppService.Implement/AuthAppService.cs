using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.AppServices;
using BAccurate.Models.Auth;
using SAM.Framework.Result;
using BAccurate.Domain;
using BAccurate.Implement.Domain;
using BAccurate.Models;

namespace BAccurate.AppService.Implement
{
    public class AuthAppService : IAuthAppService
    {
        private IOnlineUserMgr onlineUserMgr;
        private IReadAuthRepository readAuthRepository;
        public AuthAppService(IOnlineUserMgr onlineUserMgr, IReadAuthRepository readAuthRepository)
        {
            this.readAuthRepository = readAuthRepository;
            this.onlineUserMgr = onlineUserMgr;
        }

        public ResultDataInfo<LoginResultInfo> Login(LoginInfo info, RequestClientInfo clientInfo, string token = "")
        {
            int userId = 1;
            var onlineUser = new UserEntity(userId, this.readAuthRepository, clientInfo, this.onlineUserMgr);
            if (this.onlineUserMgr.Get(userId) != null)
            {
                this.onlineUserMgr.Remove(userId);
            }
            this.onlineUserMgr.Add(onlineUser);
            return new ResultDataInfo<LoginResultInfo>()
            {
                Data = new LoginResultInfo()
                {
                    Token = onlineUser.Token,
                    User = onlineUser.UserInfo
                }
            };
        }

        public ResultInfo Logout(string token)
        {
            this.onlineUserMgr.Remove(token);
            return new ResultInfo();
        }
    }
}

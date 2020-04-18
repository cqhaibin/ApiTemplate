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
using BAccurate.Repository.Freesql;

namespace BAccurate.AppService.Implement
{
    public class AuthAppService : IAuthAppService
    {
        private IOnlineUserMgr onlineUserMgr;
        private IReadAuthRepository readAuthRepository;
        private BAccurateContext context;
        public AuthAppService(IOnlineUserMgr onlineUserMgr, IReadAuthRepository readAuthRepository, BAccurateContext context)
        {
            this.readAuthRepository = readAuthRepository;
            this.onlineUserMgr = onlineUserMgr;
            this.context = context;
        }

        public ResultDataInfo<IdentityInfo> GetIdentity(string token)
        {
            return this.GetIdentity(token, string.Empty);
        }

        public ResultDataInfo<IdentityInfo> GetIdentity(string token, string resourceCode)
        {
            var menus = this.GetMenus(token).List;
            var profile = this.onlineUserMgr.Get(token);
            if (!string.IsNullOrEmpty(resourceCode) && menus != null)
            {
                menus = menus[0].Childs.Where(m => resourceCode.Contains(m.ResourceCode)).ToList();
            }

            return new ResultDataInfo<IdentityInfo>()
            {
                Data = new IdentityInfo()
                {
                    Menus = menus,
                    User = profile.UserInfo
                }
            };
        }

        public ResultListInfo<MenuInfo> GetMenus(string token)
        {
            var result = this.Verify(token);
            if (!result.IsSuccess)
            {
                return new ResultListInfo<MenuInfo>()
                {
                    IsSuccess = false,
                    ResultCode = result.ResultCode
                };
            }

            var resLs = this.onlineUserMgr.Get(token).GetMenuTree();

            return new ResultListInfo<MenuInfo>()
            {
                List = resLs
            };
        }

        public ResultDataInfo<LoginResultInfo> Login(LoginInfo info, RequestClientInfo clientInfo, string token = "")
        {
            int userId = 1;
            if (this.onlineUserMgr.Get(userId) != null)
            {
                this.onlineUserMgr.Remove(userId);
            }
            var result = new ResultDataInfo<LoginResultInfo>();

            #region 验证

            var entity = this.context.UserRepository.Select.Where(m => m.UserName == info.UserName).ToOne();
            //todo: 查找是否存在当前userId登录的数据
            if (entity == null)
            {
                result.ResultCode = OcmStatusCode.USER_NOT_FOUND; //账号不存在
                return result;
            }
            if (!entity.Enable)
            {
                //禁用
                result.ResultCode = OcmStatusCode.USER_FORBIDDEN;
                return result;
            }
            if (entity.Password != info.Password)
            {
                //密码错误
                result.ResultCode = OcmStatusCode.USER_PASSWORD_ERROR;
                return result;
            }

            #endregion

            var onlineUser = new UserEntity(userId, this.readAuthRepository, clientInfo, this.onlineUserMgr);
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
            if (string.IsNullOrEmpty(token))
            {
                return new ResultInfo()
                {
                    ResultCode = OcmStatusCode.OCM_ERROR_UNKOWN,
                    IsSuccess = false
                };
            }
            var profile = this.onlineUserMgr.Get(token);
            if (profile == null)
            {
                return new ResultInfo()
                {
                    ResultCode = OcmStatusCode.OCM_ERROR_UNKOWN,
                    IsSuccess = false
                };
            }

            this.onlineUserMgr.Remove(token);
            return new ResultInfo();
        }

        public ResultDataInfo<bool> ReLoadCache()
        {
            ResultDataInfo<bool> result = new ResultDataInfo<bool>();
            try
            {
                this.onlineUserMgr.Load();
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
            }
            return result;
        }

        public ResultInfo Verify(string token)
        {
            ResultInfo result = new ResultInfo();
            var profile = this.onlineUserMgr.Get(token);
            result.IsSuccess = false;
            if (profile == null)
            {
                result.ResultCode = OcmStatusCode.USER_VERIFY_NOT_LOGIN; //未登录
            }
            else if (!profile.Verify())
            {
                result.ResultCode = OcmStatusCode.USER_VERIFY_EXPIRE; //过期
            }
            else
            {
                result.IsSuccess = true;
            }
            return result;
        }

        public ResultInfo Verify(string token, string resCode)
        {
            var result = this.Verify(token);
            if (result.IsSuccess)
            {
                var profile = this.onlineUserMgr.Get(token);
                if (!profile.Verify(resCode))
                {
                    result.ResultCode = OcmStatusCode.USER_VERIFY_NO_ACCESS;
                    result.IsSuccess = false;
                }
            }
            return result;
        }
    }
}

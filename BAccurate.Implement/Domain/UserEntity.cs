using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Domain;
using BAccurate.Models;
using BAccurate.Models.Auth;

namespace BAccurate.Implement.Domain
{
    public class UserEntity : IUserEntity
    {
        private IReadAuthRepository readAuthRepository;
        private IOnlineUserMgr onlineUserMgr;

        public UserEntity(int userId, 
            IReadAuthRepository readAuthRepository, 
            RequestClientInfo clientInfo,
            IOnlineUserMgr onlineUserMgr)
        {
            this.readAuthRepository = readAuthRepository;
            this.onlineUserMgr = onlineUserMgr;
            this.UserInfo = this.readAuthRepository.GetUserInfo(userId);
            if (this.UserInfo == null)
            {
                this.UserInfo = new UserInfo()
                {
                    Id = userId
                };
            }
            this.ClientInfo = clientInfo;
            this.Option = new UserAuthOption();
            this.LoginTime = DateTime.Now;
            this.ExpiredTime = this.LoginTime.AddSeconds(this.Option.UserLoginExpireIn);
            //to do generate token
            string tokenstr = $"token_{UserInfo.Id}_{UserInfo.UserName}_{clientInfo.IP}_{clientInfo.OS}_{this.LoginTime.Ticks}";
            this.Token = this.GetMd5(tokenstr);
        }

        public UserEntity(TokenEntity tokenEntity,
            IReadAuthRepository readAuthRepository, IOnlineUserMgr onlineUserMgr)
        {
            this.UserInfo = tokenEntity.UserInfo;
            this.readAuthRepository = readAuthRepository;
            this.onlineUserMgr = onlineUserMgr;
            this.Option = new UserAuthOption();
            this.ClientInfo = tokenEntity.ClientInfo;
            this.LoginTime = tokenEntity.LoginTime;
            this.ExpiredTime = tokenEntity.ExpiredTime;
            this.Token = tokenEntity.Token;
        }

        
        public UserInfo UserInfo { get; protected set; }

        public string Token { get; protected set; }

        public RequestClientInfo ClientInfo { get; protected set; }

        public DateTime LoginTime { get; protected set; }

        public DateTime ExpiredTime { get; protected set; }

        public UserAuthOption Option { get; protected set; }

        public TokenEntity GetTokenEntity()
        {
            return new TokenEntity()
            {
                ClientInfo = ClientInfo,
                UserInfo = UserInfo,
                LoginTime = LoginTime,
                ExpiredTime = ExpiredTime,
                Token = this.Token
            };
        }

        public bool Verify()
        {
            return DateTime.Now <= this.ExpiredTime;
        }

        public bool Verify(string resCode)
        {
            var isVerify = this.Verify();
            if (isVerify)
            {
                isVerify = false;
                var roles = this.UserInfo.Roles;
                var roleAndRes = this.onlineUserMgr.GetAllRoleAndRes();
                if (roles != null)
                {
                    foreach(var r in roles)
                    {
                        if(roleAndRes.Any(m=>m.RoleId == r && m.ResourceCode == resCode))
                        {
                            isVerify = true;
                            break;
                        }
                    }
                }
            }
            return isVerify;
        }

        public IList<ResourceInfo> GetRes()
        {
            var roles = this.UserInfo.Roles;
            var roleAndRes = this.onlineUserMgr.GetAllRoleAndRes();
            List<string> ls = new List<string>();
            if (roles != null)
            {
                foreach (var r in roles)
                {
                    ls.AddRange(roleAndRes.Where(m => m.RoleId == r).Select(m => m.ResourceCode));
                }
            }
            var res = this.onlineUserMgr.GetAllRes();
            return res.Where(m => ls.Contains(m.ResourceCode)).ToList();
        }

        public IList<MenuInfo> GetMenuTree()
        {
            List<MenuInfo> menuInfos = new List<MenuInfo>();
            var res = this.GetRes();
            if (res != null)
            {
                res = res.Where(m => m.ResourceType == 1).ToList();
                var roots = res.Where(m => string.IsNullOrEmpty(m.ParentId)).ToList();
                foreach (var r in roots)
                {
                    var menu = this.CreateMenu(r);
                    menuInfos.Add(menu);
                    var child = res.Where(m => m.ParentId == r.Id).ToList();
                    if (child != null && child.Count > 0)
                    {
                        this.BuildMenuTree(child, menu, res);
                    }
                }
            }
            return menuInfos;
        }

        #region BuildMenu

        private void BuildMenuTree(IList<ResourceInfo> resources, MenuInfo menu, IList<ResourceInfo> allRes)
        {
            foreach (var r in resources)
            {
                var newMenu = this.CreateMenu(r);
                menu.Childs.Add(newMenu);
                var child = allRes.Where(m => m.ParentId == newMenu.Id).ToList();
                if (child != null && child.Count > 0)
                {
                    this.BuildMenuTree(child, newMenu, allRes);
                }
            }
        }
        private MenuInfo CreateMenu(ResourceInfo entity)
        {
            return new MenuInfo()
            {
                Config = entity.Config,
                Id = entity.Id,
                OrderNum = entity.OrderNum,
                PId = entity.ParentId,
                ResourceCode = entity.ResourceCode,
                ResourceName = entity.ResourceName,
                Url = entity.Url,
                Childs = new List<MenuInfo>()
            };
        }

        #endregion

        private string GetMd5(string cont)
        {
            string str = "";
            byte[] datas = Encoding.UTF8.GetBytes(cont);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(datas);
            for (var i = 0; i < bytes.Length; ++i)
            {
                str += bytes[i].ToString("x2");
            }
            return str;
        }
    }
}

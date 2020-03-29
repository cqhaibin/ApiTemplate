using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BAccurate.Domain;
using BAccurate.Models.Auth;

namespace BAccurate.Implement.Domain
{
    public class OnlineUserMgr : IOnlineUserMgr
    {
        protected Dictionary<string, IUserEntity> dicUsers = new Dictionary<string, IUserEntity>();
        /// <summary>
        /// 资源缓存池
        /// </summary>
        protected List<Models.Auth.ResourceInfo> resList = new List<Models.Auth.ResourceInfo>();
        /// <summary>
        /// 资源与角色关系的缓存
        /// </summary>
        protected List<Models.Auth.RoleAndResInfo> roleAndResList = new List<Models.Auth.RoleAndResInfo>();
        protected ITokenRepository tokenRepository;
        protected IReadAuthRepository readAuthRepository;
        protected IRoleAndResDepend roleAndResDepend;
        private object objLock = new object();

        public OnlineUserMgr(ITokenRepository tokenRepository, 
            IReadAuthRepository readAuthRepository, IRoleAndResDepend roleAndResDepend)
        {
            this.tokenRepository = tokenRepository;
            this.readAuthRepository = readAuthRepository;
            this.roleAndResDepend = roleAndResDepend;
            this.Load();
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this.ClearExpiredPro();
                    Thread.Sleep(1000 * 60 * 10);
                }
            });
        }

        public void Add(IUserEntity entity)
        {
            this.dicUsers.Add(entity.Token, entity);
            this.tokenRepository.InsertToken(entity.GetTokenEntity());
        }

        public IUserEntity Get(int userId)
        {
            var profiles = this.dicUsers.Values;
            return profiles.FirstOrDefault(m => m.UserInfo.Id == userId);
        }

        public IList<IUserEntity> GetAll()
        {
            return this.dicUsers.Values.ToList();
        }

        public List<ResourceInfo> GetAllRes()
        {
            return this.resList;
        }

        public List<RoleAndResInfo> GetAllRoleAndRes()
        {
            return this.roleAndResList;
        }

        public void Load()
        {
            #region 加载依赖数据

            var resTmpls = this.roleAndResDepend.GetAllResourceInfos();
            var roleAndResTmpls = this.roleAndResDepend.GetAllRoleAndRes();

            lock (this.objLock)
            {
                this.resList.Clear();
                this.roleAndResList.Clear();
                this.resList = resTmpls;
                this.roleAndResList = roleAndResTmpls;
            }

            #endregion

            var tokens = this.tokenRepository.GetAllTokens();
            if (tokens != null)
            {
                this.dicUsers.Clear();
                foreach(var token in tokens)
                {
                    //todo: create userEntity
                    UserEntity userEntity = new UserEntity(token, this.readAuthRepository, this);
                    this.dicUsers.Add(userEntity.Token, userEntity);
                }
            }
        }

        public bool Remove(string token)
        {
            if (this.dicUsers.ContainsKey(token))
            {
                var entity = this.dicUsers[token];
                lock (objLock)
                {
                    this.dicUsers.Remove(token);
                }
                this.tokenRepository.InsertToken(entity.GetTokenEntity(), false);
            }
            return true;
        }

        public bool Remove(int id)
        {
            var userInfo = this.Get(id);
            if (userInfo != null)
            {
                return this.Remove(userInfo.Token);
            }
            return true;
        }

        private void ClearExpiredPro()
        {
            //todo: 获取已过期的在线用户
            var ls = this.dicUsers.Values.ToList();
            lock (objLock)
            {
                foreach (var item in ls)
                {
                    if (!item.Verify())
                    {
                        //todo 自动移除过期的在线用户，要打不同的标记
                        this.Remove(item.Token);
                    }
                }
            }
        }
    }
}

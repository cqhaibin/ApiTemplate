using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BAccurate.Domain;

namespace BAccurate.Implement.Domain
{
    public class OnlineUserMgr : IOnlineUserMgr
    {
        protected Dictionary<string, IUserEntity> dicUsers = new Dictionary<string, IUserEntity>();
        protected ITokenRepository tokenRepository;
        IReadAuthRepository readAuthRepository;
        private object objLock = new object();

        public OnlineUserMgr(ITokenRepository tokenRepository, IReadAuthRepository readAuthRepository)
        {
            this.tokenRepository = tokenRepository;
            this.readAuthRepository = readAuthRepository;
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

        public void Load()
        {
            var tokens = this.tokenRepository.GetAllTokens();
            if (tokens != null)
            {
                this.dicUsers.Clear();
                foreach(var token in tokens)
                {
                    //todo: clientInfo
                    
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
                    this.dicUsers.Remove(item.Token);
                }
            }
        }
    }
}

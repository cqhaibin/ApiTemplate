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

        public UserEntity(int userId, IReadAuthRepository readAuthRepository, RequestClientInfo clientInfo)
        {
            this.readAuthRepository = readAuthRepository;
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
                ExpiredTime = ExpiredTime
            };
        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Domain;

namespace BAccurate.Repository.Freesql.Auth
{
    public class TokenRepository : ITokenRepository
    {
        List<TokenEntity> ls = new List<TokenEntity>();
        public IList<TokenEntity> GetAllTokens()
        {
            return ls;
        }

        public void InsertToken(TokenEntity entity, bool isLogin = true)
        {
            this.ls.Add(entity);
        }
    }
}

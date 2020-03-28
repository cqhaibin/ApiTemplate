﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Domain;
using SAM.Framework;
using FreeSql;
using BAccurate.Repository.Freesql.Auth.Entities;

namespace BAccurate.Repository.Freesql.Auth
{
    public class TokenRepository : ITokenRepository
    {
        private IFMapper fMapper;
        private IFreeSql freeSql;
        private BaseRepository<TokendbEntity> repository;
        public TokenRepository(IFreeSql freeSql, IFMapper fMapper)
        {
            this.fMapper = fMapper;
            this.freeSql = freeSql;
            this.repository = this.freeSql.GetRepository<TokendbEntity>();
        }

        public IList<TokenEntity> GetAllTokens()
        {
            var ls = this.repository.Select.ToList();
            return this.fMapper.Map<IList<TokenEntity>>(ls);
        }

        public void InsertToken(TokenEntity entity, bool isLogin = true)
        {
            var dbEntity = this.fMapper.Map<TokendbEntity>(entity);
            dbEntity.IsLogin = isLogin;
            this.repository.Insert(dbEntity);
        }
    }
}

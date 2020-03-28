using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Repository.Freesql.Entities;
using BAccurate.Domain;
using SAM.Framework;
using BAccurate.Repository.Freesql.Auth.Entities;

namespace BAccurate.Repository.Freesql
{
    /*
     freeSql.CodeFirst.ConfigEntity<TestEntity>(cfg =>
    {
        cfg.Name("T_Test");
        cfg.Property(m => m.Id).IsIdentity(true).IsPrimary(true);
        cfg.Property(m => m.Name).DbType("nvarchar(100)").IsNullable(true);
    });
     */
    public class BAccurateContext : DbContext
    {
        public BAccurateContext(IFreeSql freeSql, IFMapper fMapper) : base(freeSql, null)
        {
            this.TokenRepository = new Auth.TokenRepository(freeSql, fMapper);
        }

        public ITokenRepository TokenRepository { get; protected set; }


        /// <summary>
        /// 写db与实体的映射配置
        /// </summary>
        /// <param name="freeSql"></param>
        public static void Fluent(IFreeSql freeSql)
        {
            freeSql.CodeFirst.ConfigEntity<TokendbEntity>(cfg =>
            {
                cfg.Name("T_Tokens");
                cfg.Property(m => m.Id).IsIdentity(true).IsPrimary(true);
                cfg.Property(m => m.ClientInfo).DbType("nvarchar(max)");
                cfg.Property(m => m.UserInfo).DbType("nvarchar(max)");
            });
        }
    }
}

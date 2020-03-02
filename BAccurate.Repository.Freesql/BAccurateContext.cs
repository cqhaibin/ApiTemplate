using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Repository.Freesql.Entities;

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
        public BAccurateContext(IFreeSql freeSql) : base(freeSql, null)
        {
        }

        public DbSet<TestEntity> testDb { get; set; }

        /// <summary>
        /// 写db与实体的映射配置
        /// </summary>
        /// <param name="freeSql"></param>
        public static void Fluent(IFreeSql freeSql)
        {
            freeSql.CodeFirst.ConfigEntity<TestEntity>(cfg =>
            {
                cfg.Name("T_Test");
                cfg.Property(m => m.Id).IsIdentity(true).IsPrimary(true);
                cfg.Property(m => m.Name).DbType("nvarchar(100)").IsNullable(true);
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate
{
    /// <summary>
    /// 配置对象
    /// </summary>
    public class BAccurateConfig
    {
        /// <summary>
        /// 主数据库连接字符串
        /// </summary>
        public string AccurateConn { get; set; }

        public string DbType { get; set; } = ConstCfg.DbType_MSsql;
    }

    public class ConstCfg
    {
        public const string DbType_MSsql = "mssql";

        public const string DbType_Sqlite = "sqlite";

        public const string DbType_Mysql = "mysql";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models
{
    /// <summary>
    /// 请求客户端信息
    /// </summary>
    public class RequestClientInfo
    {
        public string IP { get; set; } 

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// 运行环境
        /// </summary>
        public string Agent { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public string SourceCont { get; set; }
    } 
}

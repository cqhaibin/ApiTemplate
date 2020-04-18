using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models.Auth
{
    public class MenuInfo
    {
        public string Id { get; set; }

        public string PId { get; set; }

        public string ResourceName { get; set; }

        public string ResourceCode { get; set; }

        public string Url { get; set; } 

        /// <summary>
        /// 获取参数
        /// IconUrl, Parameters, Descript etc
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public IList<MenuInfo> Childs { get; set; }
    }
}

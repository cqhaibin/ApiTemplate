using BAccurate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models.Resource
{
    public class ResourceInfo
    {
        public string Id { get; set; }

        public string PId { get; set; }

        public long? ApplicationId { get; set; }

        public bool Enable { get; set; } = true;

        public string ResourceName { get; set; }

        public string ResourceCode { get; set; }

        public string Url { get; set; }

        public ResourceType ResourceType { get; set; } = ResourceType.Menu;

        /// <summary>
        /// 获取参数
        /// IconUrl, Parameters, Descript etc
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNum { get; set; }
    }
}

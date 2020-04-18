using BAccurate.Enums;
using System;

namespace BAccurate.Repository.Freesql.Entities
{
    public class ResourceEntity:BaseEntity<string>
    {
        public string Id { get; set; }

        public string PId { get; set; }

        public long? ApplicationId { get; set; }

        public bool Enable { get; set; } = true;

        public string ResourceName { get; set; }

        public string ResourceCode { get; set; }

        public string Url { get; set; }

        public ResourceType ResourceType { get; set; } = ResourceType.Menu;

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 获取参数
        /// IconUrl, Parameters, Descript etc
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { get; set; }

        public ResourceEntity()
        {
            this.CreateTime = DateTime.Now;
        }
         
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models
{
    public abstract class BaseQuery
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        /// <summary>
        /// 排序字段：colName desc, colName2
        /// </summary>
        public string Sorts { get; set; }
    }
}

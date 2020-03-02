using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Repository.Freesql.Entities
{
    public class TestEntity:BaseEntity<int>
    {
        public string Name { get; set; }
    }
}

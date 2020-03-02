using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Repository.Freesql.Entities
{
    public abstract class BaseEntity
    {

    }

    public abstract class BaseEntity<KeyType>
    {
        public KeyType Id { get; set; }
    }
}

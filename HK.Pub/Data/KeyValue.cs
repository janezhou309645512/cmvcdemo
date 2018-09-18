using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HK.Pub.Data
{
    public class KeyValue<K,V>
    {
        public K Key { get; set; }
        public V Val { get; set; }

        public Type Tp { get; set; }
    }
}

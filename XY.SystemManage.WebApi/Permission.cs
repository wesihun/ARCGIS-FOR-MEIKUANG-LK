using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace XY.SystemManage.WebApi
{
    public class Permission 
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Predicate { get; set; }
    }
}

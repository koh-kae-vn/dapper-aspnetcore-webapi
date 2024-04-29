using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Para.Models
{
    public class paraCore
    {
        public string spName { get; set; }
        public int cmType { get; set; }

        public Dictionary<string,object> dicPara { get; set; }


        public string data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XY.Universal.Models
{
    public class AccessTokenViewModel
    {
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; }
    }
}

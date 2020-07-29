using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XY.Universal.Models
{
    public class AudienceViewModel
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string PolicyName { get; set; }
        public string DefaultScheme { get; set; }
        public bool IsHttps { get; set; }
        public string expiration { get; set; }
    }
}

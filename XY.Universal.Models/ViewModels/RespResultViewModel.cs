using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XY.Universal.Models
{
    public class RespResultViewModel
    {
        public int code { get; set; }

        public string msg { get; set; }

        public object data { get; set; }

    }

    public class RespResultCountViewModel : RespResultViewModel
    {
        public int count { get; set; }
        //public object complaindata { get; set; }
        //public string statusName { get; set; }

    }

    public class RespLoginViewModel : RespResultViewModel
    {
        public int count { get; set; }
        public string RoleName { get; set; }

    }

    public class RespLoginUserInfo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string user_id { get; set; }

        public string user_name { get; set; }

        public string avator { get; set; }

        public string access { get; set; }

    }
}

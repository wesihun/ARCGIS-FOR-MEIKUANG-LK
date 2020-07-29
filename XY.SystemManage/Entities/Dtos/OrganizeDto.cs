using System;
using System.Collections.Generic;
using System.Text;

namespace XY.SystemManage.Entities
{
    public class OrganizeDto:OrganizeEntity
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        public string AreaCode { get; set; }

    }
}

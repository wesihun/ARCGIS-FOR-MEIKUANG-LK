using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：Check_ComplaintDetail_MZLEntity
    /// 创 建 者：LK
    /// 创建日期：2019/11/19 15:35:25
    /// 最后修改者：LK
    /// 最后修改日期：2019/11/19 15:35:25
    /// </summary>
    [SugarTable("Check_ComplaintDetail_MZL")]
    public class Check_ComplaintDetail_MZLEntity
    {
        public string ComplaintDetailCode { get; set; }
        public string CheckComplainId { get; set; }
        public string ImageName { get; set; }
        public int? ImageSize { get; set; }
        public string ImageUrl { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Datatype { get; set; }
        public string FilesType { get; set; }
    }
}

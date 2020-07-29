using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_ComplaintDetail")]
    public class CheckComplaintDetailEntity
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

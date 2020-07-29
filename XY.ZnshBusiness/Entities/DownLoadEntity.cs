using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 下载管理
    /// </summary>
    [SugarTable("download")]
    public class DownLoadEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public int FileType { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public int DeleteMark { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public int SortCode { get; set; }

        public string FileUrl { get; set; }

    }
}

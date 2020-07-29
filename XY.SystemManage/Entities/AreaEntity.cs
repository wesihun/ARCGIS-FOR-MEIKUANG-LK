
using SqlSugar;
using System;

namespace XY.SystemManage.Entities
{
    /// 描述：区域管理
    [SugarTable("Base_Area")]
    public class AreaEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 区域主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string AreaId { get; set; }
        /// <summary>
        /// 父级主键
        /// </summary>		
        public string ParentId { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>		
        public string AreaCode { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>		
        public string AreaName { get; set; }
        /// <summary>
        /// 简拼
        /// </summary>		
        public string SimpleSpelling { get; set; }
        /// <summary>
        /// 层次
        /// </summary>		
        public int? Layer { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int? SortCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }
        #endregion
    }
}

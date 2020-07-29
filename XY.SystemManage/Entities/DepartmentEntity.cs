using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.SystemManage.Entities
{
    /// 描述：部门管理
    [SugarTable("base_department")]
    public class DepartmentEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>		
        public string OrgId { get; set; }
        /// <summary>
        /// 机构名
        /// </summary>		
        public string OrgName { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>		
        public string BH { get; set; }
        /// <summary>
        /// 部门名
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int SortCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteMark { get; set; }
        #endregion
    }
}

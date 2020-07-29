using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.SystemManage.Entities
{
    /// 描述：岗位管理
    [SugarTable("job")]
    public class JobEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 岗位名
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 岗位编号
        /// </summary>		
        public string JobCode { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>		
        public string SortCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Remake { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>		
        public string OrgId { get; set; }
        /// <summary>
        /// 机构名
        /// </summary>		
        public string OrgName { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>		
        public string DepId { get; set; }
        /// <summary>
        /// 部门名
        /// </summary>		
        public string DepName { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>		
        public int DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool LAY_CHECKED { get; set; }
    }
}

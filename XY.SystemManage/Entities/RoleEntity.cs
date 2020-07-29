
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.SystemManage.Entities
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/9/16 0:31:43 
    /// 版本：v1.0.0
    /// 描述：RoleEntity
    /// </summary> 
    [SugarTable("Base_Role")]
    public class RoleEntity : BaseEntity
    {

        #region 实体成员
        /// <summary>
        /// 角色主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>		
        public string RoleName { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>		
        public string RoleCode { get; set; }
        /// <summary>
        /// 分类1-角色2-岗位3-职位4-工作组
        /// </summary>		
        public string Category { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int? SortCode { get; set; }
        #endregion
    }
}

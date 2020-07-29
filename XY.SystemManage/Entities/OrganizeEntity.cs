
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XY.SystemManage.Entities
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/9/16 0:17:19 
    /// 版本：v1.0.0
    /// 描述：机构管理
    /// </summary>
    [SugarTable("Base_Organization")]
    public class OrganizeEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 机构主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string OrganizeId { get; set; }
        /// <summary>
        /// 父级主键
        /// </summary>		
        public string ParentId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 机构代码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 机构简码
        /// </summary>
        public string OrgBrevityCode { get; set; }
        /// <summary>
        /// 省级区域代码
        /// </summary>
        public string PAreaCode { get; set; }
        /// <summary>
        /// 省级区域名称
        /// </summary>
        public string PAreaName { get; set; }
        /// <summary>
        /// 市级区域代码
        /// </summary>
        public string SAreaCode { get; set; }
        /// <summary>
        /// 市级区域名称
        /// </summary>
        public string SAreaName { get; set; }
        /// <summary>
        /// 县级区域代码
        /// </summary>
        public string XAreaCode { get; set; }
        /// <summary>
        /// 县级区域名称
        /// </summary>
        public string XAreaName { get; set; }    
        /// <summary>
        /// 机构负责人
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// 机构负责人电话
        /// </summary>
        public string ManagerPhone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
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

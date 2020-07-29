using SqlSugar; 
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.SystemManage.Entities
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/9/16 0:32:34 
    /// 版本：v1.0.0
    /// 描述：数据字典
    /// </summary>
    [SugarTable("Base_DataDict")]
    public class DataDictEntity:BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string CRowId { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>
        public string OrgId { get; set; }
        /// <summary>
        /// 字典分类
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 字典编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 字典名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int? SortCode { get; set; }
    }
}

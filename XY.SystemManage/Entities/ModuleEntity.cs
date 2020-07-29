
using SqlSugar;
using System;

namespace XY.SystemManage.Entities
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/9/16 0:17:19 
    /// 版本：v1.0.0
    /// 描述：系统功能
    /// </summary>
    [SugarTable("Base_Module")]
    public class ModuleEntity :BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 功能主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string ModuleId { set; get; }
        /// <summary>
        /// 父级主键
        /// </summary>
        public string ParentId { set; get; }
        /// <summary>
        /// 编号
        /// </summary>
        public string ModuleCode { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ModuleName { set; get; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { set; get; }
        /// <summary>
        /// 导航地址
        /// </summary>
        public string Url { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }

        /// <summary>
        /// 排序码
        /// </summary>
        public int? SortCode { set; get; }


        #endregion
    }
}

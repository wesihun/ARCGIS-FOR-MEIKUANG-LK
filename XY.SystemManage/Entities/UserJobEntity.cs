using SqlSugar;
using System;
using System.Collections.Generic;
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
    [SugarTable("userjob")]
    public class UserJobEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 岗位Id
        /// </summary>
        public string JobId { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        public int DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}

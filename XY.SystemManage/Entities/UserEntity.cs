
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XY.SystemManage.Entities
{
    [SugarTable("Base_User")]
    public class UserEntity : BaseEntity
    {

        #region 实体成员
        /// <summary>
        /// 用户主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string UserId { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>		
        public string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>		
        public string Password { get; set; }
        /// <summary>
        /// 密码秘钥
        /// </summary>		
        public string SecretKey { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>		
        public string RealName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>		
        public string Gender { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>		
        public string MobilePhone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 所属机构ID
        /// </summary>
        public string OrganizeId { get; set; }
        /// <summary>
        /// 所属机构名称
        /// </summary>
        public string OrganizeName { get; set; }
        /// <summary>
        /// 所属部门Id
        /// </summary>
        public string DepId { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string DepName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>	
        public string Remark { get; set; }
        /// <summary>
        /// 是否系统管理员
        /// </summary>		
        public int IsAdmin { get; set; }
        /// <summary>
        /// 是否推送
        /// </summary>		
        public int IsNotice { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int? SortCode { get; set; }

        #endregion

    }
}

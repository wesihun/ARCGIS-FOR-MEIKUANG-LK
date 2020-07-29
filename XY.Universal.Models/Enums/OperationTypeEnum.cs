using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XY.Universal.Models
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationTypeEnum
    {
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 0,
        /// <summary>
        /// 登陆
        /// </summary>
        [Description("登录")]
        Login = 1,
        /// <summary>
        /// 登陆
        /// </summary>
        [Description("退出")]
        Exit = 2,
        /// <summary>
        /// 访问
        /// </summary>
        [Description("访问")]
        Visit = 3,
        /// <summary>
        /// 离开
        /// </summary>
        [Description("离开")]
        Leave = 4,
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Create = 5,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 6,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update = 7,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        Submit = 8,
        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Exception = 9,
        /// <summary>
        /// 异常
        /// </summary>
        [Description("移动登录")]
        AppLogin = 10,
    }

    /// <summary>
    /// 审核数据分类
    /// </summary>
    public enum CheckDataTypeEnum
    {
        /// <summary>
        /// 门诊
        /// </summary>
        [Description("门诊")]
        Clinic = 1,
        /// <summary>
        /// 住院
        /// </summary>
        [Description("住院")]
        Hos = 2
       
    }

    /// <summary>
    /// 就诊医院等级
    /// </summary>
    public enum InstitutiongGradeCodeEnum
    {
        /// <summary>
        /// 一级甲等
        /// </summary>
        [Description("一级甲等")]
        Aone = 8,
        /// <summary>
        /// 一级乙等
        /// </summary>
        [Description("一级乙等")]
        Bone = 9,
        /// <summary>
        /// 一级丙等
        /// </summary>
        [Description("一级丙等")]
        Cone = 10,
        /// <summary>
        /// 二级甲等
        /// </summary>
        [Description("二级甲等")]
        Atwo = 5,
        /// <summary>
        /// 二级乙等
        /// </summary>
        [Description("二级乙等")]
        Btwo = 6,
        /// <summary>
        /// 二级丙等
        /// </summary>
        [Description("二级丙等")]
        Ctwo = 7,
        /// <summary>
        /// 三级甲等
        /// </summary>
        [Description("三级甲等")]
        Athree = 2,
        /// <summary>
        /// 三级乙等
        /// </summary>
        [Description("三级乙等")]
        Bthree = 3,
        /// <summary>
        /// 三级丙等
        /// </summary>
        [Description("三级丙等")]
        Cthree = 4,


    }


    /// <summary>
    /// 审核结果是否涉及处方
    /// </summary>
    public enum CheckIsPreEnum
    {
        /// <summary>
        /// 不涉及
        /// </summary>
        [Description("不涉及")]
        No = 0,
        /// <summary>
        /// 涉及
        /// </summary>
        [Description("涉及")]
        Yes = 1

    }
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum CheckStates
    {
        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        A = 1,
        /// <summary>
        /// 审核有问题
        /// </summary>
        [Description("审核有问题")]
        B = 2,
        /// <summary>
        /// 审核无问题
        /// </summary>
        [Description("审核无问题")]
        C = 3,
        /// <summary>
        /// 审核有问题复核无问题
        /// </summary>
        [Description("审核有问题复核无问题")]
        D = 4,
        /// <summary>
        /// 审核有问题复核有问题
        /// </summary>
        [Description("审核有问题复核有问题")]
        E = 5,
        /// <summary>
        /// 复核有问题并已处理
        /// </summary>
    //    [Description("复核有问题并已处理")]
      //  F = 6,
        /// <summary>
        /// 申诉中 还未复审
        /// </summary>
        [Description("申诉中 还未复审")]
        G = 7,
        /// <summary>
        /// 人工初审 未通过
        /// </summary>
        [Description("人工初审未通过")]
        H = 8,
        /// <summary>
        /// 人工初审 已通过
        /// </summary>
        [Description("人工初审已通过")]
        J = 9,
        /// <summary>
        /// 疑点结论提交标识
        /// </summary>
        [Description("疑点结论提交标识")]
        K = 10
    }

    /// <summary>
    /// 审核规则
    /// </summary>
    public enum CheckRules
    {
        
        /// <summary>
        /// 住院天数异常
        /// </summary>
        [Description("住院天数异常")]
        A003,
        /// <summary>
        /// 分解住院
        /// </summary>
        [Description("分解住院")]
        A004,
        /// <summary>
        /// 床位费与住院天数不符
        /// </summary>
        [Description("床位费与住院天数不符")]
        A005,
        /// <summary>
        /// 入出院日期异常
        /// </summary>
        [Description("入出院日期异常")]
        A006,
        /// <summary>
        /// 就诊疾病异常审核
        /// </summary>
        [Description("就诊疾病异常审核")]
        A007,
        /// <summary>
        /// 限定诊疗价格审核
        /// </summary>
        [Description("限定诊疗价格审核")]
        B001,
        /// <summary>
        /// 医保目录限制用药范围
        /// </summary>
        [Description("医保目录限制用药范围")]
        B002,
        /// <summary>
        /// 医保单病种政策超限价
        /// </summary>
        [Description("医保单病种政策超限价")]
        B003,
        /// <summary>
        /// 限儿童用药审核
        /// </summary>
        [Description("限儿童用药审核")]
        C001,
        /// <summary>
        /// 限定性别用药
        /// </summary>
        [Description("限定性别用药")]
        C002,
        /// <summary>
        /// 限老年人用药
        /// </summary>
        [Description("限老年人用药")]
        C003,
        /// <summary>
        /// 限定性别诊疗服务
        /// </summary>
        [Description("限定性别诊疗服务")]
        D001,

    }
    /// <summary>
    /// 违规类型
    /// </summary>
    public enum RulesType
    {
        [Description("疑似违规")]
        R001 = 1,
        [Description("100%违规")]
        R002 = 2
       
    }
    public enum ComplaintTreeType
    {
        /// <summary>
        /// 初审树
        /// </summary>
        [Description("初审树")]
        Type1 = 1,
        /// <summary>
        /// 反馈树
        /// </summary>
        [Description("反馈树")]
        Type2 = 2,
        /// <summary>
        /// 复审树
        /// </summary>
        [Description("复审树")]
        Type3 = 3,
        /// <summary>
        /// 复审树
        /// </summary>
        [Description("疑点结论树")]
        Type4 = 4

    }

}

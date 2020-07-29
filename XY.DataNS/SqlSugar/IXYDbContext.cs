
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.DataNS
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/9/9 14:05:44 
    /// 版本：v1.0.0
    /// 描述：IZmsoDbContext
    /// </summary>
    public interface IXYDbContext
    {
        SqlSugarClient GetIntance(int commandTimeOut = 6000,  bool isAutoCloseConnection = false);
        SqlSugarClient GetIntanceForYb(int commandTimeOut = 6000, bool isAutoCloseConnection = false);
    }
}

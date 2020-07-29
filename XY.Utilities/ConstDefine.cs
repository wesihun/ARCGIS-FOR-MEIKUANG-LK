/*****************************************************************************
** Copyright (c) 2018 许洪义. All rights reserved. 
** 作者：lk
** 时间：2018/8/22 16:42:09 
** 版本：V1.0.0 
** 描述：
******************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XY.Utilities
{
    /// <summary>
    /// 常量
    /// </summary>
    public static class ConstDefine
    {
         /// <summary>
        /// 英文字母
        /// </summary>
        public const string Letters = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 数字
        /// </summary>
        public const string Numbers = "0123456789";

        /// <summary>
        /// 生成Guid
        /// </summary>
        public static string CreateGuid()
        {
             return Guid.NewGuid().ToString();
        }



    }
}

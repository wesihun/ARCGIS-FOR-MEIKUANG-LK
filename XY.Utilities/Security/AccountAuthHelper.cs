
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Utilities
{
    /// <summary>
    /// Copyright (c) 2018 lk
    /// 作者：lk
    /// 时间：2018/9/23 7:57:21 
    /// 版本：v1.0.0
    /// 描述：AuthPassword
    /// </summary>
    public class AccountAuthHelper
    {

        public static string CreateSecretKey()
        {
            string secretKey = MD5Helper.Get16MD5One(CommonHelper.CreateNo()).ToLower();
            return secretKey;
        }
        /// <summary>
        /// 生成密码
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <param name="secretkey">加密密钥</param>
        /// <returns></returns>
        public static string CreatePassword(string password,string secretKey)
        {
            string md5Password = MD5Helper.Get32MD5One(DESEncrypt.Encrypt(MD5Helper.Get32MD5One(password).ToLower(), secretKey).ToLower()).ToLower();
            return md5Password;
        }

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <param name="secretkey">加密密钥</param>
        /// <param name="userPassword">用户验证密码</param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, string secretKey, string userPassword)
        {
            string dbPassword = MD5Helper.Get32MD5One(DESEncrypt.Encrypt(MD5Helper.Get32MD5One(password).ToLower(), secretKey).ToLower()).ToLower();
            if (dbPassword == userPassword)
                return true;
            else
                return false;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;
using XY.DataNS;
using XY.Utilities;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;
using XY.Universal.Models;
using OracleInternal.Secure.Network;
using Renci.SshNet.Security;

namespace XY.SystemManage.Service
{
    public class UserService : IUserService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public UserService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region 获取数据
        /// <summary>
        /// 获取用户列表并分页
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <param name="limit">也尺寸</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public List<UserDto> GePagetListByUser(string orgid,string depid, string realname, string isdadmin, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<UserDto>();
            using (var db = _dbContext.GetIntance())
            {
                    DataResult = db.Queryable<UserEntity>().Where(it => it.DeleteMark == 1 && it.UserName != DataDictConst.USER_SUPERADMIN)
                    .WhereIF(!string.IsNullOrEmpty(orgid), it => it.OrganizeId == orgid)
                    .WhereIF(!string.IsNullOrEmpty(depid), it => it.DepId == depid)
                    .WhereIF(!string.IsNullOrEmpty(realname), it => it.RealName.Contains(realname))
                    .WhereIF(!string.IsNullOrEmpty(isdadmin), it => it.IsAdmin == Convert.ToInt32(isdadmin))
                    .OrderBy((it) => it.SortCode)
                    .Select(it => new UserDto
                    {
                        UserId = it.UserId,
                        UserName = it.UserName,
                        RealName = it.RealName,
                        GenderName = it.Gender,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        MobilePhone = it.MobilePhone,
                        Address = it.Address,
                        OrganizeId = it.OrganizeId,
                        OrganizeName = it.OrganizeName,
                        DepId = it.DepId,
                        DepName = it.DepName,
                        Remark = it.Remark,
                        IsAdmin = it.IsAdmin,
                        IsNotice = it.IsNotice,
                        SortCode = it.SortCode
                    }).ToPageList(page, limit, ref totalCount);               
                return DataResult;
            }
        }

        public List<UserDto> GetUserSelect(string depid, string roleid)
        {
            var DataResult = new List<UserDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<UserEntity, UserRoleEntity>((ue, ur) => new object[] {
                    JoinType.Left,ue.UserId == ur.UserId,
                }).Where((ue, ur) => ue.DeleteMark == 1).OrderBy((ue) => ue.SortCode)
                .WhereIF(!string.IsNullOrEmpty(depid), (ue, ur) => ue.DepId == depid)
                .WhereIF(!string.IsNullOrEmpty(roleid), (ue, ur) => ur.RoleId == roleid)
                .Select((ue, ur) => new UserDto
                {
                    UserId = ue.UserId,
                    UserName = ue.UserName,
                    RealName = ue.RealName
                }).ToList();
            }
            return DataResult;
        }

        /// <summary>
        /// 根据用户ID获取用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserDto GetEntityById(string keyValue)
        {
            var DataResult = new UserDto();
            using (var db = _dbContext.GetIntance())
            {
                 DataResult = db.Queryable<UserEntity>().Where((ue) => ue.DeleteMark == 1 && ue.UserId == keyValue).OrderBy((ue) => ue.SortCode)
               .Select((ue) => new UserDto
               {
                   UserId = ue.UserId,
                   UserName = ue.UserName,
                   RealName = ue.RealName,
                   GenderName = ue.Gender,
                   IdNumber = ue.IdNumber,
                   MobilePhone = ue.MobilePhone,
                   Address = ue.Address,
                   OrganizeId = ue.OrganizeId,
                   Remark = ue.Remark,
                   IsAdmin = ue.IsAdmin,
                   SortCode = ue.SortCode
               }).ToList().SingleOrDefault();
            }
            return DataResult;
        }
        /// <summary>
        /// 根据用户名判断是否已存在
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public bool IsExistByUserName(string userName, string keyValue)
        {
            bool result;
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    result = db.Queryable<UserEntity>().Any(it => it.UserName == userName && it.DeleteMark == 1);
                }
                else
                {
                    result = db.Queryable<UserEntity>().Any(it => it.UserName == userName && it.UserId != keyValue && it.DeleteMark == 1);
                }

            }
            return result;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public UserEntity IsExistByUserName(string userName)
        {
            var dataResult = new UserEntity();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<UserEntity>().Where(it => it.UserName == userName && it.DeleteMark == 1).Single();
            }
            return dataResult;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        public bool Insert(UserEntity userEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                userEntity.SecretKey = AccountAuthHelper.CreateSecretKey();
                userEntity.Password = AccountAuthHelper.CreatePassword(userEntity.Password, userEntity.SecretKey);
                var count = db.Insertable(userEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改(忽略密码、秘钥)
        /// </summary>
        /// <param name="roleEntity">用户实体</param>
        /// <returns></returns>
        public bool Update(UserEntity userEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(userEntity)
                .IgnoreColumns(it => new {
                    it.Password,
                    it.SecretKey,
                    it.CreateDate,
                    it.CreateUserId,
                    it.CreateUserName,
                    it.DeleteMark
                })
                .Where(it => it.UserId == userEntity.UserId)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码(页面加密过的)</param>
        public bool RevisePassword(string keyValue, string Password)
        {
            using (var db = _dbContext.GetIntance())
            {
                UserEntity userEntity = new UserEntity();
                userEntity.UserId = keyValue;
                userEntity.SecretKey = AccountAuthHelper.CreateSecretKey();
                userEntity.Password = AccountAuthHelper.CreatePassword(Password, userEntity.SecretKey);

                var count = db.Updateable(userEntity)
                    .UpdateColumns(it => new UserEntity { Password = userEntity.Password, SecretKey = userEntity.SecretKey })
                    .Where(it => it.UserId == keyValue).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool Delete(string keyValue, string UserId, string UserName)
        {
            using (var db = _dbContext.GetIntance())
            {
                var userEntity = new UserEntity();
                userEntity.DeleteMark = 0;
                userEntity.ModifyUserId = UserId;
                userEntity.ModifyUserName = UserName;
                userEntity.ModifyDate = DateTime.Now;
                //逻辑删除
                var count = db.Updateable(userEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                .Where(it => it.UserId == keyValue)
                .ExecuteCommand();
                return count == 1 ? true : false;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public bool DeleteBatch(List<string> keyValues, string UserId, string UserName)
        {
            bool result = false;
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    var userEntity = new UserEntity();
                    userEntity.DeleteMark = 0;
                    userEntity.ModifyUserId = UserId;
                    userEntity.ModifyUserName = UserName;
                    userEntity.ModifyDate = DateTime.Now;

                    //逻辑删除
                    var counts = db.Updateable(userEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                        .Where(it => keyValues.Contains(it.UserId)).ExecuteCommand();
                    result = counts > 0 ? result = true : false;

                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion
    }
}

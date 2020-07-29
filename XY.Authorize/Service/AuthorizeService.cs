using System;
using System.Collections.Generic;
using System.Text;
using XY.Authorize.IService;
using XY.DataNS;
using XY.SystemManage.Entities;

namespace XY.Authorize.Service
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IXYDbContext _dbContext;
        public AuthorizeService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserEntity CheckLogin(string userName)
        {
            var dataResult = new UserEntity();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<UserEntity>().Where(it => it.UserName == userName && it.DeleteMark == 1).Single();
            }
            return dataResult;
        }

        public string GetRoleName(string userid)
        {
            var dataResult = new UserEntity();
            using (var db = _dbContext.GetIntance())
            {
                string roleid = "";
                string rolename = "";
                if (db.Queryable<UserRoleEntity>().Any(it => it.UserId == userid))
                {
                    roleid = db.Queryable<UserRoleEntity>().Where(it => it.UserId == userid).First().RoleId;
                    if(db.Queryable<RoleEntity>().Any(it => it.DeleteMark == 1 && it.RoleId == roleid))
                    {
                        rolename = db.Queryable<RoleEntity>().Where(it => it.DeleteMark == 1 && it.RoleId == roleid).First().RoleName;
                    }
                }               
                return rolename;
            }
         
        }
    }
}

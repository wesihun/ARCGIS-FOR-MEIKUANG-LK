using System;
using System.Collections.Generic;
using System.Text;
using CSRedis;

namespace XY.DataCache.Redis
{
    public class RedisDbContext : IRedisDbContext
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        public static string RedisDbConnectionString { get; set; }
        public CSRedisClient GetRedisIntance()
        {
            return InitDB(RedisDbConnectionString);
        }

        private CSRedisClient InitDB(string redisDbConnectionString)
        {
            var redisdb = new CSRedisClient(redisDbConnectionString);
            return redisdb;
        }
    }
}

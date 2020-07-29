using System;
using System.Collections.Generic;
using System.Text;
using CSRedis;
namespace XY.DataCache.Redis
{
    public interface IRedisDbContext
    {
        CSRedisClient GetRedisIntance();
    }
}

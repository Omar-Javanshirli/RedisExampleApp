using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedisExampleApp.Cache
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;

        public RedisService( string url)
        {
            this.connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }
        
        public IDatabase GetDb(int dbIndex)
        {
            return this.connectionMultiplexer.GetDatabase(dbIndex);
        }
    }
}

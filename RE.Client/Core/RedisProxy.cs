using RE.Common;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RE.Client.Core
{
    public class RedisProxy<T> where T : BaseEntity
    {
        private readonly IRedisTypedClient<T> _client;

        public RedisProxy()
        {
            var redisManager = new RedisManagerPool($"localhost:{AppConfig.Port}");
            var client = redisManager.GetClient();

            _client = client.As<T>();
        }

        public void StoreAll(IEnumerable<T> items)
        {
            if (!items.Any())
                return;

            _client.StoreAll(items);
        }

        public IEnumerable<T> GetAll()
        {
            return _client.GetAll();
        }

        public T Get(int id)
        {
            return _client.GetById(id);
        }

        public void Delete(int id)
        {
            _client.DeleteById(id);
        }

        public void Store(T t)
        {
            _client.Store(t);
        }

    }
}

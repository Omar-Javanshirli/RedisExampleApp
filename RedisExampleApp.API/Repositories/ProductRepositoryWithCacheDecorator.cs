using RedisExampleApp.API.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository repository;
        private readonly RedisService redisService;
        private readonly IDatabase cacheRespository;

        public ProductRepositoryWithCacheDecorator(IProductRepository repository, RedisService redisService)
        {
            this.repository = repository;
            this.redisService = redisService;

            this.cacheRespository = this.redisService.GetDb(2);
        }

        public Task<Product> CreateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await this.cacheRespository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            List<Product> products = new();
            var cacheProducts = await this.cacheRespository.HashGetAllAsync(productKey);

            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value!);
                products.Add(product!);
            }
            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (await this.cacheRespository.KeyExistsAsync(productKey))
            {
                var product = (await this.cacheRespository.HashGetAsync(productKey, id))!;
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }
            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id)!;
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await this.repository.GetAsync();
            products.ForEach(p =>
            {
                this.cacheRespository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });
            return products;
        }
    }
}

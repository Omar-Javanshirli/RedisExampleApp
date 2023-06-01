using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repositories;

namespace RedisExampleApp.API.Servicies
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            return await this.productRepository.CreateAsync(product);
        }

        public async Task<List<Product>> GetAsync()
        {
            return await this.productRepository.GetAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await this.productRepository.GetByIdAsync(id);
        }
    }
}

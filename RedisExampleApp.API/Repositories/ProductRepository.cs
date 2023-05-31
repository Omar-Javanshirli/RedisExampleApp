using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Models;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await this.context.AddAsync(product);
            await this.context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await this.context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return (await this.context.Products.FindAsync(id))!;
        }
    }
}

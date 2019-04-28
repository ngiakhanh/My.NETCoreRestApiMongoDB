using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Services.Communication;

namespace WebApplication1.Domain.Services
{
    public interface IProductService
    {
        Task<ProductResponse> ListAsync();
        Task<ProductResponse> FindByIdAsync(int id);
        Task<ProductResponse> SaveAsync(Product product);
        Task<ProductResponse> UpdateAsync(int id, Product product);
        Task<ProductResponse> DeleteAsync(int id);
    }
}

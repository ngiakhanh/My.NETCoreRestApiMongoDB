using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Services.Communication;

namespace WebApplication1.Domain.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> ListAsync();
        Task<CategoryResponse> FindByIdAsync(int id);
        Task<CategoryResponse> SaveAsync(Category category);
        Task<CategoryResponse> UpdateAsync(int id, Category category);
        Task<CategoryResponse> DeleteAsync(int id);
    }
}

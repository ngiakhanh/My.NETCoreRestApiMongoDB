using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;
using WebApplication1.Domain.Services.Communication;

namespace WebApplication1.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> ListAsync()
        {
            try
            {
                var results = await _productRepository.ListAsync();
                if (results == null)
                {
                    return new ProductResponse("Product not found");
                }
                return new ProductResponse(results);
            }
            catch (Exception ex)
            {
                return new ProductResponse($"An error occurred when getting the list of products: {ex.Message}");
            }
        }

        public async Task<ProductResponse> FindByIdAsync(int id)
        {
            try
            {
                var result = await _productRepository.FindByIdAsync(id);
                if (result == null)
                {
                    return new ProductResponse("Product not found");
                }
                IEnumerable<Product> resultList = new List<Product>() { result };
                return new ProductResponse(resultList);
            }
            catch (Exception ex)
            {
                return new ProductResponse($"An error occurred when finding the product: {ex.Message}");
            }
        }

        public async Task<ProductResponse> SaveAsync(Product product)
        {
            try
            {
                var existingProduct = await _productRepository.FindByIdAsync(product.Id);
                if (existingProduct != null)
                {
                    return new ProductResponse("Product already existed");
                }

                await _productRepository.AddAsync(product);
                await _unitOfWork.CompleteAsync(_productRepository.ReturnSession());
                return await FindByIdAsync(product.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_productRepository.ReturnSession());
                return new ProductResponse($"An error occurred when saving the product: {ex.Message}");
            }

        }

        public async Task<ProductResponse> UpdateAsync(int id, Product product)
        {
            try
            {
                var existingProduct = await _productRepository.FindByIdAsync(id);
                if (existingProduct == null)
                {
                    return new ProductResponse("Product not found");
                }
                product._id = existingProduct._id;

                await _productRepository.UpdateAsync(id, product);
                await _unitOfWork.CompleteAsync(_productRepository.ReturnSession());
                return await FindByIdAsync(product.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_productRepository.ReturnSession());
                return new ProductResponse($"An error occurred when updating the product: {ex.Message}");
            }
        }

        public async Task<ProductResponse> DeleteAsync(int id)
        {
            try
            {
                var existingProduct = await _productRepository.FindByIdAsync(id);
                if (existingProduct == null)
                {
                    return new ProductResponse("Product not found");
                }

                await _productRepository.RemoveAsync(id);
                await _unitOfWork.CompleteAsync(_productRepository.ReturnSession());
                IEnumerable<Product> existingProductList = new List<Product>() { existingProduct };
                return new ProductResponse(existingProductList);
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_productRepository.ReturnSession());
                return new ProductResponse($"An error occurred when deleting the product: {ex.Message}");
            }
        }
    }
}

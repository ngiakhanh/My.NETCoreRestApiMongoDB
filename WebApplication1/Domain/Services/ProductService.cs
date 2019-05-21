using MongoDB.Driver;
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
        private readonly IClientSessionHandle _session;
        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _session = _productRepository.ReturnSession();
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
            finally
            {
                _unitOfWork.EndSession(_session);
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
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }

        public async Task<ProductResponse> SaveAsync(Product product)
        {
            _unitOfWork.StartTransaction(_session);
            try
            {
                var existingProduct = await _productRepository.FindByIdAsync(product.Id);
                if (existingProduct != null)
                {
                    return new ProductResponse("Product already existed");
                }

                await _productRepository.AddAsync(product);
                var foundProduct = await FindByIdAsync(product.Id);
                await _unitOfWork.CompleteAsync(_session);
                return foundProduct;
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_session);
                return new ProductResponse($"An error occurred when saving the product: {ex.Message}");
            }
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }

        public async Task<ProductResponse> UpdateAsync(int id, Product product)
        {
            _unitOfWork.StartTransaction(_session);
            try
            {
                var existingProduct = await _productRepository.FindByIdAsync(id);
                if (existingProduct == null)
                {
                    return new ProductResponse("Product not found");
                }
                product._id = existingProduct._id;

                await _productRepository.UpdateAsync(id, product);
                var updatedProduct = await FindByIdAsync(product.Id);
                await _unitOfWork.CompleteAsync(_session);
                return updatedProduct;
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_session);
                return new ProductResponse($"An error occurred when updating the product: {ex.Message}");
            }
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }

        public async Task<ProductResponse> DeleteAsync(int id)
        {
            _unitOfWork.StartTransaction(_session);
            try
            {
                var existingProduct = await _productRepository.FindByIdAsync(id);
                if (existingProduct == null)
                {
                    return new ProductResponse("Product not found");
                }

                await _productRepository.RemoveAsync(id);
                IEnumerable<Product> existingProductList = new List<Product>() { existingProduct };
                await _unitOfWork.CompleteAsync(_session);
                return new ProductResponse(existingProductList);
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_session);
                return new ProductResponse($"An error occurred when deleting the product: {ex.Message}");
            }
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }
    }
}
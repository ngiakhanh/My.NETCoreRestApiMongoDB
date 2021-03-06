﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;
using WebApplication1.Domain.Services.Communication;

namespace WebApplication1.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientSessionHandle _session;
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _session = _categoryRepository.ReturnSession();
        }

        public async Task<CategoryResponse> ListAsync()
        {
            try
            {
                var results = await _categoryRepository.ListAsync();
                if (results == null)
                {
                    return new CategoryResponse("Category not found");
                }
                return new CategoryResponse(results);
            }
            catch (Exception ex)
            {
                return new CategoryResponse($"An error occurred when getting the list of categories: {ex.Message}");
            }
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }

        public async Task<CategoryResponse> FindByIdAsync(int id)
        {
            try
            {
                var result = await _categoryRepository.FindByIdAsync(id);
                if (result == null)
                {
                    return new CategoryResponse("Category not found");
                }
                IEnumerable<Category> resultList = new List<Category>() { result };
                return new CategoryResponse(resultList);
            }
            catch (Exception ex)
            {
                return new CategoryResponse($"An error occurred when finding the category: {ex.Message}");
            }
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }

        public async Task<CategoryResponse> SaveAsync(Category category)
        {
            _unitOfWork.StartTransaction(_session);
            try
            {
                var existingCategory = await _categoryRepository.FindByIdAsync(category.Id);
                if (existingCategory != null)
                {
                    return new CategoryResponse("Category already existed");
                }

                await _categoryRepository.AddAsync(category);
                var foundCategory = await FindByIdAsync(category.Id);
                await _unitOfWork.CompleteAsync(_session);
                return foundCategory;
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_session);
                return new CategoryResponse($"An error occurred when saving the category: {ex.Message}");
            }
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }

        public async Task<CategoryResponse> UpdateAsync(int id, Category category)
        {
            _unitOfWork.StartTransaction(_session);
            try
            {
                var existingCategory = await _categoryRepository.FindByIdAsync(id);
                if (existingCategory == null)
                {
                    return new CategoryResponse("Category not found");
                }
                category._id = existingCategory._id;

                await _categoryRepository.UpdateAsync(id, category);
                var updatedCategory = await FindByIdAsync(category.Id);
                await _unitOfWork.CompleteAsync(_session);
                return updatedCategory;
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_session);
                return new CategoryResponse($"An error occurred when updating the category: {ex.Message}");
            }
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }

        public async Task<CategoryResponse> DeleteAsync(int id)
        {
            _unitOfWork.StartTransaction(_session);
            try
            {
                var existingCategory = await _categoryRepository.FindByIdAsync(id);
                if (existingCategory == null)
                {
                    return new CategoryResponse("Category not found");
                }

                await _categoryRepository.RemoveAsync(id);
                IEnumerable<Category> existingCategoryList = new List<Category>() { existingCategory };
                await _unitOfWork.CompleteAsync(_session);
                return new CategoryResponse(existingCategoryList);
            }
            catch (Exception ex)
            {
                await _unitOfWork.AbortAsync(_session);
                return new CategoryResponse($"An error occurred when deleting the category: {ex.Message}");
            }
            finally
            {
                _unitOfWork.EndSession(_session);
            }
        }
    }
}
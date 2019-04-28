using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Services.Communication
{
    public class CategoryResponse : BaseResponse
    {
        public IEnumerable<Category> Category { get; private set; }

        private CategoryResponse(bool success, string message, IEnumerable<Category> category) : base(success, message)
        {
            Category = category;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="category">Saved category.</param>
        /// <returns>Response.</returns>
        public CategoryResponse(IEnumerable<Category> category) : this(true, string.Empty, category)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public CategoryResponse(string message) : this(false, message, null)
        { }
    }
}

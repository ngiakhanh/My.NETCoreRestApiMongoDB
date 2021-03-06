﻿using System.Collections.Generic;
using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Services.Communication
{
    public class ProductResponse : BaseResponse
    {
        public IEnumerable<Product> Product { get; private set; }

        private ProductResponse(bool success, string message, IEnumerable<Product> product) : base(success, message)
        {
            Product = product;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="product">Saved product.</param>
        /// <returns>Response.</returns>
        public ProductResponse(IEnumerable<Product> product) : this(true, string.Empty, product)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ProductResponse(string message) : this(false, message, null)
        { }
    }
}
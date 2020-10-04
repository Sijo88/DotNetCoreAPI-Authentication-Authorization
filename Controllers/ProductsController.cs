using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTPolicies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<ProductModel> products = new List<ProductModel>()
            {
                new ProductModel
                {
                    Id=1,
                    Title="Product1",
                    ModelName="Model",
                    ProductType="ProductType1"
                },
                  new ProductModel
                {
                    Id=2,
                    Title="Product2",
                    ModelName="Model2",
                    ProductType="ProductType2"
                }
            };
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(products);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(products.SingleOrDefault(s => s.Id == Convert.ToInt32(id)));
        }

        [HttpPut]
        [Authorize(Policy = "Admin")]
        public IActionResult Put(ProductModel updatedProduct)
        {
            var product = products.SingleOrDefault(p => p.Id == updatedProduct.Id);
            products[products.IndexOf(product)] = updatedProduct;
            return Ok(updatedProduct);
        }
    }
}
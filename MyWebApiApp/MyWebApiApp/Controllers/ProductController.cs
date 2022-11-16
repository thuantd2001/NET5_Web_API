using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public static List<Product> products = new List<Product>();
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {

            try
            {
                var product = products.SingleOrDefault(p => p.Id == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Create(ProductVM categoryProduct)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = categoryProduct.Name,
                Price = categoryProduct.Price,

            };
            products.Add(product);
            return Ok(new
            {
                Success = true,
                Data = product,
            });
        }
        [HttpPut("{id}")]
        public IActionResult Edit(string id, Product newProduct)
        {
            try
            {
                var product = products.SingleOrDefault(p => p.Id == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                if (id != product.Id.ToString())
                {
                    return BadRequest();
                }
                product.Name = newProduct.Name;
                product.Price = newProduct.Price;
                return Ok(product);
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpDelete("{id}")]
        public IActionResult Remove(string id)
        {
            try
            {
                var product = products.SingleOrDefault(p => p.Id == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                products.Remove(product);
                return Ok(product);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Swagger_Use.Model;

namespace Swagger_Use.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> Products = new List<Product>
    {
        new Product { Id = 1, Name = "Product1", Price = 10.5M },
        new Product { Id = 2, Name = "Product2", Price = 20.0M }
    };

        /// <summary>
        /// Получает список всех продуктов.
        /// </summary>
        /// <returns>Список продуктов</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return Ok(Products);
        }

        /// <summary>
        /// Получает продукт по ID.
        /// </summary>
        /// <param name="id">ID продукта</param>
        /// <returns>Продукт с заданным ID</returns>
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Создает новый продукт.
        /// </summary>
        /// <param name="product">Продукт для создания</param>
        /// <returns>Созданный продукт</returns>
        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            product.Id = Products.Max(p => p.Id) + 1;
            Products.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        /// <summary>
        /// Обновляет существующий продукт.
        /// </summary>
        /// <param name="id">ID продукта для обновления</param>
        /// <param name="product">Обновленный продукт</param>
        /// <returns>Результат обновления</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            var existingProduct = Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            return NoContent();
        }

        /// <summary>
        /// Удаляет продукт по ID.
        /// </summary>
        /// <param name="id">ID продукта для удаления</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Products.Remove(product);
            return NoContent();
        }

        /// <summary>
        /// Ищет продукты по имени.
        /// </summary>
        /// <param name="name">Имя продукта для поиска</param>
        /// <returns>Список продуктов, соответствующих критерию поиска</returns>
        [HttpGet("search")]
        public ActionResult<IEnumerable<Product>> Search(string name)
        {
            var matchedProducts = Products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!matchedProducts.Any())
            {
                return NotFound();
            }
            return Ok(matchedProducts);
        }

    }
}
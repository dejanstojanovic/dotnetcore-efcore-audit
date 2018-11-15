using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Auditing.Data;
using Sample.Auditing.Data.Entities;

namespace Sample.Auditing.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CatalogDbContext dbContext;
        public ProductsController(CatalogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return dbContext.Products;
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(Guid id)
        {
            return dbContext.Products.FirstOrDefault(p => p.Id == id);
        }

        [HttpPost]
        public async Task Post([FromBody] Product value)
        {
            await dbContext.AddAsync(value);
            await dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody] Product value)
        {
            dbContext.Update(value);
            await dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            dbContext.Remove(
                dbContext.Products.FirstOrDefault(p => p.Id == id)
            );
            await dbContext.SaveChangesAsync();
        }
    }
}

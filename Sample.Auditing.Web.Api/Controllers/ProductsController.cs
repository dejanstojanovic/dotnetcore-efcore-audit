using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Auditing.Data.Entities;
using Sample.Auditing.Data.UnitsOfWork;
using Sample.Auditing.Web.Api.Models;

namespace Sample.Auditing.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork unitOfWork,IMapper mapper) : base(unitOfWork,mapper)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> Get()
        {
            return await Task.FromResult(this.mapper.Map<IEnumerable<ProductViewModel>>(unitOfWork.Products.Get()));
        }

        [HttpGet("{id}", Name = "Product")]
        public async Task<ProductEditModel> Get(Guid id)
        {
            return this.mapper.Map<ProductEditModel>(await unitOfWork.Products.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductAddModel product)
        {
            var entity = this.mapper.Map<Product>(product);
            await this.unitOfWork.Products.InsertAsync(entity);
            await this.unitOfWork.SaveAsync();

            return CreatedAtRoute(routeName: "Product", routeValues: new { id = entity.Id.ToString() }, value: entity.Id);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] ProductEditModel product)
        {
            await this.unitOfWork.Products.InsertAsync(this.mapper.Map<Product>(product));
            try
            {
                await this.unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            this.unitOfWork.Products.Remove(id);
            await this.unitOfWork.SaveAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private DataContext context;

        public SuppliersController(DataContext ctx)
        {
            context = ctx;
        }

        [HttpGet("{id}")]
        public async Task<Supplier?> GetSupplier(long id)
        {
            // return await context.Suppliers
            //     .Include(s => s.Products)
            //     .FirstAsync(s => s.SupplierId == id);
            Supplier supplier = await context.Suppliers.Include(s => s.Products)
                .FirstAsync(s => s.SupplierId == id);
            if (supplier.Products != null)
            {
                foreach (Product p in supplier.Products)
                {
                    p.Supplier = null;
                }
            }
            return supplier;

        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Supplier>> PatchSupplier(long id,
         JsonPatchDocument<Supplier> patch)
        {
            Supplier? supplier = await context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            patch.ApplyTo(supplier, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return supplier;
        }


    }
}
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ShopBridgeAPI.CustomFilters;

namespace ShopBridgeAPI.Controllers
{
    [CustomAuth]
    public class ProductsController : ApiController
    {
        private ShopBridgeEntities db = new ShopBridgeEntities();


        [HttpGet]
        public IHttpActionResult GetProducts()
        {
            if (db.Products == null)
                return NotFound();
            else
                return Ok(db.Products);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.id }, product);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            Product oldproduct = await db.Products.FindAsync(product.id);
            oldproduct.name = product.name;
            oldproduct.description = product.description;
            oldproduct.price = product.price;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        bool ProductExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }
    }
}
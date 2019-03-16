using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using WebApiAngularJS.Models;
using WebApiAngularJS.Models.ViewModels;

namespace WebApiAngularJS.Controllers
{
    public class ProductsController : ApiController
    {
        private MyCon db = new MyCon();

        // GET: api/Products
        public IHttpActionResult GetProducts()
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    data = db.Products.Select(x => new ProductViewModel()
                    {
                        CategoryID = x.CategoryID,
                        ProductID = x.ProductID,
                        ProductName = x.ProductName,
                        UnitPrice = x.UnitPrice,
                        CategoryName = x.Category.CategoryName
                    })
                });
            }
            catch (Exception e)
            {
                return BadRequest($"Bir hata oluştu:{e.Message}");
            }
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = db.Products.Find(id);

            if (id != data.ProductID)
            {
                return BadRequest();
            }

            db.Entry(data).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok(new
                {
                    message = "Güncelleme işlemi başarılı"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var allPrd = db.Products.ToList();
            foreach (var prd in allPrd)
            {
                if (product.ProductID == prd.ProductID || product.ProductName == prd.ProductName)
                {
                    ModelState.AddModelError("message","Bir hata oluştu");
                    return BadRequest(ModelState);
                }
            }

            db.Products.Add(new Product()
            {
                ProductName = product.ProductName,
                CategoryID = product.CategoryID,
                UnitPrice = product.UnitPrice
            });
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}
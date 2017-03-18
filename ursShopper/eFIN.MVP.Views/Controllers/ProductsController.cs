using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using eFIN.MVP.AzureEF;

namespace eFIN.MVP.Views.Controllers
{
    public class ProductsController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        // GET: ProductEntities
        public ActionResult Index()
        {
            return View(azureDB.ProductEntities.ToList());
        }

        // GET: ProductEntities/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductEntity product = azureDB.ProductEntities.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductEntities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_manufacturer,product_sku,product_upc,product_ean,product_gtin,product_name,product_description,product_uom,product_price,product_create,product_update")] ProductEntity product)
        {
            if (ModelState.IsValid)
            {
                azureDB.ProductEntities.Add(product);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: ProductEntities/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductEntity product = azureDB.ProductEntities.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,product_manufacturer,product_sku,product_upc,product_ean,product_gtin,product_name,product_description,product_uom,product_price,product_create,product_update")] ProductEntity product)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(product).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: ProductEntities/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductEntity product = azureDB.ProductEntities.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProductEntity product = azureDB.ProductEntities.Find(id);
            azureDB.ProductEntities.Remove(product);
            azureDB.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                azureDB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

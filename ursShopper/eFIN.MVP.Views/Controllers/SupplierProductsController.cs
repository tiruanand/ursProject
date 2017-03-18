using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using eFIN.MVP.AzureEF;

namespace eFIN.MVP.Views.Controllers
{
    public class SupplierEntityProductsController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        // GET: SupplierProductEntities
        public ActionResult Index()
        {
            var supplierProducts = azureDB.SupplierProductEntities.Include(s => s.Product).Include(s => s.Supplier);
            return View(supplierProducts.ToList());
        }

        // GET: SupplierProductEntities
        public ActionResult AJSIndex()
        {
            return View();
        }

        public JsonResult getSupplierEntityProducts(long? supplierID)
        {
            var result = azureDB.SupplierProductEntities.Where(sp => sp.supplier_id == supplierID).Include(s => s.Product).Include(s => s.Supplier).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: SupplierProductEntities/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierProductEntity supplierProduct = azureDB.SupplierProductEntities.Find(id);
            if (supplierProduct == null)
            {
                return HttpNotFound();
            }
            return View(supplierProduct);
        }

        // GET: SupplierProductEntities/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(azureDB.ProductEntities, "product_id", "product_name");
            ViewBag.supplier_id = new SelectList(azureDB.SupplierEntities, "supplier_id", "supplier_name");
            return View();
        }

        // POST: SupplierProductEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "supplierproduct_id,supplier_id,product_id,supplierproduct_create,supplierproduct_update")] SupplierProductEntity supplierProduct)
        {
            if (ModelState.IsValid)
            {
                azureDB.SupplierProductEntities.Add(supplierProduct);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(azureDB.ProductEntities, "product_id", "product_name", supplierProduct.product_id);
            ViewBag.supplier_id = new SelectList(azureDB.SupplierEntities, "supplier_id", "supplier_name", supplierProduct.supplier_id);
            return View(supplierProduct);
        }

        // GET: SupplierProductEntities/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierProductEntity supplierProduct = azureDB.SupplierProductEntities.Find(id);
            if (supplierProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(azureDB.ProductEntities, "product_id", "product_name", supplierProduct.product_id);
            ViewBag.supplier_id = new SelectList(azureDB.SupplierEntities, "supplier_id", "supplier_name", supplierProduct.supplier_id);
            return View(supplierProduct);
        }

        // POST: SupplierProductEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "supplierproduct_id,supplier_id,product_id,supplierproduct_create,supplierproduct_update")] SupplierProductEntity supplierProduct)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(supplierProduct).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(azureDB.ProductEntities, "product_id", "product_name", supplierProduct.product_id);
            ViewBag.supplier_id = new SelectList(azureDB.SupplierEntities, "supplier_id", "supplier_name", supplierProduct.supplier_id);
            return View(supplierProduct);
        }

        // GET: SupplierProductEntities/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierProductEntity supplierProduct = azureDB.SupplierProductEntities.Find(id);
            if (supplierProduct == null)
            {
                return HttpNotFound();
            }
            return View(supplierProduct);
        }

        // POST: SupplierProductEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            SupplierProductEntity supplierProduct = azureDB.SupplierProductEntities.Find(id);
            azureDB.SupplierProductEntities.Remove(supplierProduct);
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

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using eFIN.MVP.AzureEF;

namespace eFIN.MVP.Views.Controllers
{
    public class MarketerProductsController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        // GET: MarketerProductEntities
        public ActionResult Index()
        {
            var marketerProducts = azureDB.MarketerProductEntities.Include(m => m.Marketer).Include(m => m.Product);
            return View(marketerProducts.ToList());
        }

        // GET: MarketerProductEntities/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketerProductEntity marketerProduct = azureDB.MarketerProductEntities.Find(id);
            if (marketerProduct == null)
            {
                return HttpNotFound();
            }
            return View(marketerProduct);
        }

        // GET: MarketerProductEntities/Create
        public ActionResult Create()
        {
            ViewBag.marketer_id = new SelectList(azureDB.MarketerEntities, "marketer_id", "marketer_name");
            ViewBag.product_id = new SelectList(azureDB.ProductEntities, "product_id", "product_name");
            return View();
        }

        // POST: MarketerProductEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "marketerproduct_id,marketer_id,product_id,marketerproduct_create,marketerproduct_update")] MarketerProductEntity marketerProduct)
        {
            if (ModelState.IsValid)
            {
                azureDB.MarketerProductEntities.Add(marketerProduct);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.marketer_id = new SelectList(azureDB.MarketerEntities, "marketer_id", "marketer_name", marketerProduct.marketer_id);
            ViewBag.product_id = new SelectList(azureDB.ProductEntities, "product_id", "product_name", marketerProduct.product_id);
            return View(marketerProduct);
        }

        // GET: MarketerProductEntities/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketerProductEntity marketerProduct = azureDB.MarketerProductEntities.Find(id);
            if (marketerProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.marketer_id = new SelectList(azureDB.MarketerEntities, "marketer_id", "marketer_name", marketerProduct.marketer_id);
            ViewBag.product_id = new SelectList(azureDB.ProductEntities, "product_id", "product_name", marketerProduct.product_id);
            return View(marketerProduct);
        }

        // POST: MarketerProductEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "marketerproduct_id,marketer_id,product_id,marketerproduct_create,marketerproduct_update")] MarketerProductEntity marketerProduct)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(marketerProduct).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.marketer_id = new SelectList(azureDB.MarketerEntities, "marketer_id", "marketer_name", marketerProduct.marketer_id);
            ViewBag.product_id = new SelectList(azureDB.ProductEntities, "product_id", "product_name", marketerProduct.product_id);
            return View(marketerProduct);
        }

        // GET: MarketerProductEntities/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketerProductEntity marketerProduct = azureDB.MarketerProductEntities.Find(id);
            if (marketerProduct == null)
            {
                return HttpNotFound();
            }
            return View(marketerProduct);
        }

        // POST: MarketerProductEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            MarketerProductEntity marketerProduct = azureDB.MarketerProductEntities.Find(id);
            azureDB.MarketerProductEntities.Remove(marketerProduct);
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

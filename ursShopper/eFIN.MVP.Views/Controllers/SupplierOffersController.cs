using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using eFIN.MVP.AzureEF;

namespace eFIN.MVP.Views.Controllers
{
    public class SupplierEntityOffersController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        // GET: SupplierOfferEntities
        public ActionResult Index()
        {
            var supplierOffers = azureDB.SupplierOfferEntities.Include(s => s.SupplierProduct);
            return View(supplierOffers.ToList());
        }

        // GET: SupplierOfferEntities/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierOfferEntity supplierOffer = azureDB.SupplierOfferEntities.Find(id);
            if (supplierOffer == null)
            {
                return HttpNotFound();
            }
            return View(supplierOffer);
        }

        // GET: SupplierOfferEntities/Create
        public ActionResult Create()
        {
            ViewBag.supplierproduct_id = new SelectList(azureDB.SupplierProductEntities, "supplierproduct_id", "supplierproduct_id");
            return View();
        }

        // POST: SupplierOfferEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "supplieroffer_id,supplierproduct_id,supplieroffer_incl_price,supplieroffer_end_date,supplieroffer_create,supplieroffer_timestamp")] SupplierOfferEntity supplierOffer)
        {
            if (ModelState.IsValid)
            {
                azureDB.SupplierOfferEntities.Add(supplierOffer);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.supplierproduct_id = new SelectList(azureDB.SupplierProductEntities, "supplierproduct_id", "supplierproduct_id", supplierOffer.supplierproduct_id);
            return View(supplierOffer);
        }

        // GET: SupplierOfferEntities/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierOfferEntity supplierOffer = azureDB.SupplierOfferEntities.Find(id);
            if (supplierOffer == null)
            {
                return HttpNotFound();
            }
            ViewBag.supplierproduct_id = new SelectList(azureDB.SupplierProductEntities, "supplierproduct_id", "supplierproduct_id", supplierOffer.supplierproduct_id);
            return View(supplierOffer);
        }

        // POST: SupplierOfferEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "supplieroffer_id,supplierproduct_id,supplieroffer_incl_price,supplieroffer_end_date,supplieroffer_create,supplieroffer_timestamp")] SupplierOfferEntity supplierOffer)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(supplierOffer).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.supplierproduct_id = new SelectList(azureDB.SupplierProductEntities, "supplierproduct_id", "supplierproduct_id", supplierOffer.supplierproduct_id);
            return View(supplierOffer);
        }

        // GET: SupplierOfferEntities/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierOfferEntity supplierOffer = azureDB.SupplierOfferEntities.Find(id);
            if (supplierOffer == null)
            {
                return HttpNotFound();
            }
            return View(supplierOffer);
        }

        // POST: SupplierOfferEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            SupplierOfferEntity supplierOffer = azureDB.SupplierOfferEntities.Find(id);
            azureDB.SupplierOfferEntities.Remove(supplierOffer);
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

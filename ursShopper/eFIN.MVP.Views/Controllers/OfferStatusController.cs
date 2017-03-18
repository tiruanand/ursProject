using eFIN.MVP.AzureEF;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace eFIN.MVP.Views.Controllers
{
    public class OfferStatusController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        // GET: OfferStatus
        public ActionResult Index()
        {
            return View(azureDB.OfferStatusEntities.ToList());
        }

        // GET: OfferStatus/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfferStatusEntity offerStatus = azureDB.OfferStatusEntities.Find(id);
            if (offerStatus == null)
            {
                return HttpNotFound();
            }
            return View(offerStatus);
        }

        // GET: OfferStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OfferStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "offer_status_id,offer_status,offer_status_description,offer_status_update")] OfferStatusEntity offerStatu)
        {
            if (ModelState.IsValid)
            {
                azureDB.OfferStatusEntities.Add(offerStatu);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(offerStatu);
        }

        // GET: OfferStatus/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfferStatusEntity offerStatu = azureDB.OfferStatusEntities.Find(id);
            if (offerStatu == null)
            {
                return HttpNotFound();
            }
            return View(offerStatu);
        }

        // POST: OfferStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "offer_status_id,offer_status,offer_status_description,offer_status_update")] OfferStatusEntity offerStatu)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(offerStatu).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offerStatu);
        }

        // GET: OfferStatus/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfferStatusEntity offerStatu = azureDB.OfferStatusEntities.Find(id);
            if (offerStatu == null)
            {
                return HttpNotFound();
            }
            return View(offerStatu);
        }

        // POST: OfferStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OfferStatusEntity offerStatu = azureDB.OfferStatusEntities.Find(id);
            azureDB.OfferStatusEntities.Remove(offerStatu);
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

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using eFIN.MVP.AzureEF;


namespace eFIN.MVP.Views.Controllers
{
    public class ConsumerEntityOffersController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        // GET: ConsumerOfferEntities
        public ActionResult Index()
        {
            var consumerOffers = azureDB.ConsumerOfferEntities.Include(c => c.Consumer);
            return View(consumerOffers.ToList());
        }

        // GET: ConsumerOfferEntities/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerOfferEntity consumerOffer = azureDB.ConsumerOfferEntities.Find(id);
            if (consumerOffer == null)
            {
                return HttpNotFound();
            }
            return View(consumerOffer);
        }

        // GET: ConsumerOfferEntities/Create
        public ActionResult Create()
        {
            ViewBag.consumer_id = new SelectList(azureDB.ConsumerEntities, "consumer_id", "consumer_handle");
            return View();
        }

        // POST: ConsumerOfferEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "consumeroffer_id,consumer_id,marketerproduct_id,consumeroffer_status_id,consumeroffer_date,consumeroffer_qty,consumeroffer_max_price,consumeroffer_end_date,consumeroffer_current,consumeroffer_create,consumeroffer_update")] ConsumerOfferEntity consumerOffer)
        {
            if (ModelState.IsValid)
            {
                azureDB.ConsumerOfferEntities.Add(consumerOffer);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.consumer_id = new SelectList(azureDB.ConsumerEntities, "consumer_id", "consumer_handle", consumerOffer.consumer_id);
            return View(consumerOffer);
        }

        // GET: ConsumerOfferEntities/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerOfferEntity consumerOffer = azureDB.ConsumerOfferEntities.Find(id);
            if (consumerOffer == null)
            {
                return HttpNotFound();
            }
            ViewBag.consumer_id = new SelectList(azureDB.ConsumerEntities, "consumer_id", "consumer_handle", consumerOffer.consumer_id);
            return View(consumerOffer);
        }

        // POST: ConsumerOfferEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "consumeroffer_id,consumer_id,marketerproduct_id,consumeroffer_status_id,consumeroffer_date,consumeroffer_qty,consumeroffer_max_price,consumeroffer_end_date,consumeroffer_current,consumeroffer_create,consumeroffer_update")] ConsumerOfferEntity consumerOffer)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(consumerOffer).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.consumer_id = new SelectList(azureDB.ConsumerEntities, "consumer_id", "consumer_handle", consumerOffer.consumer_id);
            return View(consumerOffer);
        }

        // GET: ConsumerOfferEntities/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerOfferEntity consumerOffer = azureDB.ConsumerOfferEntities.Find(id);
            if (consumerOffer == null)
            {
                return HttpNotFound();
            }
            return View(consumerOffer);
        }

        // POST: ConsumerOfferEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ConsumerOfferEntity consumerOffer = azureDB.ConsumerOfferEntities.Find(id);
            azureDB.ConsumerOfferEntities.Remove(consumerOffer);
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

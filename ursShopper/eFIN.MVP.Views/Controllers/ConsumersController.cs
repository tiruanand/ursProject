using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using eFIN.MVP.AzureEF;
using eFIN.MVP.Views.Services;

namespace eFIN.MVP.Views.Controllers
{
    public class ConsumersController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        public ConsumersController()
        {

        }

        #region Controller View actions
        // GET: ConsumerEntities
        public ActionResult Index()
        {
            //var consumers = azureDB.ConsumerEntities.Include(c => c.Address).Include(c => c.Address1).Include(c => c.MarketerEntity);
            return View();
        }

        // GET: ConsumerEntities/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerEntity consumer = azureDB.ConsumerEntities.Find(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // GET: ConsumerEntities/Create
        public ActionResult Create()
        {
            ViewBag.bill_address_id = new SelectList(azureDB.AddressEntities, "address_id", "line_1");
            ViewBag.ship_address_id = new SelectList(azureDB.AddressEntities, "address_id", "line_1");
            ViewBag.marketer_id = new SelectList(azureDB.MarketerEntities, "marketer_id", "marketer_domain");
            return View();
        }

        // POST: ConsumerEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "consumer_id,marketer_id,bill_address_id,ship_address_id,consumer_handle,consumer_email,consumer_fname,consumer_mname,consumer_lname,consumer_phone,consumer_status,consumer_create,consumer_update")] ConsumerEntity consumer)
        {
            if (ModelState.IsValid)
            {
                azureDB.ConsumerEntities.Add(consumer);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.bill_address_id = new SelectList(azureDB.AddressEntities, "address_id", "line_1", consumer.bill_address_id);
            ViewBag.ship_address_id = new SelectList(azureDB.AddressEntities, "address_id", "line_1", consumer.ship_address_id);
            ViewBag.marketer_id = new SelectList(azureDB.MarketerEntities, "marketer_id", "marketer_domain", consumer.marketer_id);
            return View(consumer);
        }

        // GET: ConsumerEntities/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerEntity consumer = azureDB.ConsumerEntities.Find(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            ViewBag.bill_address_id = new SelectList(azureDB.AddressEntities, "address_id", "line_1", consumer.bill_address_id);
            ViewBag.ship_address_id = new SelectList(azureDB.AddressEntities, "address_id", "line_1", consumer.ship_address_id);
            ViewBag.marketer_id = new SelectList(azureDB.MarketerEntities, "marketer_id", "marketer_domain", consumer.marketer_id);
            return View(consumer);
        }

        // POST: ConsumerEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "consumer_id,marketer_id,bill_address_id,ship_address_id,consumer_handle,consumer_email,consumer_fname,consumer_mname,consumer_lname,consumer_phone,consumer_status,consumer_create,consumer_update")] ConsumerEntity consumer)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(consumer).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bill_address_id = new SelectList(azureDB.AddressEntities, "address_id", "line_1", consumer.bill_address_id);
            ViewBag.ship_address_id = new SelectList(azureDB.AddressEntities, "address_id", "line_1", consumer.ship_address_id);
            ViewBag.marketer_id = new SelectList(azureDB.MarketerEntities, "marketer_id", "marketer_domain", consumer.marketer_id);
            return View(consumer);
        }

        // GET: ConsumerEntities/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerEntity consumer = azureDB.ConsumerEntities.Find(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // POST: ConsumerEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ConsumerEntity consumer = azureDB.ConsumerEntities.Find(id);
            azureDB.ConsumerEntities.Remove(consumer);
            azureDB.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                azureDB.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region JSON WCF methods

        public JsonResult GetConsumers()
        {
            try
            {
                var data = ShopperServiceFactory.ConsumerService.GetAllConsumers();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }

        public JsonResult GetConsumerOffers(long MarketerId, string ConsumerHandle)
        {
            var data = ShopperServiceFactory.ConsumerService.GetConsumerOffers(MarketerId, ConsumerHandle);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

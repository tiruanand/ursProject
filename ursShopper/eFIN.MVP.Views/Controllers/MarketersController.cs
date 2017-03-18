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
    public class MarketersController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        // GET: Marketers
        public ActionResult Index()
        {
            return View();
        }

        // GET: Marketers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketerEntity marketer = azureDB.MarketerEntities.Find(id);
            if (marketer == null)
            {
                return HttpNotFound();
            }
            return View(marketer);
        }

        // GET: Marketers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marketers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "marketer_id,marketer_domain,marketer_type,marketer_name,marketer_create,marketer_update")] MarketerEntity marketer)
        {
            if (ModelState.IsValid)
            {
                azureDB.MarketerEntities.Add(marketer);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marketer);
        }

        // GET: Marketers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketerEntity marketer = azureDB.MarketerEntities.Find(id);
            if (marketer == null)
            {
                return HttpNotFound();
            }
            return View(marketer);
        }

        // POST: Marketers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "marketer_id,marketer_domain,marketer_type,marketer_name,marketer_create,marketer_update")] MarketerEntity marketer)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(marketer).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marketer);
        }

        // GET: Marketers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketerEntity marketer = azureDB.MarketerEntities.Find(id);
            if (marketer == null)
            {
                return HttpNotFound();
            }
            return View(marketer);
        }

        // POST: Marketers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            MarketerEntity marketer = azureDB.MarketerEntities.Find(id);
            azureDB.MarketerEntities.Remove(marketer);
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


        public JsonResult GetAllMarketers()
        {
            try
            {
                azureDB.Configuration.ProxyCreationEnabled = false;
                var data = azureDB.MarketerEntities.ToList();
                azureDB.Configuration.ProxyCreationEnabled = true;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }

            //Newtonsoft.Json.JsonConvert.SerializeObject(result, new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllMarketerProducts(long? marketerID)
        {
            azureDB.Configuration.ProxyCreationEnabled = false;
            azureDB.Configuration.LazyLoadingEnabled = false;
            var result = azureDB.MarketerProductEntities.Where(s => s.marketer_id == marketerID);

            var products = azureDB.ProductEntities.Where(p => result.Any(r => r.product_id == p.product_id)).ToList();
            azureDB.Configuration.ProxyCreationEnabled = true;
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        #region JSON WCF methods

        public JsonResult GetMarketers()
        {
            try
            {
                var data = ShopperServiceFactory.MarketerService.GetAllMarketers();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }

        //public JsonResult GetMarketerOffers(string MarketerName)
        //{
        //    var data = ShopperServiceFactory.MarketerService.GetMarketerOffers(MarketerName);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetMarketerProducts(string MarketerName)
        {
            var data = ShopperServiceFactory.MarketerService.GetMarketerProducts(MarketerName);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

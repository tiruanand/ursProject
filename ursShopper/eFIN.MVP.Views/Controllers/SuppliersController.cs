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
    public class SuppliersController : Controller
    {
        ursCoreMVPEntities azureDB = new ursCoreMVPEntities();

        // GET: SupplierEntities
        public ActionResult Index()
        {
            //return View(azureDB.SupplierEntities.ToList());
            return View();
        }

        // GET: SupplierEntities
        public ActionResult OIndex()
        {
            return View(azureDB.SupplierEntities.ToList());
        }

        // GET: SupplierEntities/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierEntity supplier = azureDB.SupplierEntities.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: SupplierEntities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "supplier_id,supplier_domain,supplier_type,supplier_name,supplier_create,supplier_update")] SupplierEntity supplier)
        {
            if (ModelState.IsValid)
            {
                azureDB.SupplierEntities.Add(supplier);
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: SupplierEntities/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierEntity supplier = azureDB.SupplierEntities.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: SupplierEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "supplier_id,supplier_domain,supplier_type,supplier_name")] SupplierEntity supplier)
        {
            if (ModelState.IsValid)
            {
                azureDB.Entry(supplier).State = EntityState.Modified;
                azureDB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: SupplierEntities/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierEntity supplier = azureDB.SupplierEntities.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: SupplierEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            SupplierEntity supplier = azureDB.SupplierEntities.Find(id);
            azureDB.SupplierEntities.Remove(supplier);
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

        public JsonResult GetAllSuppliers()
        {
            try
            {
                azureDB.Configuration.ProxyCreationEnabled = false;
                var data = azureDB.SupplierEntities.ToList(); 
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

        public JsonResult GetAllSupplierProducts(long? supplierID)
        {
            azureDB.Configuration.ProxyCreationEnabled = false;
            azureDB.Configuration.LazyLoadingEnabled = false;
            var result = azureDB.SupplierProductEntities.Where(s => s.supplier_id == supplierID);

            var products = azureDB.ProductEntities.Where(p => result.Any(r => r.product_id == p.product_id)).ToList();
            azureDB.Configuration.ProxyCreationEnabled = true;
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        #region JSON WCF methods

        public JsonResult GetSuppliers()
        {
            try
            {
                var data = ShopperServiceFactory.SupplierService.GetAllSuppliers();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }

        //public JsonResult GetSupplierOffers(string SupplierName)
        //{
        //    var data = ShopperServiceFactory.SupplierService.GetSupplierOffers(SupplierName);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetSupplierProducts(string SupplierName)
        {
            var data = ShopperServiceFactory.SupplierService.GetSupplierProducts(SupplierName);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

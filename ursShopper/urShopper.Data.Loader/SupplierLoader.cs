using System;
using System.Configuration;
using System.ServiceModel;
using urShopper.Data.Loader.SupplierService;

namespace urShopper.Data.Loader
{
    public class SupplierLoader
    {
        SupplierServiceClient supplierSvc;

        public SupplierLoader()
        {
            EndpointIdentity spn = EndpointIdentity.CreateSpnIdentity("host/mikev-ws");
            var appSettings = ConfigurationManager.AppSettings;
            Uri uri = new Uri(appSettings["SupplierServiceSvc"]);
            var address = new EndpointAddress(uri, spn);
            supplierSvc = new SupplierServiceClient("BasicHttpBinding_ISupplierService", address);
        }

        public string LoadSuppliers(string RequestXml)
        {
            try
            {
                var suppliersList = XmlUtil.ConvertXmlToObjects<Supplier>(RequestXml);
                long supplierId;
                if (suppliersList != null && suppliersList.Count > 0)
                {
                   foreach(var supplier in suppliersList)
                    {
                        supplierId = supplierSvc.SaveSupplier(supplier.Name, supplier.Domain, supplier.Type);
                        supplier.Id = supplierId;
                    }
                    return XmlUtil.ConvertObjectsToXml<Supplier>(suppliersList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            return null;
        }
    }
}

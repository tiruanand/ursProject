using System;
using System.Configuration;
using System.ServiceModel;
using urShopper.Data.Loader.SupplierService;

namespace urShopper.Data.Loader
{
    public class SupplierOfferLoader
    {
        SupplierServiceClient supplierSvc;

        public SupplierOfferLoader()
        {
            EndpointIdentity spn = EndpointIdentity.CreateSpnIdentity("host/mikev-ws");
            var appSettings = ConfigurationManager.AppSettings;
            Uri uri = new Uri(appSettings["SupplierServiceSvc"]);
            var address = new EndpointAddress(uri, spn);
            supplierSvc = new SupplierServiceClient("BasicHttpBinding_ISupplierService", address);
        }

        public string LoadSupplierOffers(string RequestXml)
        {
            try
            {
                var supplierOfferList = XmlUtil.ConvertXmlToObjects<SupplierOffer>(RequestXml);
                long supplierOfferId;
                if (supplierOfferList != null && supplierOfferList.Count > 0)
                {
                    foreach (var supplier in supplierOfferList)
                    {
                        foreach (var currentOffer in supplier.Offers)
                        {
                            supplierOfferId = supplierSvc.SaveSupplierOffer(supplier.OfferedSupplier.Name
                                                                    , currentOffer.OfferedProduct.Name
                                                                    , currentOffer.MaxPrice
                                                                    , currentOffer.EndDate);
                            currentOffer.ID = supplierOfferId;
                        }
                    }
                    return XmlUtil.ConvertObjectsToXml<SupplierOffer>(supplierOfferList);
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

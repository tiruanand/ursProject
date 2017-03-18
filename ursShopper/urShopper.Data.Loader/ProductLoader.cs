using urShopper.Data.Loader.ProductService;
using System;
using System.ServiceModel;
using System.Configuration;

namespace urShopper.Data.Loader
{
    public class ProductLoader
    {
        ProductServiceClient productSvc;

        public ProductLoader()
        {
            EndpointIdentity spn = EndpointIdentity.CreateSpnIdentity("host/mikev-ws");
            var appSettings = ConfigurationManager.AppSettings;
            Uri uri = new Uri(appSettings["ProductServiceSvc"]);
            var address = new EndpointAddress(uri, spn);
            productSvc = new ProductServiceClient("BasicHttpBinding_IProductService", address);
        }

        public string LoadProducts(string RequestXml)
        {
            try
            {
                var productsList = XmlUtil.ConvertXmlToObjects<Product>(RequestXml);
                long productId;
                if (productsList != null && productsList.Count > 0)
                {
                   foreach(var product in productsList)
                    {
                        productId = productSvc.SaveProduct(product.Name, product.Description, product.Manufacturer
                                                , product.Price, product.SKU, product.UPC
                                                , product.EAN, product.GTIN, product.UOM);
                        product.ID = productId;
                    }
                    return XmlUtil.ConvertObjectsToXml<Product>(productsList);
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

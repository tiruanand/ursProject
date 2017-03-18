using System;
using System.Configuration;
using System.ServiceModel;
using urShopper.Data.Loader.MarketerService;

namespace urShopper.Data.Loader
{
    public class MarketerLoader
    {
        MarketerServiceClient marketerSvc;

        public MarketerLoader()
        {
            EndpointIdentity spn = EndpointIdentity.CreateSpnIdentity("host/mikev-ws");
            var appSettings = ConfigurationManager.AppSettings;
            Uri uri = new Uri(appSettings["MarketerServiceSvc"]);
            var address = new EndpointAddress(uri, spn);
            marketerSvc = new MarketerServiceClient("BasicHttpBinding_IMarketerService", address);
        }


        public string LoadMarketers(string RequestXml)
        {
            try
            {
                var marketersList = XmlUtil.ConvertXmlToObjects<Marketer>(RequestXml);
                long marketerId;
                if (marketersList != null && marketersList.Count > 0)
                {
                   foreach(var marketer in marketersList)
                    {
                        marketerId = marketerSvc.SaveMarketer(marketer.Name, marketer.Domain, marketer.Type);
                        marketer.Id = marketerId;
                    }
                    return XmlUtil.ConvertObjectsToXml<Marketer>(marketersList);
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

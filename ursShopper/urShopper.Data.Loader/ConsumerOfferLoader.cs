using System;
using System.Configuration;
using System.ServiceModel;
using System.Text;
using urShopper.Data.Loader.ConsumerService;

namespace urShopper.Data.Loader
{
    public class ConsumerOfferLoader
    {
        ConsumerServiceClient consumerSvc;

        public ConsumerOfferLoader()
        {
            EndpointIdentity spn = EndpointIdentity.CreateSpnIdentity("host/mikev-ws");
            var appSettings = ConfigurationManager.AppSettings;
            Uri uri = new Uri(appSettings["ConsumerServiceSvc"]);
            var address = new EndpointAddress(uri, spn);
            consumerSvc = new ConsumerServiceClient("BasicHttpBinding_IConsumerService", address);
        }


        public string LoadConsumerOffers(string RequestXml)
        {
            try
            {
                var errMsgs = new StringBuilder();
                var consumerOfferList = XmlUtil.ConvertXmlToObjects<ConsumerOffer>(RequestXml);
                long consumerOfferId;
                if (consumerOfferList != null && consumerOfferList.Count > 0)
                {
                   foreach(var consumer in consumerOfferList)
                    {
                        foreach (var currentOffer in consumer.Offers)
                        {
                            try
                            {
                                consumerOfferId = consumerSvc.SaveConsumerOffer(consumer.OfferedConsumer.ConsumerHandle
                                                                        , consumer.OfferedConsumer.Marketer.Name
                                                                        , currentOffer.OfferedProduct.SKU
                                                                        , currentOffer.OfferDate
                                                                        , currentOffer.EndDate
                                                                        , currentOffer.Quantity
                                                                        , currentOffer.MaxPrice);
                                currentOffer.ID = consumerOfferId;
                            }
                            catch (Exception ex)
                            {
                                errMsgs.AppendLine(ex.Message);
                            }
                        }
                    }
                    return XmlUtil.ConvertObjectsToXml<ConsumerOffer>(consumerOfferList);
                }
                if (errMsgs.Length > 0)
                    throw new Exception(errMsgs.ToString());
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

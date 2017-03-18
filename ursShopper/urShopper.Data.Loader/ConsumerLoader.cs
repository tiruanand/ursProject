using System;
using System.Configuration;
using System.ServiceModel;
using urShopper.Data.Loader.ConsumerService;

namespace urShopper.Data.Loader
{
    public static class Logger
    {
        private static log4net.ILog Log { get; set; }

        static Logger()
        {
            Log = log4net.LogManager.GetLogger(typeof(Logger));
        }

        public static void Error(object msg)
        {
            Log.Error(msg);
        }

        public static void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        public static void Info(object msg)
        {
            Log.Info(msg);
        }
    }

    public class ConsumerLoader
    {
        ConsumerServiceClient consumerSvc;

        public ConsumerLoader()
        {
            Logger.Error("Test error..");
            EndpointIdentity spn = EndpointIdentity.CreateSpnIdentity("host/mikev-ws");
            var appSettings = ConfigurationManager.AppSettings;
            Uri uri = new Uri(appSettings["ConsumerServiceSvc"]);
            var address = new EndpointAddress(uri, spn);
            consumerSvc = new ConsumerServiceClient("BasicHttpBinding_IConsumerService");//, address);
        }

        public string LoadConsumers(string RequestXml)
        {
            try
            {
                var consumersList = XmlUtil.ConvertXmlToObjects<Consumer>(RequestXml);
                long consumerId;
                if (consumersList != null && consumersList.Count > 0)
                {
                   foreach(var consumer in consumersList)
                    {
                        consumerId = consumerSvc.SaveMarketerConsumer(consumer.Marketer.Name, consumer.Marketer.Domain, consumer.Marketer.Type, consumer.ConsumerHandle, consumer.ConsumerEmail
                                                , consumer.ConsumerFName, consumer.ConsumerMName, consumer.ConsumerLName
                                                , consumer.ConsumerPhone, consumer.ConsumerStatus);
                        consumer.ConsumerId = consumerId;
                    }
                    return XmlUtil.ConvertObjectsToXml<Consumer>(consumersList);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error error logging", ex);
                throw ex;
            }
            return null;
        }
    }
}

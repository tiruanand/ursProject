using eFIN.MVP.AzureEF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IConsumerService" in both code and config file together.
    public class ConsumerService : IConsumerService
    {
        #region Member Vaiables
        ursCoreMVPEntities azureDB;
        AddressService addressSvc;
        MarketerService marketerSvc;
        ConsumerOfferService consumerOfferSvc;
        ProductService productSvc;
        #endregion

        #region Constructor
        /// <summary>
        /// Service that implements IConsumerService to maintain for Consumer related actions.
        /// </summary>
        public ConsumerService()
        {
            azureDB = new ursCoreMVPEntities();
            addressSvc = new AddressService(azureDB);
            productSvc = new ProductService(azureDB);
            marketerSvc = new MarketerService(azureDB, productSvc);
            consumerOfferSvc = new ConsumerOfferService(azureDB, this, productSvc, marketerSvc);
        }
        #endregion

        #region Consumer service methods
        /// <summary>
        /// Retrieve Consumer for the given Consumer Id.
        /// </summary>
        /// <param name="ConsumerId"></param>
        /// <returns></returns>
        public Consumer GetConsumerById(long ConsumerId)
        {
            var EFConsumer = (from p
                            in azureDB.ConsumerEntities
                              where p.consumer_id == ConsumerId
                              select p).FirstOrDefault();
            if (EFConsumer != null)
                return TranslateEntityToObject(EFConsumer);
            else
                throw new Exception("Invalid consumer");
        }
        /// <summary>
        /// Retrieve Consumer for the given Consumer Name.
        /// </summary>
        /// <param name="ConsumerHandle"></param>
        /// <returns></returns>
        public Consumer GetConsumer(string ConsumerHandle)
        {
            var EFConsumer = (from p in azureDB.ConsumerEntities
                              where p.consumer_handle.Equals(ConsumerHandle.Trim(), StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFConsumer != null)
                return TranslateEntityToObject(EFConsumer);
            else
                throw new Exception(string.Format("Consumer: {0} does not exists in urshopper.", ConsumerHandle));
        }

        /// <summary>
        /// Retrieve consumer unique id for the given consumer name
        /// </summary>
        /// <param name="ConsumerHandle"></param>
        /// <returns></returns>
        public long GetConsumerId(string ConsumerHandle)
        {
            var EFConsumer = (from p
                            in azureDB.ConsumerEntities
                              where p.consumer_handle.Equals(ConsumerHandle.Trim(), StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFConsumer != null)
                return EFConsumer.consumer_id;
            else
                throw new Exception(string.Format("Consumer: {0} does not exists in urshopper.", ConsumerHandle));
        }

        /// <summary>
        /// Retrieves all available consumers.
        /// </summary>
        /// <returns></returns>
        public List<Consumer> GetAllConsumers()
        {
            var allConsumers = (from p in azureDB.ConsumerEntities
                                select p).Take(25).ToList();
                               // select p).ToList();

            var targetList = allConsumers.Select(x => new Consumer()
            {
                ConsumerId = x.consumer_id,
                ConsumerFName = x.consumer_fname,
                ConsumerMName = x.consumer_mname,
                ConsumerLName = x.consumer_lname,
                ConsumerHandle = x.consumer_handle,
                ConsumerEmail = x.consumer_email,
                ConsumerPhone = x.consumer_phone,
                ConsumerStatus = x.consumer_status,
                CreatedOn = x.consumer_create,
                LastUpdatedOn = x.consumer_update ?? DateTime.Now,
                MarketerId = x.marketer_id,
                Marketer = marketerSvc.TranslateEntityToObject(x.Marketer),
                BillAddressId = x.bill_address_id,
                BillAddress = addressSvc.TranslateEntityToObject(x.BillingAddress),
                ShipAddressId = x.ship_address_id,
                ShipAddress = addressSvc.TranslateEntityToObject(x.ShippingAddress)
            }).ToList();
            return targetList;
        }

        /// <summary>
        /// Retrieves the list of consumers with the basic details.
        /// </summary>
        /// <returns></returns>
        public List<Consumer> GetAllConsumersList()
        {
            var allConsumers = (from p in azureDB.ConsumerEntities
                                select p).ToList();

            var targetList = allConsumers.Select(x => new Consumer()
            {
                ConsumerId = x.consumer_id,
                ConsumerFName = x.consumer_fname,
                ConsumerMName = x.consumer_mname,
                ConsumerLName = x.consumer_lname
            }).ToList();

            return targetList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="MarketerDomain"></param>
        /// <param name="MarketerType"></param>
        /// <returns></returns>
        public long SaveMarketer(string MarketerName, string MarketerDomain, string MarketerType)
        {
            return marketerSvc.SaveMarketer(MarketerName, MarketerDomain, MarketerType);
        }

        /// <summary>
        /// Creates or updates consumer based on given markter and consumer details.
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="MarkterDomain"></param>
        /// <param name="MarketerType"></param>
        /// <param name="ConsumerHandle"></param>
        /// <param name="ConsumerEmail"></param>
        /// <param name="ConsumerFName"></param>
        /// <param name="ConsumerMName"></param>
        /// <param name="ConsumerLName"></param>
        /// <param name="ConsumerPhone"></param>
        /// <param name="ConsumerStatus"></param>
        /// <returns></returns>
        public long SaveMarketerConsumer(string MarketerName, string MarkterDomain, string MarketerType, string ConsumerHandle, string ConsumerEmail, string ConsumerFName, string ConsumerMName, string ConsumerLName, string ConsumerPhone, string ConsumerStatus)
        {
            long mktrId = SaveMarketer(MarketerName, MarkterDomain, MarketerType);
            return SaveConsumer(MarketerName, ConsumerHandle, ConsumerEmail, ConsumerFName, ConsumerMName, ConsumerLName, ConsumerPhone, ConsumerStatus);
        }

        /// <summary>
        /// Creates or updates consumer based on given consumer details.
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="ConsumerHandle"></param>
        /// <param name="ConsumerEmail"></param>
        /// <param name="ConsumerFName"></param>
        /// <param name="ConsumerMName"></param>
        /// <param name="ConsumerLName"></param>
        /// <param name="ConsumerPhone"></param>
        /// <param name="ConsumerStatus"></param>
        /// <returns></returns>
        public long SaveConsumer(string MarketerName, string ConsumerHandle, string ConsumerEmail, string ConsumerFName, string ConsumerMName, string ConsumerLName, string ConsumerPhone, string ConsumerStatus)
        {
            var EFConsumer = (from p in azureDB.ConsumerEntities
                              where p.consumer_handle.Equals(ConsumerHandle, StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            var mktrId = marketerSvc.GetMarketerId(MarketerName);

            if (mktrId < 1)
            {
                throw new Exception(string.Format("{0} - Marketer does not exists.",  MarketerName));
            }
            if (EFConsumer == null)
            {
                var consumerEntity = new ConsumerEntity()
                {
                    marketer_id = mktrId,
                    consumer_fname = ConsumerFName,
                    consumer_mname = ConsumerMName,
                    consumer_lname = ConsumerLName,
                    consumer_handle = ConsumerHandle,
                    consumer_email = ConsumerEmail,
                    consumer_phone = ConsumerPhone,
                    consumer_status = ConsumerStatus,
                    consumer_create = DateTime.Now,
                    consumer_update = DateTime.Now
                };
                azureDB.ConsumerEntities.Add(consumerEntity);
            }
            else
            {
                EFConsumer.marketer_id = mktrId;
                EFConsumer.consumer_fname = ConsumerFName;
                EFConsumer.consumer_mname = ConsumerMName;
                EFConsumer.consumer_lname = ConsumerLName;
                EFConsumer.consumer_handle = ConsumerHandle;
                EFConsumer.consumer_email = ConsumerEmail;
                EFConsumer.consumer_phone = ConsumerPhone;
                EFConsumer.consumer_status = ConsumerStatus;
                EFConsumer.consumer_update = DateTime.Now;
            }
            azureDB.SaveChanges();
            return GetConsumerId(ConsumerHandle);
        }

        /// <summary>
        /// Associates Consumer Billing Address for the given 
        /// </summary>
        /// <param name="ConsumerHandle"></param>
        /// <param name="Line1"></param>
        /// <param name="Line2"></param>
        /// <param name="Line3"></param>
        /// <param name="City"></param>
        /// <param name="ZipPostalCode"></param>
        /// <param name="StateProvinceCounty"></param>
        /// <param name="Country"></param>
        /// <returns></returns>
        public long SaveConsumerBillingAddress(string ConsumerHandle, string Line1, string Line2, string Line3, string City, string ZipPostalCode, string StateProvinceCounty, string Country)
        {
            var addressId = addressSvc.SaveAddress(Line1, Line2, Line3, City, ZipPostalCode, StateProvinceCounty, Country);
            var consumer = GetConsumer(ConsumerHandle);
            var consumerBillAddr = (from ba in azureDB.ConsumerEntities
                                    where ba.consumer_id == consumer.ConsumerId
                                    select ba).FirstOrDefault();

            if (consumerBillAddr == null)
            {
                if (consumerBillAddr.bill_address_id != addressId)
                {
                    consumerBillAddr.bill_address_id = addressId;
                    azureDB.SaveChanges();
                    return addressId;
                }
            }
            return -1;
        }

        public long SaveConsumerShippingAddress(string ConsumerHandle, string Line1, string Line2, string Line3, string City, string ZipPostalCode, string StateProvinceCounty, string Country)
        {
            var addressId = addressSvc.SaveAddress(Line1, Line2, Line3, City, ZipPostalCode, StateProvinceCounty, Country);
            var consumer = GetConsumer(ConsumerHandle);
            var consumerShipAddr = (from ba in azureDB.ConsumerEntities
                                    where ba.consumer_id == consumer.ConsumerId
                                    select ba).FirstOrDefault();

            if (consumerShipAddr == null)
            {
                if (consumerShipAddr.ship_address_id != addressId)
                {
                    consumerShipAddr.ship_address_id = addressId;
                    azureDB.SaveChanges();
                    return addressId;
                }
            }
            return -1;
        }

        /// <summary>
        /// Removes the consumer for the given consumer name.
        /// </summary>
        /// <param name="Name"></param>
        public void DeleteConsumer(string ConsumerHandle)
        {
            var EFConsumer = (from p
                            in azureDB.ConsumerEntities
                              where p.consumer_handle.Equals(ConsumerHandle, StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFConsumer != null)
            {
                azureDB.ConsumerEntities.Remove(EFConsumer);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("{0} - Consumer does not exists.", ConsumerHandle));
        }

        /// <summary>
        /// Removes the consumer for the given consumer unique id.
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteConsumerById(int ConsumerId)
        {
            var EFConsumer = (from p
                            in azureDB.ConsumerEntities
                              where p.consumer_id == ConsumerId
                              select p).FirstOrDefault();
            if (EFConsumer != null)
            {
                azureDB.ConsumerEntities.Remove(EFConsumer);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("Consumer: {0} does not exists in urshopper.", ConsumerId));
        }

        /// <summary>
        /// Translate Consumer entity framework object to realtime consumer object.
        /// </summary>
        /// <param name="consumer"></param>
        /// <returns></returns>
        public Consumer TranslateEntityToObject(ConsumerEntity consumer)
        {
            Consumer objConsumer = new Consumer()
            {
                ConsumerId = consumer.consumer_id,
                ConsumerFName = consumer.consumer_fname,
                ConsumerMName = consumer.consumer_mname,
                ConsumerLName = consumer.consumer_lname,
                ConsumerHandle = consumer.consumer_handle,
                ConsumerEmail = consumer.consumer_email,
                ConsumerPhone = consumer.consumer_phone,
                ConsumerStatus = consumer.consumer_status,
                CreatedOn = consumer.consumer_create,
                LastUpdatedOn = consumer.consumer_update ?? DateTime.Now,
                MarketerId = consumer.marketer_id,
                Marketer = marketerSvc.TranslateEntityToObject(consumer.Marketer)
            };
            return objConsumer;
        }
        #endregion

        #region Consumer Offer service methods
        /// <summary>
        /// Retrieves Consumer Offers based on Marketer Id and Consumer Handle
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="ConsumerHandle"></param>
        /// <returns></returns>
        public List<ConsumerOffer> GetConsumerOffers(string MarketerName, string ConsumerHandle)
        {
            return consumerOfferSvc.GetConsumerOffers(MarketerName, ConsumerHandle);
        }

        /// <summary>
        /// Save Consumer Offer for the given Consumer and Offer details.
        /// </summary>
        /// <param name="ConsumerHandle"></param>
        /// <param name="MarketerName"></param>
        /// <param name="ProductSKU"></param>
        /// <param name="ConsumerOfferDate"></param>
        /// <param name="OfferValidUpto"></param>
        /// <param name="ConsumerOfferQty"></param>
        /// <param name="ConsumerOfferMaxPrice"></param>
        /// <returns></returns>
        public long SaveConsumerOffer(string ConsumerHandle, string MarketerName, string ProductSKU, DateTime ConsumerOfferDate, DateTime OfferValidUpto, int ConsumerOfferQty, decimal ConsumerOfferMaxPrice)
        {
            return consumerOfferSvc.SaveConsumerOffer(ConsumerHandle, MarketerName, ProductSKU, ConsumerOfferDate, OfferValidUpto, 1, ConsumerOfferQty, ConsumerOfferMaxPrice, true);
        }

        public bool RemoveConsumerOffer(string ConsumerName, string MarketerName, string ProductSKU, decimal ConsumerOfferMaxPrice, DateTime OfferValidUpto)
        {
            return consumerOfferSvc.RemoveConsumerOffer(ConsumerName, MarketerName, ProductSKU, ConsumerOfferMaxPrice, OfferValidUpto);
        }

        public ConsumerOffer GetConsumerOffer(string ConsumerName, string MarketerName, string ProductSKU, decimal ConsumerOfferMaxPrice, DateTime OfferValidUpto)
        {
            return consumerOfferSvc.GetConsumerOffer(ConsumerName, MarketerName, ProductSKU, ConsumerOfferMaxPrice, OfferValidUpto);
        }
        #endregion
    }
}

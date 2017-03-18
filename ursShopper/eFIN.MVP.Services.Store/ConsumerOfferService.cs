using eFIN.MVP.AzureEF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFIN.MVP.Services.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsumerOfferService : IConsumerOfferService
    {
        ursCoreMVPEntities azureDB;
        ConsumerService consumerSvc;
        ProductService productSvc;
        MarketerService marketerSvc;

        public ConsumerOfferService()
        { }

        public ConsumerOfferService(ursCoreMVPEntities AzureEFEntities, ConsumerService ConsumerSvc, ProductService ProductSvc, MarketerService MarketerSvc)
        {
            azureDB = AzureEFEntities;
            consumerSvc = ConsumerSvc;
            productSvc = ProductSvc;
            marketerSvc = MarketerSvc;
        }

        public long SaveConsumerOffer(string ConsumerHandle, string MarketerName, string ProductSKU, DateTime ConsumerOfferDate, DateTime OfferValidUpto, long ConsumerOfferStatusId,int ConsumerOfferQty, decimal ConsumerOfferMaxPrice, bool IsConsumerOfferCurrent)
        {
            var consumerOffer = GetConsumerOffer(ConsumerHandle, MarketerName, ProductSKU, ConsumerOfferMaxPrice, OfferValidUpto);
            long productId = productSvc.GetProductId(ProductSKU);
            long consumerId = consumerSvc.GetConsumerId(ConsumerHandle);
            long marketerId = marketerSvc.GetMarketerId(MarketerName);
            long marketerProductId = marketerSvc.GetMarketerProductId(marketerId, productId);

            if (marketerProductId == 0)
                throw new Exception(string.Format("Marketer {0} does not have the product {1} in urshopoer.", MarketerName, ProductSKU));

            if (consumerOffer == null)
            {
                azureDB.ConsumerOfferEntities.Add(new ConsumerOfferEntity()
                {
                    consumer_id = consumerId,
                    marketerproduct_id = marketerProductId,
                    consumeroffer_status_id = ConsumerOfferStatusId,
                    consumeroffer_date = ConsumerOfferDate,
                    consumeroffer_qty = ConsumerOfferQty,
                    consumeroffer_max_price = ConsumerOfferMaxPrice,
                    consumeroffer_current = IsConsumerOfferCurrent,
                    consumeroffer_end_date = OfferValidUpto,
                    consumeroffer_create = DateTime.Now
                });
                azureDB.SaveChanges();
            }
            else
            {
                var consumerOfferEntity = (from so in azureDB.ConsumerOfferEntities
                                where so.consumeroffer_id == consumerOffer.ID
                                select so).FirstOrDefault();

                if (consumerOfferEntity != null)
                {
                    consumerOfferEntity.consumer_id = consumerId;
                    consumerOfferEntity.marketerproduct_id = marketerProductId;
                    consumerOfferEntity.consumeroffer_status_id = ConsumerOfferStatusId;
                    consumerOfferEntity.consumeroffer_date = ConsumerOfferDate;
                    consumerOfferEntity.consumeroffer_qty = ConsumerOfferQty;
                    consumerOfferEntity.consumeroffer_max_price = ConsumerOfferMaxPrice;
                    consumerOfferEntity.consumeroffer_current = IsConsumerOfferCurrent;
                    consumerOfferEntity.consumeroffer_end_date = OfferValidUpto;
                    consumerOfferEntity.consumeroffer_create = DateTime.Now;
                    azureDB.SaveChanges();
                }
            }
            azureDB.SaveChanges();

            consumerOffer = GetConsumerOffer(ConsumerHandle, MarketerName, ProductSKU, ConsumerOfferMaxPrice, OfferValidUpto);
            return consumerOffer == null ? -1: consumerOffer.ID;
        }

        public bool RemoveConsumerOffer(string ConsumerHandle, string MarketerName, string ProductSKU, decimal ConsumerOfferMaxPrice, DateTime OfferValidUpto)
        {
            var consumerOffer = GetConsumerOffer(ConsumerHandle, MarketerName, ProductSKU, ConsumerOfferMaxPrice, OfferValidUpto);

            if (consumerOffer != null)
            {
                var consumerOfferEntity = (from entity in azureDB.ConsumerOfferEntities
                                           where entity.consumeroffer_id == consumerOffer.ID
                                           select entity).FirstOrDefault();

                if (consumerOfferEntity != null)
                {
                    azureDB.ConsumerOfferEntities.Remove(consumerOfferEntity);
                    azureDB.SaveChanges();
                    return true;
                }
            }
            else
                throw new Exception(string.Format("No Offer exists for the Consumer {0} and Product {1} in urshopper.", ConsumerHandle, ProductSKU));
            return false;
        }

        public ConsumerOffer GetConsumerOffer(string ConsumerHandle, string MarketerName, string ProductSKU, decimal ConsumerOfferMaxPrice, DateTime OfferValidUpto)
        {
            long productId = productSvc.GetProductId(ProductSKU);
            long consumerId = consumerSvc.GetConsumerId(ConsumerHandle);
            long marketerId = marketerSvc.GetMarketerId(MarketerName);
            long marketerProductId = marketerSvc.GetMarketerProductId(marketerId, productId);
            return GetConsumerOfferById(marketerProductId, consumerId);
        }

        private ConsumerOffer GetConsumerOfferById(long MarketerProductId, long ConsumerId)
        {
            var consumerOfferEntity = (from sp in azureDB.ConsumerOfferEntities
                            where sp.marketerproduct_id == MarketerProductId
                            && sp.consumer_id == ConsumerId
                            select sp).FirstOrDefault();

            return TranslateEntityToObject(consumerOfferEntity);
        }

        ConsumerOffer TranslateEntityToObject(ConsumerOfferEntity consumerOfferEntity)
        {
            if (consumerOfferEntity == null)
                return null;

            var consumerOffers = new ConsumerOffer()
            {
                Offers = new List<Offer>()
                {
                    new Offer()
                    {
                        OfferId = consumerOfferEntity.consumeroffer_id,
                        // MarketerProductId = consumerOfferEntity.marketerproduct_id
                        OfferedProduct = productSvc.GetMarketerProductById(consumerOfferEntity.marketerproduct_id),
                        MarketerProductId = consumerOfferEntity.marketerproduct_id,
                        MaxPrice= consumerOfferEntity.consumeroffer_max_price,
                        Quantity = consumerOfferEntity.consumeroffer_qty,
                        OfferDate = consumerOfferEntity.consumeroffer_date,
                        EndDate = consumerOfferEntity.consumeroffer_end_date,
                        StatusId = consumerOfferEntity.consumeroffer_status_id,
                        IsCurrentOffer = consumerOfferEntity.consumeroffer_current,
                        CreatedOn = consumerOfferEntity.consumeroffer_create,
                        LastUpdatedOn = consumerOfferEntity.consumeroffer_update ?? DateTime.Now
                    }
                },
                OfferedConsumer = consumerSvc.TranslateEntityToObject(consumerOfferEntity.Consumer),
                ConsumerId = consumerOfferEntity.consumer_id,
                MarketerId = consumerOfferEntity.Consumer.marketer_id
            };
            return consumerOffers;
        }

        private bool IsConsumerOfferExists(long MarketerProductId, decimal ConsumerOfferMaxPrice, DateTime OfferValidUpto)
        {
            return (from sp in azureDB.ConsumerOfferEntities
                    where sp.marketerproduct_id == MarketerProductId
                        && sp.consumeroffer_max_price == ConsumerOfferMaxPrice
                        && sp.consumeroffer_end_date == OfferValidUpto
                    select sp.consumeroffer_id).Any();
        }

        public List<ConsumerOffer> GetConsumerOffers(string MarketerName, string ConsumerHandle)
        {
            var consumerOfferEntities = (from sp in azureDB.ConsumerOfferEntities
                                         where sp.Consumer.Marketer.marketer_name.Equals(MarketerName, StringComparison.InvariantCultureIgnoreCase)
                                            && sp.Consumer.consumer_handle.Equals(ConsumerHandle, StringComparison.InvariantCultureIgnoreCase)
                                         select sp);
            var consumerOffersList = new List<ConsumerOffer>();
            foreach (var coe in consumerOfferEntities)
            {
                consumerOffersList.Add(TranslateEntityToObject(coe));
            }
            return consumerOffersList;
        }
    }
}

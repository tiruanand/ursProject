using eFIN.MVP.AzureEF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFIN.MVP.Services.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class SupplierOfferService : ISupplierOfferService
    {
        ursCoreMVPEntities azureDB;
        SupplierService supplierSvc;
        ProductService productSvc;

        public SupplierOfferService(ursCoreMVPEntities AzureEFEntities, SupplierService SupplierSvc, ProductService ProductSvc)
        {
            azureDB = AzureEFEntities;
            supplierSvc = SupplierSvc;
            productSvc = ProductSvc;
        }

        /// <summary>
        /// Persists the Supplier offer based on the supplier and product offer details.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="ProductSKU"></param>
        /// <param name="PriceOffered"></param>
        /// <param name="OfferValidUpto"></param>
        /// <returns></returns>
        public long SaveSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto)
        {
            var supplierOffer = GetSupplierOffer(SupplierName, ProductSKU, PriceOffered, OfferValidUpto);

            if (supplierOffer == null)
            {
                long productId = productSvc.GetProductId(ProductSKU);
                long supplierId = supplierSvc.GetSupplierId(SupplierName);
                if (supplierSvc.IsSupplierProductExists(supplierId, productId))
                {
                    long supplierProductId = supplierSvc.GetSupplierProductId(supplierId, productId);

                    azureDB.SupplierOfferEntities.Add(new SupplierOfferEntity()
                    {
                        supplierproduct_id = supplierProductId,
                        supplieroffer_incl_price = PriceOffered,
                        supplieroffer_end_date = OfferValidUpto,
                        supplieroffer_create = DateTime.Now,
                        supplieroffer_timestamp = DateTime.Now
                    });
                    azureDB.SaveChanges();
                }
                else
                    throw new Exception(string.Format("Supplier: {0} does not have Product: {1} in urshopper.", SupplierName, ProductSKU));
            }
            else
            {
                var soEntity = (from so in azureDB.SupplierOfferEntities
                                where so.supplieroffer_id == supplierOffer.ID
                                select so).FirstOrDefault();

                if (soEntity != null)
                {
                    soEntity.supplieroffer_incl_price = PriceOffered;
                    soEntity.supplieroffer_end_date = OfferValidUpto;
                    soEntity.supplieroffer_timestamp = DateTime.Now;
                    azureDB.SaveChanges();
                }
            }
            azureDB.SaveChanges();
            supplierOffer = GetSupplierOffer(SupplierName, ProductSKU, PriceOffered, OfferValidUpto);
            return (supplierOffer == null) ? -1 : supplierOffer.ID;
        }

        /// <summary>
        /// Removes the supplier offer based on the supplier and product offer details.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="ProductSKU"></param>
        /// <param name="PriceOffered"></param>
        /// <param name="OfferValidUpto"></param>
        /// <returns></returns>
        public bool RemoveSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto)
        {
            var supplierOffer = GetSupplierOffer(SupplierName, ProductSKU, PriceOffered, OfferValidUpto);

            if (supplierOffer != null)
            {
                var soEntity = (from so in azureDB.SupplierOfferEntities
                                where so.supplieroffer_id == supplierOffer.ID
                                select so).FirstOrDefault();

                if (soEntity != null)
                {
                    azureDB.SupplierOfferEntities.Remove(soEntity);
                    azureDB.SaveChanges();
                    return true;
                }
            }
            else
                throw new Exception(string.Format("No Offer found for the Supplier: {0} and Product: {1} in urshopper.", SupplierName, ProductSKU));
            return false;
        }

        /// <summary>
        /// Retrieves the Supplier offers for the given product offer details.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="ProductSKU"></param>
        /// <param name="PriceOffered"></param>
        /// <param name="OfferValidUpto"></param>
        /// <returns></returns>
        public SupplierOffer GetSupplierOffer(string SupplierName, string ProductSKU, decimal PriceOffered, DateTime OfferValidUpto)
        {
            long productId = productSvc.GetProductId(ProductSKU);
            long supplierId = supplierSvc.GetSupplierId(SupplierName);
            if (supplierSvc.IsSupplierProductExists(supplierId, productId))
            {
                return GetSupplierOfferById(supplierSvc.GetSupplierProductId(supplierId, productId));
            }
            return null;
        }

        /// <summary>
        /// Retrieves the supplier offer based on the given supplier product id.
        /// </summary>
        /// <param name="SupplierProductId"></param>
        /// <returns></returns>
        private SupplierOffer GetSupplierOfferById(long SupplierProductId)
        {
            var soEntity = (from sp in azureDB.SupplierOfferEntities
                            where sp.supplierproduct_id == SupplierProductId
                            select sp).FirstOrDefault();

            return TranslateEntityToObject(soEntity);
        }

        /// <summary>
        /// Converts Supplier Offer Entity to Supplier Offer object.
        /// </summary>
        /// <param name="supplierOfferEntity"></param>
        /// <returns></returns>
        SupplierOffer TranslateEntityToObject(SupplierOfferEntity supplierOfferEntity)
        {
            if (supplierOfferEntity == null)
                return null;

            var supplierOffers = new SupplierOffer()
            {
                Offers = new List<Offer>()
                {
                    new Offer()
                    {
                        OfferId = supplierOfferEntity.supplieroffer_id,
                        MaxPrice= supplierOfferEntity.supplieroffer_incl_price,
                        EndDate = supplierOfferEntity.supplieroffer_end_date,
                        CreatedOn = supplierOfferEntity.supplieroffer_create,
                        LastUpdatedOn = supplierOfferEntity.supplieroffer_timestamp ?? DateTime.Now,
                        OfferedProduct = productSvc.TranslateEntityToObject(supplierOfferEntity.SupplierProduct.Product)
                    }
                },
                OfferedSupplier = supplierSvc.TranslateEntityToObject(supplierOfferEntity.SupplierProduct.Supplier),
                SupplierId = supplierOfferEntity.SupplierProduct.supplier_id
            };
            return supplierOffers;
        }
        
        /// <summary>
        /// Verifies whether any offer exists for the given supplier and offer.
        /// </summary>
        /// <param name="SupplierProductId"></param>
        /// <param name="PriceOffered"></param>
        /// <param name="OfferValidUpto"></param>
        /// <returns></returns>
        private bool IsSupplierOfferExists(long SupplierProductId, decimal PriceOffered, DateTime OfferValidUpto)
        {
            return (from sp in azureDB.SupplierOfferEntities
                    where sp.supplierproduct_id == SupplierProductId
                        && sp.supplieroffer_incl_price == PriceOffered
                        && sp.supplieroffer_timestamp == OfferValidUpto
                    select sp.supplieroffer_id).Any();
        }

        /// <summary>
        /// Retrieves all offers from the given supplier.
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <returns></returns>
        public List<SupplierOffer> GetSupplierOffers(string SupplierName)
        {
            var supplierOfferEntities = (from sp in azureDB.SupplierOfferEntities
                                         where sp.SupplierProduct.Supplier.supplier_name.Equals(SupplierName, StringComparison.InvariantCultureIgnoreCase)
                                         select sp);

            var supplierOffersList = new List<SupplierOffer>();

            foreach (var soe in supplierOfferEntities)
            {
                supplierOffersList.Add(TranslateEntityToObject(soe));
            }
            return supplierOffersList;
        }
    }
}

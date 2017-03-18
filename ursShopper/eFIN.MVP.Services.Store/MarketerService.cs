using eFIN.MVP.AzureEF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFIN.MVP.Services.Store
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IMarketerService" in both code and config file together.
    public class MarketerService : IMarketerService
    {
        #region Member Variables
        ursCoreMVPEntities azureDB;
        ProductService prodSvc;
        #endregion

        #region Constructor
        /// <summary>
        /// Service that implements IMarketerService to maintain for Marketer related actions.
        /// </summary>
        public MarketerService(ursCoreMVPEntities AzureEFEntities, ProductService ProductSvc)
        {
            azureDB = AzureEFEntities;
            prodSvc = ProductSvc;
        }

        public MarketerService()
        {
            azureDB = new ursCoreMVPEntities();
            prodSvc = new ProductService();
        }
        #endregion

        #region Marketer service methods
        /// <summary>
        /// Retrieve Marketer for the given Marketer Id.
        /// </summary>
        /// <param name="MarketerId"></param>
        /// <returns></returns>
        public Marketer GetMarketer(int MarketerId)
        {
            var EFMarketer = (from p
                            in azureDB.MarketerEntities
                              where p.marketer_id == MarketerId
                              select p).FirstOrDefault();
            if (EFMarketer != null)
                return TranslateEntityToObject(EFMarketer);
            else
                throw new Exception("Invalid marketer");
        }

        /// <summary>
        /// Retrieve marketer unique id for the given marketer name
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <returns></returns>
        public long GetMarketerId(string MarketerName)
        {
            var EFMarketer = (from p
                            in azureDB.MarketerEntities
                              where p.marketer_name.Equals(MarketerName, StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFMarketer != null)
                return EFMarketer.marketer_id;
            else
                throw new Exception("Invalid marketer");
        }

        /// <summary>
        /// Retrieves all available marketers.
        /// </summary>
        /// <returns></returns>
        public List<Marketer> GetAllMarketers()
        {
            var allMarketers = (from p
                            in azureDB.MarketerEntities
                                select p).ToList();

            var targetList = allMarketers.Select(x => new Marketer()
            {
                Id = x.marketer_id,
                Name = x.marketer_name,
                Type = x.marketer_type,
                Domain = x.marketer_domain
                                        ,
                CreatedOn = x.marketer_create,
                LastUpdatedOn = x.marketer_update ?? DateTime.Now
            }).ToList();
            return targetList;
        }

        public List<Marketer> GetAllMarketersList()
        {
            var allMarketers = (from p
                            in azureDB.MarketerEntities
                                select p).ToList();

            var targetList = allMarketers.Select(x => new Marketer()
            {
                Id = x.marketer_id,
                Name = x.marketer_name
            }).ToList();
            return targetList;
        }


        /// <summary>
        /// Creates or updates marketer based on given marketer details.
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="MarketerDomain"></param>
        /// <param name="MarketerType"></param>
        /// <returns></returns>
        public long SaveMarketer(string MarketerName, string MarketerDomain, string MarketerType)
        {
            var EFMarketer = (from p
                            in azureDB.MarketerEntities
                              where p.marketer_name.Equals(MarketerName, StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFMarketer == null)
            {
                var marketerEntity = new MarketerEntity() { marketer_name = MarketerName, marketer_type = MarketerType, marketer_domain = MarketerDomain, marketer_create = DateTime.Now, marketer_update = DateTime.Now };
                azureDB.MarketerEntities.Add(marketerEntity);
            }
            else
            {
                EFMarketer.marketer_type = MarketerType;
                EFMarketer.marketer_domain = MarketerDomain;
            }
            azureDB.SaveChanges();
            return GetMarketerId(MarketerName);
        }

        /// <summary>
        /// Removes the marketer for the given marketer name.
        /// </summary>
        /// <param name="Name"></param>
        public void DeleteMarketer(string MarketerName)
        {


            var EFMarketer = (from p
                            in azureDB.MarketerEntities
                              where p.marketer_name.Equals(MarketerName, StringComparison.InvariantCultureIgnoreCase)
                              select p).FirstOrDefault();
            if (EFMarketer != null)
            {
                azureDB.MarketerEntities.Remove(EFMarketer);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("{0} - Marketer does not exists.", MarketerName));
        }

        /// <summary>
        /// Removes the marketer for the given marketer unique id.
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteMarketerById(int MarketerId)
        {
            var EFMarketer = (from p
                            in azureDB.MarketerEntities
                              where p.marketer_id == MarketerId
                              select p).FirstOrDefault();
            if (EFMarketer != null)
            {
                azureDB.MarketerEntities.Remove(EFMarketer);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("{0} - Marketer does not exists.", MarketerId));
        }

        /// <summary>
        /// Translate Marketer entity framework object to realtime marketer object.
        /// </summary>
        /// <param name="marketer"></param>
        /// <returns></returns>
        public Marketer TranslateEntityToObject(MarketerEntity marketer)
        {
            Marketer objMarketer = new Marketer()
            {
                Id = marketer.marketer_id,
                Name = marketer.marketer_name,
                Domain = marketer.marketer_domain,
                Type = marketer.marketer_type,
                CreatedOn = marketer.marketer_create,
                LastUpdatedOn = marketer.marketer_update ?? DateTime.Now
            };
            return objMarketer;
        }
        #endregion

        #region MarketerProduct service methods
        /// <summary>
        /// Retrieves marketer-product based on given marketer unique id and product unique id
        /// </summary>
        /// <param name="MarketerId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        private bool IsMarketerProductExists(long MarketerId, long ProductId)
        {
            return (from sp in azureDB.MarketerProductEntities
                    where sp.marketer_id == MarketerId && sp.product_id == ProductId
                    select sp.marketerproduct_id).Any();
        }

        /// <summary>
        /// Create or update marketer-product based on given marketer-product details.
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="ProductName"></param>
        /// <param name="ProductDescription"></param>
        /// <param name="ProductManufacturer"></param>
        /// <param name="ProductPrice"></param>
        /// <param name="ProductSKU"></param>
        /// <param name="ProductUPC"></param>
        /// <param name="ProductEAN"></param>
        /// <param name="ProductGTIN"></param>
        /// <param name="ProductUOM"></param>
        public void SaveMarketerProduct(string MarketerName, string ProductName, string ProductDescription, string ProductManufacturer, decimal ProductPrice, string ProductSKU, string ProductUPC, string ProductEAN, string ProductGTIN, string ProductUOM)
        {
            long productId = prodSvc.SaveProduct(ProductName, ProductDescription, ProductManufacturer, ProductPrice, ProductSKU, ProductUPC, ProductEAN, ProductGTIN, ProductUOM);
            long marketerId = this.GetMarketerId(MarketerName);

            if (!IsMarketerProductExists(marketerId, productId))
                azureDB.MarketerProductEntities.Add(new MarketerProductEntity() { product_id = productId, marketer_id = marketerId, marketerproduct_create = DateTime.Now, marketerproduct_update = DateTime.Now });
        }

        /// <summary>
        /// Associates the given product to the given marketer.
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="ProductName"></param>
        public void AddProductToMarketer(string MarketerName, string ProductName)
        {
            long productId = prodSvc.GetProductId(ProductName);
            long marketerId = this.GetMarketerId(MarketerName);

            if (!IsMarketerProductExists(marketerId, productId))
            {
                azureDB.MarketerProductEntities.Add(new MarketerProductEntity() { product_id = productId, marketer_id = marketerId, marketerproduct_create = DateTime.Now, marketerproduct_update = DateTime.Now });
                azureDB.SaveChanges();
            }
        }

        /// <summary>
        /// Disassociate given product from the given marketer.
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="ProductName"></param>
        public void RemoveProductFromMarketer(string MarketerName, string ProductName)
        {
            long productId = prodSvc.GetProductId(ProductName);
            long marketerId = this.GetMarketerId(MarketerName);

            if (!IsMarketerProductExists(marketerId, productId))
            {
                azureDB.MarketerProductEntities.Remove(new MarketerProductEntity() { product_id = productId, marketer_id = marketerId, marketerproduct_create = DateTime.Now, marketerproduct_update = DateTime.Now });
                azureDB.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves avaiable marketer-products for the given marketer and product.
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <param name="ProductName"></param>
        /// <returns></returns>
        public MarketerProducts GetMarketerProduct(string MarketerName, string ProductName)
        {
            var marketerProducts = (from sp in azureDB.MarketerProductEntities
                                    where sp.Marketer.marketer_name.Equals(MarketerName, StringComparison.InvariantCultureIgnoreCase)
                                    select sp);

            var prodMarketers = new MarketerProducts();
            if (marketerProducts != null && marketerProducts.Count() > 0)
            {
                prodMarketers.Marketer = marketerProducts.Select(sp => new Marketer()
                {
                    Name = sp.Marketer.marketer_name,
                    Domain = sp.Marketer.marketer_domain,
                    Type = sp.Marketer.marketer_type
                }).FirstOrDefault();

                var supProduct = (from p in marketerProducts
                                  where p.Product.product_name.Equals(ProductName, StringComparison.InvariantCultureIgnoreCase)
                                  select p).FirstOrDefault();

                prodMarketers.Products = new List<RelatedProduct>()
                { new RelatedProduct()
                        {
                            Id = supProduct.marketerproduct_id,
                            Product = new Product()
                            {
                                ID = supProduct.product_id,
                                Name = supProduct.Product.product_name,
                                Manufacturer = supProduct.Product.product_manufacturer,
                                Description = supProduct.Product.product_description,
                                CreatedOn = supProduct.Product.product_create,
                                LastUpdatedOn = supProduct.Product.product_update ?? DateTime.Now
                            },
                            CreatedOn = supProduct.marketerproduct_create ?? DateTime.Now,
                            LastUpdatedOn = supProduct.marketerproduct_update ?? DateTime.Now
                        }
                };
            }
            return prodMarketers;
        }

        /// <summary>
        /// Retrieves marketer-product combination unique id from the given marketer unique id and product unique id.
        /// </summary>
        /// <param name="MarketerId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public long GetMarketerProductId(long MarketerId, long ProductId)
        {
            return (from sp in azureDB.MarketerProductEntities
                    where sp.marketer_id == MarketerId && sp.product_id == ProductId
                    select sp.marketerproduct_id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves marketer-products for the given marketer name.
        /// </summary>
        /// <param name="MarketerName"></param>
        /// <returns></returns>
        public MarketerProducts GetMarketerProducts(string MarketerName)
        {
            var marketerProducts = (from sp in azureDB.MarketerProductEntities
                                    where sp.Marketer.marketer_name.Equals(MarketerName, StringComparison.InvariantCultureIgnoreCase)
                                    select sp);

            var prodMarketers = new MarketerProducts();
            if (marketerProducts != null && marketerProducts.Count() > 0)
            {
                prodMarketers.Marketer = marketerProducts.Select(sp => new Marketer()
                {
                    Name = sp.Marketer.marketer_name,
                    Domain = sp.Marketer.marketer_domain,
                    Type = sp.Marketer.marketer_type
                }).FirstOrDefault();

                prodMarketers.Products = marketerProducts.Select(sp => new RelatedProduct()
                {
                    Id = sp.marketerproduct_id,
                    Product = new Product()
                    {
                        ID = sp.product_id,
                        Name = sp.Product.product_name,
                        SKU = sp.Product.product_sku,
                        Price = sp.Product.product_price,
                        Manufacturer = sp.Product.product_manufacturer,
                        Description = sp.Product.product_description,
                        CreatedOn = sp.Product.product_create,
                        LastUpdatedOn = sp.Product.product_update ?? DateTime.Now
                    },
                    CreatedOn = sp.marketerproduct_create ?? DateTime.Now,
                    LastUpdatedOn = sp.marketerproduct_update ?? DateTime.Now
                }).ToList();
            }
            return prodMarketers;
        }

        /// <summary>
        /// Retrieves product marketers for the given product name.
        /// </summary>
        /// <param name="ProductName"></param>
        /// <returns></returns>
        public ProductMarketers GetProductMarketers(string ProductName)
        {
            var marketerProducts = (from sp in azureDB.MarketerProductEntities
                                    where sp.Product.product_name.Equals(ProductName, StringComparison.InvariantCultureIgnoreCase)
                                    select sp);

            var prodMarketers = new ProductMarketers();

            if (marketerProducts != null && marketerProducts.Count() > 0)
            {
                prodMarketers.Product = marketerProducts.Select(sp => new Product()
                {
                    ID = sp.product_id,
                    Name = sp.Product.product_name,
                    Manufacturer = sp.Product.product_manufacturer,
                    Description = sp.Product.product_description,
                    CreatedOn = sp.Product.product_create,
                    LastUpdatedOn = sp.Product.product_update ?? DateTime.Now
                }).FirstOrDefault();

                prodMarketers.Marketers = marketerProducts.Select(sp => new RelatedMarketer()
                {
                    Id = sp.marketerproduct_id,
                    Marketer = new Marketer()
                    {
                        Name = sp.Marketer.marketer_name,
                        Domain = sp.Marketer.marketer_domain,
                        Type = sp.Marketer.marketer_type
                    },
                    CreatedOn = sp.marketerproduct_create ?? DateTime.Now,
                    LastUpdatedOn = sp.marketerproduct_update ?? DateTime.Now
                }).ToList();
            }
            return prodMarketers;
        }
        #endregion
    }
}

using eFIN.MVP.AzureEF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFIN.MVP.Services.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class AddressService : IAddressService
    {
        ursCoreMVPEntities azureDB;

        public AddressService(ursCoreMVPEntities AzureEFEntities)
        {
            azureDB = AzureEFEntities;
        }

        public Address GetAddress(int AddressId)
        {
            var EFAddress = (from p
                            in azureDB.AddressEntities
                              where p.address_id == AddressId
                              select p).FirstOrDefault();
            if (EFAddress != null)
                return TranslateEntityToObject(EFAddress);
            else
                throw new Exception("Invalid address");
        }

        public List<Address> GetAllAddresss()
        {
            var allAddresss = (from p
                            in azureDB.AddressEntities
                               select p).ToList();

            var targetList = allAddresss.Select(x => new Address()
            {
                AddressId = x.address_id,
                Line1 = x.line_1,
                Line2 = x.line_2,
                Line3 = x.line_3,
                City = x.city,
                ZipPostalCode = x.zip_postal_code,
                StateProvinceCounty = x.state_province_county,
                Country = x.country,
                CreatedOn = x.address_create,
                LastUpdatedOn = x.address_update ?? DateTime.Now
            }).ToList();
            return targetList;
        }

        public Address GetAddress(string Line1, string City, string ZipPostalCode, string StateProvinceCounty, string Country)
        {
            var efAddress = (from p
                            in azureDB.AddressEntities
                             where p.line_1.Equals(Line1, StringComparison.InvariantCultureIgnoreCase)
                             && p.city.Equals(City, StringComparison.InvariantCultureIgnoreCase)
                             && p.zip_postal_code.Equals(ZipPostalCode, StringComparison.InvariantCultureIgnoreCase)
                             && p.state_province_county.Equals(StateProvinceCounty, StringComparison.InvariantCultureIgnoreCase)
                             && p.country.Equals(Country, StringComparison.InvariantCultureIgnoreCase)
                             select p).FirstOrDefault();

            return TranslateEntityToObject(efAddress);
        }

        public long GetAddressId(string Line1, string City, string ZipPostalCode, string StateProvinceCounty, string Country)
        {
            return (from p
                            in azureDB.AddressEntities
                             where p.line_1.Equals(Line1, StringComparison.InvariantCultureIgnoreCase)
                             && p.city.Equals(City, StringComparison.InvariantCultureIgnoreCase)
                             && p.zip_postal_code.Equals(ZipPostalCode, StringComparison.InvariantCultureIgnoreCase)
                             && p.state_province_county.Equals(StateProvinceCounty, StringComparison.InvariantCultureIgnoreCase)
                             && p.country.Equals(Country, StringComparison.InvariantCultureIgnoreCase)
                             select p.address_id ).FirstOrDefault();
        }

        public long SaveAddress(string Line1, string Line2, string Line3, string City, string ZipPostalCode, string StateProvinceCounty, string Country)
        {
            var EFAddress = (from p
                            in azureDB.AddressEntities
                             where p.line_1.Equals(Line1, StringComparison.InvariantCultureIgnoreCase)
                             && p.city.Equals(City, StringComparison.InvariantCultureIgnoreCase)
                             && p.zip_postal_code.Equals(ZipPostalCode, StringComparison.InvariantCultureIgnoreCase)
                             && p.state_province_county.Equals(StateProvinceCounty, StringComparison.InvariantCultureIgnoreCase)
                             && p.country.Equals(Country, StringComparison.InvariantCultureIgnoreCase)
                             select p).FirstOrDefault();
            if (EFAddress == null)
            {
                var addressEntity = new AddressEntity()
                {
                    line_1 = Line1,
                    line_2 = Line2,
                    line_3 = Line3,
                    city = City,
                    zip_postal_code = ZipPostalCode,
                    state_province_county = StateProvinceCounty,
                    country = Country,
                    address_create = DateTime.Now,
                    address_update = DateTime.Now
                };
                azureDB.AddressEntities.Add(addressEntity);
            }
            else
            {
                EFAddress.line_1 = Line1;
                EFAddress.line_2 = Line2;
                EFAddress.line_3 = Line3;
                EFAddress.city = City;
                EFAddress.zip_postal_code = ZipPostalCode;
                EFAddress.state_province_county = StateProvinceCounty;
                EFAddress.country = Country;
                EFAddress.address_update = DateTime.Now;
            }
            azureDB.SaveChanges();

            return GetAddressId(Line1, City, ZipPostalCode, StateProvinceCounty, Country);
        }

        public void RemoveAddress(string Line1, string Line2, string Line3, string City, string ZipPostalCode, string StateProvinceCounty, string Country)
        {
            var EFAddress = (from p
                            in azureDB.AddressEntities
                             where p.line_1.Equals(Line1, StringComparison.InvariantCultureIgnoreCase)
                             && p.city.Equals(City, StringComparison.InvariantCultureIgnoreCase)
                             && p.zip_postal_code.Equals(ZipPostalCode, StringComparison.InvariantCultureIgnoreCase)
                             && p.state_province_county.Equals(StateProvinceCounty, StringComparison.InvariantCultureIgnoreCase)
                             && p.country.Equals(Country, StringComparison.InvariantCultureIgnoreCase)
                             select p).FirstOrDefault();
            if (EFAddress != null)
            {
                azureDB.AddressEntities.Remove(EFAddress);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("{0} {1} {2} {3} {4} - Address does not exists.", Line1, City, ZipPostalCode, StateProvinceCounty, Country));
        }

        public void DeleteAddressById(int Id)
        {
            var EFAddress = (from p
                            in azureDB.AddressEntities
                              where p.address_id == Id
                              select p).FirstOrDefault();
            if (EFAddress != null)
            {
                azureDB.AddressEntities.Remove(EFAddress);
                azureDB.SaveChanges();
            }
            else
                throw new Exception(string.Format("{0} - Address does not exists.", Id));
        }

        public Address TranslateEntityToObject(AddressEntity addressObj)
        {
            Address objAddress = new Address()
            {
                AddressId = addressObj.address_id,
                Line1 = addressObj.line_1,
                Line2 = addressObj.line_2,
                Line3 = addressObj.line_3,
                City = addressObj.city,
                ZipPostalCode = addressObj.zip_postal_code,
                StateProvinceCounty = addressObj.state_province_county,
                Country = addressObj.country,
                CreatedOn = addressObj.address_create,
                LastUpdatedOn = addressObj.address_update ?? DateTime.Now
            };
            return objAddress;
        }
    }
}
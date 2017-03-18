//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eFIN.MVP.Views.Models.AzureDataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class SupplierOffer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupplierOffer()
        {
            this.OfferMatches = new HashSet<OfferMatch>();
        }
    
        public long supplieroffer_id { get; set; }
        public long supplierproduct_id { get; set; }
        public decimal supplieroffer_incl_price { get; set; }
        public System.DateTime supplieroffer_end_date { get; set; }
        public System.DateTime supplieroffer_create { get; set; }
        public Nullable<System.DateTime> supplieroffer_timestamp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferMatch> OfferMatches { get; set; }
        public virtual SupplierProduct SupplierProduct { get; set; }
    }
}

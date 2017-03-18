//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eFIN.MVP.Views.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConsumerOffer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsumerOffer()
        {
            this.OfferMatches = new HashSet<OfferMatch>();
        }
    
        public long consumeroffer_id { get; set; }
        public long consumer_id { get; set; }
        public long marketerproduct_id { get; set; }
        public long consumeroffer_status_id { get; set; }
        public System.DateTime consumeroffer_date { get; set; }
        public int consumeroffer_qty { get; set; }
        public decimal consumeroffer_max_price { get; set; }
        public System.DateTime consumeroffer_end_date { get; set; }
        public bool consumeroffer_current { get; set; }
        public System.DateTime consumeroffer_create { get; set; }
        public Nullable<System.DateTime> consumeroffer_update { get; set; }
    
        public virtual Consumer Consumer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferMatch> OfferMatches { get; set; }
    }
}

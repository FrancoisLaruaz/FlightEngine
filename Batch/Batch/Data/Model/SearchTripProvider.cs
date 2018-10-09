//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SearchTripProvider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SearchTripProvider()
        {
            this.Trips = new HashSet<Trip>();
        }
    
        public int Id { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int ProviderId { get; set; }
        public int SearchTripId { get; set; }
        public string Proxy { get; set; }
        public bool SearchSuccess { get; set; }
        public Nullable<System.DateTime> EndSearchDate { get; set; }
        public int AttemptsNumber { get; set; }
        public string Url { get; set; }
    
        public virtual Provider Provider { get; set; }
        public virtual SearchTrip SearchTrip { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trip> Trips { get; set; }
    }
}

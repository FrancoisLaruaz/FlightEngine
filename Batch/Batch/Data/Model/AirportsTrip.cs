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
    
    public partial class AirportsTrip
    {
        public int Id { get; set; }
        public int FromAirportId { get; set; }
        public int ToAirportId { get; set; }
        public int Attractiveness { get; set; }
    
        public virtual Airport Airport { get; set; }
        public virtual Airport Airport1 { get; set; }
    }
}
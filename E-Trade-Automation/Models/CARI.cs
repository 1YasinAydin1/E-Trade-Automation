//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asp.NET_E_Commerce_MVC5_ENTITY_.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CARI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CARI()
        {
            this.SALESMOVEMENT = new HashSet<SALESMOVEMENT>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public string LASTNAME { get; set; }
        public string CITY { get; set; }
        public string MAIL { get; set; }
        public string IMAGE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SALESMOVEMENT> SALESMOVEMENT { get; set; }
    }
}

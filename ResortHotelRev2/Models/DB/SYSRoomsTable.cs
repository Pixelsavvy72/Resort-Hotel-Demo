//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResortHotelRev2.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class SYSRoomsTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SYSRoomsTable()
        {
            this.SYSOccupiedRoomTables = new HashSet<SYSOccupiedRoomTable>();
        }
    
        public int Id { get; set; }
        public string RoomType { get; set; }
        public int Occupancy { get; set; }
        public int NumberQueenBeds { get; set; }
        public int NumberKingBeds { get; set; }
        public string Status { get; set; }
        public bool Smoking { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYSOccupiedRoomTable> SYSOccupiedRoomTables { get; set; }
    }
}

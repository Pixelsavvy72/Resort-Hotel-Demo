using ResortHotelRev2.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResortHotelRev2.Models.ViewModel
{
    public class RoomProfileView 
    {
        [Key]        
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public int Occupancy { get; set; }
        public int QueenBeds { get; set; }
        public int KingBeds { get; set; }
        public string Status { get; set; }
        public bool Smoking { get; set; }
        public string Description { get; set; }

        public string Image { get; set; }

        public bool IsSelected { get; set; }
        [DataType(DataType.Date)]
        //TODO: Get attribute validation working. Doesn't currently like partial view.
        //[CheckInValidation(ErrorMessage = "Check-in date must not have passed.")] Causes fail.
        public DateTime CheckIn { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        public string RoomDescription { get; set; }

    }
        
    public class RoomDataView
    {
        public List<RoomProfileView> RoomProfile { get; set; }
    }

    

}
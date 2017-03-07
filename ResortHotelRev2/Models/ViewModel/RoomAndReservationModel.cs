using ResortHotelRev2.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ResortHotelRev2.Models.ViewModel
{
    public class RoomAndReservationModel
    {
        //Room
        public List<RoomProfileView> RoomResRmProfile { get; set; }


        //Reservation
        [Key]
        public int Id { get; set; }
        public int GuestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
        public MadeByType MadeBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime DatePlaced { get; set; }

    }



}
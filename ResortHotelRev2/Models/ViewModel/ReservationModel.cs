using ResortHotelRev2.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResortHotelRev2.Models.ViewModel
{


    public class ReservationView 
    {
        [Key]
        public int Id { get; set; }
        public int GuestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="Please enter a valid check-in date.")]
        [Display(Name = "Check-in Date:")]
        public DateTime CheckIn { get; set; }
        [Required(ErrorMessage = "Please enter a valid check-out date.")]
        [Display(Name = "Check-out Date:")]
        public DateTime CheckOut { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
        public MadeByType MadeBy { get; set; }


    }

    public enum MadeByType
    {
        Online,
        Phone,
        [Display(Name = "Front Desk")]
        FrontDesk,
        [Display(Name = "Travel Agent")]
        TravelAgent
    }

    public enum ReservationStatus
    {
        Confirmed,
        Canceled,
        [Display(Name = "In Progress")]
        InProgress,
        Completed
    }


}
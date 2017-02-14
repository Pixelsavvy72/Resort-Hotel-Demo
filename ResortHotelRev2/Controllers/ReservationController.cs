using ResortHotelRev2.Models.EntityManager;
using ResortHotelRev2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ResortHotelRev2.Controllers
{
    public class ReservationController : Controller
    {
        
        // GET: Reservation
        [Authorize]
        public ActionResult Reservation()
        {

            return View();
        }
        

        // Takes in a starting date and end date from JS Calendar in Reservation.cshtml and returns
        // partial view to Reservation.cshtml showing rooms available for those dates.
        [Authorize]
        [HttpGet]
        public ActionResult SelectRoomPartial(string startDate, string endDate)
        {
            if (ModelState.IsValid)
            {



                DateTime startDateConverted = DateTime.ParseExact(startDate, "MM/dd/yyyy", new CultureInfo("en-US"));
                DateTime endDateConverted = DateTime.ParseExact(endDate, "MM/dd/yyyy", new CultureInfo("en-US"));

                RoomManager RoomManager = new RoomManager();
                RoomDataView RoomDataView = RoomManager.GetRoomProfileView(startDateConverted, endDateConverted);

                //Passes check in/out dates to model for view to send back to VerifyReservationInfo below

                foreach (var room in RoomDataView.RoomProfile)
                {
                    room.CheckIn = startDateConverted;
                    room.CheckOut = endDateConverted;
                }

                ViewBag.startDateToPartial = startDateConverted.ToShortDateString();
                ViewBag.endDateToPartial = endDateConverted.ToShortDateString();

                return PartialView(RoomDataView);
            }
            
            return View();

        }

        //Send selected rooms information (from partial view) and user profile information to VerifyReservationInfo view.
        [HttpPost]
        [Authorize]
        public ActionResult VerifyReservationInfo(RoomDataView model)
        {
            if (ModelState.IsValid)
            {


                string loginName = User.Identity.Name;
                UserManager userManager = new UserManager();
                UserProfileView userProfileView = userManager.GetUserProfile(userManager.GetUserID(loginName));

                RoomAndReservationModel RoomResModel = new RoomAndReservationModel();
                List<RoomProfileView> RoomsSelectedList = new List<RoomProfileView>();

                GetSelectedRooms(model, RoomsSelectedList);

                //Check user selected at least one room.
                if (RoomsSelectedList.Count == 0)
                {
                    return RedirectToAction("Reservation", "Reservation");
                }

                RoomResModel.RoomResRmProfile = RoomsSelectedList;
                RoomResModel.GuestId = userProfileView.SYSUserID;
                RoomResModel.FirstName = userProfileView.FirstName;
                RoomResModel.LastName = userProfileView.LastName;
                RoomResModel.PhoneNumber = userProfileView.PhoneNumber;
                //TODO: All rooms on one reservation currently must have same dates. Think about adjusting this in future.
                RoomResModel.CheckIn = RoomsSelectedList[0].CheckIn;
                RoomResModel.CheckOut = RoomsSelectedList[0].CheckOut;

                return View(RoomResModel);
            }
            
            return View();

        }

        [HttpPost]
        [Authorize]
        public ActionResult PlaceReservation(RoomAndReservationModel RoomResModel)
        {
            if (ModelState.IsValid)
            {
                ReservationManager reservationManager = new ReservationManager();

                reservationManager.AddReservation(RoomResModel);
            }
            
            return View();
        }

        //Gets a list of selected rooms used in ActionResult VerifyReservationInfo.
        private static void GetSelectedRooms(RoomDataView model, List<RoomProfileView> RoomsSelectedList)
        {

            foreach (var item in model.RoomProfile)
            {
                if (item.IsSelected == true)
                {

                    RoomsSelectedList.Add(item);
                }

            }
        }
    }
}
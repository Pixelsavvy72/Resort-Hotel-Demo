using ResortHotelRev2.Models.EntityManager;
using ResortHotelRev2.Models.ViewModel;
using ResortHotelRev2.Security;
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

        public ActionResult Reservation(int id = 0)
        {
            TempData["AdminControlUserId"] = id;
            return View();
        }
        

        // Takes in a starting date and end date from JS Calendar in Reservation.cshtml and returns
        // partial view to Reservation.cshtml showing rooms available for those dates.
        
        [HttpGet]
        public ActionResult SelectRoomPartial(string startDate, string endDate)
        {
            if (ModelState.IsValid)
            {



                DateTime startDateConverted = DateTime.ParseExact(startDate, "MM/dd/yyyy", new CultureInfo("en-US"));
                DateTime endDateConverted = DateTime.ParseExact(endDate, "MM/dd/yyyy", new CultureInfo("en-US"));

                RoomManager roomManager = new RoomManager();
                RoomDataView roomDataView = roomManager.GetRoomProfileView(startDateConverted, endDateConverted);

                //Passes check in/out dates to model for view to send back to VerifyReservationInfo below

                foreach (var room in roomDataView.RoomProfile)
                {
                    room.CheckIn = startDateConverted;
                    room.CheckOut = endDateConverted;
                }

                ViewBag.startDateToPartial = startDateConverted.ToShortDateString();
                ViewBag.endDateToPartial = endDateConverted.ToShortDateString();

                return PartialView(roomDataView);
            }
            
            return View();

        }

        //If user is not logged in when attempting to place a reservation, AccountController/Login will login the user and call
        //this method, which collects the model from tempData and passes it to the HttpPost.

        [HttpGet]
        public ActionResult VerifyReservationInfo()
        {
            RoomDataView tempData = (RoomDataView) TempData["roomDataView"];
            return VerifyReservationInfo(tempData);
        }


        //Send selected rooms information (from partial view) and user profile information to VerifyReservationInfo view.

        [HttpPost]        
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

                if (userManager.IsUserInRole(loginName, "admin"))
                {
                    
                    int id = (int) TempData["AdminControlUserId"];
                    UserProfileView adminSuppliedUser = userManager.GetUserProfile(id);
                    RoomResModel.RoomResRmProfile = RoomsSelectedList;
                    RoomResModel.GuestId = adminSuppliedUser.SYSUserID;
                    RoomResModel.FirstName = adminSuppliedUser.FirstName;
                    RoomResModel.LastName = adminSuppliedUser.LastName;
                    RoomResModel.PhoneNumber = adminSuppliedUser.PhoneNumber;
                    //TODO: All rooms on one reservation currently must have same dates. Think about adjusting this in future.
                    RoomResModel.CheckIn = RoomsSelectedList[0].CheckIn;
                    RoomResModel.CheckOut = RoomsSelectedList[0].CheckOut;
                }
                else
                {
                    RoomResModel.RoomResRmProfile = RoomsSelectedList;
                    RoomResModel.GuestId = userProfileView.SYSUserID;
                    RoomResModel.FirstName = userProfileView.FirstName;
                    RoomResModel.LastName = userProfileView.LastName;
                    RoomResModel.PhoneNumber = userProfileView.PhoneNumber;
                    //TODO: All rooms on one reservation currently must have same dates. Think about adjusting this in future.
                    RoomResModel.CheckIn = RoomsSelectedList[0].CheckIn;
                    RoomResModel.CheckOut = RoomsSelectedList[0].CheckOut;

                    
                }


                if (User.Identity.IsAuthenticated)
                {
                    TempData.Remove("roomDataView");
                    return View(RoomResModel);
                }
                else
                {

                    TempData["roomDataView"] = model;

                    return View("MustLogin");
                }

                
            }
            
            return View();

        }

        [HttpPost]
        [Authorize]
        public ActionResult PlaceReservation(RoomAndReservationModel RoomResModel)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                UserManager userManager = new UserManager();
                ReservationManager reservationManager = new ReservationManager();

                reservationManager.AddReservation(RoomResModel, userManager.GetUserID(userName));
            }

            return View();
        }

        [Authorize]
        public ActionResult ViewMyReservations()
        {
            ReservationManager reservationManager = new ReservationManager();
            UserManager userManager = new UserManager();
            var loginName = User.Identity.Name;
            var userId = userManager.GetUserID(loginName);
            List<RoomAndReservationModel> myReservations = reservationManager.GetMyReservations(userId);


            return View(myReservations);
        }

        [Authorize]
        public ActionResult FindReservationById(int id)
        {
            ReservationManager reservationManager = new ReservationManager();
            RoomAndReservationModel selectedReservation = reservationManager.FindReservationById(id);

            return View(selectedReservation);


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

        //CANCEL RESERVATION
        //[AuthorizeRoles("Admin,Member")]
        public ActionResult CancelReservation(int resID, string status)
        {
            ReservationManager resManager = new ReservationManager();
            resManager.CancelReservation(resID);

            return Json(new { success = true });


        }

    }
}
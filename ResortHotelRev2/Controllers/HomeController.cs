using ResortHotelRev2.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ResortHotelRev2.Models.ViewModel;
using ResortHotelRev2.Models.EntityManager;

//TODO: Add password verification field during sign-up.
//TODO: Add reset password funtionality.
//TODO: Before going live, remove password from list of users returned by ManageUserPartial. Now, good for testing.
//TODO: Format phone numbers throughout based on country of residence
//TODO: Add validation to fields throughout.



namespace ResortHotelRev2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }

        [AuthorizeRoles("Admin")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }

        [AuthorizeRoles("Admin")]
        public ActionResult ManageUserPartial(string status = "", string name = "", string id = "" )
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager userManager = new UserManager();
                UserDataView userDataView = userManager.GetUserDataView(loginName);

                string message = string.Empty;
                if (status.Equals("update"))
                    message = "Update Successful";
                else if (status.Equals("delete"))
                    message = "Delete Successful";

                ViewBag.Message = message;

                if (name != string.Empty)
                {

                    userDataView.UserProfile = userDataView.UserProfile.Where(x => x.LastName == name);

                                        
                    return PartialView(userDataView);
                }

                if (id != string.Empty)
                {

                    userDataView.UserProfile = userDataView.UserProfile.Where(x => x.SYSUserID.ToString() == id);


                    return PartialView(userDataView);
                }

                else
                {
                    return PartialView(userDataView);
                }
                
            }

            return RedirectToAction("Index", "Home");
        } //END MANAGE USER PARTIAL

        [AuthorizeRoles("Admin")]
        public ActionResult UpdateUserData(int userID, string loginName, string password, string firstName, string lastName, string phone, string email, string userNotes, int roleID = 0)        
        {
            UserProfileView userProfileView = new UserProfileView();
            userProfileView.SYSUserID = userID;
            userProfileView.LoginName = loginName;
            userProfileView.Password = password;
            userProfileView.FirstName = firstName;
            userProfileView.LastName = lastName;
            userProfileView.PhoneNumber = phone;
            userProfileView.Email = email;
            userProfileView.UserNotes = userNotes;

            if (roleID > 0)
                userProfileView.LOOKUPRoleID = roleID;

            UserManager UM = new UserManager();
            UM.UpdateUserAccount(userProfileView);

            return Json(new { success = true });
        } //END UPDATE USER DATA

        [AuthorizeRoles("Admin")]
        public ActionResult DeleteUser(int userID)
        {
            UserManager userManager = new UserManager();
            userManager.DeleteUser(userID);
            return Json(new { success = true });
        }

        
        [Authorize]
        public ActionResult EditProfile()
        {
            string loginName = User.Identity.Name;
            UserManager userManager = new UserManager();
            UserProfileView userProfileView = userManager.GetUserProfile(userManager.GetUserID(loginName));
            return View(userProfileView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize]
        public ActionResult EditProfile(UserProfileView profile)
        {
            if (ModelState.IsValid)
            {
                UserManager userManager = new UserManager();
                userManager.UpdateUserAccount(profile);

                ViewBag.Status = "Update Successful!";

                FormsAuthentication.SetAuthCookie(profile.LoginName, false);

            }

            return View(profile);
        }//END EDIT PROFILE


        //STATIC HTML PAGES FOLLOW

        public ActionResult Accomodations()
        {
            return View();
        }

        public ActionResult Activities()
        {
            return View();
        }

        public ActionResult Access()
        {
            return View();
        }



    }
}
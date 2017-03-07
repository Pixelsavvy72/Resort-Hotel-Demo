using ResortHotelRev2.Models.EntityManager;
using ResortHotelRev2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;


//TODO: Automatically assign Member role.
namespace ResortHotelRev2.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SignUp(UserSignUpView userSignUpView)
        {
            if (ModelState.IsValid)
            {
                UserManager userManager = new UserManager();
                if (!userManager.IsLoginNameExist(userSignUpView.LoginName))
                {
                    userManager.AddUserAccount(userSignUpView);
                    FormsAuthentication.SetAuthCookie(userSignUpView.LoginName, false); //Login from first
                    return RedirectToAction("Welcome", "Home");

                }
                else
                    ModelState.AddModelError("", $"The login name {userSignUpView.LoginName} is unavailable.");
            }
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult LogIn(UserLoginView userLoginView, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserManager userManager = new UserManager();
                string password = userManager.GetUserPassword(userLoginView.LoginName);

                if (string.IsNullOrEmpty(password))
                    ModelState.AddModelError("", "The user login or password provided is incorrect.");
                else
                {
                    //Login and take user to either admin panel or welcome screen depending on role.
                    if (userLoginView.Password.Equals(password) && (userManager.IsUserInRole(userLoginView.LoginName, "Admin")))
                    {
                        FormsAuthentication.SetAuthCookie(userLoginView.LoginName, false);
                        return RedirectToAction("AdminOnly", "Home");
                    }
                    if (userLoginView.Password.Equals(password))
                    {
                        FormsAuthentication.SetAuthCookie(userLoginView.LoginName, false);
                        
                        if (TempData.ContainsKey("roomDataView"))
                        {                            
                            return RedirectToAction("VerifyReservationInfo", "Reservation");
                        }


                        return RedirectToAction("Welcome", "Home");
                    } 
                    else
                    {
                        ModelState.AddModelError("", "The password provided is incorrect.");
                    }
                }
            }
                        
            return View(userLoginView);
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }

}
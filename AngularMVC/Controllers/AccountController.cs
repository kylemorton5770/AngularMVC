﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using AngularMVC.Models;

namespace AngularMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        #region Identity Dependendancy Injection

        private AngularMVC.App_Start.IdentityConfig.ApplicationUserManager _userManager;
        private AngularMVC.App_Start.IdentityConfig.ApplicationSignInManager _signInManager;

        public AccountController() { }

        public AccountController(AngularMVC.App_Start.IdentityConfig.ApplicationUserManager userManager, AngularMVC.App_Start.IdentityConfig.ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AngularMVC.App_Start.IdentityConfig.ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AngularMVC.App_Start.IdentityConfig.ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public AngularMVC.App_Start.IdentityConfig.ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<AngularMVC.App_Start.IdentityConfig.ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        #endregion

        #region Get Views

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        #endregion

        #region Login/Register endpoints

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> Login(LoginViewModel model)
        {
            Log.debug("Model for Login: " + model);
            var signInResult = false;

            try
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                switch (result)
                {
                    case SignInStatus.Success:
                        signInResult = true;
                        break;
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        signInResult = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.error("Exception: " + ex);
            }

            Log.debug("Sign In Result: " + signInResult);

            return signInResult;

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> Register(RegisterViewModel model)
        {
            Log.debug("Model for Register: " + model);
            var registerResult = true;
            try
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                Log.debug("User: " + user.UserName + " " + user.Email);
                var result = await UserManager.CreateAsync(user, model.Password);
                Log.debug("Create User: " + result.Succeeded);
                if (!result.Succeeded){
                    registerResult = false;
                    Log.error("Errors on registration: ");
                    result.Errors.ToList().ForEach(e => Log.error("Error: " + e));
                }
                else
                    await SignInManager.SignInAsync(user, false, false);
            }
            catch (Exception ex)
            {
                Log.error("Exception: " + ex);
            }

            Log.debug("Register result: " + registerResult);

            return registerResult;
        }

        #endregion

    }
}
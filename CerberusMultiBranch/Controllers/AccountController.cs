using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities;
using CerberusMultiBranch.Support;
using System.Data.Entity;
using CerberusMultiBranch.Models.ViewModels.Config;
using Microsoft.AspNet.Identity.EntityFramework;
using CerberusMultiBranch.Models.Entities.Config;

namespace CerberusMultiBranch.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        #region Properties
        public AccountController()
        {

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion


        #region Anonumouse Allowed
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            var roleM = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

           /* if (!roleM.RoleExists("Administrador"))
                roleM.Create(new IdentityRole("Administrador"));

            if (!roleM.RoleExists("Cajero"))
                roleM.Create(new IdentityRole("Cajero"));

            if (!roleM.RoleExists("Vendedor"))
                roleM.Create(new IdentityRole("Vendedor"));

            if (!roleM.RoleExists("Almacenista"))
                roleM.Create(new IdentityRole("Almacenista"));

            if (!roleM.RoleExists("Capturista"))
                roleM.Create(new IdentityRole("Capturista"));

            if (!roleM.RoleExists("Supervisor"))
                roleM.Create(new IdentityRole("Supervisor"));*/

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

    
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

        
            switch (result)
            {
                case SignInStatus.Success:
                    //return RedirectToLocal(returnUrl);
                    return RedirectToAction("Index", "Manage");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

      
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        #endregion



        [Authorize(Roles = "Administrador")]
        public ActionResult UserConfig(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.Include(u => u.Roles).Include(u=> u.UserBranches).FirstOrDefault(u => u.Id == id);
                var model = new UserInRole(user);

                var rId = user.Roles.Select(r => r.RoleId).ToList();

                var rs = db.Roles.Where(r => !rId.Contains(r.Id)).ToList();
                var ra = db.Roles.Where(r => rId.Contains(r.Id)).ToList();

                model.AvailableRoles = rs.ToSelectList();
                model.SelectedRoles = ra.ToSelectList();

                var bIds = user.UserBranches.Select(ub => ub.BranchId).ToList();

                var ubA = db.Branches.Where(ub => !bIds.Contains(ub.BranchId)).ToList();
                var ubS = db.Branches.Where(ub => bIds.Contains(ub.BranchId)).ToList();

                model.AvailableBranches = ubA.ToSelectList();
                model.SelectedBranches = ubS.ToSelectList();

                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public JsonResult AddRole(string role)
        {
            IdentityResult result = null;
            var roleM = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleM.RoleExists(role))
                result = roleM.Create(new IdentityRole(role));
            else
                return Json(new { Result = "Role duplicado", Data = "El rol " + role + " ya se encuntra dado de alta" });

            return result.Succeeded ? Json(new { Result = "OK" }) :
                Json(new { Result = "Ocurrio un error al agregar el rol", Data = result.Errors.Where(e => e.Length > Cons.Zero) });
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public JsonResult AddOrRemove(string userId, string[] roles, bool add)
        {
            try
            {
                IdentityResult result = null;
                if (add)
                    result = UserManager.AddToRoles(userId, roles);
                else
                    result = UserManager.RemoveFromRoles(userId, roles);

                if (result.Succeeded)
                    return Json(new { Result = "OK" });
                else
                    return Json(new { Result = "Error al modificar los permisos", Data = result.Errors.Where(e => e.Length > 0) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al modificar los permisos", Data = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult SaveChanges([Bind(Include = "Id,Email,PhoneNumber,ComissionForSale")] ApplicationUser user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                   var um = db.Users.Find(user.Id);
                    um.Email = user.Email;
                    um.PhoneNumber = user.PhoneNumber;
                    um.ComissionForSale = user.ComissionForSale;

                    db.Entry(um).State = EntityState.Modified;

                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                }
              
                return RedirectToAction("UserConfig", new { id = user.Id });
            }
               
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public JsonResult AddOrRemoveBranches(string userId, int[] branches, bool add)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                if (add)
                {
                    foreach (var i in branches)
                    {
                        UserBranch us = new UserBranch { BranchId = i, UserId = userId };
                        db.UserBranches.Add(us);
                    }
                }
                else
                {
                    var bSession = UserManager.GetClaims(userId).FirstOrDefault(c => c.Type == Cons.BranchSession);

                    foreach (var i in branches)
                    {
                        if (bSession != null && bSession.Value.Contains(i.ToString()))
                            UserManager.RemoveClaim(userId, bSession);

                        var us = db.UserBranches.Find(i, userId);
                        db.UserBranches.Remove(us);
                    }
                }

                db.SaveChanges();

                return Json(new { Result = "OK" });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al modificar las sucursales", Data = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult UserList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var users = db.Users.Include(u => u.Roles).
                    Include(u=> u.UserBranches).
                    Where(u=> u.UserName!="Administrador").ToList();
                return View(users);
            }
        }


        [Authorize(Roles = "Administrador")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    ComissionForSale = model.Comission,
                    PhoneNumber = model.Phone
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("UserList", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
using DAL.EF_Core;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Main.Controllers
{
    [Authorize]
    public class AccountController: Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly DbMainContext db;

        public AccountController(SignInManager<IdentityUser> signInManager,
                                 UserManager<IdentityUser> userManager,
                                 DbMainContext db)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = db;
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = default;
                try
                {
                    user = await userManager.FindByNameAsync(model.UserName);
                }
                catch (Exception e)
                {
                    return new ContentResult { Content = e.Message + "\n\n" + e.InnerException?.Message };
                }



                if (user != null)
                {
                    await signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult res =
                        await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (res.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/Admin/notes");
                    }
                }
                ModelState.AddModelError("", "Неверные входные данные");
            }

            return View(model);
            //return Redirect(returnUrl ?? "/Admin/notes");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

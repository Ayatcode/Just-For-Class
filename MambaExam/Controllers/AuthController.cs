using Core3.Entities;
using MambaExam.Models;
using MambaExam.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace MambaExam.Controllers
{
	public class AuthController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

		public AuthController(UserManager<AppUser> userManager, 
			SignInManager<AppUser> signInManager, 
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}



		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Register(RegisterModel registerModel) 
		{
			if(!ModelState.IsValid) return View(registerModel);
			AppUser user = new()
			{
				Email = registerModel.Email,
				UserName = registerModel.UserName,

			};
			var Identityresult=await _userManager.CreateAsync(user,registerModel.Password);
			if (!Identityresult.Succeeded)
			{
                foreach (var error in Identityresult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(registerModel);
                }
            }
			
			await _userManager.AddToRoleAsync(user, RoleType.Member.ToString());
			return RedirectToAction(nameof(Login));
		}
        public IActionResult Login()
        {
            return View();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
			if(!ModelState.IsValid)return View(model);
			var user = await _userManager.FindByEmailAsync(model.UserOrEmail);
			if(user == null)
			{
				user =await _userManager.FindByNameAsync(model.UserOrEmail);
				if(user == null)
				{
					ModelState.AddModelError("", "Mail or password false");
					return View(model);
				}
            }
			var login = await _signInManager.PasswordSignInAsync(user, model.Password,true,true);
			if (!login.Succeeded)
			{
                ModelState.AddModelError("", "Mail or password false");
                return View(model);
            }
			
			
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) { ModelState.AddModelError("Email", "Email not found"); return View(model); }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string? link = Url.Action("ResetPassword", "Auth", new { userId = user.Id, tok = token }, HttpContext.Request.Scheme);

            return Json(link);
        }


        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token)) { return BadRequest(); }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) { return NotFound(); }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token)) { return BadRequest(); }
            if (!ModelState.IsValid) { return View(model); }

            var user = await _userManager.FindByNameAsync(userId);
            if (user == null) { return NotFound(); }

            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);


                }
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				await _signInManager.SignOutAsync();
			}
			return RedirectToAction("Index","Home");
		}

		public async Task CreateRole()
		{
			foreach (var role in Enum.GetValues(typeof(RoleType)))
			{
				if (!await _roleManager.RoleExistsAsync(role.ToString())) 
				{
					await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
				}
			}
		}
    }
}

using DentistRegistrationForm.Models;
using DentistRegistrationForm.Services;
using DentistRegistrationFormData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DentistRegistrationForm.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IMailMessageService mailMessageService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppDbContext context;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMailMessageService mailMessageService,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context
            )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mailMessageService = mailMessageService;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel { RememberMe = true, ReturnUrl = HttpContext.Request.Query["ReturnUrl"] });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return Redirect(model.ReturnUrl ?? "/");
            else
            {
                if (result.IsNotAllowed)
                    ModelState.AddModelError("", "E-Mail adress not confirmed");
                else if (result.IsLockedOut)
                    ModelState.AddModelError("", "Too many tries");
                else
                    ModelState.AddModelError("", "Invalid user");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var newUser = new User
            {
                UserName = model.UserName,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber
            };
            var result = await userManager.CreateAsync(newUser, model.Password);
            await userManager.AddToRoleAsync(newUser, "Clients");

            if (result.Succeeded)
            {
                var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var messageBody =
                    string.Format(
                    System.IO
                    .File
                    .ReadAllText(System.IO.Path.Combine(webHostEnvironment.WebRootPath, "Content", "EMailConfirmationTemplate.html"))
                    , model.Name
                    , Url.Action("ConfirmEmail", "Account", new { id = newUser.Id, token = emailConfirmationToken }, httpContextAccessor.HttpContext.Request.Scheme)
                    );

                await mailMessageService.Send(model.UserName, "E-Mail Confirmation Message", messageBody);
                return View("RegisterSuccess");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "DuplicateUserName":
                            ModelState.AddModelError("", "E-Mail is in use");
                            break;
                        case "PasswordTooShort":
                            ModelState.AddModelError("", "Lorem ipsum");
                            break;
                        case "PasswordRequiresDigit":
                            ModelState.AddModelError("", "Lorem ipsum");
                            break;
                        case "PasswordRequiresNonAlphanumeric":
                            ModelState.AddModelError("", "Lorem ipsum");
                            break;
                        case "PasswordRequiresLower":
                            ModelState.AddModelError("", "Lorem ipsum");
                            break;
                        case "PasswordRequiresUpper":
                            ModelState.AddModelError("", "Lorem ipsum");
                            break;
                    }                    
                }
                return View(model);
            }
        }
    }
}

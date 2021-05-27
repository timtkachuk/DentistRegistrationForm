using DentistRegistrationForm.Models;
using DentistRegistrationForm.Services;
using DentistRegistrationFormData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            try
            {
                var result = await userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Clients");
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
                            case "DuplicatePhoneNumber":
                                ModelState.AddModelError("", "Phone number is in use");
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
            catch (DbUpdateException)
            {
                TempData["error"] = $"The same name and phone number of {model.Name} {model.PhoneNumber} exists.";
                return View(model);
            }
        }
        public async Task<IActionResult> ConfirmEmail(int id, string token)
        {
            var user = context.Users.Find(id);
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("EMailSuccess");
            }
            else
            {
                return View("EMailFail");
            }
        }

        public IActionResult ResetPasswordForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordForm(ResetPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                var messageBody =
                    string.Format(
                    System.IO
                    .File.ReadAllText(System.IO.Path.Combine(webHostEnvironment.WebRootPath, "Content", "ResetPasswordTemplate.html"))
                    , user.Name
                    , Url.Action("ResetPassword", "Account", new { id = user.Id, token = passwordResetToken }, httpContextAccessor.HttpContext.Request.Scheme)
                    );
                await mailMessageService.Send(user.UserName, "Dentist App Reset Password", messageBody);
                return View("ResetPasswordFormSuccess");
            }
            else
            {
                ModelState.AddModelError("", "E-Mail adress does not match with our system");
                return View(model);
            }

        }

        public IActionResult ResetPassword(string id, string token)
        {
            return View(new NewPasswordViewModel { Id = id, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(NewPasswordViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return View("ResetPasswordSuccess");

        }

        public async Task<IActionResult> NewBooking()
        {
            ViewData["Doctors"] = new SelectList(context.Users.ToList().Where(p => userManager.IsInRoleAsync(p, "Doctors").Result), "Id", "Name");
            ViewData["Procedures"] = new SelectList(await context.Procedures.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewBooking(Booking model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            context.Entry(model).State = EntityState.Added;
            await context.SaveChangesAsync();

            var messageBody =
                string.Format(
                System.IO
                .File.ReadAllText(System.IO.Path.Combine(webHostEnvironment.WebRootPath, "Content", "BookingConfirmationTemplate.html"))
                , user.Name
                , model.DateTime.ToShortDateString()
                , model.DateTime.ToShortTimeString()
                );

            await mailMessageService.Send(user.UserName, "Booking Confirmation Message", messageBody);

            return RedirectToAction("Index", "Home");
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Gbc_Travel_Group63.Models;
using Gbc_Travel_Group63.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


public class AuthorizationController : Controller
{

    private readonly IEmailSender _emailSender;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthorizationController> _logger;
    private readonly ISessionService _sessionService;

    public AuthorizationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
        RoleManager<IdentityRole> roleManager,
        ILogger<AuthorizationController> logger,
        ISessionService sessionService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _roleManager = roleManager;
        _logger = logger;
        _sessionService = sessionService;
    }

    public IActionResult Signin() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Signin(SignInViewModel model)
    {
        _logger.LogInformation("Sign in attempt made for" + User);
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Display error message for unconfirmed email
                    ModelState.AddModelError(string.Empty, "Your account has not been confirmed yet. Please check your email to confirm your account.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }


    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Signup(SignupViewModel model)
    {
        _logger.LogInformation("New Signup was done for " + User);
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.IsAdmin)
                {
                    // Check if admin role exists, if not, create it
                    var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
                    if (!adminRoleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    // Assign user to admin role
                    await _userManager.AddToRoleAsync(user, "Admin");
                }

                // Send email confirmation
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Authorization", new { userId = user.Id, token = token }, Request.Scheme);
                var subject = "Welcome to our application! Confirm your account.";
                var message = "Thank you for registering. Please confirm your email by clicking <a href='" + confirmationLink + "'>here</a>.";
                await _emailSender.SendEmailAsync(model.Email, subject, message);

                return RedirectToAction("SuccessfulSignUp");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    public IActionResult SuccessfulSignUp() {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return RedirectToAction("Error", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return RedirectToAction("Error", "Home");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return RedirectToAction("EmailConfirmed", "Authorization");
        }
        else
        {
            // Handle email confirmation failure
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult EmailConfirmed() {
        return View();
    }


    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Authorization", new { email = user.Email, token = token }, Request.Scheme);

            // Send reset password email with the reset link
            await _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        // If model state is invalid, redisplay the form
        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (token == null || email == null)
        {
            return RedirectToAction("Error", "Home");
        }

        var model = new ResetPasswordViewModel { Token = token, Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return RedirectToAction("ResetPasswordConfirmation", "Authorization");
        }
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("ResetPasswordConfirmation", "Authorization");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }



    [Authorize]
    public async Task<IActionResult> ViewProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var model = new ApplicationUser
        {
            FullName = user.FullName,
            ContactNumber = user.ContactNumber,
            Preferences = user.Preferences,
            ProfilePicture = user.ProfilePicture
        };

        return View("ViewProfile", model); // Specify the name of the view
    }

    [Authorize]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var model = new ApplicationUser
        {
            FullName = user.FullName,
            ContactNumber = user.ContactNumber,
            Preferences = user.Preferences,
            ProfilePicture = user.ProfilePicture

        };

        return View("UpdateProfile", model );
    }



    [HttpPost("AuthorizationController/Updateprofiles")]
    public async Task<IActionResult> UpdateProfile(ApplicationUser model,IFormFile profilePicture)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.FullName = model.FullName;
                user.ContactNumber = model.ContactNumber;
                user.Preferences = model.Preferences;

                if (profilePicture != null && profilePicture.Length > 0)
                {
                    // Read the file data into a byte array
                    using (var memoryStream = new MemoryStream())
                    {
                        await profilePicture.CopyToAsync(memoryStream);
                        model.ProfilePicture = memoryStream.ToArray();
                        user.ProfilePicture = model.ProfilePicture;
                    }
                }
                Console.WriteLine("profilePicture data : " + user.ProfilePicture);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Profile updated successfully
                    //return RedirectToAction("Index", "Home");
                    return View("ViewProfile", user);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                // User not found
                return NotFound();
            }
        }
        // If model state is invalid, redisplay the form with validation errors
        return View("ViewProfile", model);
    }


}

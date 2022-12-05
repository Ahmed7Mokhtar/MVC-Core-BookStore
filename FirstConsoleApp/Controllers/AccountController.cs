using BookStore.Models;
using FirstConsoleApp.Models;
using FirstConsoleApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace FirstConsoleApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        [Route("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpUserModel userModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel); 
                if(!result.Succeeded)
                {
                    // get all error messages
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }

                    return View(userModel);
                }

                ModelState.Clear();
                return RedirectToAction("ConfirmEmail", new { email = userModel.Email });
            }

            return View();
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signInModel, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignIn(signInModel);
                if(result.Succeeded)
                {
                    // redirect to the last page before sign in
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                if(result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Not allowed to login!");
                }
                else if(result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Account blocked. Try after sometime!");
                }
                else
                {
                    ModelState.AddModelError("", "invalid Credentials!");
                }

            }

            return View();
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("change-password")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(changePasswordModel);
                if(result.Succeeded)
                {
                    ViewBag.isSuccess = true;
                    ModelState.Clear();
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(changePasswordModel);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmModel emailConfirmModel = new EmailConfirmModel()
            {
                Email = email,
            };

            if(!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {

                token = token.Replace(' ', '+');

                var result = await _accountRepository.ConfirmEmailAsync(uid, token);
                if(result.Succeeded)
                {
                    emailConfirmModel.EmailVerified = true;
                }
            }

            return View(emailConfirmModel);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel emailConfirmModel)
        {
            var user = await _accountRepository.GetUserByEmailAsync(emailConfirmModel.Email);
            if(user != null)
            {
                if(user.EmailConfirmed)
                {
                    emailConfirmModel.EmailVerified = true;
                    return View(emailConfirmModel);
                }

                await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                emailConfirmModel.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong!");
            }


            return View(emailConfirmModel);
        }

        [AllowAnonymous, HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous, HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailAsync(forgotPasswordModel.Email);
                if(user != null)
                {
                    await _accountRepository.GenerateForgotPasswordTokenAsync(user);
                }

                ModelState.Clear();
                forgotPasswordModel.EmailSent = true;
            }

            return View(forgotPasswordModel);
        }

        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel()
            {
                UserId = uid,
                Token = token
            };


            return View(resetPasswordModel);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                resetPasswordModel.Token = resetPasswordModel.Token.Replace(' ', '+');

                var result = await _accountRepository.RestPasswordAsync(resetPasswordModel);
                if(result.Succeeded)
                {
                    ModelState.Clear();
                    resetPasswordModel.IsSuccess = true;
                    return View(resetPasswordModel);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(resetPasswordModel);
        }
    }
}

using BookStore.Models;
using FirstConsoleApp.Models;
using Microsoft.AspNetCore.Identity;

namespace FirstConsoleApp.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
        Task<SignInResult> PasswordSignIn(SignInModel signInModel);
        Task SignOutAsync();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePasswordModel);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task GenerateForgotPasswordTokenAsync(AppUser user);
        Task<IdentityResult> RestPasswordAsync(ResetPasswordModel resetPasswordModel);
    }
}
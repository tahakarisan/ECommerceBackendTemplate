using Core.Entities.Concrete.Auth;
using Core.Utilities.Results;

namespace Business.Utilities.Mail
{
    public interface IMailService
    {
        Task<IResult> SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);
        Task<IResult> SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task<IResult> SendPasswordResetMailAsync(ResetPasswordCode resetPasswordCode);
    }
}
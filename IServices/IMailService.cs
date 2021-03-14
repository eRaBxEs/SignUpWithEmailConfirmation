using System.Threading.Tasks;
using SignupWithMailConfirmation.Models;

namespace SignupWithMailConfirmation.IServices
{
    public interface IMailService
    {
         Task<string> SendMail(MailClass oMailClass);
         string GetMailBody(LoginInfo oLoginInfo);
    }
}
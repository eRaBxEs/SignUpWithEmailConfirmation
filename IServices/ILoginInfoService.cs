using System.Threading.Tasks;
using SignupWithMailConfirmation.Models;

namespace SignupWithMailConfirmation.IServices
{
    public interface ILoginInfoService
    {
         Task<LoginInfo> SignUp(LoginInfo oLoginInfo);
         Task<string> ConfirmMail(string username);
         Task<LoginInfo> GetLoginUser(string username);
         Task<LoginInfo> GetLoginUserByID(string username);
    }
}
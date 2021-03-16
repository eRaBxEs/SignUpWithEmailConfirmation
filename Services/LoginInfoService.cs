using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SignUpWithEmailConfirmation.Common;
using SignupWithMailConfirmation.Common;
using SignupWithMailConfirmation.Data;
using SignupWithMailConfirmation.IServices;
using SignupWithMailConfirmation.Models;

namespace SignupWithMailConfirmation.Services
{
    public class LoginInfoService : ILoginInfoService
    {
        private readonly DataContext _context;

        public LoginInfoService(DataContext context)
        {
            _context = context;

        }
        LoginInfo _oLoginInfo = new LoginInfo();

        public async Task<string> ConfirmMail(string guid)
        {
            try
            {
                if (string.IsNullOrEmpty(guid)) return "Invalid Username";

                LoginInfo oLoginInfo = new LoginInfo()
                {
                    GUID = guid, 
                };
                LoginInfo loginInfo = await this.CheckGUIDExistence(oLoginInfo);

                if (loginInfo == null)
                {
                    return Message.InvalidUser;
                }
                else
                {
                    // var oLoginInfos = await _context.LoginInfos.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

                    loginInfo.IsmailConfirmed = true;
                    
                    _context.LoginInfos.Update(loginInfo);
                    await _context.SaveChangesAsync();

                    
                    return "Mail Confirmed";
                }

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        // public async Task<string> ConfirmMail(string username)
        // {
        //     try
        //     {
        //         if (string.IsNullOrEmpty(username)) return "Invalid Username";

        //         LoginInfo oLoginInfo = new LoginInfo()
        //         {
        //             Username = username
        //         };
        //         LoginInfo loginInfo = await this.CheckRecordExistence(oLoginInfo);

        //         if (loginInfo == null)
        //         {
        //             return Message.InvalidUser;
        //         }
        //         else
        //         {
        //             using (IDbConnection con = new SqlConnection(Global.ConnectionString))
        //             {
        //                 if (con.State == ConnectionState.Closed) con.Open();
        //                 var oLoginInfos = await con.QueryAsync<LoginInfo>("SP_LoginInfo"
        //                 , this.SetParameters(oLoginInfo, (int)OperationType.UpdateConfirmMail)
        //                 , commandType: CommandType.StoredProcedure);

        //                 if (oLoginInfos != null && oLoginInfos.Count() > 0)
        //                 {
        //                     _oLoginInfo = oLoginInfos.FirstOrDefault();
        //                 }
        //                 return "Mail Confirmed";
        //             }
        //         }

        //     }
        //     catch (Exception ex)
        //     {

        //         return ex.Message;
        //     }
        // }

        public async Task<LoginInfo> SignUp(LoginInfo oLoginInfo)
        {
            _oLoginInfo = new LoginInfo();
            try
            {
                LoginInfo loginInfo = await this.CheckRecordExistence(oLoginInfo);
                Console.WriteLine("found:" + loginInfo);
                if (loginInfo == null)
                {
                    oLoginInfo.GUID = GUIDToken.Generate();
                    await _context.LoginInfos.AddAsync(oLoginInfo);
                    await _context.SaveChangesAsync();
                
                    _oLoginInfo = oLoginInfo; // very important
                    _oLoginInfo.Message = Message.Success;
                    
                }
                else
                {
                    _oLoginInfo = loginInfo;
                }
            }
            catch (Exception ex)
            {

                _oLoginInfo.Message = ex.Message;

            }
            return _oLoginInfo;
        }

        // public async Task<LoginInfo> SignUp(LoginInfo oLoginInfo)
        // {
        //     _oLoginInfo = new LoginInfo();

        //     try
        //     {
        //         LoginInfo loginInfo = await this.CheckRecordExistence(oLoginInfo);
        //         if (loginInfo == null)
        //         {
        //             using (IDbConnection con = new SqlConnection(Global.ConnectionString))
        //             {
        //                 if (con.State == ConnectionState.Closed) con.Open();
        //                 var oLoginInfos = await con.QueryAsync<LoginInfo>("SP_LoginInfo"
        //                 , this.SetParameters(oLoginInfo, (int)OperationType.SignUp)
        //                 , commandType: CommandType.StoredProcedure);

        //                 if (oLoginInfos != null && oLoginInfos.Count() > 0)
        //                 {
        //                     _oLoginInfo = oLoginInfos.FirstOrDefault();
        //                 }
        //                 _oLoginInfo.Message = Message.Success;
        //             }
        //         }
        //         else
        //         {
        //             _oLoginInfo = loginInfo;
        //         }
        //     }
        //     catch (Exception ex)
        //     {

        //         _oLoginInfo.Message = ex.Message;

        //     }
        //     return _oLoginInfo;
        // }

        private async Task<LoginInfo> CheckRecordExistence(LoginInfo oLoginInfo)
        {
            LoginInfo loginInfo = new LoginInfo();
            if (!string.IsNullOrEmpty(oLoginInfo.Username))
            {
                loginInfo = await this.GetLoginUser(oLoginInfo.Username);
                if (loginInfo != null)
                {
                    if (!loginInfo.IsmailConfirmed)
                    {
                        loginInfo.Message = Message.VerifyEmail;
                    }
                    else if (loginInfo.IsmailConfirmed)
                    {
                        loginInfo.Message = Message.UserAlreadyCreated;
                    }
                }
            }
            if (!string.IsNullOrEmpty(oLoginInfo.GUID))
            {
                loginInfo = await this.GetLoginUserByID(oLoginInfo.GUID);
                if (loginInfo != null)
                {
                    if (!loginInfo.IsmailConfirmed)
                    {
                        loginInfo.Message = Message.VerifyEmail;
                    }
                    else if (loginInfo.IsmailConfirmed)
                    {
                        loginInfo.Message = Message.UserAlreadyCreated;
                    }
                }
            }

            return loginInfo;
        }

        private async Task<LoginInfo> CheckGUIDExistence(LoginInfo oLoginInfo)
        {
            LoginInfo loginInfo = new LoginInfo();
            if (!string.IsNullOrEmpty(oLoginInfo.GUID))
            {
                loginInfo = await this.GetLoginUserByID(oLoginInfo.GUID);
                if (loginInfo != null)
                {
                    if (!loginInfo.IsmailConfirmed)
                    {
                        loginInfo.Message = Message.VerifyEmail;
                    }
                    else if (loginInfo.IsmailConfirmed)
                    {
                        loginInfo.Message = Message.UserAlreadyCreated;
                    }
                }
            }

            return loginInfo;
        }

        // public async Task<LoginInfo> GetLoginUser(string username)
        // {
        //     _oLoginInfo = new LoginInfo();
        //     using (IDbConnection con = new SqlConnection(Global.ConnectionString))
        //     {
        //         if (con.State == ConnectionState.Closed) con.Open();
        //         string sSQL = "SELECT * FROM LoginInfo WHERE 1=1";

        //         if (!string.IsNullOrEmpty(username)) sSQL += " AND Username='" + username + "'";
        //         var oLoginInfos = (await con.QueryAsync<LoginInfo>(sSQL)).ToList();
        //         if (oLoginInfos != null && oLoginInfos.Count > 0) _oLoginInfo = oLoginInfos.SingleOrDefault();
        //         else return null;
        //     }

        //     return _oLoginInfo;
        // }
        public async Task<LoginInfo> GetLoginUser(string username)
        {
            _oLoginInfo = new LoginInfo();
            _oLoginInfo = await _context.LoginInfos.FirstOrDefaultAsync(u => u.Username == username);
            return _oLoginInfo;
        }

        public async Task<LoginInfo> GetLoginUserByID(string guid)
        {
            _oLoginInfo = new LoginInfo();
            _oLoginInfo = await _context.LoginInfos.FirstOrDefaultAsync(u => u.GUID == guid);
            return _oLoginInfo;
        }

        private DynamicParameters SetParameters(LoginInfo oLoginInfo, int nOperationType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserInfoId", oLoginInfo.UserInfoId);
            parameters.Add("@EmailId", oLoginInfo.EmailId);
            parameters.Add("@Username", oLoginInfo.Username);
            parameters.Add("@Password", oLoginInfo.Password);
            parameters.Add("@IsMailConfirmed", oLoginInfo.IsmailConfirmed);
            parameters.Add("@OperationType", nOperationType);

            return parameters;
        }
    }
}
using PROACC2.BL;
using PROACC2.BL.General;
using PROACC2.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static PROACC2.BL.Base;
using System.Web.Configuration;
using System.Threading.Tasks;

namespace PROACC2.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        RstPassword rt = new RstPassword();
        public ActionResult Login()
        {
            if (Session["loginid"] != null)
            { 
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        //Test First Commit
        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult Validate(UserModel user)
        {
            Base _Base = new Base();
            LogHelper _Log = new LogHelper();
            try
            {

                LogedUser logedUser = new LogedUser();
                logedUser.Username = user.Username;
                logedUser.Password = user.Password;
                _Log.createLog("Start ---> Validadate");
                if (!String.IsNullOrEmpty(user.Username) && (!String.IsNullOrEmpty(user.Password)))
                {
                   
                    
                    logedUser = _Base.UserValidation(logedUser);
                    if (logedUser.ID != Guid.Empty)
                    {
                        _Log.createLog("------>After");
                        FormsAuthentication.SetAuthCookie(logedUser.Username, false);
                        Session["log-id"] = logedUser.LogID.ToString();
                        Session["loginid"] = logedUser.ID.ToString();
                        Session["UserName"] = logedUser.Name.ToString();
                        string Timezone = user.TimeZone.ToString();
                        Session["TimeZone"] = GetTImezone(Timezone);
                        Session["ShortName"] = _Base.GetFirstandLastName(logedUser.Name.ToString());

                        Session["InstanceId"] = Guid.Empty;
                        string UserType = "";
                        if (logedUser.Type == 1)
                        {
                            UserType = "Admin";
                        }
                        else if (logedUser.Type == 2)
                        {
                            UserType = "Consultant";
                        }
                        else if (logedUser.Type == 3)
                        {
                            UserType = "Customer";
                        }
                        else if (logedUser.Type == 4)
                        {
                            UserType = "Project Manager";
                        }
                        Session["UserType"] = UserType;
                        if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                        {
                            String Controller = Request.Form["ReturnUrl"].Split('/')[1].ToString();
                            String Action = Request.Form["ReturnUrl"].Split('/')[2].ToString();

                            return RedirectToAction(Action, Controller);
                        }
                        else
                        {
                            return RedirectToAction("Home", "Home");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "User name & password supplied doesn't Match";
                    }
                }
                else
                {
                    TempData["Message"] = "Enter User name & password";
                }
            }
            catch (Exception ex)
            {

                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, Url);
                TempData["Message"] = "Login failed.Error - " + ex.Message;
                throw ex;
            }
            return RedirectToAction("Login", "Login");
        }


        private string GetTImezone(string Time_zone)
        {
            LogHelper _Log = new LogHelper();
            string NewTimeZne = "";
            try
            {

                //var timeZoneIds = TimeZoneInfo.GetSystemTimeZones().Select(t => t.Id);

                if (Time_zone == "Asia/Calcutta")
                {
                    Time_zone = "India Standard Time";
                }

                var Time = TimeZoneInfo.FindSystemTimeZoneById(Time_zone);
                //DateTimeOffset UTC = TimeZoneInfo.ConvertTime(Dt, Time);
                //DateTime other = DateTime.SpecifyKind(Dt, DateTimeKind.Utc);


                //DateTime localDate = DateTime.Now.Date;
                //var s = DateTimeOffset.Now();
                //var AUSOffset = new DateTimeOffset(localDate, TimeSpan.Zero);
                NewTimeZne = Time.Id.ToString();

            }
            catch (Exception e)
            {
                _Log.createLog("GetTImezone --->"+e);
                _Log.createLog("for TimeZone --->" + Time_zone);
               // throw e;
            }


            return NewTimeZne;

        }

        #region ForgotPwd
        public ActionResult CheckUserNameEmailAvailabiltiy(string Name, string EmailId)
        {
            Base _Base = new Base();
            var status = false;
            try
            {
                UserModel result = _Base.Sp_GetNameEmailCheck(Name, EmailId);
                if (result.LoginId.ToLower() == Name.ToLower() && result.Email.ToLower() == EmailId.ToLower())
                {
                    status = true;
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoginIdCheck(string username, string mail,string code)
        {
            Base _Base = new Base();
            //var status = false;
            try
            {
                UserModel result = _Base.Sp_GetNameEmailCheck(username, mail);
                if (result.LoginId == username)
                {
              //      status = true;
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Name, string EmailId)
        {
            Base _Base = new Base();
            LogHelper _Log = new LogHelper();
            UserModel _userId = _Base.Sp_GetNameEmailCheck(Name, EmailId);
            // var _userid = _userId.ToList().Where(x=>x.Email==EmailId && x.Name==Name).FirstOrDefault().UserId;

            //string _Userid=null;
            //foreach (var item in _userId)
            //{
            //    _Userid = item.UserId.ToString();
            //}

            Guid UserID = _userId.UserId;
            if (UserID != null)
            {
                try
                {
                    Guid UserId = UserID;
                    Guid CreatedBy = UserID;
                    DateTime CreatedOn = DateTime.UtcNow;
                    bool Status = true;
                    bool isActive = true;
                    bool result = _Base.Sp_ResetPasswordStatus(UserId, Status, CreatedBy, CreatedOn, isActive);

                    List<RstPassword> rt2 = new List<RstPassword>();
                    rt2 = _Base.Sp_GetResetId(UserId);
                    var _ResetPasswordId = rt2.FirstOrDefault().ResetId;
                    var link = Url.Action("ResetPassWord", "Login", new { email = EmailId, code = _ResetPasswordId });

                    //var linkUrl = "/Login/ResetLink?" + "email=" + emailId + "&code=" + rt.ResetId;
                    // string Ab_Path = Request.Url.PathAndQuery;
                    //var url = "https://localhost:44352";
                    //string msg = "<b>Please find the Password Reset Link. </b><br/>" + "<a href='" + url + link + "'>Click Link To Reset Password</a>";
                    //string Server_Path = Request.UrlReferrer.ToString();
                    string Server_Path = WebConfigurationManager.AppSettings["Server_Path"];
                    _Log.createLog(Server_Path);
                    _Log.createLog(link);
                    _Log.createLog(Server_Path + link);


                    string msg = "<b>You recently requested to reset your password for your ProAcc Application account <br> Please find the Password Reset Link Below . </b><br/>" + "<a href='" + Server_Path + link + "'>Click Link To Reset Password</a>";

                    bool s = _Base.AddResetMail(_ResetPasswordId, UserId, EmailId, msg);

                    if (s == true)
                    {
                        return Json("success");
                    }
                    else
                    {
                        return Json("error");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return Json("error");
            }
        }

        [AllowAnonymous]  
        public ActionResult ResetPassWord(string email, string code)
        {
            Base _Base = new Base();
            if (code != null)
            {


                Guid ResetId = Guid.Parse(code);
                //var result = db.ResetPasswords.ToList().Exists(x => x.ResetId == ResetId && x.IsActive == true && x.Status == false);

                List<RstPassword> result = _Base.Sp_GetResetList(ResetId);
                var count = result.ToList().Count();
                if (count == 1)
                {
                    if (email != null && code != null)
                    {
                        ViewData["Mail"] = email;
                        ViewData["code"] = code;
                        return View();
                    }
                    else
                    {
                        ViewData["Status"] = false;
                        return View();
                        //return Json("error");
                    }
                }
                else
                {
                    ViewData["Status"] = false;
                    return View();
                    //return Json("error");
                }
            }
            else
            {
                ViewData["Status"] = false;
                    return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassWord(string Password, string Mail, string code)
        {
            Base _Base = new Base();
            Boolean flag = false;
            Guid ResetId = Guid.Parse(code);
            try
            {
                //Guid _userId = db.ResetPasswords.Where(x => x.IsActive == true && x.Status == false && x.ResetId == ResetId).FirstOrDefault().UserId;

                List<RstPassword> _result = _Base.Sp_GetResetList(ResetId);
                Guid _userId = _result.ToList().Where(x => x.ResetId == ResetId).FirstOrDefault().UserId;

                //string User_MailID = db.UserMasters.Where(x => x.isActive == true && x.UserId == _userId).FirstOrDefault().EMail;

                List<UserModel> _emailList = _Base.Sp_EmailList();
                string User_MailID = _emailList.ToList().Where(x => x.UserId == _userId).FirstOrDefault().Email;

                if (Mail == User_MailID)
                {
                    UserModel D = new UserModel();
                    D.UserId = _userId;
                    D.Password = _Base.Encrypt(Password);
                    D.Modified_by = _userId;
                    D.Modified_On = DateTime.UtcNow;
                    flag = _Base.Sp_ResetPassword(D);
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
                //var _userId = (from emp2 in db.UserMasters
                //               where emp2.EMail == Mail && emp2.isActive == true
                //               select emp2.UserId).FirstOrDefault();
                //if (_userId != null)
                //{
                //    try
                //    {
                //        UserMaster emp = new UserMaster();
                //        emp.Password = password;
                //        emp.Modified_by = _userId;
                //        emp.Modified_On = DateTime.UtcNow;
                //        emp.isActive = true;

                //        //bool result2 = _Base.Sp_ResetPassword(emp);
                //        bool result2 = false;
                //        if (result2 == true)
                //        {
                //            flag = true;
                //            return Json("success");
                //        }
                //        else
                //        {
                //            return Json("error");
                //        }
                //    }

            }
            catch (Exception ex)
            {
                return Json("error");
                throw ex;

            }
        }

        public ActionResult NewPasswordcheck(string password,string username)
        {
            Base _Base = new Base();
            string passwordEncrypt = _Base.Encrypt(password);
            //string userid = Session["loginid"].ToString();
            var result = _Base.SpResetCheckPassword(passwordEncrypt,username); 
            //db.UserMasters.ToList().Exists(x => x.Password == passwordEncrypt && x.isActive == true);
            if (result != true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region UpdatePassword

        [HttpPost]
        public ActionResult UpdatePassword(string Password)
        {
            Base _Base = new Base();
            LogHelper _Log = new LogHelper();
            var status = "fail";
            try
            {
                if (Password != null)
                {
                    UserModel userMaster = new UserModel();
                    userMaster.Modified_On = DateTime.UtcNow;
                    userMaster.Modified_by = Guid.Parse(Session["loginid"].ToString());
                    userMaster.isActive = true;
                    userMaster.Password = _Base.Encrypt(Password.ToString());
                    userMaster.UserId = Guid.Parse(Session["loginid"].ToString());
                    bool Result = _Base.Sp_ResetPassword(userMaster);

                    if (Result == true)
                    {
                        status = "success";
                    }
                    else
                    {
                        status = "fail";
                    }
                }
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->UpdatePassword Post" + Url);
                throw ex;
            }
            return Json(status);
        }

        public ActionResult OldPasswordcheck(string password)
        {
            Base _base = new Base();
            string passwordEncrypt = _base.Encrypt(password);
            string userid = Session["loginid"].ToString();
            var result = _base.SpCheckPassword(passwordEncrypt,userid);
            if (result != true)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
    }
}
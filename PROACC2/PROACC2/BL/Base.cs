using PROACC2.BL.General;
using PROACC2.BL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static PROACC2.BL.Model.Common;

namespace PROACC2.BL
{
    public class Base
    {
        #region Login
        public LogedUser UserValidation(LogedUser user)
        {
            LogHelper _Log = new LogHelper();
            _Log.createLog("Came inside user validation");
            try
            {

                DataSet ds = new DataSet();

                //string Browser, IP, MachineName;
                //Browser = HttpContext.Current.Request.Browser.Browser;
                //IP = HttpContext.Current.Request.UserHostAddress;

                //System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(IP);
                //MachineName = hostEntry.HostName;
                _Log.createLog("Before Db helper");
                DBHelper dB = new DBHelper("SP_Login", CommandType.StoredProcedure);
                dB.addIn("@Type", "Login");
                dB.addIn("@UserName", user.Username);
                dB.addIn("@Password", Encrypt(user.Password));

                //dB.addIn("@Browser", "Browser");
                //dB.addIn("@IP", "IP");
                //dB.addIn("@MachineName", "MachineName");

                _Log.createLog("Inside User validation");
                ds = dB.ExecuteDataSet();
                DataTable dt = new DataTable();
                if (ds.Tables.Count != 0)
                {
                    //DataTable dt1 = new DataTable();
                    dt = ds.Tables[0];
                    //dt1 = ds.Tables[1];
                    if (dt.Rows.Count == 1)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        {


                            user.ID = Guid.Parse(dt.Rows[0][0].ToString());
                            user.Type = Convert.ToInt32(dt.Rows[0][1].ToString());
                            user.Name = dt.Rows[0][2].ToString();
                            user.LogID = Guid.Parse(dt.Rows[0][3].ToString());
                            //User_ID = user.ID;
                            //User_Name = user.Name;
                            //for (int i = 0; i < dt1.Rows.Count; i++)
                            //{
                            //    if (user.Type == Convert.ToInt32(dt1.Rows[i]["id"]))
                            //    {
                            //        User_Type = dt1.Rows[0]["UserType"].ToString().ToUpper();
                            //        break;
                            //    }
                            //}
                        }
                    }
                }
                _Log.createLog("Inside User validation Before return" + user.Username.ToString() + "---------" + user.ID.ToString());
                return user;
            }
            catch (Exception ex)
            {
                
                _Log.createLog(ex);
                throw ex;
                //return user;                
            }
        }

            #region ForgotPassword

        public Boolean AddResetMail(Guid ResetId, Guid UserID, String emailId, string msg)
        {
            Boolean Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper Db1 = new DBHelper("SP_Mail", CommandType.StoredProcedure);
                Db1.addIn("@Type", "ResetMail");
                Db1.addIn("@UserID", UserID);
                Db1.addIn("@Q_Mail", emailId);
                Db1.addIn("@Q_Name", msg);
                Db1.addIn("@PM_ID", ResetId);
                Db1.addIn("@Cre_By", UserID);
                DataTable dt = new DataTable();
                dt = Db1.ExecuteDataTable();

                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->Reset_Mail" + ex.Message.ToString());
            }

            return Status;
        }

        public UserModel Sp_GetNameEmailCheck(string Name, string EmailId)
        {
            DataTable dt = new DataTable();

            UserModel P = new UserModel();
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
                dB.addIn("@Type", "CheckNameEmail");
                dB.addIn("@Name", Name);
                dB.addIn("@Email", EmailId);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        P.Email = dr["EMail"].ToString();
                        P.LoginId = dr["LoginId"].ToString();
                        P.UserId = Guid.Parse(dr["UserId"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "-->Sp_GetNameEmailCheck" + ex.Message.ToString());
            }
            return P;
        }

        public Boolean Sp_ResetPasswordStatus(Guid UserId, bool Status, Guid CreatedBy, DateTime CreatedOn, bool isValid)
        {
            Boolean Result = false;
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Login", CommandType.StoredProcedure);
            dB.addIn("@Type", "ResetPasswordStatus");
            dB.addIn("@UserId", UserId);
            dB.addIn("@CreatedBy", CreatedBy);
            dB.ExecuteScalar();
            Result = true;
            return Result;
        }
        public Boolean Sp_ResetPassword(UserModel Data)
        {
            Boolean Result = false;
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Login", CommandType.StoredProcedure);
            dB.addIn("@Type", "ResetPassword");
            dB.addIn("@UserId", Data.UserId);
            dB.addIn("@Password", Data.Password);
            dB.addIn("@ModifiedBy", Data.Modified_by);
            dB.addIn("@ModifiedOn", Data.Modified_On);
            dB.ExecuteScalar();
            Result = true;
            return Result;
        }

        public List<UserModel> Sp_EmailList()
        {
            DataTable dt = new DataTable();

            List<UserModel> PM = new List<UserModel>();

            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetEmailList");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel P = new UserModel();
                    P.Email = dr["EMail"].ToString();
                    P.UserId = Guid.Parse(dr["UserId"].ToString());
                    PM.Add(P);
                }
            }
            return PM;
        }
        public List<RstPassword> Sp_GetResetId(Guid Id)
        {
            List<RstPassword> rm = new List<RstPassword>();
            DataTable dt = new DataTable();
            DBHelper db = new DBHelper("SP_Login", CommandType.StoredProcedure);
            db.addIn("@Type", "FetchResetId");
            db.addIn("@UserId", Id);
            dt = db.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RstPassword rt = new RstPassword();
                    rt.ResetId = Guid.Parse(dr["ResetId"].ToString());
                    rm.Add(rt);
                }
            }

            return rm;
        }

        public List<RstPassword> Sp_GetResetList(Guid Id)
        {
            RstPassword rm = new RstPassword();
            DataTable dt = new DataTable();
            DBHelper db = new DBHelper("SP_Master", CommandType.StoredProcedure);
            db.addIn("@Type", "GetResetList");
            db.addIn("@Id", Guid.Parse(Id.ToString()));
            dt = db.ExecuteDataTable();
            List<RstPassword> rt = new List<RstPassword>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rm.ResetId = Guid.Parse(dr["ResetId"].ToString());
                    rm.UserId = Guid.Parse(dr["UserId"].ToString());
                    rt.Add(rm);
                }
            }

            return rt;
        }

        public Boolean SpCheckPassword(string passwordEncrypt,string userId)
        {
            Boolean Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper Db1 = new DBHelper("SP_Master", CommandType.StoredProcedure);
               
                    Db1.addIn("@Type", "Passwordcheck");
                    Db1.addIn("@Password", passwordEncrypt);
                    Db1.addIn("@UserId", userId);
               
                DataTable dt = new DataTable();
                dt = Db1.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }
                else
                {
                    Status = false;
                }
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->Reset_Mail" + ex.Message.ToString());
            }

            return Status;
        }

        public Boolean SpResetCheckPassword(string passwordEncrypt,string username)
        {
            Boolean Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper Db1 = new DBHelper("SP_Master", CommandType.StoredProcedure);
                    Db1.addIn("@Type", "ResetPasswordcheck");
                    Db1.addIn("@Password", passwordEncrypt);
                    Db1.addIn("@UserName", username);
                
                DataTable dt = new DataTable();
                dt = Db1.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }
                else
                {
                    Status = false;
                }
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->Reset_Mail" + ex.Message.ToString());
            }

            return Status;
        }


        public class RstPassword
        {
            public Guid ResetId { get; set; }
            public Guid UserId { get; set; }
        }

        #endregion

        #endregion

        #region General



        public string _salt = "d5cc07aa70fd47";
        //private string constr = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
        public string Encrypt(string st)
        {
            return Cipher.Encrypt(st, _salt);
        }
        public string Decrypt(string st)
        {
            return Cipher.Decrypt(st, _salt);
        }
        public void CreateIfMissing(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public void FileDelete(string path)
        {
            LogHelper _Log = new LogHelper();

            if (Directory.Exists(path))
            {
                File.Delete(path);
                _Log.createLog(path + "---->File Deleted Successfully..!");
            }
        }
        public void SendExcepToDB(string ExceptionMsg, string ExceptionType, string ExceptionURL, string ExceptionSource)
        {
            DBHelper dB = new DBHelper("Sp_ExceptionLogging", CommandType.StoredProcedure);
            int A = 32;
            dB.addIn("@ExceptionMsg", ExceptionMsg);
            dB.addIn("@ExceptionType", ExceptionType);
            dB.addIn("@ExceptionURL", ExceptionURL);
            dB.addIn("@ExceptionSource", ExceptionSource);
            dB.addOut("@ReturnID", SqlDbType.Int, A);
            var R = dB.Execute();
        }

        public static byte[] ConvertToByteArray(string input)
        {
            return input.Select(Convert.ToByte).ToArray();
        }
        public static string ConvertToString(byte[] bytes)
        {
            return new string(bytes.Select(Convert.ToChar).ToArray());
        }
        public static string ConvertToBase64String(byte[] bytes) 
        {
            return Convert.ToBase64String(bytes); 
        }

        #region FirstLastNameCrop

        public string GetFirstandLastName(string LoginId)
        {
            int len = LoginId.Length;
            // to remove any leading or trailing spaces
            //LoginId = LoginId.Trim();

            string t = null;

            if (len == 0)
            {
                t = null;
            }
            else
            {
                t = Char.ToUpper(LoginId[0]).ToString();
            }
            // Traverse rest of the string and print the characters after spaces.
            for (int i = 1; i < len - 1; i++)
            {
                if (LoginId[i] == ' ')
                {
                    t = t + Char.ToUpper(LoginId[i + 1]).ToString();
                }
            }
            return t;
        }
        #endregion

        #endregion

        #region Customer

        public List<CustomerModel> Sp_GetCustomerList()
        {
            List<CustomerModel> CML = new List<CustomerModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Customer", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetCustomers");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CustomerModel CM = new CustomerModel();
                    CM.Customer_ID = Guid.Parse(dr["Customer_ID"].ToString());
                    CM.Company_Name = dr["Company_Name"].ToString();
                    CM.IndustrySector_ID = Convert.ToInt32(dr["IndustrySector_ID"].ToString());
                    CM.Contact = dr["Contact"].ToString();
                    if (dr["DialCode"].ToString() != "")
                    {
                        CM.DialCode = "+" + dr["DialCode"].ToString();
                    }
                    else
                    {
                        CM.DialCode = dr["DialCode"].ToString();
                    }
                    CM.Phone = dr["Phone"].ToString();
                    CM.Email = dr["Email"].ToString();
                    CM.IndustrySector = dr["IndustrySector"].ToString();
                   // CM.Customer_Count = dt.Rows.Count.ToString();
                    CML.Add(CM);
                }
            }
            return CML;
        }

        public List<CustomerModel> Sp_GetIndustrySectorList()
        {
            List<CustomerModel> CML = new List<CustomerModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Customer", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetIndustrySector");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CustomerModel CM = new CustomerModel();
                    CM.IndustrySector = dr["Industry_Sector"].ToString();
                    CM.IndustrySector_ID = Convert.ToInt32(dr["IndustrySector_ID"].ToString());
                    CML.Add(CM);
                }
            }
            return CML;
        }

        public Boolean Sp_CheckCustomersNameAvailability(string namedata, Guid? id)
        {
            LogHelper _log = new LogHelper();
            //CustomerModel CM = new CustomerModel();
            DataTable dt = new DataTable();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_Customer", CommandType.StoredProcedure);
                if (id != null)
                {
                    dB.addIn("@Type", "CheckCustomersNameAvailability1");
                    dB.addIn("@Id", id);
                }
                else
                {
                    dB.addIn("@Type", "CheckCustomersNameAvailability");
                    //dB.addIn("@Id", id);
                }                
                dB.addIn("@CompanyName", namedata);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }                
            }
            catch(Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public Boolean Sp_Customer_Create_Update(CustomerModel customer)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_Customer", CommandType.StoredProcedure);
                if (customer.Customer_ID == Guid.Empty)
                {
                    dB.addIn("@Type", "CustomerCreate");
                }
                else
                {
                    dB.addIn("@Type", "CustomerUpdate");
                    dB.addIn("@Id", customer.Customer_ID);
                    dB.addIn("@TS", customer.TS);
                }

                dB.addIn("@CompanyName", customer.Company_Name);
                dB.addIn("@IndustrySector_ID", customer.IndustrySector_ID);
                dB.addIn("@Contact", customer.Contact);
                dB.addIn("@Countrycode", customer.Countrycode);
                dB.addIn("@Customer_DialCode", customer.DialCode);
                dB.addIn("@Phone", customer.Phone);
                dB.addIn("@Email", customer.Email);
                dB.addIn("@Cre_By", customer.Cre_By);
                dB.addReturn();
                dB.Execute();
                int Ret = dB.getReturned();
                if (Ret==0)
                {
                    Status = true;
                }
                else if (Ret == 1)
                {
                    Status = false;
                }
                else if (Ret == 2)
                {
                    Status = false;
                }

                
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public CustomerModel Sp_GetCustomerById(Guid? id)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Customer", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetCustomerById");
            dB.addIn("@Id", id);
            dt = dB.ExecuteDataTable();
            CustomerModel a = new CustomerModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    a.Customer_ID = Guid.Parse(dr["Customer_ID"].ToString());
                    a.Company_Name = dr["Company_Name"].ToString();
                    a.IndustrySector_ID = Convert.ToInt32(dr["IndustrySector_ID"].ToString());
                    a.Contact = dr["Contact"].ToString();
                    a.Phone = dr["Phone"].ToString();
                    a.Email = dr["Email"].ToString();
                    a.Countrycode = dr["Countrycode"].ToString();
                    a.TS = dr.Field<byte[]>("TS");
                    
                    //a.TS = ConvertToByteArray(dr["TS"].ToString());
                    //a.TS1= ConvertToString(a.TS.ToString());
                    //a.TS= System.Text.Encoding.Unicode.GetBytes(dr["TS"].ToString());
                    //a.TS1 = dr["TS"].ToString();
                }
            }
            return a;
        }


        public Boolean Sp_DeleteCustomerById(Guid? id)
        {
            LogHelper _log = new LogHelper();
            bool Status = false;
            DataTable dt = new DataTable();

            try
            {
                DBHelper dB = new DBHelper("SP_Customer", CommandType.StoredProcedure);
                dB.addIn("@Type", "DeleteCustomerById");
                dB.addIn("@Id", id);
                dt=dB.ExecuteDataTable();
                //Status = true;
                if (dt.Rows.Count > 0)
                    Status = false;
                else
                    Status = true;

            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        #endregion

        #region User

        public List<UserModel> Sp_GetUserList()
        {
            List<UserModel> UML = new List<UserModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_User", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetUsers");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel UM = new UserModel();
                    UM.UserId = Guid.Parse(dr["UserId"].ToString());
                    UM.Name = dr["Name"].ToString();
                    UM.Email = dr["EMail"].ToString();
                    UM.Phone = dr["Phone"].ToString();
                    UM.Countrycode =  dr["Countrycode"].ToString() ;
                    if (dr["DialCode"].ToString() != "")
                    {
                        UM.DialCode = "+" + dr["DialCode"].ToString();
                    }
                    else
                    {
                        UM.DialCode = dr["DialCode"].ToString();
                    }
                    UM.LoginId = dr["LoginId"].ToString();
                    UM.RoleName = dr["RoleName"].ToString();
                    UM.UserType = dr["UserType"].ToString();
                    UM.Customer_Name = dr["Customer_Name"].ToString();
                    if (dr["isActive"].ToString() == "True")
                    {
                        UM.Status = "Active";
                    }
                    else
                    {
                        UM.Status = "In Active";
                    }

                    UML.Add(UM);
                }
            }
            return UML;
        }

        public List<UserModel> Sp_GetUserTypeList()
        {
            List<UserModel> UML = new List<UserModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_User", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetUserTypeList");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel UM = new UserModel();
                    UM.UserType = dr["UserType"].ToString();
                    UM.UserTypeID = Convert.ToInt32(dr["UserTypeID"].ToString());
                    UML.Add(UM);
                }
            }
            return UML;
        }

        public List<UserModel> Sp_GetRoles()
        {
            List<UserModel> UML = new List<UserModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_User", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetRoleMaster");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel UM = new UserModel();
                    UM.RoleName = dr["RoleName"].ToString();
                    UM.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                    UML.Add(UM);
                }
            }
            return UML;
        }

        public Boolean Sp_NameAvailability(string namedata, Guid? id)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_User", CommandType.StoredProcedure);
                if (id != null)
                {
                    dB.addIn("@Type", "NameAvailability1");
                    dB.addIn("@Id", id);
                }
                else
                {
                    dB.addIn("@Type", "NameAvailability");
                }
                dB.addIn("@Name", namedata);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }
        
        public Boolean Sp_UsernameAvailability(string namedata, Guid? id)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_User", CommandType.StoredProcedure);
                if (id != null)
                {
                    dB.addIn("@Type", "UserNameAvailability1");
                    dB.addIn("@Id", id);
                }
                else
                {
                    dB.addIn("@Type", "UserNameAvailability");
                }
                dB.addIn("@LoginId", namedata);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public Boolean Sp_User_Create_Update(UserModel userModel)
        {
            LogHelper _log = new LogHelper();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_User", CommandType.StoredProcedure);
                if (userModel.UserId == Guid.Empty)
                {
                    dB.addIn("@Type", "userCreate");
                }
                else
                {
                    dB.addIn("@Type", "userUpdate");
                    dB.addIn("@Id", userModel.UserId);
                    dB.addIn("@TS", userModel.TS);
                }

                dB.addIn("@Name", userModel.Name);
                dB.addIn("@LoginId", userModel.LoginId);
                dB.addIn("@Password", userModel.Password);
                dB.addIn("@Countrycode", userModel.Countrycode);
                dB.addIn("@DialCode", userModel.DialCode);
                dB.addIn("@Phone", userModel.Phone);
                dB.addIn("@Email", userModel.Email);
                dB.addIn("@UserTypeID", userModel.UserTypeID);
                dB.addIn("@RoleID", userModel.RoleID);
                dB.addIn("@Customer_Id", userModel.Customer_Id);
                dB.addIn("@Cre_By", userModel.Cre_By);

                //dB.ExecuteScalar();
                dB.addReturn();
                dB.Execute();
                int Ret = dB.getReturned();
                if (Ret == 0)
                {
                    Status = true;
                }
                else if (Ret == 1)
                {
                    Status = false;
                }
                else if (Ret == 2)
                {
                    Status = false;
                }

            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public UserModel Sp_GetUserById(Guid? id)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_User", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetUserById");
            dB.addIn("@Id", id);
            dt = dB.ExecuteDataTable();
            UserModel a = new UserModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    a.UserId = Guid.Parse(dr["UserId"].ToString());
                    a.Name = dr["Name"].ToString();
                    a.LoginId = dr["LoginId"].ToString();
                    a.Password = dr["Password"].ToString();
                    a.Phone = dr["Phone"].ToString();
                    a.Email = dr["Email"].ToString();
                    a.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                    a.UserTypeID = Convert.ToInt32(dr["UserTypeID"].ToString());
                    a.Customer_Id = Guid.Parse(dr["Customer_Id"].ToString());
                    a.Customer_Name= dr["Customer_Name"].ToString();
                    a.RoleName = dr["RoleName"].ToString();
                    a.Countrycode = dr["Countrycode"].ToString();
                    a.TS = dr.Field<byte[]>("TS");
                }
            }
            return a;
        }

        public Boolean Sp_DeleteUserById(Guid? id)
        {
            LogHelper _log = new LogHelper();
            bool Status = false;
            DataTable dt = new DataTable();

            try
            {
                DBHelper dB = new DBHelper("SP_User", CommandType.StoredProcedure);
                dB.addIn("@Type", "DeleteUserById");
                dB.addIn("@Id", id);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {

                }
                else
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }
        #endregion

        #region Activity

        public List<ActivityModel> Sp_GetActivityList(Guid ? VersionID)
        {
            
            List<ActivityModel> AMM = new List<ActivityModel>();
            LogHelper _log = new LogHelper();
            try
            {                
                DataTable dt = new DataTable();

                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetActivity");
                dB.addIn("@VersionID", VersionID);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ActivityModel a = new ActivityModel();
                        a.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                        a.Task = dr["Task"].ToString();
                        a.BuildingBlock_id = Convert.ToInt32(dr["BuildingBlock_id"].ToString());
                        a.PhaseID = Convert.ToInt32(dr["PhaseID"].ToString());
                        a.Sequence_Num = Convert.ToInt32(dr["Sequence_Num"].ToString());
                        a.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                        a.ApplicationAreaID = Convert.ToInt32(dr["ApplicationAreaID"].ToString());
                        a.EST_hours1 = dr["EST_hours"].ToString().Replace(".", ":");
                        a.Buildingblock = dr["Buildingblock"].ToString();
                        a.Phase = dr["Phase"].ToString();
                        a.Role = dr["Role"].ToString();
                        a.ApplicationArea = dr["ApplicationArea"].ToString();
                        a.Version_Id = Guid.Parse(dr["Version_Id"].ToString());
                        a.Tasktype = dr["TaskType"].ToString();
                        if (a.Tasktype == "Parallel")
                        {
                            //a.ParallelType = "P" + dr["ParallelType"].ToString();
                            a.ParallelType = dr["Parallel_Name"].ToString();
                            a.ParallelId = Guid.Parse(dr["Parallel_id"].ToString());
                        }


                        AMM.Add(a);
                    }
                }
                
            }
            catch(Exception ex)
            {
                _log.createLog(ex, "");
            }
            return AMM;
        }

        public List<ParallelType> Sp_GetAllParallelType()
        {
            List<ParallelType> ParallelTypesList = new List<ParallelType>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetAllParallelType");
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ParallelType P = new ParallelType();
                        P.ParallelId = Guid.Parse(dr["ParallelId"].ToString());
                        P.ParallelName =Convert.ToInt32(dr["ParallelName"].ToString());
                        P.Parallel_Name = dr["Parallel_Name"].ToString();
                        ParallelTypesList.Add(P);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return ParallelTypesList;

        }
        
        public DataTable Sp_GetExcelActivityList( Guid VID)
        {
            List<ActivityModel> AMM = new List<ActivityModel>();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            try
            {   
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetExcelActivity");
                dB.addIn("@VersionID", VID);
                dt = dB.ExecuteDataTable();
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return dt;
        }
        public List<BuildingBlockModel> Sp_GetBuildingBlocklist(string InstanceId)
        {
            List<BuildingBlockModel> BBM = new List<BuildingBlockModel>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetBuildingBlock");
                if (InstanceId != null)
                {
                    dB.addIn("@InstanceID", InstanceId);
                }
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BuildingBlockModel a = new BuildingBlockModel();
                        a.Block_Name= dr["Block_Name"].ToString();
                        a.block_ID= Convert.ToInt32(dr["block_ID"].ToString());
                        BBM.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return BBM;
        }

        public List<ApplicationAreaModel> Sp_GetApplicationArealist()
        {
            List<ApplicationAreaModel> AAM = new List<ApplicationAreaModel>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetApplicationArea");
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ApplicationAreaModel a = new ApplicationAreaModel();
                        a.ApplicationArea = dr["ApplicationArea"].ToString();
                        a.Id = Convert.ToInt32(dr["Id"].ToString());
                        AAM.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return AAM;
        }

        public List<PhaseModel> Sp_GetPhaselist()
        {
            List<PhaseModel> PM = new List<PhaseModel>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetPhase");
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PhaseModel a = new PhaseModel();
                        a.PhaseName = dr["PhaseName"].ToString();
                        a.Id = Convert.ToInt32(dr["Id"].ToString());
                        PM.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return PM;
        }

        public List<TaskTypeModel> Sp_GetTaskTypelist()
        {
            List<TaskTypeModel> TTM = new List<TaskTypeModel>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetTaskType");
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        TaskTypeModel a = new TaskTypeModel();
                        a.TaskName = dr["TaskName"].ToString();
                        a.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                        TTM.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return TTM;
        }

        public List<ParallelTypeModel> Sp_GetParallelTypelist(int Id, int taskvalue,Guid instanceid, bool first)
        {
            List<ParallelTypeModel> PTM = new List<ParallelTypeModel>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                if (first == false)
                {
                    dB.addIn("@Type", "GetParallelNameByPhase");
                }
                else
                {
                    dB.addIn("@Type", "GetParallelNameByPhase1");
                    dB.addIn("@InstanceID", instanceid);
                }

                dB.addIn("@Phase", Id);
                dB.addIn("@TaskId", taskvalue);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ParallelTypeModel a = new ParallelTypeModel();
                        a.Parallel_Name = dr["Parallel_Name"].ToString();
                        a.ParallelId = Guid.Parse(dr["ParallelId"].ToString());
                        PTM.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return PTM;
        }

        public List<ActivityModel> Sp_GetTaskByParallelType(int Id, Guid Parallel_Id, Guid instanceid,bool first,Guid VID)
        {
            List<ActivityModel> PTM = new List<ActivityModel>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                if (first == false)
                {
                    dB.addIn("@Type", "GetTaskByParallelType");
                    dB.addIn("@VersionID", VID);
                }
                else
                {
                    dB.addIn("@Type", "GetTaskByParallelType1");
                    dB.addIn("@InstanceID", instanceid);
                }
                dB.addIn("@phase", Id);
                dB.addIn("@Parallel_Id", Parallel_Id);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ActivityModel a = new ActivityModel();
                        a.Task = dr["Task"].ToString();
                        a.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                        PTM.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return PTM;
        }

        public List<ActivityModel> Sp_GetAllTask(int Id ,Guid VID)
        {
            List<ActivityModel> PTM = new List<ActivityModel>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetAllTask");
                dB.addIn("@Phase", Id);
                dB.addIn("@VersionID", VID);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ActivityModel a = new ActivityModel();
                        a.Task = dr["Task"].ToString();
                        a.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                        PTM.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return PTM;
        }
       
        public Boolean Sp_CheckTaskAvailability(string namedata, int? id,Guid VID,Guid InsId)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                if (id != null)
                {
                    dB.addIn("@Type", "CheckTaskAvailability1");
                    dB.addIn("@Id", id);
                }
                else if (id == null && VID.ToString()== "00000000-0000-0000-0000-000000000000")
                {
                    dB.addIn("@Type", "CheckTaskAvailability2");
                    dB.addIn("@InstanceID", InsId);
                }
                else
                {
                    dB.addIn("@Type", "CheckTaskAvailability");
                    dB.addIn("@VersionID", VID);
                }
                dB.addIn("@Task", namedata);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public Boolean Activity_Master_Add_Update(ActivityModel AM)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                if (AM.Activity_ID == 0)
                {
                    dB.addIn("@Type", "ADD");
                }
                else
                {
                    dB.addIn("@Type", "Update");
                    dB.addIn("@Activity_ID", AM.Activity_ID);
                }
                dB.addIn("@Task", AM.Task);
                dB.addIn("@BuildingBlock_id", AM.BuildingBlock_id);
                dB.addIn("@PhaseID", AM.PhaseID);
                dB.addIn("@PreviousId", AM.PreviousId);
                dB.addIn("@ApplicationAreaID", AM.ApplicationAreaID);
                dB.addIn("@RoleID", AM.RoleID);
                dB.addIn("@EST_hours", AM.EST_hours);
                dB.addIn("@Cre_By", AM.Modified_by);

                dB.addIn("@PM_Add", 0);
                dB.addIn("@Task_Id", AM.TaskId);
                if (AM.TaskId == 2 && AM.ParallelId != Guid.Empty)
                {
                    dB.addIn("@Parallel_Id", AM.ParallelId);
                }
                dB.addIn("@Parallel_Name", AM.Parallel_Name);
                dB.addIn("@VersionID", AM.Version_Id);
                dB.Execute();

                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public bool Sp_CheckParallel_NameAvailability(String namedata,Guid InstanceId)
        {
            bool status = false;
            DataTable dt = new DataTable();
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("Sp_Activity", CommandType.StoredProcedure);
                if(InstanceId==Guid.Empty)
                {
                    dB.addIn("@Type", "CheckParallel_NameAvailability");
                }
                else
                {
                    dB.addIn("@Type", "CheckParallel_NameAvailability1");
                    dB.addIn("@InstanceID", InstanceId);
                }
               
                dB.addIn("@Parallel_Name", namedata);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public ActivityModel Sp_GetActivityCreationById(int? id)
        {

            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
            dB.addIn("@Type", "EditActivityCreationById");
            dB.addIn("@Activity_ID", id);
            dt = dB.ExecuteDataTable();
            ActivityModel a = new ActivityModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    a.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                    a.Task = dr["Task"].ToString();
                    a.BuildingBlock_id = Convert.ToInt32(dr["BuildingBlock_id"].ToString());
                    a.PhaseID = Convert.ToInt32(dr["PhaseID"].ToString());
                    a.RoleID = Convert.ToInt32(dr["RoleID"].ToString());
                    a.ApplicationAreaID = Convert.ToInt32(dr["ApplicationAreaID"].ToString());
                    a.EST_hours1 = dr["EST_hours"].ToString().Replace(".", ":");
                    a.Buildingblock = dr["Buildingblock"].ToString();
                    a.Phase = dr["Phase"].ToString();
                    a.Role = dr["Role"].ToString();
                    a.ApplicationArea = dr["ApplicationArea"].ToString();
                    a.Tasktype = dr["Task_id"].ToString();                    
                    if (Convert.ToInt32(a.Tasktype) == 2)
                    {
                        a.Parallel_Type = Guid.Parse(dr["Parallel_Id"].ToString());
                        a.ParallelType = dr["Parallel_Name"].ToString();
                    }
                }
            }
            return a;

        }

        public Boolean Sp_DeleteActivityById(int id)
        {
            LogHelper _log = new LogHelper();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                dB.addIn("@Type", "DeleteActivityById");
                dB.addIn("@Activity_ID", id);
                dB.ExecuteScalar();
                Status = true;

            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public List<ParallelType> Sp_GetParallelNames()
        {
            List<ParallelType> L = new List<ParallelType>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetAllActiveParallelType");
            LogHelper _log = new LogHelper();
            try
            {
                dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ParallelType a = new ParallelType();
                    a.ParallelId = Guid.Parse(dr["ParallelId"].ToString());
                    a.ParallelName=Convert.ToInt32(dr["ParallelName"].ToString());
                    a.Parallel_Name= (dr["Parallel_Name"].ToString());
                    L.Add(a);
                }
            }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return L;
        }

        public Boolean Sp_InsertParallel(Array ParallelTypeNames)
        {
            LogHelper _log = new LogHelper();
            bool Status = false;
            try
            {

                for (int i = 0; i < ParallelTypeNames.Length; i++)
                {
                    DBHelper dB = new DBHelper("SP_Activity", CommandType.StoredProcedure);
                    dB.addIn("@Type", "InsertParallelType");
                    dB.addIn("@ParallelTypeName", ((string[])ParallelTypeNames)[i].ToString());
                    dB.ExecuteScalar();
                    Status = true;
                }


                //dB.addIn("@Activity_ID", id);


            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        
        #endregion

        #region ProjectCreation
        public bool SP_CreateProject(Project Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Project", CommandType.StoredProcedure);
                dB.addIn("@Type", "CreateProject");
                dB.addIn("@ProjectName", Data.ProjectName);
                dB.addIn("@ProjectManagerID", Data.ProjectManagerID);
                dB.addIn("@CustomerId", Data.CustomerID);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@Scenario_ID", Data.ScenerioID);
                dB.addIn("@isActive", Data.IsActive);
                dB.addIn("@Cre_By", Data.CreatedBy);

                dB.ExecuteScalar();

                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public bool SP_UpdateProject(Project Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Project", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateProject");
                dB.addIn("@Project_ID", Data.ProjectId);
                dB.addIn("@ProjectName", Data.ProjectName);
                dB.addIn("@ProjectManagerID", Data.ProjectManagerID);
                dB.addIn("@CustomerId", Data.CustomerID);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@Scenario_ID", Data.ScenerioID);
                dB.addIn("@isActive", Data.IsActive);
                dB.addIn("@Cre_By", Data.CreatedBy);

                dB.ExecuteScalar();

                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public List<Project> Sp_Project()
        {
            DataTable dt = new DataTable();

            List<Project> PM = new List<Project>();

            DBHelper dB = new DBHelper("SP_Project", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProject");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    Project P = new Project();
                    P.ProjectId = Guid.Parse(dr["Project_Id"].ToString());
                    P.CustomerID = Guid.Parse(dr["Customer_Id"].ToString());
                    P.ProjectManagerID = Guid.Parse(dr["ProjectManager_Id"].ToString());
                    P.ScenerioID = Convert.ToInt32(dr["ScenarioId"].ToString());

                    P.CustomerName = dr["Company_Name"].ToString();
                    P.ProjectManagerName = dr["ProjectManagerName"].ToString();
                    P.ProjectName = dr["Project_Name"].ToString();
                    P.ScenarioName = dr["ScenarioName"].ToString();
                    P.Description = dr["Description"].ToString();
                    P.Count = Convert.ToInt32(dr["Count"].ToString());
                    P.CreatedOn = Convert.ToDateTime(dr["Cre_on"].ToString());


                    // var obj = P.Instance.ToList();

                    DBHelper dh = new DBHelper("SP_Project", CommandType.StoredProcedure);
                    dh.addIn("@Type", "GetInstance");
                    dt = dh.ExecuteDataTable();
                    List<Instances> It = new List<Instances>();
                    if (dt.Rows.Count > 0)
                    {                        
                        foreach (DataRow dm in dt.Rows)
                        {
                            It.Add(new Instances
                            {
                                Instance_id = Guid.Parse(dm["Instance_id"].ToString()),
                                InstanceName = dm["InstaceName"].ToString(),
                                Cre_By = dm["UserName"].ToString(),
                                Description = dm["Description"].ToString(),
                                Cre_on = Convert.ToDateTime(dm["Cre_on"].ToString()),
                                Project_ID = Guid.Parse(dm["Project_ID"].ToString()),
                                Version_Name = dm["Version_Name"].ToString()
                            });                          
                        }
                    }
                    P.Instance = It;
                    PM.Add(P);
                }
            }

            return PM;
        }

        public Project SP_GetProject(Guid Id)
        {
            DataSet ds = new DataSet();
            Project P = null;
            DBHelper dB = new DBHelper("SP_Project", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectDetail");
            dB.addIn("@Project_ID", Guid.Parse(Id.ToString()));
            ds = dB.ExecuteDataSet();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    P = new Project()
                    {
                        ProjectId = Guid.Parse(dr["Project_Id"].ToString()),
                        CustomerID = Guid.Parse(dr["Customer_Id"].ToString()),
                        ProjectManagerID = Guid.Parse(dr["ProjectManager_Id"].ToString()),
                        ScenerioID = Convert.ToInt32(dr["ScenarioId"].ToString()),

                        CustomerName = dr["Company_Name"].ToString(),
                        ProjectManagerName = dr["ProjectManagerName"].ToString(),
                        ProjectName = dr["Project_Name"].ToString(),
                        ScenarioName = dr["ScenarioName"].ToString(),
                        Description = dr["Description"].ToString(),
                        BuildingBlockName = dr["BuildingBlockName"].ToString(),
                        //Count = Convert.ToInt32(dr["Count"].ToString()),
                        CreatedOn = Convert.ToDateTime(dr["Cre_on"].ToString()),
                        Scenario_Status = Convert.ToBoolean(dr["Scenario_Status"].ToString()),
                        // var obj = P.Instance.ToList();
                    };
                    count = count++;
                }
            }
            return P;
        }

        public List<Project> SP_GetProjectDrpDwnList(Guid Id)
        {
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_Project", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectDrpList");
            dB.addIn("@Project_ID", Guid.Parse(Id.ToString()));
            ds = dB.ExecuteDataSet();

            List<Project> PM = new List<Project>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    Project P = new Project();

                    P.ProjectName = dr["Project_Name"].ToString();
                    P.ProjectID = dr["Project_Id"].ToString();
                    PM.Add(P);
                    count = count++;
                }
            }
            return PM;
        }


        //public bool Sp_ProjectDelete(Project Data)
        //{
        //    bool status = false;
        //    LogHelper _log = new LogHelper();
        //    DataSet ds = new DataSet();
        //    DBHelper dB = new DBHelper("SP_Project", CommandType.StoredProcedure);
        //    dB.addIn("@Type", "InstanceCount");
        //    dB.addIn("@Project_ID", Data.ProjectId);
        //    ds = dB.ExecuteDataSet();
        //    if (ds.Tables.Count > 0)
        //    {
        //        DataTable dt = new DataTable();
        //        dt = ds.Tables[0];
        //        Project P = new Project();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            P.Count = Convert.ToInt32(dr["Count"].ToString());
        //        }
        //        if (P.Count == 0)
        //        {
        //            try
        //            {
        //                DataTable dt1 = new DataTable();
        //                DBHelper dB2 = new DBHelper("SP_Project", CommandType.StoredProcedure);
        //                dB2.addIn("@Type", "Delete");
        //                dB2.addIn("@Project_ID", Data.ProjectId);
        //                dB2.addIn("@isDelete", Data.IsDelete);
        //                dB2.addIn("@isActive", Data.IsActive);
        //                dt1=dB2.ExecuteDataTable();

        //                if (dt1.Rows.Count > 0)
        //                {

        //                }
        //                else
        //                {
        //                    status = true;
        //                }                       
        //            }
        //            catch (Exception ex)
        //            {
        //                _log.createLog(ex, "");
        //            }
        //        }
        //        else
        //        {
        //            status = false;
        //        }
        //    }
        //    else
        //    {
        //        status = false;
        //    }    
        //    return status;
        //}

        public int Sp_ProjectDelete(Guid id)
        {
            Project P = new Project();
            LogHelper _log = new LogHelper();
            int a=0;
            try
            {
                DataTable dt1 = new DataTable();
                DBHelper dB2 = new DBHelper("SP_Project", CommandType.StoredProcedure);
                dB2.addIn("@Type", "Delete");
                dB2.addIn("@Project_ID", id);                
                dt1 = dB2.ExecuteDataTable();

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        P.Ins_Count = Convert.ToInt32(dr["InsCount"].ToString());
                        P.PM_Count = Convert.ToInt32(dr["PMCount"].ToString());
                    }
                    if (P.Ins_Count>0)
                    {
                        a = 1;
                    }
                    else if (P.PM_Count > 0)
                    {
                        a = 2;
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return a;
        }
        public List<UserModel> SP_GetProjectManager()
        {
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectManager");
            ds = dB.ExecuteDataSet();

            List<UserModel> PM = new List<UserModel>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel P = new UserModel();

                    P.ProjectManagerName = dr["Name"].ToString();
                    P.ProjectManagerId = Guid.Parse(dr["UserId"].ToString());
                    PM.Add(P);
                    count = count++;
                }
            }
            return PM;
        }
        public List<Scenario> Sp_GetScenario()
        {
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetScenario");
            ds = dB.ExecuteDataSet();

            List<Scenario> PM = new List<Scenario>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    Scenario P = new Scenario();

                    P.ScenarioName = dr["ScenarioName"].ToString();
                    P.ScenarioId =Convert.ToInt32( dr["ScenarioId"].ToString());
                    PM.Add(P);
                    count = count++;
                }
            }
            return PM;
        }
        public List<int> Sp_GetScenario_Status()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetScenario_Status");
            dt = dB.ExecuteDataTable();
            var dataList = new List<int>();
            List<DataRow> rows = dt.Rows.Cast<DataRow>().ToList();
            foreach (var item in rows)
            {
                dataList.Add(Convert.ToInt32(item.ItemArray[0].ToString()));
            }
            return dataList;
        }
        public List<CustomerModel> Sp_GetCompany()
        {
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetCustomerName");
            ds = dB.ExecuteDataSet();

            List<CustomerModel> PM = new List<CustomerModel>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    CustomerModel P = new CustomerModel();

                    P.Company_Name = dr["Company_Name"].ToString();
                    P.Customer_ID = Guid.Parse(dr["Customer_ID"].ToString());
                    PM.Add(P);
                    count = count++;
                }
            }
            return PM;
        }

        public Boolean SP_GetProjName(string ProjectName,Guid? Id)
        {
            bool status = false;
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectName");
            dB.addIn("@ProjectName", ProjectName);
            dB.addIn("@Id", Id);

            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                status = true;
            }
            else
            {
                DataTable dt2 = new DataTable();
                DBHelper dB2 = new DBHelper("SP_Master", CommandType.StoredProcedure);
                dB2.addIn("@Type", "GetProjectAvailability");
                dB2.addIn("@ProjectName", ProjectName);
                dt2 = dB2.ExecuteDataTable();
                if (dt2.Rows.Count > 0)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            return status;
        }

        #endregion


        #region Instance CRUD
        public Boolean Sp_InstanceClone(Instances Data, Guid Previous_Instance)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);

                dB.addIn("@Type", "InstanceClone");
                dB.addIn("@Project_ID", Data.Project_ID);
                dB.addIn("@InstaceName", Data.InstanceName);
                dB.addIn("@Previous_Instance", Previous_Instance);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@Cre_By", Data.Cre_By);
                dB.Execute();
                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public List<Project> SP_GetProjectByCustomer(Guid LoginId,Guid Id)
        {
            DataTable dt = new DataTable();

            List<Project> PM= new List<Project>();

            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectByCustomer");
            dB.addIn("@Id", Id);
            dB.addIn("@LoginId", LoginId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Project P = new Project();
                    P.ProjectId = Guid.Parse(dr["Project_Id"].ToString());
                    P.ProjectName = dr["Project_Name"].ToString();
                    PM.Add(P);
                }
            }
            return PM;
        }

        public List<Instances> SP_GetCloneInstance(Guid Id)
        {
            DataTable dt = new DataTable();

            List<Instances> PM = new List<Instances>();

            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetInstanceByProject"); //CloneGetInstance
            dB.addIn("@Id", Id);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    Instances P = new Instances();
                    P.Instance_id = Guid.Parse(dr["Instance_id"].ToString());
                    P.InstanceName =dr["InstaceName"].ToString();
                    PM.Add(P);
                }

            }
            return PM;
        }
        public List<Instances> Sp_GetProjectList()
        {
            DataTable dt = new DataTable();

            List<Instances> PM = new List<Instances>();

            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetAllProjectNames");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    Instances P = new Instances();
                    //P.Instance_id = Guid.Parse(dr["Instance_id"].ToString());
                    P.ProjectName = dr["Project_Name"].ToString();
                    P.Project_ID = Guid.Parse(dr["Project_Id"].ToString());
                    PM.Add(P);
                }

            }
            return PM;
        }

        public List<BuildingBlockModel> Sp_GetScenario_BuildingBlock(int id)
        {
            List<BuildingBlockModel> BBM = new List<BuildingBlockModel>();
            
            try
            {
                
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("Sp_Project", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetScenario_BuildingBlock");
                dB.addIn("@Scenario_ID", id);
                dt = dB.ExecuteDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    BuildingBlockModel bb = new BuildingBlockModel();
                    bb.Block_Name = dr["Block_Name"].ToString();
                    BBM.Add(bb);
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return BBM;

        }
        public bool Sp_InstaCreate(Instances Data, DataTable SALESDATA, DataTable BILLSDATA, DataTable ORDERSDATA, DataTable FACOUNTDATA, DataTable SAPDATA1, DataTable SAPDATA2,DataTable SAPDATA3, DataTable SAPDATA4)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
                dB.addIn("@Type", "CreateInstance");

                dB.addIn("@Project_ID", Data.Project_ID);
                dB.addIn("@Version_ID", Data.Version_ID);
                dB.addIn("@InstaceName", Data.InstanceName);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@Sys_Landscape", Data.Sys_Landscape);
                
                dB.addIn("@destinationName", Data.destinationName);
                dB.addIn("@AppServerHost", Data.AppServerHost);
                dB.addIn("@SystemNumber", Data.SystemNumber);
                dB.addIn("@SAPRouter", Data.SAPRouter);
                dB.addIn("@SAPUser", Data.User);
                dB.addIn("@SAPPassword", Data.Password);
                dB.addIn("@Client", Data.Client);
                dB.addIn("@Language", Data.Language);
                dB.addIn("@PoolSize", Data.PoolSize);

                dB.addIn("@Cre_By", Data.Cre_By);

                if (Data.Sys_Landscape == "Automation")
                {
                    dB.addIn("@tblSalesDoc", SALESDATA);
                    dB.addIn("@tblBillingDoc", BILLSDATA);
                    dB.addIn("@tblOrderDoc", ORDERSDATA);
                    dB.addIn("@tblMatScore", FACOUNTDATA);
                    dB.addIn("@tblRFCFM", SAPDATA1);
                    dB.addIn("@tblSOFTWARECOMPONENT", SAPDATA2);
                    dB.addIn("@tblPRODUCTVERSIONS", SAPDATA3);
                    dB.addIn("@tblSFW5REPORT", SAPDATA4);
                }

                dB.ExecuteScalar();

                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public Instances Sp_GetInstance(Guid Id)
        {
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Instances P = new Instances();
            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetInstanceData");
            dB.addIn("@Id", Guid.Parse(Id.ToString()));
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    P.Instance_id = Guid.Parse(dr["Instance_id"].ToString());

                    P.InstanceName = dr["InstaceName"].ToString();
                    P.Cre_By = dr["UserName"].ToString();
                    P.Description = dr["Description"].ToString();
                    P.Cre_on = Convert.ToDateTime(dr["Cre_on"].ToString());
                    P.Project_ID = Guid.Parse(dr["PID"].ToString());
                    P.Sys_Landscape = dr["Sys_Landscape"].ToString();
                    P.Version_ID = Guid.Parse(dr["VID"].ToString());

                    if (dr["Sys_Landscape"].ToString() == "Automation")
                    {
                        P.destinationName = dr["DestinationName"].ToString();
                        P.AppServerHost = dr["AppServerHost"].ToString();
                        P.SystemNumber = dr["SystemNumber"].ToString();
                        P.SAPRouter = dr["SAPRouter"].ToString();
                        P.User = dr["SAPUser"].ToString();
                        P.Password = dr["SAPPassword"].ToString();
                        P.Client = dr["Client"].ToString();
                    }
                }
            }
            return P;
        }

        public bool SP_UpdateInstance(Instances Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateInstance");
                dB.addIn("@Id", Data.Instance_id);
                dB.addIn("@Project_ID", Data.Project_ID);
                dB.addIn("@InstaceName", Data.InstanceName);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@ModifiedBy", Data.Modified_by);
                dB.addIn("@Sys_Landscape", Data.Sys_Landscape);

                dB.addIn("@destinationName", Data.destinationName);
                dB.addIn("@AppServerHost", Data.AppServerHost);
                dB.addIn("@SystemNumber", Data.SystemNumber);
                dB.addIn("@SAPRouter", Data.SAPRouter);
                dB.addIn("@SAPUser", Data.User);
                dB.addIn("@SAPPassword", Data.Password);
                dB.addIn("@Client", Data.Client);

                dB.ExecuteScalar();

                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public bool Sp_Delete(Instances Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            try
            {
                DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
                dB.addIn("@Type", "Delete");
                dB.addIn("@Id", Data.Instance_id);
                dB.addIn("@isDelete", Data.IsDeleted);
                dB.addIn("@isActive", Data.isActive);
                dt = dB.ExecuteDataTable();



                if (dt.Rows.Count > 0)
                {



                }
                else
                {
                    status = true;
                }

            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }


        public Boolean SP_GetInstaName(string InstaName, Guid? ProjectId, Guid? InsId)
        {
            bool status = false;
            DataTable ds = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectInstance");
            dB.addIn("@InstanceName", InstaName);
            dB.addIn("@ProjectId", ProjectId);
            dB.addIn("@Id", InsId);

            ds = dB.ExecuteDataTable();

            Instances PM = new Instances();
            if (ds.Rows.Count > 0)
            {
                PM.InstanceName = ds.Rows[0][0].ToString();
                if (PM.InstanceName == InstaName)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                DataTable dt2 = new DataTable();
                DBHelper dB2 = new DBHelper("SP_Master", CommandType.StoredProcedure);
                dB2.addIn("@Type", "GetInstanceAvailability");
                dB2.addIn("@InstanceName", InstaName);
                dB2.addIn("@ProjectId", ProjectId);
                dt2 = dB2.ExecuteDataTable();

                if (dt2.Rows.Count > 0)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            return status;
        }


        #endregion


        #region RoleCrud

        public bool SP_CreateRole(Role Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Role", CommandType.StoredProcedure);
                dB.addIn("@Type", "CreateRole");
                dB.addIn("@RoleName", Data.RoleName);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@CreatedBy", Data.CreatedBy);
                dB.addIn("@isActive", Data.IsActive);
                dB.ExecuteScalar();

                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }


        public List<Role> SP_GetRole()
        {
            DataTable dt = new DataTable();

            List<Role> PM = new List<Role>();

            DBHelper dB = new DBHelper("SP_Role", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetRole");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    Role P = new Role();
                    P.RoleId = Convert.ToInt32(dr["RoleId"].ToString());
                    P.RoleName = dr["RoleName"].ToString();
                    P.Description = dr["Description"].ToString();
                    P.IsActive = Convert.ToBoolean(dr["isActive"].ToString());
                    P.CreatedOn = Convert.ToDateTime(dr["Cre_on"].ToString());
                    PM.Add(P);

                }
            }
            return PM;
        }


        public Role SP_GetRole(int Id)
        {
            DataSet ds = new DataSet();
            Role Rp = null;
            DBHelper dB = new DBHelper("SP_Role", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetRoleDetail");
            dB.addIn("@RoleId", Convert.ToInt32(Id.ToString()));
            ds = dB.ExecuteDataSet();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    Rp = new Role()
                    {
                        RoleId = Convert.ToInt32(dr["RoleId"].ToString()),
                        RoleName = dr["RoleName"].ToString(),
                        Description = dr["Description"].ToString(),
                        CreatedOn = Convert.ToDateTime(dr["Cre_on"].ToString()),
                        TS = dr.Field<byte[]>("TS"),
                };
                    count = count++;
                }
            }
            return Rp;
        }

        public bool SP_UpdateRole(Role Data)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Role", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateRole");
                dB.addIn("@RoleId", Data.RoleId);
                dB.addIn("@RoleName", Data.RoleName);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@ModifiedBy", Data.ModifiedBy);
                dB.addIn("@isActive", Data.IsActive);
                dB.addIn("@TS", Data.TS);

                //dB.ExecuteScalar();
                dB.addReturn();
                dB.Execute();
                int Ret = dB.getReturned();
                if (Ret == 0)
                {
                    Status = true;
                }
                else if (Ret == 1)
                {
                    Status = false;
                }
                else if (Ret == 2)
                {
                    Status = false;
                }
                //status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public bool Sp_DeleteRole(Role Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_Project", CommandType.StoredProcedure);
            dB.addIn("@Type", "RoleCount");
            dB.addIn("@RoleId", Data.RoleId);
            ds = dB.ExecuteDataSet();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                Role P = new Role();
                foreach (DataRow dr in dt.Rows)
                {
                    P.Count = Convert.ToInt32(dr["RId"].ToString());
                }
                if (P.Count == 0)
                {
                    try
                    {
                        DBHelper dB2 = new DBHelper("Sp_Role", CommandType.StoredProcedure);
                        dB2.addIn("@Type", "Delete");
                        dB2.addIn("@RoleId", Data.RoleId);
                        dB2.addIn("@isDelete", Data.IsDelete);
                        dB2.addIn("@isActive", Data.IsActive);
                        dB2.ExecuteScalar();

                        status = true;
                    }
                    catch (Exception ex)
                    {
                        _log.createLog(ex, "");
                    }
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }
        public Boolean SP_GetRoleName(string RoleName,int? Id)
        {
            bool status = false;
            DataTable ds = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetRole");
            dB.addIn("@RoleName", RoleName);
            dB.addIn("@RoleId", Id);
            ds = dB.ExecuteDataTable();

            List<Role> PM = new List<Role>();
            if (ds.Rows.Count > 0)
            {
                status = true;
            }
            else
            {
                DataTable dt = new DataTable();
                DBHelper dB2 = new DBHelper("SP_Master", CommandType.StoredProcedure);
                dB2.addIn("@Type", "GetRoleAvailable");
                dB2.addIn("@RoleName", RoleName);
                
                dt = dB2.ExecuteDataTable();
                if(dt.Rows.Count>0)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            return status;
        }
        
        #endregion


        #region PMCRUD
        public List<PMTask> Sp_PMDetail()
        {
            List<PMTask> PTM = new List<PMTask>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "PMData");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PMTask a = new PMTask();
                    a.PMTaskId = Guid.Parse(dr["PMTaskId"].ToString());
                    a.PMTaskName = dr["PMTaskName"].ToString();
                    a.PMTaskCategoryID = Convert.ToInt32(dr["PMTaskCategoryID"].ToString());
                    a.EST_Hours = dr["EST_hours"].ToString().Replace(".", ":");
                    a.PMTaskCategory = dr["PMTaskCategory"].ToString();

                    PTM.Add(a);
                }
            }

            return PTM;
        }

        public List<PMTask> Sp_GetMList()
        {
            DataTable dt = new DataTable();

            List<PMTask> PM = new List<PMTask>();

            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetTaskDetail");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    PMTask P = new PMTask();
                    //P.Instance_id = Guid.Parse(dr["Instance_id"].ToString());
                    P.PMTaskCategoryID = Convert.ToInt32(dr["ID"].ToString());
                    P.PMTaskCategory = dr["Name"].ToString();
                    PM.Add(P);
                }

            }
            return PM;
        }

        public bool SP_CreatePM(PMTask Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "PMAdd");
                dB.addIn("@PMTaskName", Data.PMTaskName);
                dB.addIn("@PMTaskCategoryID", Data.PMTaskCategoryID);
                dB.addIn("@Cre_By", Data.Cre_By);
                dB.addIn("@EST_hours", Data.EST_hours);
                dB.addIn("@isActive", Data.isActive);
                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public PMTask Sp_GetPMtaskById(Guid? id)
        {

            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "EditPMTaskById");
            dB.addIn("@PMTaskId", id);
            dt = dB.ExecuteDataTable();
            PMTask a = new PMTask();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    a.PMTaskId = Guid.Parse(dr["PMTaskId"].ToString());
                    a.PMTaskName = dr["PMTaskName"].ToString();
                    a.PMTaskCategoryID = Convert.ToInt32(dr["PMTaskCategoryID"].ToString());
                    a.EST_Hours = dr["EST_hours"].ToString().Replace(".", ":");
                    a.Cre_on = DateTime.Parse(dr["Cre_on"].ToString());
                    a.Cre_By = Guid.Parse(dr["Cre_By"].ToString());
                    a.PMtTaskCatName = dr["PmTaskCatName"].ToString();
                    a.TS = dr.Field<byte[]>("TS");
                }
            }
            return a;
        }
        public List<PMTask> Sp_GetTaskCategoryById()
        {
            List<PMTask> _List = new List<PMTask>();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetTaskDetail");
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PMTask a = new PMTask();

                    a.PMTaskCategoryID = Convert.ToInt32(dr["ID"].ToString());
                    a.PMtTaskCatName = dr["Name"].ToString();
                    //a.PMTaskId = Guid.Parse(dr["PMTaskId"].ToString());
                    _List.Add(a);

                }
            }
            return _List;
        }

        public bool SP_UpdatePM(PMTask Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "PMUpdate");
                dB.addIn("@PMTaskId", Data.PMTaskId);
                dB.addIn("@PMTaskName", Data.PMTaskName);
                dB.addIn("@PMTaskCategoryID", Data.PMTaskCategoryID);
                dB.addIn("@ModifiedBy", Data.Cre_By);
                dB.addIn("@EST_hours", Data.EST_hours);
                dB.addIn("@isActive", Data.isActive);
                dB.addIn("@TS", Data.TS);
                //dB.ExecuteScalar();
                //status = true;
                dB.addReturn();
                dB.Execute();
                int Ret = dB.getReturned();
                if (Ret == 0)
                {
                    status = true;
                }
                else if (Ret == 1)
                {
                    status = false;
                }
                else if (Ret == 2)
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public bool Sp_DeletePM(PMTask Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "Delete");
                dB.addIn("@PMTaskId", Data.PMTaskId);
                dB.addIn("@isDelete", Data.IsDeleted);
                dB.addIn("@isActive", Data.isActive);
                dB.ExecuteScalar();

                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public Boolean SP_GetTaskName(string TaskName,Guid? Pid)
        {
            bool status = false;
            DataTable ds = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetPmTask");
            dB.addIn("@PmtaskName", TaskName);
            dB.addIn("@PmId", Pid);

            ds = dB.ExecuteDataTable();

            List<PMTask> PM = new List<PMTask>();
            if (ds.Rows.Count > 0)
            {
                status = true;
            }
            else
            {
                DataTable dt = new DataTable();
                DBHelper dB2 = new DBHelper("SP_Master", CommandType.StoredProcedure);
                dB2.addIn("@Type", "GetPmTaskAvailable");
                dB2.addIn("@PmtaskName", TaskName);

                dt = dB2.ExecuteDataTable();

                if(dt.Rows.Count>0)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            return status;
        }
        #endregion

        #region Scenario
        public List<Scenario> Sp_Scenario()
        {
            List<Scenario> scenarios = new List<Scenario>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetScenario");
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Scenario a = new Scenario();
                    a.ScenarioId = Convert.ToInt32(dr["ScenarioId"].ToString());
                    a.ScenarioName = dr["ScenarioName"].ToString();
                    a.BuildingBlock_Id = dr["BuildingBlock_Id"].ToString();
                    List<int> TagIds = a.BuildingBlock_Id.Split(',').Select(int.Parse).ToList();

                    a.TagIds = TagIds;
                    //a.EST_Hours = dr["EST_hours"].ToString().Replace(".", ":");
                    //a.PMTaskCategory = dr["PMTaskCategory"].ToString();

                    scenarios.Add(a);
                }
            }

            return scenarios;
        }

        public Scenario Sp_GetScenarioById(int? id)
        {

            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
            dB.addIn("@Type", "EditScenarioById");
            dB.addIn("@ScenarioId", id);
            dt = dB.ExecuteDataTable();
            Scenario a = new Scenario();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    
                    a.ScenarioId = Convert.ToInt32(dr["ScenarioId"].ToString());
                    a.ScenarioName = dr["ScenarioName"].ToString();
                }
            }
            return a;
        }

        public List<BuildingBlockModel> Sp_GetBuldingBlock()
        {
            List<BuildingBlockModel> buildingBlocks = new List<BuildingBlockModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetBuildingBlock");
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BuildingBlockModel a = new BuildingBlockModel();
                    a.block_ID = Convert.ToInt32(dr["block_ID"].ToString());
                    a.Block_Name = dr["Block_Name"].ToString();
                    //a.PMTaskCategoryID = Convert.ToInt32(dr["PMTaskCategoryID"].ToString());
                    //a.EST_Hours = dr["EST_hours"].ToString().Replace(".", ":");
                    //a.PMTaskCategory = dr["PMTaskCategory"].ToString();

                    buildingBlocks.Add(a);
                }
            }

            return buildingBlocks;
        }

        public bool SP_UpdateScenario(Scenario SC)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateScenario");
                dB.addIn("@ScenarioName", SC.ScenarioName);
                dB.addIn("@ScenarioId", SC.ScenarioId);
                dB.addIn("@BuildingBlock_Id", SC.BuildingBlock_Id);
                dB.addIn("@ModifiedBy", SC.Modified_by);
                dB.addReturn();
                dB.Execute();
                int Ret = dB.getReturned();
                if (Ret == 0)
                {
                    status = true;
                }
                else if (Ret == 1)
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }

        public bool Sp_DeleteScenario(Scenario Data)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
                dB.addIn("@Type", "Sp_DeleteScenario");
                dB.addIn("@ScenarioId", Data.ScenarioId);
                dB.addIn("@ModifiedBy", Data.Modified_by);
                dB.addReturn();
                dB.Execute();
                int Ret = dB.getReturned();

                if (Ret == 0)
                {
                    status = true;
                }
                else if (Ret == 1)
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }
        public bool SP_ScenarioCreation (Scenario SC)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
                dB.addIn("@Type", "CreateScenario");
                dB.addIn("@ScenarioName", SC.ScenarioName);
                dB.addIn("@BuildingBlock_Id", SC.BuildingBlock_Id);
                dB.addIn("@Cre_By", SC.Cre_By);
                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }
        public Scenario SP_GetScenariobyid(int id)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetScenariobyid");
            dB.addIn("@ScenarioId", id);
            dt = dB.ExecuteDataTable();
            Scenario SC = new Scenario();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    SC.ScenarioId = Convert.ToInt32(dr["ScenarioId"].ToString());
                    SC.ScenarioName = dr["ScenarioName"].ToString();
                    SC.BuildingBlock_Id= dr["BuildingBlock_Id"].ToString();
                }
            }

            return SC;
        }

        public Boolean SP_GetScenarioName(string ScenarioName,int ?ID)
        {
            bool status = false;
            DataTable dt = new DataTable();
            DBHelper dB2 = new DBHelper("SP_Scenario", CommandType.StoredProcedure);
            dB2.addIn("@Type", "GetScenarioName");
            dB2.addIn("@ScenarioName", ScenarioName);
            dB2.addIn("@ScenarioId", ID);

            dt = dB2.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                status = false;
            }
            else
            {
                status = true;
            }
            return status;
        }

        #endregion


        #region Version
        public List<AMVersion> Sp_GetVersion()
        {
            List<AMVersion> L = new List<AMVersion>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Version", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetVersion");
            LogHelper _log = new LogHelper();
            try
            {
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AMVersion a = new AMVersion();
                        a.ID = Guid.Parse(dr["ID"].ToString());
                        a.Version_Name = dr["Version_Name"].ToString();
                        a.FileName = Guid.Parse(dr["FileName"].ToString());
                        a.isActive = Convert.ToBoolean(dr["isActive"].ToString());
                        a.CreatedOn = Convert.ToDateTime(dr["Cre_On"].ToString());
                        //a.Modified_By = Guid.Parse(dr["Modified_By"].ToString());
                        L.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return L;
        }
        public string Sp_GetVersionName(Guid VersionID)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            string VersionName = null;
            try
            {
                DBHelper dB = new DBHelper("SP_Version", CommandType.StoredProcedure);
                if (VersionID != null)
                {
                    dB.addIn("@Type", "GetVersionName");
                    dB.addIn("@VersionID", VersionID);
                    dt = dB.ExecuteDataTable();
                    //VersionName = dB.ExecuteScalar().ToString();
                }


                if (dt.Rows.Count == 1)
                {
                    VersionName = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return VersionName;
        }

        public AMVersion SP_GetVersionbyid(Guid id)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Version", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetVersionbyid");
            dB.addIn("@VersionId", id);
            dt = dB.ExecuteDataTable();

            AMVersion V = new AMVersion();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    V.ID = Guid.Parse(dr["Id"].ToString());
                    V.Version_Name = dr["Version_Name"].ToString();
                    V.FileName = Guid.Parse(dr["FileName"].ToString());
                    V.isActive = Convert.ToBoolean(dr["isActive"].ToString());
                }
            }

            return V;
        }


        public bool SP_UpdateVersion(AMVersion Data)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Version", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateVersion");
                dB.addIn("@VersionId", Data.ID);
                dB.addIn("@VersionName", Data.Version_Name);
                dB.addIn("@Modified_By", Data.Modified_By);
                dB.Execute();
                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }
        #endregion
        #region MailSettings
        public List<MailModel> GetMailList()
        {
            List<MailModel> ListM = new List<MailModel>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Mail", CommandType.StoredProcedure);
            dB.addIn("@Type", "MailList");
            //dB.addIn("@InstunceID", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MailModel M = new MailModel();
                    M.Name = dr["Name"].ToString();
                    M.Running_ID = Convert.ToInt32(dr["Running_ID"].ToString());
                    M.To = Convert.ToString(dr["TO"].ToString());
                    M.Subject = dr["Subject"].ToString();
                    M.Body = dr["Body"].ToString();
                    M.TemplateName = dr["FileName"].ToString();
                    M.Q_UserID = Convert.ToInt32(dr["Q_UserID"].ToString());
                    if (dr["LineID"].ToString() != "")
                    {
                        M.LineID = Guid.Parse(dr["LineID"].ToString());
                    }                        
                    ListM.Add(M);
                }
            }
            return ListM;
        }
        public Boolean UpdateMailList(int ID)
        {
            Boolean Res = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Mail", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateMailList");
                dB.addIn("@Running_ID", ID);
                dB.ExecuteScalar();
                Res = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->UpdateMailList" + ex.Message.ToString());
            }
            return Res;
        }
        public List<Role> SP_GetRoleList()
        {
            DataTable dt = new DataTable();

            List<Role> PM = new List<Role>();

            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetRoleList");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    Role P = new Role();
                    P.RoleId = Convert.ToInt32(dr["RoleId"].ToString());
                    P.RoleName = dr["RoleName"].ToString();
                    P.Description = dr["Description"].ToString();
                    P.IsActive = Convert.ToBoolean(dr["isActive"].ToString());
                    P.CreatedOn = Convert.ToDateTime(dr["Cre_on"].ToString());
                    PM.Add(P);

                }
            }
            return PM;
        }
        public List<ProjectMonitorModel> SP_GetProjectManagerList()
        {
            DataTable dt = new DataTable();

            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();

            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectManagerId");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.Id = Guid.Parse(dr["ProjectManager_Id"].ToString());
                    PM.Add(P);

                }
            }
            return PM;
        }
        public UserModel Sp_GetMailList(Guid? LineID)
        {
            DataTable dt = new DataTable();


            UserModel P = new UserModel();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectManagerEmail");
            dB.addIn("@Id", LineID);

            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    P.Email = dr["EMail"].ToString();
                }
            }
            return P;
        }
        public MailModel GetEmailStatus(int ID)
        {
            DataTable dt = new DataTable();

            //List<MailModel> PM = new List<MailModel>();

            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetEmailStatus");
            dB.addIn("@Runningid", ID);
            dt = dB.ExecuteDataTable();
            MailModel P = new MailModel();
            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {                   
                    P.MailStatus = Convert.ToBoolean(dr["MailStatus"].ToString());
                    //PM.Add(P);
                }
            }
            return P;
        }

        private Boolean UpdateMonitor_Mail(ProjectMonitorModel PM)
        {
            Boolean Status = false;

            LogHelper _log = new LogHelper();
            try
            {
                DBHelper Db1 = new DBHelper("SP_Mail", CommandType.StoredProcedure);
                Db1.addIn("@Type", "ProjectMonitor");
                Db1.addIn("@PhaseID", PM.PhaseId);
                Db1.addIn("@InstanceID", PM.Instance);
                Db1.addIn("@PM_ID", PM.Id);
                Db1.addIn("@Cre_By", PM.Cre_By);
                DataTable dt = new DataTable();
                dt = Db1.ExecuteDataTable();

                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->UpdateMonitor_Mail" + ex.Message.ToString());
            }

            return Status;
        }
        #endregion



        


        #region ResourcePlanning
        public GeneralList spCustomerDropdown(string Id, int type)
        {
            GeneralList sP_ = new GeneralList();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_CustomerDrp", CommandType.StoredProcedure);
            if (type == 1)
            {
                dB.addIn("@Type", "CustomerDrp_Admin");

            }
            else if (type == 2)
            {
                dB.addIn("@Type", "CustomerDrp_Consultant");
                dB.addIn("@Id", Id);
            }
            else if (type == 3)
            {
                dB.addIn("@Type", "CustomerDrp_Customer");
                dB.addIn("@Id", Id);
            }

            ds = dB.ExecuteDataSet();
            List<Lis> _Lob = new List<Lis>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(new Lis
                    {
                        Name = dr["Name"].ToString(),
                        Value = dr["id"].ToString()
                    });
                    count = count++;
                }
            }
            sP_._List = _Lob;
            return sP_;
        }

        public List<Instances> Sp_GetProjectListByUser(Guid LoginId, int UserTypeID)
        {
            DataTable dt = new DataTable();

            List<Instances> PM = new List<Instances>();

            DBHelper dB = new DBHelper("SP_ProgressMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "ProjectNamesByUser");
            dB.addIn("@LoginId", LoginId);
            dB.addIn("@UserTypeID", UserTypeID);

            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    Instances P = new Instances();
                    P.ProjectName = dr["Project_Name"].ToString();
                    P.Project_ID = Guid.Parse(dr["Project_Id"].ToString());
                    PM.Add(P);
                }

            }
            return PM;
        }
        public List<ActivityMasterModel> Sp_GetActivityMasterData(Guid? Id)
        {
            List<ActivityMasterModel> AMM = new List<ActivityMasterModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetResource");
            dB.addIn("@InstanceId", Id);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ActivityMasterModel a = new ActivityMasterModel();
                    //a.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                    a.Task = dr["Task"].ToString();
                    a.Buldingblock = dr["BuildingBlock"].ToString();
                    a.Phase = dr["Phase"].ToString();
                    a.Role = dr["Owner"].ToString();
                    a.ApplicationArea = dr["ApplicationArea"].ToString();
                    a.Status = dr["Status"].ToString();
                    a.OwnerID = Guid.Parse(dr["OwnerId"].ToString());
                    a.Record = "NO Record Found";
                    AMM.Add(a);
                }
            }

            return AMM;
        }

        public List<ProjectMonitorModel> Sp_GetResourceAllocationData(int PhaseId, Guid InstanceId, bool? first, Guid CreBy)
        {
            List<ProjectMonitorModel> AMM = new List<ProjectMonitorModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            try
            {
                if (first == false)
                {
                    dB.addIn("@Type", "GetResourceAllocationDataFromActivity");
                    dB.addIn("@CreBy", CreBy);
                }
                else
                {
                    dB.addIn("@Type", "GetResourceAllocationData");
                }
                dB.addIn("@InstanceId", InstanceId);
                dB.addIn("@PhaseId", PhaseId);

                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ProjectMonitorModel a = new ProjectMonitorModel();
                        a.Id = Guid.Parse(dr["Id"].ToString());
                        a.Task = dr["Task"].ToString();
                        a.BuildingBlock = dr["BuildingBlock"].ToString();
                        a.Phase = dr["Phase"].ToString();
                        a.Owner = dr["Owner"].ToString();
                        a.Role = dr["RoleName"].ToString();
                        a.RoleID = Convert.ToInt32(dr["RoleId"].ToString());
                        a.ApplicationArea = dr["ApplicationArea"].ToString();
                        a.Status = dr["Status"].ToString();
                        a.UserID = Guid.Parse(dr["UserID"].ToString());
                        a.Cre_on_str = dr["Cre_on_str"].ToString();
                        //a.Record = "NO Record Found";
                        AMM.Add(a);
                    }
                }

            }
            catch (Exception EX)
            {

                throw EX;
            }
            
            return AMM;
        }

        public List<UserModel> Sp_GetUserByRole(int RoleId,Guid InstanceId)
        {
            List<UserModel> UMM = new List<UserModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetUserByRole");
            dB.addIn("@RoleId", RoleId);
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel UM = new UserModel();
                    UM.UserId = Guid.Parse(dr["UserId"].ToString());
                    UM.Name = dr["Name"].ToString();
                    UMM.Add(UM);
                }
            }
            return UMM;
        }

        public List<Role> Sp_GetRoleFromRA(int PhaseId, Guid InstanceId)
        {
            List<Role> RMM = new List<Role>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetRoleFromRA");
            dB.addIn("@PhaseId", PhaseId);
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Role R = new Role();
                    R.RoleId= Convert.ToInt32(dr["RoleId"].ToString());
                    R.RoleName= dr["RoleName"].ToString();
                    RMM.Add(R);
                }
            }
            return RMM;
        }

        public List<UserModel> Sp_GetUserByRoleBulkAllocate(int RoleId, Guid InstanceId)
        {
            List<UserModel> UMM = new List<UserModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetUserByRoleBulkAllocate");
            dB.addIn("@RoleId", RoleId);
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel UM = new UserModel();
                    UM.UserId = Guid.Parse(dr["UserId"].ToString());
                    UM.Name = dr["Name"].ToString();
                    UMM.Add(UM);
                }
            }
            return UMM;
        }

        public Boolean Sp_GetUpdateOwnerResourceAllocation(int PhaseId, Guid InstanceId, Guid rowID, Guid OwnerId,Guid Cre_By)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateOwnerResourceAllocation");
                dB.addIn("@PhaseId", PhaseId);
                dB.addIn("@InstanceId", InstanceId);
                dB.addIn("@Id", rowID);
                dB.addIn("@UserId", OwnerId);
                dB.addIn("@CreBy", Cre_By);
                dB.ExecuteScalar();
                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public ProjectMonitorModel Sp_GetUnassingnedTaskCount(int PhaseId, Guid InstanceId)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            ProjectMonitorModel PM = new ProjectMonitorModel();
            try
            {
                DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
                dB.addIn("@Type", "UnassingnedTaskCount");
                dB.addIn("@PhaseId", PhaseId);
                dB.addIn("@InstanceId", InstanceId);
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PM.Unassigned_Count = dr["Unassigned_Count"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return PM;
        }

        public Boolean Sp_GetBulkAllocateOwnerResourceAllocation(int PhaseId, Guid InstanceId, int roleID, Guid OwnerId, Guid Cre_By)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            bool Status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetBulkAllocateOwnerResourceAllocation");
                dB.addIn("@PhaseId", PhaseId);
                dB.addIn("@InstanceId", InstanceId);
                dB.addIn("@RoleId", roleID);
                dB.addIn("@UserId", OwnerId);
                dB.addIn("@CreBy", Cre_By);
                //dB.ExecuteScalar();
                //Status = true;
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {

                }
                else
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public List<ActivityMasterModel> Sp_GetPhaseResource(int Id)
        {
            List<ActivityMasterModel> AMM = new List<ActivityMasterModel>();
            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetPhaseResource");
            dB.addIn("@PhaseId", Id);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ActivityMasterModel a = new ActivityMasterModel();
                    //a.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                    a.Task = dr["Task"].ToString();
                    a.Buldingblock = dr["BuildingBlock"].ToString();
                    a.Phase = dr["Phase"].ToString();
                    a.Role = dr["Owner"].ToString();
                    a.ApplicationArea = dr["ApplicationArea"].ToString();
                    a.Status = dr["Status"].ToString();
                    a.OwnerID = Guid.Parse(dr["OwnerId"].ToString());
                    a.Record = "NO Record Found";
                    AMM.Add(a);
                }
            }

            return AMM;
        }

        public List<ProjectMonitorModel> Sp_GetMasterAdd(Guid InstanceId, int PhaseId)
        {
            DataTable dt = new DataTable();
            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();
            DBHelper dB = new DBHelper("SP_ProgressMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetMasterAdd");
            dB.addIn("@InstanceID", InstanceId);
            dB.addIn("@PhaseID", PhaseId);
            try
            {
                dt = dB.ExecuteDataTable();
                
                if (dt.Rows.Count > 0)
                {
                    var myLocalDateTime = DateTime.UtcNow;
                    foreach (DataRow dr in dt.Rows)
                    {
                        ProjectMonitorModel P = new ProjectMonitorModel();
                        P.ActivityID = Convert.ToInt32(dr["Activity_ID"].ToString());
                        P.Task = dr["Task"].ToString();
                        P.PhaseId = Convert.ToInt32(dr["PhaseId"].ToString());
                        P.SequenceNum = Convert.ToInt32(dr["Sequence_Num"].ToString());
                        P.RoleID = Convert.ToInt32(dr["RoleID"].ToString());

                        PM.Add(P);
                    }
                }
            }
            catch (Exception EX)
            {

                throw EX;
            }
            

            return PM;
        }

        public Boolean Sp_AddResource(Guid InstanceId, int Activity_ID, string LoginID)
        {
            LogHelper _log = new LogHelper();
            Boolean Result = false;
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_ProgressMonitor", CommandType.StoredProcedure);
                dB.addIn("@Type", "AddResource");
                dB.addIn("@InstanceID", InstanceId);
                dB.addIn("@ActivityId", Activity_ID);
                dB.addIn("@Cre_By", LoginID);
                dB.ExecuteScalar();
                Result = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Result;
        }
        //public List<PhaseModel> SP_GetPhaseList(Guid Id, Guid PId)
        //{
        //    DataTable dt = new DataTable();

        //    List<PhaseModel> PM = new List<PhaseModel>();

        //    DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
        //    dB.addIn("@Type", "GetPhase");
        //    dB.addIn("@InstanceId", Id);
        //    dB.addIn("@ProjectId", PId);
        //    dt = dB.ExecuteDataTable();

        //    if (dt.Rows.Count > 0)
        //    {
        //        //int count = 1;
        //        var myLocalDateTime = DateTime.UtcNow;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            PhaseModel P = new PhaseModel();
        //            P.Id = Convert.ToInt32(dr["Id"].ToString());
        //            P.PhaseName = dr["PhaseName"].ToString();
        //            PM.Add(P);
        //        }

        //    }
        //    return PM;
        //}


        public List<UserModel> Sp_GetUserRoleList(Guid InstanceId)
        {
            DataTable dt = new DataTable();

            List<UserModel> UM = new List<UserModel>();
            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetUserRole");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    UserModel U = new UserModel();
                    U.RoleID = Convert.ToInt32(dr["RoleId"].ToString());
                    U.UserId = Guid.Parse(dr["UserId"].ToString());
                    U.Name = dr["Name"].ToString();
                    UM.Add(U);
                }
            }
            return UM;
        }

        public Boolean Sp_PullTaskButton(int PhaseId,Guid InstanceId)
        {
            var val = true;
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("@Type", "PullTaskButton");
            dB.addIn("@PhaseId", PhaseId);
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                val = false;
            }
                return val;
        }
        #endregion


        #region Dashboard

        public CustomerModel Sp_GetCustomerDashboard(String loginID)
        {
            //List<CustomerModel> cust = new List<CustomerModel>();
            CustomerModel CM = new CustomerModel();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Dashboard", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetCustomerDetailDashboard_Top");            
            dB.addIn("@CustomerId", loginID);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    CM.Customer_ID = Guid.Parse(dr["Customer_ID"].ToString());
                    CM.Company_Name = dr["Company_Name"].ToString();
                    CM.IndustrySector_ID = Convert.ToInt32(dr["IndustrySector_ID"].ToString());
                    CM.Contact = dr["Contact"].ToString();
                    CM.Phone = dr["Phone"].ToString();
                    CM.Email = dr["Email"].ToString();
                    CM.IndustrySector = dr["IndustryName"].ToString();
                    if (dr["DialCode"].ToString() != "")
                    {
                        CM.DialCode = "+" + dr["DialCode"].ToString();
                    }
                    else
                    {
                        CM.DialCode = dr["DialCode"].ToString();
                    }
                }
            }
            return CM;
        }
        public UserModel Sp_GetConsultantDashboard(String loginID)
        {
            //List<CustomerModel> cust = new List<CustomerModel>();
            UserModel UM = new UserModel();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Dashboard", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetConsultantDetailDashboard_Top");
            dB.addIn("@UserId", loginID);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    UM.UserId = Guid.Parse(dr["UserId"].ToString());
                    UM.Name = dr["Name"].ToString();
                    UM.Email = dr["EMail"].ToString();
                    UM.Phone = dr["Phone"].ToString();
                    UM.LoginId = dr["LoginId"].ToString();
                    UM.RoleName = dr["RoleName"].ToString();
                    UM.UserType = dr["UserType"].ToString();
                    if (dr["DialCode"].ToString() != "")
                    {
                        UM.DialCode = "+" + dr["DialCode"].ToString();
                    }
                    else
                    {
                        UM.DialCode = dr["DialCode"].ToString();
                    }
                    //UM.Customer_Name = dr["Customer_Name"].ToString();
                }
            }
            return UM;
        }
        public Dashboard Sp_GetDashboard(Guid loginID)
        {
            Dashboard Das = new Dashboard();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable(); 
            DBHelper dB = new DBHelper("SP_Dashboard", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetDasboard_Top");
            
            dB.addIn("@loginID", loginID);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Das.proj = Convert.ToInt32(dr["proj"].ToString());
                    Das.TotalTaskComp = Convert.ToInt32(dr["TotalTaskComp"].ToString());
                    Das.TotalTask = Convert.ToInt32(dr["TotalTask"].ToString());
                    Das.client = Convert.ToInt32(dr["client"].ToString());
                    Das.consult = Convert.ToInt32(dr["consult"].ToString());
                    Das.ClientIDs = dr["ClientIDs"].ToString();
                    Das.ClientIDs_Image = List_LoadPhoto(Das.ClientIDs);
                    Das.ClientNames = dr["ClientNames"].ToString();
                    Das.All_ClientIDs = dr["All_ClientIDs"].ToString();
                    Das.All_UserIDs = dr["ALL_UserIDs"].ToString();
                    Das.ConsuIDs = dr["ConsuIDs"].ToString();
                    Das.ConsuIDs_Image = List_LoadPhoto(Das.ConsuIDs);
                    Das.ConsuNames = dr["ConsuNames"].ToString();
                }

            }

            return Das;
        }
        public List<Dasboard_Donut> Sp_GetDashboard_Donut(Guid loginID)
        {
            List<Dasboard_Donut> Return_List = new List<Dasboard_Donut>();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            try
            {

            
            DBHelper dB = new DBHelper("SP_Dashboard", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetDasboard_Donut");
            dB.addIn("@loginID", loginID);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Dasboard_Donut Das = new Dasboard_Donut();
                    int min_Count = Convert.ToInt32(dr["Total_Task"].ToString());
                    if (min_Count > 0)
                    {
                        Das.customerId = Guid.Parse(dr["customerId"].ToString());
                        Das.InstanceId = Guid.Parse(dr["InstanceId"].ToString());
                        Das.ProjectId = Guid.Parse(dr["ProjectId"].ToString());

                        Das.ProjectName = dr["ProjectName"].ToString();
                        Das.InstanceName = dr["InstanceName"].ToString();
                        Das.customerName = dr["customerName"].ToString();
                        Das.ReadinessCheck = Convert.ToInt32(dr["ReadinessCheck"].ToString());

                        Das.PhaseId = Convert.ToInt32(dr["PhaseId"].ToString());

                        Das.Total_Task = Convert.ToInt32(dr["Total_Task"].ToString());
                        Das.Comp_Task = Convert.ToInt32(dr["Comp_Task"].ToString());

                        Das.Top5Con_ID = dr["Top5Con_ID"].ToString();
                        Das.Top5Con_Image= List_LoadPhoto(Das.Top5Con_ID.ToString());
                        Das.Top5Con_Name = dr["Top5Con_Name"].ToString();

                        Das.StartDate = dr["Start_Dt"].ToString();
                        Das.EndDate = dr["End_Dt"].ToString();
                        Das.Colour = dr["Color"].ToString();

                            Das.TotalTask = dr["TotalTask"].ToString();
                            Das.Completed = dr["Completed"].ToString();
                            Das.YetToStart = dr["YetToStart"].ToString();




                            Return_List.Add(Das);
                    }
                    
                }

            }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Return_List;
        }

        //Sp_GetBellData
        public string List_LoadPhoto(string listID)
        {
            string[] List = listID.Split(',');
            string Result = "";
            foreach (string lis in List)
            {
                if (lis!="")
                {
                    Guid Id = Guid.Parse(lis);
                    Result = Result + LoadPhotoFileName(Id);
                }
            }
            return Result;
        }

        public string LoadPhotoFileName(Guid Id)
        {
            string Return = "";
            List<FileUpload_Model> data =SP_Getfullpath(Id);
            var List = data.ToList();
            if (List.Count>0)
            {
                string fileName = List.FirstOrDefault().FileName.ToString();
                string fullPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Upload"].ToString() + fileName + ".jpg");
                if (File.Exists(fullPath))
                {
                    Return = fileName + ",";
                }
                else
                {
                    Return = " ";
                }
            }
            else
            {
                Return = " ";
            }
            
            return Return;
        }
        #endregion

        #region ProgressMonitor
        public List<ProjectMonitorModel> Sp_GetProgressMonitor(int PhaseId, Guid InstanceId)
        {
            DataTable dt = new DataTable();
            bool Status = false;
            if (PhaseId != 1)
            {
                DBHelper dB1 = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
                dB1.addIn("@Type", "CheckPreviousPhase");
                dB1.addIn("@InstanceId", InstanceId);
                dB1.addIn("@PhaseId", PhaseId);
                dt = dB1.ExecuteDataTable();
                if (dt.Rows.Count == 0)
                {
                    Status = true;
                }
            }
            else
            {
                Status = true;
            }
            List<ProjectMonitorModel> PMM = new List<ProjectMonitorModel>();
            if (Status)
            {
                DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetProgressMonitor");
                dB.addIn("@PhaseId", PhaseId);
                dB.addIn("@InstanceId", InstanceId);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        sno=sno + 1;
                        ProjectMonitorModel PM = new ProjectMonitorModel();
                        PM.SNO = sno;
                        PM.Id = Guid.Parse(dr["Id"].ToString());
                        PM.BuildingBlock = dr["Block_Name"].ToString();
                        PM.Task = dr["Task"].ToString();
                        PM.Role = dr["RoleName"].ToString();
                        PM.Owner = dr["Name"].ToString();
                        PM.UserID = Guid.Parse(dr["UserID"].ToString());
                        PM.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                        PM.Status = dr["Status"].ToString();
                        PM.TaskType = dr["TaskType"].ToString();
                        PM.Comments_Count = dr["Comments_Count"].ToString();
                        PM.Task_id = Convert.ToInt32(dr["Task_id"].ToString());
                        PM.PlanedDate = dr["PlanedDate"].ToString();
                        PM.PlanedEn_Date = dr["PlanedEn_Date"].ToString();
                        PM.ActualDate = dr["ActualDate"].ToString();
                        PM.ActualEn_Date = dr["ActualEn_Date"].ToString();
                        PM.Modified_date = dr["Modified_ON"].ToString();
                        if (dr["ParallelName"].ToString() != "")
                        {
                            PM.ParallelName = Convert.ToInt32(dr["ParallelName"].ToString());
                            PM.Parallel_Name = dr["Parallel_Name"].ToString();
                        }

                        PMM.Add(PM);
                    }
                }
            }
            return PMM;
        }


        public List<ProjectMonitorModel> Sp_GetProgressMonitorTaskCount(int PhaseId, Guid InstanceId)
        {
            DataTable dt = new DataTable();
            List<ProjectMonitorModel> PMM = new List<ProjectMonitorModel>();
            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("Type", "GetProgressMonitorTaskCount");
            dB.addIn("PhaseId", PhaseId);
            dB.addIn("InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProjectMonitorModel PM = new ProjectMonitorModel();
                    PM.TotalTask = dr["TotalTask"].ToString();
                    PM.Completed = dr["Completed"].ToString();
                    PM.WIP = dr["WIP"].ToString();
                    PM.ONHOLD = dr["ONHOLD"].ToString();
                    PM.YetToStart = dr["YetToStart"].ToString();
                    PM.StartDate= dr["StartDate"].ToString();
                    PM.EndDate= dr["EndDate"].ToString();
                    PM.Colour = dr["Colour"].ToString();

                    PMM.Add(PM);
                }
            }
            return PMM;
        }
        public List<ProjectMonitorModel> Sp_GetProgressMonitorData(Guid Id)
        {
            DataTable dt = new DataTable();

            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();
            ProjectMonitorModel P = new ProjectMonitorModel();
            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);
            dB.addIn("Type", "GetProgressMonitorData");
            dB.addIn("Id", Id);

            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    P.Id = Guid.Parse(dr["Id"].ToString());
                    P.BuildingBlock = dr["BuildingBlock"].ToString();
                    P.Task = dr["Task"].ToString();
                    P.Role = dr["RoleName"].ToString();
                    P.Owner = dr["Owner"].ToString();
                    P.Status = dr["Status"].ToString();
                    P.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    P.Planed__St_Date = Convert.ToDateTime(dr["Planed__St_Date"].ToString());
                    P.Planed__En_Date = Convert.ToDateTime(dr["Planed__En_Date"].ToString());
                    P.Actual_St_Date = Convert.ToDateTime(dr["Actual_St_Date"].ToString());
                    P.Actual_En_Date = Convert.ToDateTime(dr["Actual_En_Date"].ToString());
                    P.EST_hrs = dr["EST_hours"].ToString();
                    P.EST_hrs = P.EST_hrs.Replace(".", ":");
                    P.Actual_St_hrs= dr["Actual_St_hours"].ToString();
                    P.Actual_St_hrs = P.Actual_St_hrs.Replace(".", ":");

                    P.CreatedDate = dr["CreatedDate"].ToString();
                    P.PlanedDate = dr["PlanedDate"].ToString();
                    P.ActualDate = dr["ActualDate"].ToString();
                    P.PlanedEn_Date = dr["PlanedEn_Date"].ToString();
                    P.ActualEn_Date = dr["ActualEn_Date"].ToString();
                    P.Comments_Count = dr["Comments_Count"].ToString();
                    P.TS = dr.Field<byte[]>("TS");
                    PM.Add(P);
                }
            }
            return PM;
        }
        public List<Status> Sp_GetSatus(bool? bit)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<Status> SM = new List<Status>();
            DBHelper dB = new DBHelper("SP_ResourceAllocation", CommandType.StoredProcedure);

            if (bit == true)
            {
                dB.addIn("Type", "GetStatus_PM");
            }
            else
            {
                dB.addIn("Type", "GetStatus_cons_Cust");
            }

            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Status P = new Status();
                    P.Id = Convert.ToInt32(dr["Id"].ToString());
                    P.StatusName = dr["StatusName"].ToString();

                    SM.Add(P);
                }
            }
            return SM;
        }

        public bool Sp_UpdateProgressMonitor(ProjectMonitorModel PM)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();

            try
            {
                DBHelper dB = new DBHelper("SP_ProgressMonitor", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateTask");
                dB.addIn("@Id", PM.Id);
                dB.addIn("@StatusId", PM.StatusId);
                dB.addIn("@EST_hours", PM.EST_hours);
                dB.addIn("@Actual_St_hours", PM.Actual_St_hours);
                dB.addIn("@Planed__St_Date", PM.Planed__St_Date);
                dB.addIn("@Planed__En_Date", PM.Planed__En_Date);
                dB.addIn("@Actual_St_Date", PM.Actual_St_Date);
                dB.addIn("@Actual_En_Date", PM.Actual_En_Date);
                dB.addIn("@Cre_By", PM.Cre_By);
                dB.addIn("@TS", PM.TS);

                //dB.ExecuteScalar();

                dB.addReturn();
                dB.Execute();
                int Ret = dB.getReturned();
                if (Ret == 0)
                {
                    Status = true;
                }
                else if (Ret == 1)
                {
                    Status = false;
                }
                else if (Ret == 2)
                {
                    Status = false;
                }

                if (Status)
                {
                    Boolean s = UpdateMonitor_Mail(PM);
                    if (s)
                        Status = true;
                }
               
                //Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public List<ProjectMonitorModel> Sp_Comments(Guid ProjectMonitor_Id, int timezoneOffset)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ProgressMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetComments");
            dB.addIn("@Id", ProjectMonitor_Id);

            dt = dB.ExecuteDataTable();
            List<ProjectMonitorModel> L = new List<ProjectMonitorModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.Comments = dr["Comments"].ToString();
                    P.Comments_Id = Guid.Parse(dr["Comments_Id"].ToString());
                    //P.UserID = Guid.Parse(dr["User_Id"].ToString());
                    P.UserID1 = dr["USERID"].ToString();
                    P.Image = List_LoadPhoto(P.UserID1);
                    P.Name = dr["Name"].ToString();

                    //P.Cre_on_str = dr["coments_Date"].ToString();
                    P.Cre_on = Convert.ToDateTime(dr["Cre_on"].ToString());
                    P.Cre_on_str = FromUTCData(P.Cre_on, timezoneOffset).ToString();
                    //P.ShortName = GetFirstandLastName(P.Name);
                    L.Add(P);
                }
            }

            return L;
        }
        public bool Sp_UpdateComments(String Comments, Guid id, Guid Cre_By)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();

            try
            {
                DBHelper dB = new DBHelper("SP_ProgressMonitor", CommandType.StoredProcedure);
                dB.addIn("@Type", "InsertComments");
                dB.addIn("@Id", id);
                dB.addIn("@Comments", Comments);
                dB.addIn("@Cre_By", Cre_By);
                dB.ExecuteScalar();
                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }


        public List<ActivityMasterModel> GetActivity(int PhaseId, Guid InstanceId)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ProgressMonitor", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetTaskFromPhase");
            dB.addIn("@InstanceID", InstanceId);
            dB.addIn("@PhaseID", PhaseId);
            dt = dB.ExecuteDataTable();
            List<ActivityMasterModel> L = new List<ActivityMasterModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ActivityMasterModel P = new ActivityMasterModel();
                    P.Activity_ID = Convert.ToInt32(dr["Activity_ID"].ToString());
                    P.Task = dr["Task"].ToString();
                    L.Add(P);
                }
            }

            return L;
        }

        public Boolean Sp_AddTask(ProjectMonitorModel Data)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_ProgressMonitor", CommandType.StoredProcedure);

                dB.addIn("@Type", "GetTask");
                dB.addIn("@Task", Data.Task);
                dB.addIn("@InstanceID", Data.Instance);
                dB.addIn("@BuildingBlock_id", Data.BuildingBlockID);
                dB.addIn("@PhaseID", Data.PhaseId);
                dB.addIn("@PreviousId", Data.PreviousID);
                dB.addIn("@ApplicationAreaID", Data.ApplicationAreaID);
                dB.addIn("@RoleID", Data.RoleID);
                //dB.addIn("@Notes", Data.Notes);
                Data.EST_hrs = Data.EST_hrs.Replace(":", ".");
                dB.addIn("@EST_hours", Data.EST_hrs);
                dB.addIn("@UserID", Data.Owner);
                dB.addIn("@Task_Id", Data.Task_id);

                if (Data.Task_id == 2 && Data.parallel_Id != Guid.Empty)
                {
                    dB.addIn("@Parallel_Id", Data.parallel_Id);

                }
                dB.addIn("@Parallel_Name", Data.Parallel_Name);
                dB.addIn("@Cre_By", Data.Cre_By);

                dB.Execute();

                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "");
            }
            return Status;
        }


        #region PMUpload
        public Boolean PMUpload(string filetype, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_PMupload");
                //dB.addIn("@tblSimplification", CustomTable);
                dB.addIn("@File_Type", filetype);
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "-->PMUpload" + ex.Message.ToString());
            }
            return status;

        }

        public List<FileUploadMaster> GetPMuploadlist(Guid Instance_ID)
        {
            List<FileUploadMaster> Fu = new List<FileUploadMaster>();
            LogHelper _log = new LogHelper();
            try
            {

                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetPMuploadlist");
                dB.addIn("@instanceId", Instance_ID);
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FileUploadMaster M = new FileUploadMaster();
                        M.C_FileName = dr["_FileName"].ToString();
                        M.FileType = Convert.ToInt32(dr["FileType"].ToString());
                        M.File_Type = dr["File_Type"].ToString();
                        if (CheckFileExist(M.C_FileName))
                        {
                            Fu.Add(M);
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->GetPMuploadlist" + ex.Message.ToString());
            }

            return Fu;
        }
        public Boolean CheckFileExist(string C_FileName)
        {
            Boolean status = false;
            string Path_ = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
            DirectoryInfo dir = new DirectoryInfo(Path_);
            FileInfo[] files = dir.GetFiles(C_FileName + ".*");
            if (files.Length > 0)
            {
                status = true;
            }
            return status;
        }

        public List<FileUploadMaster> GetPMFileList(Guid Instance_ID)
        {
            List<FileUploadMaster> Fu = new List<FileUploadMaster>();
            LogHelper _log = new LogHelper();
            try
            {

                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetPMFileList");
                dB.addIn("@instanceId", Instance_ID);
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FileUploadMaster M = new FileUploadMaster();
                        M.C_FileName = dr["File"].ToString();
                        M.id_Int = Convert.ToInt32(dr["Id"].ToString());
                        Fu.Add(M);
                    }
                }
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "-->GetPMFileList" + ex.Message.ToString());
            }

            return Fu;
        }
        #endregion


        #endregion

        #region PMTask

        public List<PMTaskModel> GetPMTask(Guid IDProject,bool first,Guid LoginId)
        {
            List<PMTaskModel> ListM = new List<PMTaskModel>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            if (first == false)
            {
                dB.addIn("@Type", "GetPMTask");
            }
            else
            {
                dB.addIn("@Type", "GetPMTask1");
            }
            
            dB.addIn("@ProjectID", IDProject);
            dB.addIn("@Cre_By", LoginId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PMTaskModel M = new PMTaskModel();
                    M.Id = Guid.Parse(dr["id"].ToString());
                    M.PMTaskID = Guid.Parse(dr["PMTaskID"].ToString());
                    M.PMTaskName = dr["PMTaskName"].ToString();
                    M.PMTaskCategory = dr["Name"].ToString();
                    M.ProjectId = Guid.Parse(dr["ProjectId"].ToString());
                    M.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    M.StatusName = dr["StatusName"].ToString();
                    M.EST_hours1 = dr["EST_hours"].ToString().Replace(".", ":");
                    M.Actual_St_hours = dr["Actual_St_hours"].ToString().Replace(".", ":");

                    M.Task_Other_Environment = Convert.ToBoolean(dr["Task_Other_Environment"].ToString());

                    M.Planed__St_Date = Convert.ToDateTime(dr["Planed__St_Date"].ToString());
                    M.Planed__En_Date = Convert.ToDateTime(dr["Planed__En_Date"].ToString());
                    M.Actual_St_Date = Convert.ToDateTime(dr["Actual_St_Date"].ToString());
                    M.Actual_En_Date = Convert.ToDateTime(dr["Actual_En_Date"].ToString());

                    M.PlanedDate = dr["PlanedDate"].ToString();
                    M.ActualDate = dr["ActualDate"].ToString();
                    M.PlanedEn_Date = dr["PlanedEn_Date"].ToString();
                    M.ActualEn_Date = dr["ActualEn_Date"].ToString();

                    M.Notes = Convert.ToString(dr["Notes"].ToString());


                    ListM.Add(M);
                }
            }
            return ListM;
        }
        public PMTaskModel Sp_GetPMTaskMonitorById(Guid Id)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetPMTaskMonitorById");
            dB.addIn("@PMTaskId", Id);

            dt = dB.ExecuteDataTable();
            PMTaskModel P = new PMTaskModel();
            foreach (DataRow dr in dt.Rows)
            {
                //P.ProjectId = Guid.Parse(dr["Project_Id"].ToString());
                //P.ProjectName = dr["Project_Name"].ToString();
                P.Id = Guid.Parse(dr["Id"].ToString());
                P.PMTaskName = dr["PMTaskName"].ToString();
                P.StatusName = dr["StatusName"].ToString();
                P.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                P.EST_hours1 = dr["EST_hours"].ToString().Replace(".", ":");
                P.Actual_St_hours = dr["Actual_St_hours"].ToString().Replace(".", ":");
                P.Planed__St_Date = Convert.ToDateTime(dr["Planed__St_Date"].ToString());
                P.Planed__En_Date = Convert.ToDateTime(dr["Planed__En_Date"].ToString());
                P.Actual_St_Date = Convert.ToDateTime(dr["Actual_St_Date"].ToString());
                P.Actual_En_Date = Convert.ToDateTime(dr["Actual_En_Date"].ToString());

                P.minDate = dr["minDate"].ToString();
                P.PlanedDate = dr["PlanedDate"].ToString();
                P.ActualDate = dr["ActualDate"].ToString();
                P.PlanedEn_Date = dr["PlanedEn_Date"].ToString();
                P.ActualEn_Date = dr["ActualEn_Date"].ToString();

                P.Notes = dr["Notes"].ToString();
            }
            return P;
        }

        public List<PMTaskModel> Sp_GetPMProjectList(Guid LoginId)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetPMProjectList");
            dB.addIn("@LoginId", LoginId);

            dt = dB.ExecuteDataTable();
            List<PMTaskModel> PM = new List<PMTaskModel>();
            if (dt.Rows.Count > 0)
            {               
                foreach (DataRow dr in dt.Rows)
                {
                    PMTaskModel P = new PMTaskModel();
                    P.ProjectId = Guid.Parse(dr["Project_Id"].ToString());
                    P.ProjectName = dr["Project_Name"].ToString();

                    PM.Add(P);
                }
            }
            return PM;
        }

        public bool Sp_SubmitPMTaskMonitor(PMTaskModel PMTM)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();

            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdatePMTaskMonitor");
                dB.addIn("@PM_id", PMTM.Id);
                dB.addIn("@StatusId", PMTM.StatusId);
                dB.addIn("@EST_hours", PMTM.EST_hours);
                dB.addIn("@Actual_St_hours", PMTM.Actual_St_hrs);
                dB.addIn("@Planed__St_Date", PMTM.Planed__St_Date);
                dB.addIn("@Planed__En_Date", PMTM.Planed__En_Date);
                dB.addIn("@Actual_St_Date", PMTM.Actual_St_Date);
                dB.addIn("@Actual_En_Date", PMTM.Actual_En_Date);
                dB.addIn("@Notes", PMTM.Notes);
                dB.addIn("@Cre_By", PMTM.Cre_By);

                dB.ExecuteScalar();
                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        #endregion

        #region Fileupload
        public Boolean SP_Uploadfile(Guid Id, string Fname, string FileType, bool valid, Guid Createdby)
        {

            bool status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);
                dB.addIn("@Type", "UploadPhoto");
                dB.addIn("@FileUploadID", Id);
                dB.addIn("@InstanceId", "00000000-0000-0000-0000-000000000000");
                dB.addIn("@File_Type", FileType);
                dB.addIn("@FileName", Fname);
                dB.addIn("@isActive", valid);
                dB.addIn("@Createdby", Createdby);
                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<FileUpload_Model> SP_Getfullpath(Guid Id)
        {
            List<FileUpload_Model> PTM = new List<FileUpload_Model>();
            LogHelper _log = new LogHelper();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetFullPath");
                dB.addIn("@Createdby", Id);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FileUpload_Model a = new FileUpload_Model();
                        a.Createdon = Convert.ToDateTime(dr["Cre_on"].ToString());
                        a.CreatedBy = Guid.Parse(dr["Cre_By"].ToString());
                        a.FileName = Guid.Parse(dr["_FileName"].ToString());
                        PTM.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return PTM;
        }
            #region Analysis
        public Boolean Upload_Activities(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_Activities");
            dB.addIn("@tblActivities", CustomTable);
            dB.addIn("@File_Type", "Activities");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_CustomCode(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_CustomCode");
            dB.addIn("@tblCustomCode", CustomTable);
            dB.addIn("@File_Type", "CustomCode");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        } 
        public Boolean Upload_HanaDatabase(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_HanaDatabaseTables");
                dB.addIn("@tblHanaDatabaseTables", CustomTable);
                dB.addIn("@File_Type", "HanaDatabaseTables");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }           
            return status;

        }
        public Boolean Upload_FioriApps(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_FioriApps");
            dB.addIn("@tblFioriApps", CustomTable);
            dB.addIn("@File_Type", "RecommendedFioriApp");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_Simplification(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_Simplification");
            dB.addIn("@tblSimplification", CustomTable);
            dB.addIn("@File_Type", "RelevantSimplificationItems");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_Bwextractors(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;

            try{
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_Bwextractors");
                dB.addIn("@tblBwExtractors", CustomTable);
                dB.addIn("@File_Type", "Bwextractors");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;

        }
        public Boolean Upload_HanaDatabaseTables(string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_HanaDatabaseTables");
            //dB.addIn("@tblSimplification", CustomTable);
            dB.addIn("@File_Type", "HanaDatabaseTables");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_SAPReadinessCheck(string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_SAPReadinessCheck");
            //dB.addIn("@tblSimplification", CustomTable);
            dB.addIn("@File_Type", "SAPReadinessCheck");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        public Boolean Upload_SAPPreConvertion(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_SAPPreConvertion");
            dB.addIn("@tblPreConvertion", CustomTable);
            dB.addIn("@File_Type", "PreConvertion");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            var a = dB.ExecuteScalar();
            status = true;
            return status;


        }


        public Boolean Upload_SAPUserList(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_UserList");
                dB.addIn("@tblSAPUserList", CustomTable);
                dB.addIn("@File_Type", "SAPUserList");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                var a = dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
          
            return status;
        }

        public Boolean Confirm_AnalysisUpload(Guid Instance_ID, Guid User_Id)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);
                dB.addIn("@Type", "Confirm_AnalysisUpload");
                dB.addIn("@Createdby", User_Id);
                dB.addIn("@InstanceID", Instance_ID);

                var a = dB.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return status;
        }
        public Boolean Confirm_SAPFileUpload(Guid Instance_ID, Guid User_Id)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);
                dB.addIn("@Type", "Confirm_SAPFileUpload");
                dB.addIn("@Createdby", User_Id);
                dB.addIn("@InstanceID", Instance_ID);

                var a = dB.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return status;
        }

        public Boolean Upload_ActivityMaster(DataTable CustomTable, string NewId, Guid User_ID)
        {
            Boolean status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_ActivityFileUpload");
                dB.addIn("@File_Type", "ActivitiUpload");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@tblUploadActivityMaster", CustomTable);
                dB.addIn("@fileName", NewId);
                //dB.addIn("@File_Type", "SAPUserList");
                //dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@Createdby", User_ID);

                var a = dB.ExecuteScalar();
                status = true;
            }
            catch(Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        } 
        public Boolean Upload_SAPIssueTrackDump(DataTable CustomTable, string NewId, Guid User_ID,Guid Project_Id)
        {
            Boolean status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "SAPIssueTrackUpload");
                dB.addIn("@tblSAPIssueTrackUpload", CustomTable);
                dB.addIn("@ProjectId", Project_Id);
                dB.addIn("@fileName", NewId);
                dB.addIn("@File_Type", "SAPIssueTrackUpload");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@Createdby", User_ID);

                var a = dB.ExecuteScalar();
                status = true;
            }
            catch(Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;
        }


        public Boolean GetUploadRevert(Guid Instance_ID, Guid User_Id)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);
                dB.addIn("@Type", "CreateAnalysis_UploadRevert");
                dB.addIn("@Createdby", User_Id);
                dB.addIn("@InstanceID", Instance_ID);

                var a = dB.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            return status;
        }
        public List<PicturetoData> Sp_GetPicturetoDatas()
        {

            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PictureTodata", CommandType.StoredProcedure);
            dB.addIn("@Type", "DropDown");
            //dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            List<PicturetoData> Pic = new List<PicturetoData>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    PicturetoData P = new PicturetoData();
                    P.ID = Convert.ToInt32(dr["Id"].ToString());
                    P.PictureName = dr["PictureName"].ToString();
                    P.GivenName = dr["GivenName"].ToString();
                    Pic.Add(P);
                }
            }
            return Pic;
        }
        #endregion

        #region SAPUploadFiles

        public Boolean Upload_RFC_FM_Output(DataTable CustomTable, DataTable dataTable_Sheet2, DataTable dataTable_Sheet3, DataTable dataTable_Sheet4,string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_RFCFM");
                dB.addIn("@tblRFCFM", CustomTable);
                dB.addIn("@tblSOFTWARECOMPONENT", dataTable_Sheet2);
                dB.addIn("@tblPRODUCTVERSIONS", dataTable_Sheet3);
                dB.addIn("@tblSFW5REPORT", dataTable_Sheet4);
                dB.addIn("@File_Type", "RFCFM");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Boolean Upload_BillingDocuments(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_BillingDocuments");
                dB.addIn("@tblBillingDocument", CustomTable);
                dB.addIn("@File_Type", "BillingDocuments");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Boolean Upload_OrderDocuments(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_OrderDocuments");
                dB.addIn("@tblOrderDocument", CustomTable);
                dB.addIn("@File_Type", "OrderDocuments");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean Upload_SalesDocuments(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_SalesDocuments");
                dB.addIn("@tblSalesDocument", CustomTable);
                dB.addIn("@File_Type", "SalesDocuments");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
         public Boolean Upload_ComplexityAnalysis(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_ComplexityAnalysis");
                dB.addIn("@tblComplexityAnalysis", CustomTable);
                dB.addIn("@File_Type", "ComplexityAnalysis");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        } 
        public Boolean Upload_MaterialityScore(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;
            try
            {
                DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

                dB.addIn("@Type", "up_MaterialityScore");
                dB.addIn("@tblMaterialityScore", CustomTable);
                dB.addIn("@File_Type", "MaterialityScore");
                dB.addIn("@FileUploadID", Guid.NewGuid());
                dB.addIn("@instanceId", Instance_ID);
                dB.addIn("@fileName", fileName);
                dB.addIn("@Createdby", User_ID);

                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }


        #endregion
        public Boolean Lic_Upload(DataTable CustomTable, string fileName, Guid Instance_ID, Guid User_ID)
        {
            Boolean status = false;


            DBHelper dB = new DBHelper("SP_FileUpload", CommandType.StoredProcedure);

            dB.addIn("@Type", "up_LicUpload");
            dB.addIn("@tblUserlist", CustomTable);
            dB.addIn("@File_Type", "UserList");
            dB.addIn("@FileUploadID", Guid.NewGuid());
            dB.addIn("@instanceId", Instance_ID);
            dB.addIn("@fileName", fileName);
            dB.addIn("@Createdby", User_ID);

            dB.ExecuteScalar();
            status = true;
            return status;

        }
        #endregion

        #region ReportdataDownload
        public List<ProjectMonitorModel> SP_GetReportData(Guid? InstanceId)
        {
            DataTable dt = new DataTable();

            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();

            DBHelper dB = new DBHelper("SP_GetReportData", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetReportData");
            dB.addIn("@InstunceID", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {

                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.Id = Guid.Parse(dr["id"].ToString());
                    P.ActivityID = Convert.ToInt32(dr["ActivityID"].ToString());
                    P.Instance = Guid.Parse(dr["InstanceID"].ToString());
                    P.BuildingBlockID = Convert.ToInt32(dr["BuildingBlock_id"].ToString());
                    P.Task = dr["Task"].ToString();
                    P.PhaseId = Convert.ToInt32(dr["PhaseId"].ToString());
                    P.SequenceNum = Convert.ToInt32(dr["Sequence_Num"].ToString());
                    P.ApplicationAreaID = Convert.ToInt32(dr["ApplicationAreaID"].ToString());  //dr["ApplicationArea"].ToString();
                    P.Task_Other_Environment = Convert.ToBoolean(dr["Task_Other_Environment"].ToString());
                    P.Dependency = Convert.ToBoolean(dr["Dependency"].ToString());
                    P.Pending = dr["Pending"].ToString();
                    P.Delay_occurred = Convert.ToBoolean(dr["Delay_occurred"].ToString());

                    if (P.Delay_occurred)
                    {
                        P.Delay_occurred_Report = "Yes";
                    }
                    else
                    {
                        P.Delay_occurred_Report = "NO";
                    }

                    P.RoleID = Convert.ToInt32(dr["RoleId"].ToString());
                    P.UserID = Guid.Parse(dr["UserID"].ToString());
                    P.StatusId = Convert.ToInt32(dr["StatusId"].ToString());


                    P.BuildingBlock = dr["BuildingBlock"].ToString();
                    P.Owner = dr["Owner"].ToString();
                    P.Phase = dr["Phase"].ToString();
                    P.Role = dr["RoleName"].ToString();
                    P.Status = dr["Status"].ToString();
                    P.ApplicationArea = dr["ApplicationArea"].ToString();

                    P.EST_hours = decimal.Parse(dr["EST_hours"].ToString());
                    P.EST_hrs = dr["EST_hours"].ToString();
                    P.EST_hrs = P.EST_hrs.Replace(".",":");
                    P.Actual_St_hours = decimal.Parse(dr["Actual_St_hours"].ToString());
                    P.Actual_St_hrs = dr["Actual_St_hours"].ToString();

                    P.PlanedDate = dr["PlanedDate"].ToString();
                    P.ActualDate = dr["ActualDate"].ToString();
                    P.PlanedEn_Date = dr["PlanedEn_Date"].ToString();
                    P.ActualEn_Date = dr["ActualEn_Date"].ToString();
                    P.Notes = dr["Notes"].ToString();


                    PM.Add(P);
                }
            }

            return PM;
        }
        public List<ProjectMonitorModel> Sp_GetReportDataReportPDF(Guid InstanceId)
        {
            DataTable dt = new DataTable();

            List<ProjectMonitorModel> PM = new List<ProjectMonitorModel>();

            DBHelper dB = new DBHelper("SP_GetReportData", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetReportDataPDF");
            dB.addIn("@InstunceID", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                var count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    count = count + 1;
                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.SNO = count;
                    P.Id = Guid.Parse(dr["id"].ToString());
                    P.ActivityID = Convert.ToInt32(dr["ActivityID"].ToString());
                    P.Instance = Guid.Parse(dr["InstanceID"].ToString());
                    P.BuildingBlock = dr["BuildingBlock"].ToString();
                    P.Task = dr["Task"].ToString();
                    P.Phase = dr["Phase"].ToString();
                    P.SequenceNum = Convert.ToInt32(dr["Sequence_Num"].ToString());
                    P.ApplicationArea = dr["Applicationarea"].ToString();  //dr["ApplicationArea"].ToString();
                    P.Task_Other_Environment = Convert.ToBoolean(dr["Task_Other_Environment"].ToString());
                    P.Dependency = Convert.ToBoolean(dr["Dependency"].ToString());
                    P.Pending = dr["Pending"].ToString();
                    P.Delay_occurred = Convert.ToBoolean(dr["Delay_occurred"].ToString());
                    if (P.Delay_occurred)
                    {
                        P.Delay_occurred_Report = "Yes";
                    }
                    else
                    {
                        P.Delay_occurred_Report = "NO";
                    }

                    P.RoleID = Convert.ToInt32(dr["RoleId"].ToString());
                    P.Owner = dr["Owner"].ToString();
                    P.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    P.Status = dr["Status"].ToString();

                    P.EST_hours = decimal.Parse(dr["EST_hours"].ToString());
                    P.EST_hrs = dr["EST_hours"].ToString();
                    P.EST_hrs = P.EST_hrs.Replace(".", ":");

                    P.Actual_St_hours = decimal.Parse(dr["Actual_St_hours"].ToString());
                    P.Actual_St_hrs = dr["Actual_St_hours"].ToString();
                    P.Actual_St_hrs = P.Actual_St_hrs.Replace(".", ":");

                    P.PlanedDate = dr["PlanedDate"].ToString();
                    P.ActualDate = dr["ActualDate"].ToString();
                    P.PlanedEn_Date = dr["PlanedEn_Date"].ToString();
                    P.ActualEn_Date = dr["ActualEn_Date"].ToString();
                    P.Notes = dr["Notes"].ToString();

                    PM.Add(P);
                }
            }

            return PM;
        }

        #endregion

        #region IssueTrack

        public List<UserModel> Sp_AssignedTo(Guid Pid, String login)
        {
            List<UserModel> ListM = new List<UserModel>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "AssignedTo");
            dB.addIn("@Project_Id", Pid);
            dB.addIn("@Id", login);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel U = new UserModel();
                    U.UserId = Guid.Parse(dr["UserId"].ToString());
                    U.Name = dr["Name"].ToString();

                    ListM.Add(U);
                }
            }
            return ListM;
        }

        public List<ActivityMasterModel> Sp_GetIssueTrackTask(int PhaseID,Guid Instance_Id)
        {
            List<ActivityMasterModel> ListM = new List<ActivityMasterModel>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetIssueTrackTask");
            dB.addIn("@PhaseID", PhaseID);
            dB.addIn("@Instance_Id", Instance_Id);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ActivityMasterModel U = new ActivityMasterModel();
                    U.Activity_ID = Convert.ToInt32(dr["ActivityID"].ToString());
                    U.Task = dr["Task"].ToString();

                    ListM.Add(U);
                }
            }
            return ListM;
        }

        public bool Sp_InsertIssue(IssueTrackModel blism)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "AddIssueTrack");
                dB.addIn("@ProjectInstance_Id", blism.ProjectInstance_Id);
                dB.addIn("@IssueName", blism.IssueName);
                dB.addIn("@PhaseID", blism.PhaseID);
                dB.addIn("@TaskId", blism.TaskId);
                //dB.addIn("@StartDate", blism.StartDate);
                //dB.addIn("@EndDate", blism.EndDate);
                //dB.addIn("@LastUpdatedDate", blism.LastUpdatedDate);
                dB.addIn("@AssignedTo", blism.AssignedTo);
                dB.addIn("@Status", blism.Status);
                dB.addIn("@IsApproved", false);
                dB.addIn("@Cre_By", blism.Cre_By);
                dB.addIn("@Comments", Encrypt(blism.Comments));
                dB.addIn("@Description", blism.Description);

                dB.ExecuteScalar();
                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "");
            }
            return Status;
        }

        public List<IssueTrackModel> Sp_GetIssueTrackData(String Id, int type, IssueTrackModel Model)
        {
            List<IssueTrackModel> ITM = new List<IssueTrackModel>();
            DataTable dt = new DataTable();
            //if (Status)
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                if (type == 2)
                {
                    dB.addIn("@Type", "Pull_Consultant_Data");
                }
                if (type == 3)
                {
                    dB.addIn("@Type", "PullData");
                }
                if (type == 4)
                {
                    dB.addIn("@Type", "Pull_PM_Data");
                }
                dB.addIn("@Id", Id);
                dB.addIn("@Customer_Id", Model.Customer_Id);
                dB.addIn("@Project_Id", Model.Project_Id);
                dB.addIn("@ProjectInstance_Id", Model.Instance_Id);

                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        IssueTrackModel P = new IssueTrackModel();
                        P.Issuetrack_Id = Guid.Parse(dr["Issuetrack_Id"].ToString());
                        P.RunningID = Convert.ToInt32(dr["RunningID"].ToString());
                        P.IssueName = dr["IssueName"].ToString();
                        P.PhaseID = Convert.ToInt32(dr["PhaseID"].ToString());
                        //P.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                        P.Task = dr["Task"].ToString();
                        P.ProjectInstance_Id = Guid.Parse(dr["ProjectInstance_Id"].ToString());
                        P.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                        //P.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                        P.LastUpdatedDate = Convert.ToDateTime(dr["LastUpdatedDate"].ToString());
                        P.AssignedTo = Guid.Parse(dr["AssignedTo"].ToString());
                        P.Status = dr["Status"].ToString();
                        P.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                        P.Comments = dr["Comments"].ToString();
                        P.Phase = dr["Phase"].ToString();
                        P.Assigned = dr["Assigned"].ToString();
                        P.Instance = dr["Instance"].ToString();
                        P.Project = dr["Project"].ToString();
                        P.Customer = dr["Customer"].ToString();
                        P.Created_By = dr["Created_By"].ToString();
                        P.Cre_By = Guid.Parse(dr["Cre_By"].ToString());
                        // P.IssueID = dr["IssueID"].ToString();
                        if (P.PhaseID == 1)
                        {
                            P.IssueID = "Ass_" + P.RunningID;
                        }
                        if (P.PhaseID == 2)
                        {
                            P.IssueID = "Pre_" + P.RunningID;
                        }
                        if (P.PhaseID == 3)
                        {
                            P.IssueID = "Con_" + P.RunningID;
                        }
                        if (P.PhaseID == 4)
                        {
                            P.IssueID = "Post_" + P.RunningID;
                        }
                        if (P.PhaseID == 5)
                        {
                            P.IssueID = "Val_" + P.RunningID;
                        }

                        ITM.Add(P);
                    }
                }
            }
            return ITM;
        }

        public IssueTrackModel Sp_EditIssueTrackData(Guid id)
        {

            LogHelper _log = new LogHelper();

            DataTable dt = new DataTable();

            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "EditIssueTrack");

            dB.addIn("@Id", id);
            dt = dB.ExecuteDataTable();
            IssueTrackModel P = new IssueTrackModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    P.Issuetrack_Id = Guid.Parse(dr["Issuetrack_Id"].ToString());
                    P.RunningID = Convert.ToInt32(dr["RunningID"].ToString());
                    P.IssueName = dr["IssueName"].ToString();
                    P.PhaseID = Convert.ToInt32(dr["PhaseID"].ToString());
                    P.Task = dr["Task"].ToString();
                    P.ProjectInstance_Id = Guid.Parse(dr["ProjectInstance_Id"].ToString());
                    P.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    //P.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                    P.LastUpdatedDate = Convert.ToDateTime(dr["LastUpdatedDate"].ToString());
                    P.AssignedTo = Guid.Parse(dr["AssignedTo"].ToString());
                    P.Status = dr["Status"].ToString();
                    P.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                    //P.Comments = dr["Comments"].ToString();
                    P.Phase = dr["Phase"].ToString();
                    P.Assigned = dr["Assigned"].ToString();
                    P.Cre_By = Guid.Parse(dr["Cre_By"].ToString());

                }
            }
            return P;
        }
        public SAPIssueTrackModel Sp_EditSAPIssueTrackData(Guid id)
        {

            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "SAPEditIssueTrack");

            dB.addIn("@Id", id);
            dt = dB.ExecuteDataTable();
            SAPIssueTrackModel P = new SAPIssueTrackModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    P.SAPIssuetrack_Id = Guid.Parse(dr["Id"].ToString());
                    P.RunningID = Convert.ToInt32(dr["RunningID"].ToString());
                    P.IssueName = dr["IssueName"].ToString();
                    P.status = dr["SAPIssueDumpStatus_Id"].ToString();                    
                    P.Comments = dr["Comments"].ToString();                    
                }
            }
            return P;
        }
        public bool Sp_UpdateSAPIssueTrack(Guid Id, int SAPStatus, string Comments)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateSAPIssueTrack");
                dB.addIn("@Id", Id);
                dB.addIn("@Status", SAPStatus);
                dB.addIn("@Comments", Comments);
               
                dB.ExecuteScalar();
                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "");
            }
            return Status;
        }


        public List<UserModel> Sp_EditAssignedTo(Guid Iid, Guid id)
        {
            List<UserModel> ListM = new List<UserModel>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "EditAssignedTo");
            dB.addIn("@Instance_Id", Iid);
            dB.addIn("@Id", id);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel U = new UserModel();
                    U.UserId = Guid.Parse(dr["UserId"].ToString());
                    U.Name = dr["Name"].ToString();

                    ListM.Add(U);
                }
            }
            return ListM;
        }

        public List<ProjectMonitorModel> Sp_GetStatus()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Master", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetStatus");
            dt = dB.ExecuteDataTable();
            List<ProjectMonitorModel> L = new List<ProjectMonitorModel>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    ProjectMonitorModel P = new ProjectMonitorModel();
                    P.StatusId = Convert.ToInt32(dr["Id"].ToString());
                    P.Status = dr["StatusName"].ToString();
                    L.Add(P);
                }
            }

            return L;
        }

        public bool Sp_UpdateIssueTrack(IssueTrackModel Data)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateIssueTrack");
                dB.addIn("@Id", Data.Issuetrack_Id);
                dB.addIn("@Status", Data.Status);
                //dB.addIn("@EndDate", Data.EndDate);
                dB.addIn("@Description", Data.Description);
                dB.addIn("@AssignedTo", Data.AssignedTo);
               // dB.addIn("@Comments", Data.Comments);
                dB.addIn("@Cre_By", Data.Modified_by);

                dB.Execute();

                Status = true;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "");
            }

            return Status;
        }
        //Manoj
        public List<IssueTrackModel> Sp_IssueTrackComments(Guid ProjectMonitor_Id,int timezoneOffset)
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetComments");
            dB.addIn("@Id", ProjectMonitor_Id);

            dt = dB.ExecuteDataTable();
            List<IssueTrackModel> L = new List<IssueTrackModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    IssueTrackModel P = new IssueTrackModel();
                    P.Comments = Decrypt(dr["HistoryComment"].ToString());

                    P.HistoryLogId = Convert.ToInt32(dr["HistoryLogId"].ToString());
                    //P.UserID = Guid.Parse(dr["User_Id"].ToString());
                    P.UserID = dr["USERID"].ToString();
                    P.Image = List_LoadPhoto(P.UserID);
                    P.Name = dr["Name"].ToString();                    
                   // P.Cre_on_str = dr["Cre_on"].ToString();
                    P.Cre_on = DateTime.Parse(dr["Cre_on"].ToString());
                    P.Cre_on_str = FromUTCData(P.Cre_on, timezoneOffset).ToString();
                    //P.ShortName = GetFirstandLastName(P.Name);
                    L.Add(P);
                }
            }

            return L;
        }

        //public bool Sp_UpdateIssueTrackComments(String Comments, Guid id, Guid Cre_By)
        public bool Sp_UpdateIssueTrackComments(CkEditor ckEditor)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();

            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "InsertComments");
                dB.addIn("@Id", ckEditor.Id);
                dB.addIn("@Comments", Encrypt(ckEditor.Comments));
                dB.addIn("@Cre_By", ckEditor.Cre_By);
                dB.ExecuteScalar();
                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public bool Sp_AssigneeCount(Guid loginid)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "AssigneeCount");
                dB.addIn("@Id", loginid);
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }                    
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }

        public List<SAPIssueTrackModel> SP_LoadSAPIssueTrack(string ProjectId, string TimeZone)
        {
            List<SAPIssueTrackModel> L = new List<SAPIssueTrackModel>();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();

            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "LoadSAPIssueTrack");
                dB.addIn("@Project_Id", ProjectId);
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        
                        SAPIssueTrackModel ITM = new SAPIssueTrackModel();
                        ITM.SAPIssuetrack_Id = Guid.Parse(dr["Id"].ToString());
                        ITM.RunningID = Convert.ToInt32(dr["RunningID"].ToString());
                        ITM.IssueNo = Convert.ToInt32(dr["IssueNo"].ToString());
                        ITM.IssueName = dr["IssueName"].ToString();
                        ITM.Category = dr["CategoryName"].ToString();
                        ITM.Priority = dr["PriorityName"].ToString();
                        ITM.Assignee = dr["Assignee"].ToString();
                        ITM.RaisedBy = dr["RaisedBy"].ToString();
                        ITM.ApplicationArea = dr["ApplicationAreaName"].ToString();
                        
                        var OpenDt1 = DateTimeOffset.Parse(dr["OpenDt1"].ToString() + "+00:00");
                        ITM.OpenDt = ConvertUtcToLocal(OpenDt1, TimeZone).ToString();

                        var CloseDt = DateTimeOffset.Parse(dr["CloseDt1"].ToString() + "+00:00");
                        ITM.CloseDt = ConvertUtcToLocal(CloseDt, TimeZone).ToString();
                        

                        ITM.status = dr["StatusName"].ToString();
                        ITM.Resolution = dr["Resolution"].ToString();
                        ITM.Comments = dr["Comments"].ToString();
                        L.Add(ITM);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return L;
        }


        public DateTimeOffset ConvertUtcToLocal(DateTimeOffset Dt ,string LocalTimeZone)
        {
            var Timezone = TimeZoneInfo.FindSystemTimeZoneById(LocalTimeZone);
            DateTimeOffset ConvertedTime = TimeZoneInfo.ConvertTime(Dt, Timezone);

            return ConvertedTime;
        }
        public DateTime ConvertLocalToUTC(DateTime Dt, string LocalTimeZone)
        {
            

            var Timezone = TimeZoneInfo.FindSystemTimeZoneById(LocalTimeZone);
            var ConvertedUtcTime= TimeZoneInfo.ConvertTimeToUtc(Dt, Timezone);
           // DateTimeOffset ConvertedTime = TimeZoneInfo.ConvertTime(Dt, Timezone);

            return ConvertedUtcTime;
        }
        public List<SAPIssueTrackModel> Sp_GetSAPStatus()
        {
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetSAPStatus");
            dt = dB.ExecuteDataTable();
            List<SAPIssueTrackModel> L = new List<SAPIssueTrackModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SAPIssueTrackModel P = new SAPIssueTrackModel();
                    P.Status_Id = Convert.ToInt32(dr["Id"].ToString());
                    P.status = dr["StatusName"].ToString();
                    L.Add(P);
                }
            }

            return L;
        }

        #endregion

        #region Drop down
        public GeneralList GetInstanceDropdown(string projectID, string pagename)
        {
            GeneralList sP_ = new GeneralList();
            DataSet ds = new DataSet();
            LogHelper _log = new LogHelper();
            try
            {
                Guid ID = Guid.Parse(projectID);
                DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
                if (pagename == "Analysis")
                {
                    dB.addIn("@Type", "GetInstance_Analysis");
                }
                else if (pagename == "Readiness")
                {
                    dB.addIn("@Type", "GetInstance_Readiness");
                }
                dB.addIn("@Id", ID);
                ds = dB.ExecuteDataSet();
                List<Lis> _Lob = new List<Lis>();
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    int count = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        _Lob.Add(new Lis
                        {
                            Name = dr["InstaceName"].ToString(),
                            Value = dr["Id"].ToString()
                        });
                        count = count++;
                    }
                }


                sP_._List = _Lob;
            }
            catch (Exception ex)
            {

                _log.createLog(ex, "--->GetInstanceDropdown");
            }



            return sP_;
        }

        public Boolean SP_GetInstanceStatus(Guid Instance)
        {
            Boolean Result = true;
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetInstanceStatus");
            dB.addIn("@Previous_Instance", Instance);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count>0)
            {
                int Total = 0,Actual=0;
                Total = Convert.ToInt32(dt.Rows[0]["Total"].ToString());
                Actual = Convert.ToInt32(dt.Rows[0]["Actual"].ToString());
                if (Total==0)
                {
                    Result = false;
                }
                else if(Actual>0)
                {
                    Result = false;
                }
            }

            return Result;
        }

        public Boolean SAPUploadList(Guid Instance_Id)
        {
            Boolean Status = false;
            DataTable dt = new DataTable();
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetSAPFileUploadStatus");
                dB.addIn("@InstanceID", Instance_Id);
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "--->SP_SAPUploadList");
            }
            return Status;
        }

        #endregion


        #region Send Mail To Customer
        public Boolean sendmailtocustomer(String Name, String Email)
        {
            Boolean Status = false;

            LogHelper _log = new LogHelper();
            try
            {
                DBHelper dB = new DBHelper("SP_Mail", CommandType.StoredProcedure);
                dB.addIn("@Type", "SendCustomerMail");
                dB.addIn("@Q_Name", Name);
                dB.addIn("@Q_Mail", Email);
                dB.addIn("@Cre_By", "00000000-0000-0000-0000-000000000000");
                dB.ExecuteScalar();

                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "-->AddIssueTrack_Mail" + ex.Message.ToString());
            }

            return Status;
        }

        #endregion

        #region Audit
        public List<AuditReport.ProjectMonitorModel> Sp_GetAuditDatas(AuditReport.ProjectMonitorModel model, int timezoneOffset)
        {
            DataTable dt = new DataTable();
            AuditReport A = new AuditReport();
            List<AuditReport.ProjectMonitorModel> AR = new List<AuditReport.ProjectMonitorModel>();
            DBHelper dB = new DBHelper("SP_Audit", CommandType.StoredProcedure);
            if (model.ActionID == null && model.TABLE_NAME == null)
            {
                dB.addIn("@Type", "AuditReport");
            }
            else
            {
                dB.addIn("@Type", "AuditReportsearch");
                dB.addIn("@Action", model.ActionID);
                dB.addIn("@Tablename", model.TABLE_NAME);
            }
            dB.addIn("@Startdate", model.startdate);
            dB.addIn("@enddate", model.enddate);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AuditReport.ProjectMonitorModel A_PM = new AuditReport.ProjectMonitorModel();
                    A_PM.Id = Convert.ToInt32(dr["AUDIT_ID"].ToString());
                    A_PM.NAME = dr["NAME"].ToString();
                    A_PM.TABLE_NAME = dr["TABLE_NAME"].ToString();
                    A_PM.SUMMARY = dr["SUMMARY"].ToString();
                    A_PM.ACTION = dr["ACTION"].ToString();
                    A_PM.CREATED_DATE = Convert.ToDateTime(dr["CREATED_DATE"].ToString());
                    //A_PM.Str_CREATED_DATE = dr["CREATED_DATE"].ToString();

                    //DateTime convertedDate = DateTime.Parse(dr["CREATED_DATE"].ToString());
                    // DateTime convertedDate = DateTime.SpecifyKind(DateTime.Parse(dr["CREATED_DATE"].ToString()), DateTimeKind.Utc);
                    //DateTime dt1 = convertedDate.ToLocalTime();
                    //A_PM.Str_CREATED_DATE = convertedDate.ToLocalTime().ToString();

                    //DateTime now = DateTime.Parse(dr["CREATED_DATE"].ToString());
                    //TimeZoneInfo tzinfo = TimeZoneInfo.Local;
                    //DateTime localNow = TimeZoneInfo.ConvertTimeFromUtc(now, tzinfo);
                    //A_PM.Str_CREATED_DATE = localNow.ToString();

                    A_PM.CREATED_DATE = FromUTCData(A_PM.CREATED_DATE, timezoneOffset);
                    A_PM.Str_CREATED_DATE = A_PM.CREATED_DATE.ToString();
                    AR.Add(A_PM);

                }
            }
            return AR;
        }
        public static DateTime FromUTCData(DateTime? dt, int timezoneOffset)
        {
            ////  Convert a DateTime (which might be null) from UTC timezone
            ////  into the user's timezone. 

            //if (dt == null)
            //    return null;

            DateTime newDate = dt.Value - new TimeSpan(timezoneOffset / 60, timezoneOffset % 60, 0);
            return newDate;
        }


        #endregion

        #region Chart
        public SP_ReadinessReport_Result sAPInput(Guid InstanceId)
        {
            SP_ReadinessReport_Result GetRelevant = new SP_ReadinessReport_Result();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "Simple_Donut");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count == 1)
            {
                GetRelevant.RC = Convert.ToInt32(dt.Rows[0]["RC"].ToString());
                GetRelevant.RC_NON = Convert.ToInt32(dt.Rows[0]["RC_NON"].ToString());
                GetRelevant.R_NON = Convert.ToInt32(dt.Rows[0]["R_NON"].ToString());
                GetRelevant.R = Convert.ToInt32(dt.Rows[0]["R"].ToString());
            }
            return GetRelevant;
        }
        public GeneralList sP_GetActivities_Bar1(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "Activities_Bar1");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["ACT_NAME"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        public GeneralList sP_GetCustomCode_Bar(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "CustomCode_Bar");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["_Status"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }

        public GeneralList sP_GetActivities_Donut(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "Activities_Donut");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["Condition"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }

        public GeneralList sP_GetActivities_Bar2(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "Activities_Bar2");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["Phase"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }

        public GeneralList sP_GetFiori_Bar(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "Fiori_Bar");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["Application_Area"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }

        public GeneralList sP_SimplificationReport(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_SimplificationReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetDropdown");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            List<Lis> _Lob = new List<Lis>();

            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                _Lob.Add(new Lis { Name = dr["LOB"].ToString(), _Value = count });
                count = count++;
            }

            sP_._List = _Lob;
            return sP_;
        }
        public GeneralList sP_SimplificationReport_Bar(String Input, Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_SimplificationReport", CommandType.StoredProcedure);
            if (Input == "ALL")
            {
                dB.addIn("@Type", "ALL");
                dB.addIn("@InstanceId", InstanceId);
            }
            else
            {
                dB.addIn("@Type", "Single");
                dB.addIn("@Input", Input);
                dB.addIn("@InstanceId", InstanceId);
            }
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["LOB_NAME"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        public List<SimplificationReport> SAPInput_Simplification(Guid InstanceId, string LOB)
        {
            List<SimplificationReport> SR = new List<SimplificationReport>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_SimplificationReport", CommandType.StoredProcedure);

            dB.addIn("@InstanceId", InstanceId);
            if (LOB == "ALL")
            {
                dB.addIn("@Type", "SR_Table_All");
            }
            else
            {
                dB.addIn("@Type", "SR_Table_Single");
                dB.addIn("@Input", LOB);

            }
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    SimplificationReport data = new SimplificationReport();
                    data.S_No = i;
                    data.Title = dr["Title"].ToString();
                    data.Category = dr["Category"].ToString();
                    data.Relevance = dr["Relevance"].ToString();
                    data.LoB_Technology = dr["LoB/Technology"].ToString();
                    data.Business_Area = dr["Business Area"].ToString();
                    data.Consistency_Status = dr["Consistency Status"].ToString();
                    data.Manual_Status = dr["Manual Status"].ToString();
                    data.SAP_Notes = dr["SAP Notes"].ToString();
                    data.Relevance_Summary = dr["Relevance Summary"].ToString();
                    SR.Add(data);
                }
            }
            return SR;
        }

        public Instances Sp_GetSysLandScape(Guid InstanceId)
        {
            DataTable dt = new DataTable();
            Instances i = new Instances();
            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetSysLandScape");
            dB.addIn("@InstanceID", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i.Sys_Landscape = dr["Sys_Landscape"].ToString();
                }
            }
            return i;
        }

        public List<RFCFMReport> SAPInput_RFCFMReport(Guid InstanceId, int val,string sys_LandScape)
        {
            List<RFCFMReport> SR = new List<RFCFMReport>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_RFCFMReport", CommandType.StoredProcedure);
            LogHelper _log = new LogHelper();
            if (val == 1)
                dB.addIn("@Type", "CoreSystemDetails");
            else if (val == 2)
                dB.addIn("@Type", "InstalledSoftwareComponent");
            else if (val == 3)
                dB.addIn("@Type", "InstalledProductVersions");
            else if (val == 4)
                dB.addIn("@Type", "SFW5REPORT");

            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@LandScape", sys_LandScape);
            try
            {
                dt = dB.ExecuteDataTable();
                int i = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        i += 1;
                        RFCFMReport data = new RFCFMReport();
                        data.S_No = i;
                        if (val == 1)
                        {
                            data.Parameter = dr["Parameter"].ToString();
                            data.Value = dr["Value"].ToString();
                        }
                        else if (val == 2)
                        {
                            data.Component = dr["Component"].ToString();
                            data.Release = dr["Release"].ToString();
                            data.SP_Level = dr["SP_Level"].ToString();
                            data.Comp_Type = dr["Comp_Type"].ToString();
                            data.Description = dr["Description"].ToString();
                        }
                        else if (val == 3)
                        {
                            data.Product = dr["Product"].ToString();
                            data.Release = dr["Release"].ToString();
                            data.SP_Stack = dr["SP_Stack"].ToString();
                            data.Vendor = dr["Vendor"].ToString();
                            data.Description = dr["Description"].ToString();
                        }
                        else if (val == 4)
                        {
                            data.Group = dr["Group"].ToString();
                            data.BF_Name = dr["BF_Name"].ToString();
                            data.Description = dr["BF_Description"].ToString();
                            data.Dependency = dr["Dependency"].ToString();
                            data.Component = dr["SW_Component"].ToString();
                            data.Release = dr["SW_Release"].ToString();
                        }

                        SR.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }



            return SR;
        }

        public RFCFMReport SAPInput_summery_sys_report(Guid InstanceId, string sys_LandScape)
        {
            RFCFMReport rFCFM = new RFCFMReport();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_RFCFMReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "summery_sys_report");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@LandScape", sys_LandScape);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rFCFM.SAPECC = dr["SAPECC"].ToString();
                    rFCFM.NetWeaver = dr["NetWeaver"].ToString();
                    rFCFM.TypeVersion = dr["TypeVersion"].ToString();
                    rFCFM.Database = dr["Database"].ToString();
                    rFCFM.OS = dr["OS"].ToString();
                }
            }
                return rFCFM;
        }
        public List<RFCFMReport> Sp_GetBuildingDocuments(Guid InstanceId, string sys_LandScape)
        {
            List<RFCFMReport> SR = new List<RFCFMReport>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_RFCFMReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "BillingDocuments");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@LandScape", sys_LandScape);
            dt = dB.ExecuteDataTable();

            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    RFCFMReport data = new RFCFMReport();
                    data.S_No = i;

                    data.CompanyCode = dr["CompanyCode"].ToString();
                    data.Currency = dr["Currency"].ToString();
                    data.FKREL = dr["FKREL"].ToString();
                    data.BillingCreatedBy = dr["BillingCreatedBy"].ToString();
                    data.NoofDocuments = dr["NoofDocuments"].ToString();

                    SR.Add(data);
                }
            }
            return SR;
        }
        public List<RFCFMReport> Sp_GetOrderDocuments(Guid InstanceId, string sys_LandScape)
        {
            List<RFCFMReport> SR = new List<RFCFMReport>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_RFCFMReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "OrderDocuments");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@LandScape", sys_LandScape);
            dt = dB.ExecuteDataTable();

            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    RFCFMReport data = new RFCFMReport();
                    data.S_No = i;

                    data.ControllingArea = dr["ControllingArea"].ToString();
                    data.CCbilled = dr["CCbilled"].ToString();
                    data.Plant = dr["Plant"].ToString();
                    data.DocCategory = dr["DocCategory"].ToString();
                    data.NoofDocuments = dr["NoofDocuments"].ToString();

                    SR.Add(data);
                }
            }
            return SR;
        }
        public List<RFCFMReport> Sp_GetComplexityAnalysis(Guid InstanceId)
        {
            List<RFCFMReport> SR = new List<RFCFMReport>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_RFCFMReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "ComplexityAnalysis");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();

            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    RFCFMReport data = new RFCFMReport();
                    data.S_No = i;

                    data.CompanyCode = dr["CompanyCode"].ToString();
                    data.NewGL = dr["NewGL"].ToString();
                    data.Leadingledger = dr["Leadingledger"].ToString();
                    data.SpecialPurposeLedger = dr["SpecialPurposeLedger"].ToString();
                    data.Treasury_SWF5_FSCM_CLM = dr["Treasury_SWF5_FSCM_CLM"].ToString();
                    data.Treasury_SWF5_FSCM_BNK = dr["Treasury_SWF5_FSCM_BNK"].ToString();
                    data.Multicurrencymodel = dr["Multicurrencymodel"].ToString();
                    data.NewAssetAccounting = dr["NewAssetAccounting"].ToString();
                    data.BusinessPartner = dr["BusinessPartner"].ToString();
                    data.BPActive = dr["BPActive"].ToString();
                    data.MaterialLedger = dr["MaterialLedger"].ToString();
                    data.Active = dr["Active"].ToString();

                    SR.Add(data);
                }
            }
            return SR;
        }
        public List<RFCFMReport> Sp_GetSalesDocuments(Guid InstanceId, string sys_LandScape)
        {
            List<RFCFMReport> SR = new List<RFCFMReport>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_RFCFMReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "SalesDocuments");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@LandScape", sys_LandScape);
            dt = dB.ExecuteDataTable();

            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    RFCFMReport data = new RFCFMReport();
                    data.S_No = i;

                    data.CCbilled = dr["CCbilled"].ToString();
                    data.SalesOrg = dr["SalesOrg"].ToString();
                    data.DistChannel = dr["DistChannel"].ToString();
                    data.Division = dr["Division"].ToString();
                    data.Plant = dr["Plant"].ToString();
                    data.NoofDocuments = dr["NoofDocuments"].ToString();

                    SR.Add(data);
                }
            }
            return SR;
        }

        public List<RFCFMReport> Sp_GetMaterialityScoreDocuments(Guid InstanceId)
        {
            List<RFCFMReport> SR = new List<RFCFMReport>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_RFCFMReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "MaterialityScoreDocuments");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();

            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    RFCFMReport data = new RFCFMReport();
                    data.S_No = i;

                    data.FunctionalArea = dr["FunctionalArea"].ToString();
                    data.Count = dr["Count"].ToString();
                    data.Percentage = dr["Percentage"].ToString();
                    
                    SR.Add(data);
                }
            }
            return SR;
        }

        public GeneralList sP_GetBreakdownbyLob_Bar(Guid InstanceId, string sys_LandScape)
        {
            GeneralList sP_ = new GeneralList();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_RFCFMReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "MaterialityScoreDocuments");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@LandScape", sys_LandScape);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["FunctionalArea"].ToString(),
                            _Value = Convert.ToInt32(dr["Count"].ToString()),
                            percent= dr["Percentage"].ToString()
                        });

                }
                sP_._List = _Lob;
            }
            return sP_;
        }

        public bool Sp_StoreSAPTables(Guid Ins,DataTable SALESDATA, DataTable BILLSDATA, DataTable ORDERSDATA, DataTable FACOUNTDATA, DataTable SAPDATA1, DataTable SAPDATA2, DataTable SAPDATA3, DataTable SAPDATA4, string Cre_By)
        {
            bool status = false;
            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            try
            {
                dB.addIn("@Type", "SetSapTables");
                dB.addIn("@tblSalesDoc", SALESDATA);
                dB.addIn("@tblBillingDoc", BILLSDATA);
                dB.addIn("@tblOrderDoc", ORDERSDATA);
                dB.addIn("@tblMatScore", FACOUNTDATA);
                dB.addIn("@tblRFCFM", SAPDATA1);
                dB.addIn("@tblSOFTWARECOMPONENT", SAPDATA2);
                dB.addIn("@tblPRODUCTVERSIONS", SAPDATA3);
                dB.addIn("@tblSFW5REPORT", SAPDATA4);
                dB.addIn("@Id", Ins);
                dB.addIn("@Cre_By", Cre_By);

                dB.ExecuteScalar();
                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return status;
        }

        public Tuple<List<Lis>, List<Lis>> sp_GetActivitiesReportDropdown(Guid InstanceId)
        {
            List<Lis> list1 = new List<Lis>();
            List<Lis> list2 = new List<Lis>();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_ActivitiesReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetDropdown");
            dB.addIn("@InstanceId", InstanceId);
            ds = dB.ExecuteDataSet();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    int count = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        list1.Add(new Lis
                        {
                            Name = dr["Phase"].ToString(),
                            _Value = count
                        });
                        count = count++;
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[1];
                    int count = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        list2.Add(new Lis
                        {

                            Name = dr["Condition"].ToString(),
                            _Value = count
                        });
                        count = count++;
                    }
                }
            }

            return Tuple.Create(list1, list2);
        }
        public List<Activities_Report> GetActivitiesReport_Table(string Phase, string condition, Guid InstanceId)
        {
            List<Activities_Report> AR = new List<Activities_Report>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ActivitiesReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "ACT_Table");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@Phase", Phase);
            dB.addIn("@condition", condition);
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    Activities_Report data = new Activities_Report();
                    data.S_No = i;
                    data.Related_Simplification_Items = dr["Related Simplification Items"].ToString();
                    data.Activities = dr["Activities"].ToString();
                    data.Phase = dr["Phase"].ToString();
                    data.Condition = dr["Condition"].ToString();
                    data.Additional_Information = dr["Additional Information"].ToString();

                    AR.Add(data);
                }
            }
            return AR;
        }
        public GeneralList sP_GetActivitiesReport_Bar(string Phase, string condition, Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();//manoj
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ActivitiesReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "ALL");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@Phase", Phase);
            dB.addIn("@condition", condition);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["ACT_NAME"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }
        public List<SAPInput_CustomCode> SAPInput_CustomCodeReport(Guid InstanceId)
        {
            List<SAPInput_CustomCode> CR = new List<SAPInput_CustomCode>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_CustomCode", CommandType.StoredProcedure);
            dB.addIn("@Type", "CustomTable");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    SAPInput_CustomCode data = new SAPInput_CustomCode();
                    data.S_No = i;
                    data.Custom_Code_Topic = dr["Custom Code Topic"].ToString();
                    data.Status = dr["Status"].ToString();
                    data.Application_Component = dr["Application Component"].ToString();
                    data.Custom_Code_Note = dr["Custom Code Note"].ToString();

                    CR.Add(data);
                }
            }
            return CR;
        }  
        public List<SAPInput_HanaDatabase> SAPInput_S4hanaDatabaseReport(Guid InstanceId)
        {
            List<SAPInput_HanaDatabase> CR = new List<SAPInput_HanaDatabase>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "hanaDatabaseReport");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    SAPInput_HanaDatabase data = new SAPInput_HanaDatabase();
                    data.S_No = i;
                    data.Name = dr["Name"].ToString();
                    data.StoreType = dr["Store Type"].ToString();
                    data.DataSize = dr["Data Size in GB"].ToString();
                    data.No_of_Records = dr["Number of Records"].ToString();

                    CR.Add(data);
                }
            }
            return CR;
        }

        public GeneralList sp_GetFioriAppsReportDropdown(Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();
            DataSet ds = new DataSet();
            DBHelper dB = new DBHelper("SP_FioriAppsReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetDropdown");
            dB.addIn("@InstanceId", InstanceId);
            ds = dB.ExecuteDataSet();
            List<Lis> _Lob = new List<Lis>();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(new Lis
                    {
                        Name = dr["Roles"].ToString(),
                        _Value = count
                    });
                    count = count++;
                }
            }


            sP_._List = _Lob;
            return sP_;
        }
        public List<SAPFioriAppsModel> sp_GetSAPFioriAppsTable(String Role, Guid InstanceId)
        {
            List<SAPFioriAppsModel> FiR = new List<SAPFioriAppsModel>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_FioriAppsReport", CommandType.StoredProcedure);
            //dB.addIn("@Type", "FioriApps_Table");
            dB.addIn("@InstanceId", InstanceId);
            if (Role == "ALL")
            {
                dB.addIn("@Type", "FioriApps_Table");
            }
            else
            {
                dB.addIn("@Type", "FioriApps_Table_Single");
                dB.addIn("@Input", Role);

            }
            dt = dB.ExecuteDataTable();
            //  List<DataRow> list = new List<DataRow>(dt.Select());
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i += 1;
                    SAPFioriAppsModel data = new SAPFioriAppsModel();
                    data.S_No = i;
                    data.Role = dr["Role"].ToString();
                    data.Name = dr["Name"].ToString();
                    data.Application_Area = dr["Application Area"].ToString();
                    data.Description = dr["Description"].ToString();

                    FiR.Add(data);
                }
            }
            return FiR;
        }
        public GeneralList sP_GetSAPFioriAppsReport_Bar(string Input, Guid InstanceId)
        {
            GeneralList sP_ = new GeneralList();//manoj
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_FioriAppsReport", CommandType.StoredProcedure);

            dB.addIn("@Type", "ALL");
            dB.addIn("@InstanceId", InstanceId);
            dB.addIn("@Input", Input);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in dt.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = dr["Roles"].ToString(),
                            _Value = Convert.ToInt32(dr["_Count"].ToString()
                            )
                        });

                }

                sP_._List = _Lob;


            }
            return sP_;
        }


        public SP_ReadinessReport_Result HanaCountInput(Guid InstanceId)
        {
            LogHelper _log = new LogHelper();
            SP_ReadinessReport_Result GetRelevant = new SP_ReadinessReport_Result();
            DataTable dt = new DataTable();
            try
            {
                DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
                dB.addIn("@Type", "ECC_HanaCount");
                dB.addIn("@InstanceId", InstanceId);
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count == 1)
                {
                    GetRelevant.Hana = Convert.ToInt32(dt.Rows[0]["Hana"].ToString());
                    GetRelevant.ECC = Convert.ToInt32(dt.Rows[0]["ECC"].ToString());
                }
            }
            catch(Exception ex)
            {
                _log.createLog("HanaCountInput-->"+ex.Message);
            }           
            return GetRelevant;
        }
        public List<EccHanaCountModel> HanaCountTable(Guid InstanceId)
        {
            List<EccHanaCountModel> ECC_CountTable = new List<EccHanaCountModel>();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
                dB.addIn("@Type", "ECC_HanaCount_Table");
                dB.addIn("@InstanceId", InstanceId);
                dt = dB.ExecuteDataTable();
                int i = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        i += 1;
                        EccHanaCountModel data = new EccHanaCountModel();
                        data.S_No = i;
                        data.User = dr["User"].ToString();
                        data.UserType = dr["UserType"].ToString();
                        data.UserCategory = dr["Catergory"].ToString();
                        //data.User_Status = dr["User_Status"].ToString();
                        data.System = dr["System"].ToString();
                        data.Last_Logon = dr["Last_Logon"].ToString();

                        ECC_CountTable.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ECC_CountTable;
        }  
        
        public List<BwExtractorsModel> GETBwextractorsData(Guid InstanceId)
        {
            List<BwExtractorsModel> BwExtractorsTable = new List<BwExtractorsModel>();
            try
            {
                DataTable dt = new DataTable();
                DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
                dB.addIn("@Type", "BwExtractors");
                dB.addIn("@InstanceId", InstanceId);
                dt = dB.ExecuteDataTable();
                int i = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        i += 1;
                        BwExtractorsModel data = new BwExtractorsModel();
                        data.S_No = i;
                        data.Extractor_Name = dr["Extractor Name"].ToString();
                        data.Application_Area = dr["Application Area"].ToString();
                        data.Related_Simplification_Items = dr["Related Simplification Items"].ToString();
                        data.Additional_Information = dr["Additional Information"].ToString();

                        BwExtractorsTable.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return BwExtractorsTable;
        }

        

        #region Project View
        public List<PlannedVsActual> sp_PlannedVsActual(Guid InstanceId)
        {              
           // PlannedVsActual P = new PlannedVsActual();
           List <PlannedVsActual> DL = new List<PlannedVsActual>();
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //DataTable dt1 = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "PlannedVsActual");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            //dt = ds.Tables[0];
            //dt1 = ds.Tables[1];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PlannedVsActual d = new PlannedVsActual();

                    d.ID = Guid.Parse(dr["id"].ToString());
                    d.ActivityID = Convert.ToInt32(dr["ActivityID"].ToString());
                    d.Task = dr["Task"].ToString();
                    d.Planed__St_Date = dr["Planed__St_Date"].ToString();
                    d.Planed__En_Date = dr["Planed__En_Date"].ToString();
                    d.Actual_St_Date = dr["Actual_St_Date"].ToString();
                    d.Actual_En_Date = dr["Actual_En_Date"].ToString();
                    d.Actual_St_hours = Convert.ToDecimal(dr["Actual_St_hours"].ToString());

                    DL.Add(d); 
                }
            }
            //if (dt1.Rows.Count > 0)
            //{
             
            //    DL.minDate=dt1.Rows[0].ToString();
            //    DL.maxDate = dt1.Rows[1].ToString();
            //}
                return DL;
        }

        public List<Planned_Dates> sp_Planned_Dates(Guid InstanceId)
        {
            // PlannedVsActual P = new PlannedVsActual();
            List<Planned_Dates> DL = new List<Planned_Dates>();
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //DataTable dt1 = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "Planned_Dates");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            //dt = ds.Tables[0];
            //dt1 = ds.Tables[1];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Planned_Dates d = new Planned_Dates();

                    d.Id = Convert.ToInt32(dr["id"].ToString());
                    d.Planed_StDate = dr["Planed_StDate"].ToString();
                    d.Planed_EnDate = dr["Planed_EnDate"].ToString();
                    d.Phase = dr["Phase"].ToString();
                    
                    DL.Add(d);
                }
            }
            //if (dt1.Rows.Count > 0)
            //{

            //    DL.minDate=dt1.Rows[0].ToString();
            //    DL.maxDate = dt1.Rows[1].ToString();
            //}
            return DL;
        }

        public ReadinssChaeckOrStatus Sp_ReadinssChaeckOrStatus(Guid InstanceId)
        {
            ReadinssChaeckOrStatus d = new ReadinssChaeckOrStatus();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "ReadinssChaeckOrStatus");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    bool b = Convert.ToBoolean(dr["UploadStatus"].ToString());
                    if (b)
                    {
                        d.UploadStatus = 1;
                    }
                    d.Completed = Convert.ToInt32(dr["Completed"].ToString());
                    d.Pending = Convert.ToInt32(dr["Pending"].ToString());
                    // ret = ;
                }
            }
            return d;

        }

        public List<ReadinssChaeckOrStatus> Sp_GetProjectViewCount(Guid InstanceId)
        {
            List<ReadinssChaeckOrStatus> dM = new List<ReadinssChaeckOrStatus>();
            ReadinssChaeckOrStatus d = new ReadinssChaeckOrStatus();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_ReadinessReport", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetProjectView_Count");
            dB.addIn("@InstanceId", InstanceId);
            dt = dB.ExecuteDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    d.TotalTask = Convert.ToInt32(dr["TotalTask"].ToString());
                    d.Completed = Convert.ToInt32(dr["Completed"].ToString());
                    d.WIP = Convert.ToInt32(dr["WIP"].ToString());
                    d.ONHOLD = Convert.ToInt32(dr["ONHOLD"].ToString());
                    d.YetToStart = Convert.ToInt32(dr["YetToStart"].ToString());
                    dM.Add(d);
                }
            }
            return dM;

        }

        #endregion
        #endregion

        #region PdfMailAttachment
        public Boolean SendMailPdfAttachment(Guid UserID, String emailId, string msg, string filename, string Subject)
        {
            Boolean Status = false;
            LogHelper _log = new LogHelper();
            try
            {
                DBHelper Db1 = new DBHelper("SP_Mail", CommandType.StoredProcedure);
                Db1.addIn("@Type", "PdfAttachmentMail");
                Db1.addIn("@UserID", UserID);
                Db1.addIn("@Q_Mail", emailId);
                Db1.addIn("@Q_Name", msg);
                Db1.addIn("@Q_Subject", Subject);
                Db1.addIn("@FileName_ID", filename.ToString());
                Db1.addIn("@Cre_By", UserID);
                DataTable dt = new DataTable();
                dt = Db1.ExecuteDataTable();

                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "-->Pdf_MailAttachment" + ex.Message.ToString());
            }

            return Status;
        }

        #endregion

        #region ViewMessage
        public List<BellModel> Sp_ViewMessage(Guid UserId)
        {
            DataTable dt = new DataTable();

            List<BellModel> PM = new List<BellModel>();

            DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
            dB.addIn("@Type", "GetMessage");
            dB.addIn("@UserId", UserId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                //int count = 1;
                var myLocalDateTime = DateTime.UtcNow;
                foreach (DataRow dr in dt.Rows)
                {
                    BellModel P = new BellModel();

                    P.ConsUserName = dr["UserName"].ToString().ToUpper();
                    P.PmName = dr["ProjectManagerName"].ToString().ToUpper();
                    P.IssueName = dr["IssueName"].ToString().ToUpper();
                    P.CountMessgae = Convert.ToInt32(dt.Rows.Count.ToString());
                    //P.Cre_on = Convert.ToDateTime(dr["CreatedOn"]);
                    P.Createdon = dr["CreatedOn"].ToString();
                    P.Id = Guid.Parse(dr["Id"].ToString());
                    PM.Add(P);
                }
            }

            return PM;
        }
        public Boolean Sp_DeleteMessage(Guid id)
        {
            bool status = false;
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            try
            {
                DBHelper dB = new DBHelper("SP_IssueTrack", CommandType.StoredProcedure);
                dB.addIn("@Type", "DeleteMessage");
                dB.addIn("@UserId", id);
                dt = dB.ExecuteDataTable();

                status = true;


            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return status;

        }

        #endregion

        #region SAPConnection
        public Instances Sp_GetSAPConnectionData(string InstanceId)
        {
            Instances i = new Instances();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_Instance", CommandType.StoredProcedure);
            dB.addIn("@Type", "SAPConnectionData");
            dB.addIn("@InstanceID", InstanceId);
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    i.destinationName = dr["destinationName"].ToString();
                    i.AppServerHost = dr["AppServerHost"].ToString();
                    i.SystemNumber = dr["SystemNumber"].ToString();
                    i.SAPRouter = dr["SAPRouter"].ToString();
                    i.User = dr["SAPUser"].ToString();
                    i.Password = dr["SAPPassword"].ToString();
                    i.Client = dr["Client"].ToString();
                    i.Language = dr["Language"].ToString();
                    i.PoolSize = dr["PoolSize"].ToString();
                }
            }
            return i;
        }
        #endregion

        #region RiskAssessment
        public List<RA_Model> GetRAProbability()
        {
            List<RA_Model> Prob = new List<RA_Model>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "RA_ProbabilityData");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RA_Model Prb = new RA_Model();
                    Prb.Id = Convert.ToInt32(dr["id"].ToString());
                    Prb.Probability = dr["Probability"].ToString();
                    Prob.Add(Prb);
                }
            }
            return Prob;
        }

        public List<RA_Model> GetRARiskClass()
        {
            List<RA_Model> RiskClass = new List<RA_Model>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "RA_RiskClassData");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RA_Model Prb = new RA_Model();
                    Prb.Id = Convert.ToInt32(dr["id"].ToString());
                    Prb.RiskClass = dr["Risk Class"].ToString();
                    RiskClass.Add(Prb);
                }
            }
            return RiskClass;
        }

        public List<RA_Model> GetRARiskOwner()
        {
            List<RA_Model> RiskOwner = new List<RA_Model>();
            DataTable dt = new DataTable();
            DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
            dB.addIn("@Type", "RA_RiskOwnerData");
            dt = dB.ExecuteDataTable();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RA_Model Prb = new RA_Model();
                    Prb.Id = Convert.ToInt32(dr["id"].ToString());
                    Prb.RiskOwner = dr["Risk Owner"].ToString();
                    RiskOwner.Add(Prb);
                }
            }
            return RiskOwner;
        }

        public bool SubmitRiskAssessment(RiskAssessmentModel ra)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();

            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "SubmitRiskAssessment");
                //dB.addIn("@Id", ra.Id);
                dB.addIn("@RiskDescription", ra.RiskDescription);
                dB.addIn("@RiskManagement", ra.RiskManagement);
                dB.addIn("@Probability_Id", ra.Probability_Id);
                //dB.addIn("@Consequence", ra.Consequence);
                dB.addIn("@RiskClass_Id", ra.RiskClass_Id);
                dB.addIn("@RiskOwner_Id", ra.RiskOwner_Id);
                dB.addIn("@Area", ra.Area);
                dB.addIn("@Cre_By", ra.Cre_By);
                dB.addIn("@ProjectID", ra.Project_Id);

                dB.ExecuteScalar();
                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        } 
        
        public bool UpdateRiskAssessment(RiskAssessmentModel ra)
        {
            bool Status = false;
            LogHelper _log = new LogHelper();

            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "UpdateRiskAssessment");
                dB.addIn("@RiskDescription", ra.RiskDescription);
                dB.addIn("@RiskManagement", ra.RiskManagement);
                dB.addIn("@Probability_Id", ra.Probability_Id);
                dB.addIn("@RiskClass_Id", ra.RiskClass_Id);
                dB.addIn("@RiskOwner_Id", ra.RiskOwner_Id);
                dB.addIn("@Area", ra.Area);
                dB.addIn("@Cre_By", ra.Cre_By);
                dB.addIn("@ProjectID", ra.Project_Id);
                dB.addIn("@RiskAssessment_ID", ra.Id);

                dB.ExecuteScalar();
                Status = true;
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Status;
        }


        public List<RiskAssessmentModel> GetRiskAssessmentData(string ProjectId)
        {
            List<RiskAssessmentModel> Risk_Assessment = new List<RiskAssessmentModel>();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetRiskAssessmentData");
                dB.addIn("@ProjectID", ProjectId);
                dt=dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        RiskAssessmentModel ra = new RiskAssessmentModel();

                        ra.Id = Guid.Parse(dr["Id"].ToString());
                        ra.RiskId = Convert.ToInt32(dr["RiskId"].ToString());
                        ra.RiskId_ = 'R' + ra.RiskId.ToString();
                        ra.RiskDescription = dr["RiskDescription"].ToString();
                        ra.RiskManagement = dr["RiskManagement"].ToString();
                        //ra.Probability_Id = Convert.ToInt32(dr["Probability_Id"].ToString());
                        ra.Probability = dr["Probability"].ToString();
                        ra.Consequence = Convert.ToInt32(dr["Consequence"].ToString());
                        //ra.RiskClass_Id = Convert.ToInt32(dr["RiskClass_Id"].ToString());
                        ra.RiskClass = dr["RiskClass"].ToString();
                        //ra.RiskOwner_Id = Convert.ToInt32(dr["RiskOwner_Id"].ToString());
                        ra.RiskOwner = dr["RiskOwner"].ToString();
                        ra.Area = dr["Area"].ToString();
                        Risk_Assessment.Add(ra);
                    }
                }

            }
            catch(Exception ex)
            {
                _log.createLog(ex, "");
            }
            return Risk_Assessment;
        }

        public RiskAssessmentModel GetRiskAssessmentById(string id)
        {
            RiskAssessmentModel ra = new RiskAssessmentModel();
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetRiskAssessmentById");
                dB.addIn("@RiskAssessment_ID", id);
                dt = dB.ExecuteDataTable();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ra.Id = Guid.Parse(dr["Id"].ToString());
                        ra.RiskId = Convert.ToInt32(dr["RiskId"].ToString());
                        //ra.RiskId_ = 'R' + ra.RiskId.ToString();
                        ra.RiskDescription = dr["RiskDescription"].ToString();
                        ra.RiskManagement = dr["RiskManagement"].ToString();
                        ra.Probability = dr["Probability_Id"].ToString();
                        ra.Consequence = Convert.ToInt32(dr["Consequence"].ToString());
                        ra.RiskClass = dr["RiskClass_Id"].ToString();
                        ra.RiskOwner = dr["RiskOwner_Id"].ToString();
                        ra.Area = dr["Area"].ToString();                      
                    }
                }

            }
            catch (Exception ex)
            {
                _log.createLog(ex, "");
            }
            return ra;
        }

        public RiskAssessmentDiagram PMRiskAssessmentDiagram(string Project_Id)
        {
            LogHelper _log = new LogHelper();
            DataTable dt = new DataTable();
            RiskAssessmentDiagram ra = new RiskAssessmentDiagram();
            try
            {
                DBHelper dB = new DBHelper("SP_PMTask", CommandType.StoredProcedure);
                dB.addIn("@Type", "GetRiskAssessmentDiagram");
                dB.addIn("@ProjectID", Project_Id);
                dt = dB.ExecuteDataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ra.PC11 = dr["a1"].ToString();
                        ra.PC12 = dr["a2"].ToString();
                        ra.PC13 = dr["a3"].ToString();
                        ra.PC14 = dr["a4"].ToString();
                        ra.PC15 = dr["a5"].ToString();

                        ra.PC21 = dr["b1"].ToString();
                        ra.PC22 = dr["b2"].ToString();
                        ra.PC23 = dr["b3"].ToString();
                        ra.PC24 = dr["b4"].ToString();
                        ra.PC25 = dr["b5"].ToString();

                        ra.PC31 = dr["c1"].ToString();
                        ra.PC32 = dr["c2"].ToString();
                        ra.PC33 = dr["c3"].ToString();
                        ra.PC34 = dr["c4"].ToString();
                        ra.PC35 = dr["c5"].ToString();

                        ra.PC41 = dr["d1"].ToString();
                        ra.PC42 = dr["d2"].ToString();
                        ra.PC43 = dr["d3"].ToString();
                        ra.PC44 = dr["d4"].ToString();
                        ra.PC45 = dr["d5"].ToString();

                        ra.PC51 = dr["e1"].ToString();
                        ra.PC52 = dr["e2"].ToString();
                        ra.PC53 = dr["e3"].ToString();
                        ra.PC54 = dr["e4"].ToString();
                        ra.PC55 = dr["e5"].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                _log.createLog(ex, "");
            }
            return ra;
        }
        #endregion


        #region Check DB Status
        
        public bool IsServerConnected()
        {
             string constr = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
        var DBconnection = Cipher.Decrypt(constr,_salt);
            using (var l_oConnection = new SqlConnection(DBconnection))
            {
                try
                {
                    l_oConnection.Open();
                    return true;
                    l_oConnection.Close();
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        #endregion
    }
}

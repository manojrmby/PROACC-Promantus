using PROACC2.BL;
using PROACC2.BL.General;
using PROACC2.BL.Model;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PROACC2.BL.Model.Common;

namespace PROACC2.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant,customer,Project Manager")]
    public class HomeController : Controller
    {
        Base _Base = new Base();
        LogHelper _Log = new LogHelper();
        public ActionResult Home()
        {
            string str_loginid = Session["loginid"].ToString();
            Guid loginid= Guid.Parse(str_loginid);
            Dashboard DashTop = _Base.Sp_GetDashboard(loginid);
            ViewBag.DashTop = DashTop;
            List<Dasboard_Donut> Donut_Dash = _Base.Sp_GetDashboard_Donut(loginid);
            ViewBag.Donut_Dash = Donut_Dash;
            return View();
        }
        #region ReadinessCheck
        [Authorize(Roles = "Consultant,Customer,Project Manager")]
        public ActionResult ReadinessCheck()
        {
            var LoginId = Guid.Parse(Session["loginid"].ToString());
            var UserType = Session["UserType"].ToString();
            int Type = 0;
            //if (UserType == "Admin")
            //{
            //    Type = 1;
            //}
            if (UserType == "Consultant")
            {
                Type = 2;
            }
            else if (UserType == "Customer")
            {
                Type = 3;
            }
            else if (UserType == "Project Manager")
            {
                Type = 4;
            }
            GeneralList sP_ = _Base.spCustomerDropdown(Session["loginid"].ToString(), Type);
            ViewBag.CustomerName = sP_._List;
           // var result2 = _Base.Sp_GetProjectList();
            List<Instances> result2 = _Base.Sp_GetProjectListByUser(LoginId, Type);
            ViewBag.ProjectName = result2.ToList();
            List<PhaseModel> Phaselist = _Base.Sp_GetPhaselist();
            ViewBag.Phaselist = Phaselist.ToList();
            return View();
        }
        [Authorize(Roles = "Consultant,Customer,Project Manager")]
        public ActionResult _Readiness(string name)
        {
            return PartialView(name);
        }
        #endregion

        #region Chart
        public ActionResult GetSimplificationReport(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            SP_ReadinessReport_Result GetRelevant = _Base.sAPInput(InstanceId);
            return Json(GetRelevant, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomCode_Bar(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_GetCustomCode_Bar(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActivities_Bar1(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_GetActivities_Bar1(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetActivities_Donut(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_GetActivities_Donut(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetActivities_Bar2(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_GetActivities_Bar2(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFiori_Bar(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_GetFiori_Bar(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SimplificationReport(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_SimplificationReport(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSimplificationReport_Bar(string Instance,string LOB)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_SimplificationReport_Bar(LOB, InstanceId);

            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SimplicationTable(string Instance,string LOB)
        {
            Guid InstanceId = Guid.Parse(Instance);
            
            List<SimplificationReport> SR = _Base.SAPInput_Simplification(InstanceId, LOB);

            return Json(SR, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivitiesReport_Dropdown(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            Tuple<List<Lis>, List<Lis>> sP_ = _Base.sp_GetActivitiesReportDropdown(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActivitiesTable(string Instance,string Phase, string condition)
        {
            Guid InstanceId = Guid.Parse(Instance);
            List<Activities_Report> AC = _Base.GetActivitiesReport_Table(Phase, condition, InstanceId);

            return Json(AC, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivitiesReport_Bar(string Instance, string Phase, string condition)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_GetActivitiesReport_Bar(Phase, condition, InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CustomCodeTable(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            List<SAPInput_CustomCode> CC = _Base.SAPInput_CustomCodeReport(InstanceId);

            return Json(CC, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HanaDataBaseTables(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            List<SAPInput_HanaDatabase> CC = _Base.SAPInput_S4hanaDatabaseReport(InstanceId);

            return Json(CC, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFiori_Dropdown(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sp_GetFioriAppsReportDropdown(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FioriAppsTable(string Role, string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            List<SAPFioriAppsModel> AC = _Base.sp_GetSAPFioriAppsTable(Role, InstanceId);
            return Json(AC, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFioriAppsReportt_Bar(string Role, string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            GeneralList sP_ = _Base.sP_GetSAPFioriAppsReport_Bar(Role, InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHanaCountReport(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            SP_ReadinessReport_Result GetRelevant = _Base.HanaCountInput(InstanceId);
            return Json(GetRelevant, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHanaCountTable(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            List<EccHanaCountModel> GetRelevant = _Base.HanaCountTable(InstanceId);
            return Json(GetRelevant, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBWExtractorsTable(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            List<BwExtractorsModel> GetRelevant = _Base.GETBwextractorsData(InstanceId);
            return Json(GetRelevant, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Table_SAPDirect

        public ActionResult GetInstance_LandScape(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);

            var SR = _Base.Sp_GetSysLandScape(InstanceId);

            return Json(SR.Sys_Landscape, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSAPData_Table(string Instance, int val, string sys_LandScape)
        {
            Guid InstanceId = Guid.Parse(Instance);

            List<RFCFMReport> SR = _Base.SAPInput_RFCFMReport(InstanceId,val, sys_LandScape);

            return Json(SR, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult summery_sys_report(string Instance,string sys_LandScape)
        {
            Guid InstanceId = Guid.Parse(Instance);

            RFCFMReport SR = _Base.SAPInput_summery_sys_report(InstanceId, sys_LandScape);

            return Json(SR, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BillingDocuments(string Ins, string sys_LandScape)
        {
            Guid InstanceId = Guid.Parse(Ins);

            var BD = _Base.Sp_GetBuildingDocuments(InstanceId, sys_LandScape);

            return Json(BD, JsonRequestBehavior.AllowGet);
        } 
        public ActionResult OrderDocuments(string Ins, string sys_LandScape)
        {
            Guid InstanceId = Guid.Parse(Ins);

            var OD = _Base.Sp_GetOrderDocuments(InstanceId, sys_LandScape);

            return Json(OD, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ComplexityAnalysis(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);

            var CA = _Base.Sp_GetComplexityAnalysis(InstanceId);

            return Json(CA, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SalesDocuments(string Ins,string sys_LandScape)
        {
            Guid InstanceId = Guid.Parse(Ins);

            var CA = _Base.Sp_GetSalesDocuments(InstanceId, sys_LandScape);

            return Json(CA, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaterialityScoreDocuments(string Ins)
        {
            Guid InstanceId = Guid.Parse(Ins);

            var MS = _Base.Sp_GetMaterialityScoreDocuments(InstanceId);

            return Json(MS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBreakdownbyLob_Bar(string Ins, string sys_LandScape)
        {
            Guid InstanceId = Guid.Parse(Ins);
            GeneralList sP_ = _Base.sP_GetBreakdownbyLob_Bar(InstanceId, sys_LandScape);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        
        [Authorize(Roles = "Consultant,Customer,Project Manager")]
        public ActionResult GetSAPTables(Guid Ins)
        {
            ECCDestinationConfig cfg = new ECCDestinationConfig();
            try
            {
                DataTable SALESDATA = new DataTable();
                DataTable BILLSDATA = new DataTable();
                DataTable ORDERSDATA = new DataTable();
                DataTable FACOUNTDATA = new DataTable();

                DataTable SAPDATA1 = new DataTable();
                DataTable SAPDATA2 = new DataTable();
                DataTable SAPDATA3 = new DataTable();
                DataTable SAPDATA4 = new DataTable();

                if (true)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                    RfcDestination dest = RfcDestinationManager.GetDestination("ABAP_AS_WITH_POOL " + Ins);
                     
                    RfcRepository repo = dest.Repository;

                    IRfcFunction testfn = repo.CreateFunction("ZPROACC_ENT_STRUC_MINING_OTC");
                    testfn.Invoke(dest);
                    var companyCodeList = testfn.GetTable("I_SALES");
                    SALESDATA = companyCodeList.ToDataTable("ZPRO_SALES");

                    var companyCodeList1 = testfn.GetTable("I_BILLS");
                    BILLSDATA = companyCodeList1.ToDataTable("ZPRO_BILLS");

                    var companyCodeList2 = testfn.GetTable("I_ORDERS");
                    ORDERSDATA = companyCodeList2.ToDataTable("ZPRO_ORDERS");

                    IRfcFunction testfn1 = repo.CreateFunction("ZPROACC_MAT_SCORE");
                    testfn1.Invoke(dest);
                    var companyCodeList3 = testfn1.GetTable("I_FACOUNT");
                    FACOUNTDATA = companyCodeList3.ToDataTable("ZPRO_FACOUNT");

                    IRfcFunction testfn2 = repo.CreateFunction("ZPROACC_SYSTEM_CONFIG_READ_V1");
                    testfn2.SetValue("OPT", 1);
                    testfn2.Invoke(dest);
                    var companyCodeList4 = testfn2.GetTable("IT_RESULT");
                    SAPDATA1 = companyCodeList4.ToDataTable("ZPRO_STRUC1");
                    SAPDATA1.Columns.Remove("FIELD3");
                    SAPDATA1.Columns.Remove("FIELD4");
                    SAPDATA1.Columns.Remove("FIELD5");
                    SAPDATA1.Columns.Remove("FIELD6");
                    for (int i = SAPDATA1.Rows.Count - 1; i >= 0; i--)
                    {
                        if (SAPDATA1.Rows[i][1].ToString() == "")
                        {
                            SAPDATA1.Rows[i].Delete();
                        }
                    }
                    SAPDATA1.Rows[0].Delete();
                    SAPDATA1.AcceptChanges();


                    testfn2.SetValue("OPT", 2);
                    testfn2.Invoke(dest);
                    var companyCodeList5 = testfn2.GetTable("IT_RESULT");
                    SAPDATA2 = companyCodeList5.ToDataTable("ZPRO_STRUC2");
                    SAPDATA2.Columns.Remove("FIELD6");
                    for (int i = SAPDATA2.Rows.Count - 1; i >= 0; i--)
                    {
                        if (SAPDATA2.Rows[i][1].ToString() == "")
                        {
                            SAPDATA2.Rows[i].Delete();
                        }
                    }
                    SAPDATA2.Rows[0].Delete();
                    SAPDATA2.AcceptChanges();

                    testfn2.SetValue("OPT", 3);
                    testfn2.Invoke(dest);
                    var companyCodeList6 = testfn2.GetTable("IT_RESULT");
                    SAPDATA3 = companyCodeList6.ToDataTable("ZPRO_STRUC3");
                    SAPDATA3.Columns.Remove("FIELD6");
                    for (int i = SAPDATA3.Rows.Count - 1; i >= 0; i--)
                    {
                        if (SAPDATA3.Rows[i][1].ToString() == "")
                        {
                            SAPDATA3.Rows[i].Delete();
                        }
                    }
                    SAPDATA3.Rows[0].Delete();
                    SAPDATA3.AcceptChanges();

                    testfn2.SetValue("OPT", 4);
                    testfn2.Invoke(dest);
                    var companyCodeList7 = testfn2.GetTable("IT_RESULT");
                    SAPDATA4 = companyCodeList7.ToDataTable("ZPRO_STRUC4");
                    for (int i = SAPDATA4.Rows.Count - 1; i >= 0; i--)
                    {
                        if (SAPDATA4.Rows[i][1].ToString() == "")
                        {
                            SAPDATA4.Rows[i].Delete();
                        }
                    }
                    SAPDATA4.Rows[0].Delete();
                    SAPDATA4.AcceptChanges();
                }
                var Cre_By = Session["loginid"].ToString();
                var result = _Base.Sp_StoreSAPTables(Ins,SALESDATA, BILLSDATA, ORDERSDATA, FACOUNTDATA, SAPDATA1, SAPDATA2, SAPDATA3, SAPDATA4, Cre_By);
                if (result == true)
                {
                    return Json("success");
                    //return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("error");
                    //return Json("error", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
            }
        }
        #endregion



        #region PROJECT CREATION

        public ActionResult _CreateProject()
        {
            List<UserModel> result5 = _Base.SP_GetProjectManager();
            ViewBag.ProjectManager = result5.ToList();

            List<Scenario> result6 = _Base.Sp_GetScenario();
            ViewBag.Scenario = result6.ToList();

            List<CustomerModel> result7 = _Base.Sp_GetCompany();
            ViewBag.Company = result7.ToList();
            return PartialView("_CreateProject");
        }

        
        public ActionResult GetCompanyName()
        {
            var Projresult = _Base.Sp_GetCompany(); ;
            var data = Projresult.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckProjectAvailabilty(string ProjectName, Guid? Id)
        {
            var result = _Base.SP_GetProjName(ProjectName, Id);
            bool status = false;
            try
            {
                if (result != true)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }



        #endregion

        [Authorize(Roles = "Consultant,Customer,Project Manager")]
        public ActionResult _Test()
        {
            return PartialView("_Test");
        }


       

        public ActionResult ProjectHome()
        {
            return View();
        }

        #region FileUpload
        [HttpPost]
        public ActionResult UploadPhoto()
        {
            string status = null;
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        Base _base = new Base();
                        string fname, ext;
                        fname = file.FileName;
                        ext = Path.GetExtension(fname);
                        ext = ext.Replace(ext, ".jpg");

                        string NewId = Guid.NewGuid().ToString();
                        fname = NewId + ext;
                        string filePath = Server.MapPath(ConfigurationManager.AppSettings["Upload"].ToString());
                        _base.CreateIfMissing(filePath);

                        string fpath = Path.Combine(filePath, fname);
                        file.SaveAs(fpath);

                        string ftype = file.ContentType;

                        ////string[] filePaths = Directory.GetFiles(Server.MapPath(ConfigurationManager.AppSettings["Upload"]));

                        int size = file.ContentLength;
                        if (size > 0)
                        {


                            Guid Id = Guid.NewGuid();
                            string FileType = "Profile";
                            bool valid = true;
                            Guid CreatedBy = Guid.Parse(Session["loginid"].ToString());

                            bool upload = _Base.SP_Uploadfile(Id, NewId, FileType, valid, CreatedBy);
                            if (upload == true)
                            {
                                status = "success";
                            }
                            else
                            {
                                status = "error";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json("Error" + e.Message);
            }
            return Json(status);
        }


        public ActionResult Data(string col)
        {
            Guid Id = Guid.Parse(Session["loginid"].ToString());
            List<FileUpload_Model> data = _Base.SP_Getfullpath(Id);
            var data2 = data.ToList();
            string url = "false";

            List<String> filePaths = Directory.GetFiles(Server.MapPath(ConfigurationManager.AppSettings["Upload"])).OrderByDescending(x => new System.IO.FileInfo(x).CreationTimeUtc).ToList();
            Session["RamdomColor"] = col;
            if (data2.Count > 0 && filePaths!=null)
            {
                foreach (var item in filePaths)
                {
                    var DATA = Path.GetFileNameWithoutExtension(item);
                    DateTime GetCreationTime = System.IO.File.GetCreationTime(item);


                    var data3 = data2.FirstOrDefault();

                    if (data3.FileName.ToString() == DATA)
                    {
                        var _path = ConfigurationManager.AppSettings["Upload"].ToString();
                        url = Path.GetFileName(item);
                        url = _path + url;
                        string fullPath = Server.MapPath(url);
                        if (!System.IO.File.Exists(fullPath))
                        {
                            url = "";
                        }

                        //var newName = Path.GetFileName(item);
                        //var ext = Path.GetExtension(newName);
                        //var extName = Path.GetFileNameWithoutExtension(newName);
                        //Random rand = new Random();
                        //extName = "Pic" + rand.Next(0, 100) + ext;

                        //var newPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["CopyImage"]), newName);
                        //System.IO.File.Copy(item, newPath, true);

                        //byte[] imageByteData = System.IO.File.ReadAllBytes(item);
                        //string imageBase64Data = Convert.ToBase64String(imageByteData);
                        //string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                        //Session["UserInfo"] = imageDataURL;
                        //ViewData["URL"] = Session["UserInfo"];
                        //url = ViewData["URL"].ToString();
                    }
                }
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }

        public class UploadImage
        {
            public static void Crop(int Width, int Height, Stream streamImg, string saveFilePath)
            {
                Bitmap sourceImage = new Bitmap(streamImg);
                using (Bitmap objBitmap = new Bitmap(Width, Height))
                {
                    objBitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
                    using (Graphics objGraphics = Graphics.FromImage(objBitmap))
                    {
                        // Set the graphic format for better result cropping   
                        objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        objGraphics.DrawImage(sourceImage, 0, 0, Width, Height);

                        // Save the file path, note we use png format to support png file   
                        objBitmap.Save(saveFilePath);
                    }
                }
            }
        }
        #endregion

        #region _DashboardViewallCustomerConsultant
        public ActionResult __ViewAllCustomer(String id)
        {

            //String st = string.Join(",", id.Split(',').Select(x => string.Format("'{0}'", x.Trim())).ToList());
            String[] st2 = id.Split(',');
            List<CustomerModel> DashTop = new List<CustomerModel>();
            CustomerModel DashCust = new CustomerModel();
            foreach (var item in st2)
            {
                string loginId = item.TrimStart();
                DashCust = _Base.Sp_GetCustomerDashboard(loginId);
                DashTop.Add(DashCust);
            }
            ViewBag.DashTop = DashTop;
            return PartialView("__ViewAllCustomer");
        }

        public ActionResult __ViewAllConsultant(String id)
        {

            //String st = string.Join(",", id.Split(',').Select(x => string.Format("'{0}'", x.Trim())).ToList());
            String[] st3 = id.Split(',');
            List<UserModel> DashTop2 = new List<UserModel>();
            UserModel DashUser = new UserModel();
            foreach (var item2 in st3)
            {
                string loginId2 = item2.TrimStart();
                DashUser = _Base.Sp_GetConsultantDashboard(loginId2);
                DashTop2.Add(DashUser);
            }
            ViewBag.DashTop2 = DashTop2;
            return PartialView("__ViewAllConsultant");
        }
        #endregion

        #region BellViewMessage
        public ActionResult __ViewMessage()
        {
            Guid UseId = Guid.Parse(Session["loginid"].ToString());
            ViewBag.ViewMessage  = _Base.Sp_ViewMessage(UseId);
            var _viewMessage = ViewBag.ViewMessage;
            return Json(_viewMessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMessage(Guid id)
        {
            bool status = false;
            var _status = _Base.Sp_DeleteMessage(id);
            if(_status==true)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return Json(status);
        }

        #endregion

    }
}
using PROACC2.BL;
using PROACC2.BL.General;
using PROACC2.BL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PROACC2.BL.Model.Common;

namespace PROACC2.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant,customer,Project Manager")]
    public class ConsultantController : Controller
    {
        // GET: Consultant

        Base _base = new Base();
        LogHelper _logHelper = new LogHelper();
        public ActionResult Analysis(string name)
        {
            var LoginId = Guid.Parse(Session["loginid"].ToString());
            var UserType = Session["UserType"].ToString();
            int Type = 0;
            if (UserType == "Project Manager")
            {
                Type = 1;
            }
            else if (UserType == "Consultant")
            {
                Type = 2;
            }
            else if (UserType == "Customer")
            {
                Type = 3;
            }
            GeneralList sP_ = _base.spCustomerDropdown(Session["loginid"].ToString(), Type);
            ViewBag.CustomerName = sP_._List; //new SelectList(sP_._List, "Value", "Name");

            List<Instances> result2 = _base.Sp_GetProjectListByUser(LoginId, Type);
            ViewBag.ProjectName = result2;

            List<PhaseModel> Phaselist = _base.Sp_GetPhaselist();
            ViewBag.Phaselist = Phaselist.ToList();

            return PartialView(name);
        }

        //public ActionResult Index(string name)
        //{
        //    return PartialView(name);
        //}
        public PartialViewResult _CreateAnalysis()
        {
            return PartialView("_CreateAnalysis");
        }
        public ActionResult _SAPFileUpload()
        {
            return PartialView("_SAPFileUpload");
        }

        [HttpPost]
        public ActionResult uploadFiles()
        {
            HttpFileCollectionBase files = Request.Files;
            string ProjectID = Request.Params["ProjectID"].ToString();
            string InstanceID = Request.Params["InstanceID"].ToString();
            //string InstanceName = Request.QueryString["InstanceID"].ToString();
            bool Result = false;
            string fname = "", ext, GivenName = "";

            if (files.Count > 0)
            {
                HttpPostedFileBase file = files[0];
                fname = file.FileName;
                GivenName = Path.GetFileNameWithoutExtension(fname);
                if (ProjectID != "" && InstanceID != "")
                {
                    Guid Instance_ID = Guid.Parse(InstanceID);
                    Guid User_Id = Guid.Parse(Session["loginid"].ToString());
                    try
                    {
                        string NewID = Guid.NewGuid().ToString();
                        //Checking for Internet Explorer
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            ext = System.IO.Path.GetExtension(fname);
                        }
                        else
                        {
                            fname = file.FileName;
                            ext = System.IO.Path.GetExtension(fname);
                        }
                        String _fname = NewID + ext;
                        FileUpload _fileUpload = new FileUpload();
                        string Folder_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
                        _base.CreateIfMissing(Folder_Path);
                        fname = Path.Combine(Folder_Path, _fname);
                        file.SaveAs(fname);

                        if (GivenName == "Activities")
                        {
                            Result = _fileUpload.Process_Activities(fname, NewID, Instance_ID, User_Id);

                        }
                        else if (GivenName == "Bwextractors")
                        {
                            Result = _fileUpload.Process_Bwextractors(fname,NewID, Instance_ID, User_Id);
                            //Result = _base.Upload_Bwextractors(NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "CustomCode")
                        {
                            Result = _fileUpload.Process_CustomCode(fname, NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "HanaDatabaseTables")
                        {
                            Result = _fileUpload.Process_HanaDatabaseTables(fname,NewID, Instance_ID, User_Id);
                            //Result = _base.Upload_HanaDatabaseTables(NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "RecommendedFioriApps")
                        {
                            Result = _fileUpload.Process_FioriApps(fname, NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "RelevantSimplificationItems")
                        {
                            Result = _fileUpload.Process_Simplification(fname, NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "SAP Readiness Check")
                        {
                            Result = _base.Upload_SAPReadinessCheck(NewID, Instance_ID, User_Id);
                            Session["IsCreateAnalysisDone"] = true;
                        }
                        else if (GivenName == "UserList")
                        {
                            Result = _fileUpload.Process_SAPUserList(fname, NewID, Instance_ID, User_Id);
                        }
                        _base.Confirm_AnalysisUpload(Instance_ID, User_Id);
                    }
                    catch (Exception ex)
                    {

                        _base.GetUploadRevert(Instance_ID, User_Id);
                        _logHelper.createLog("File Upload Failed..." + ex.ToString());
                        //throw;
                    }
                }
            }
            var MyReturn = new
            {
                Status = Result,
                FileName = GivenName
            };
            //for (int i = 0; i < files.Count; i++)
            //{
            //HttpPostedFileBase file = files[0];
            // fname = file.FileName;
            //    if (files.Count==1)
            //    {
            //        try
            //        {
            //            bool Result_Process_Activities = false, Result_Process_Bwextractors = false,
            //                Result_Process_CustomCode = false, Result_Processup_HanaDatabaseTables = false,
            //                Result_Process_FioriApps = false, Result_Process_Simplification = false,
            //                Result_Process_SAPReadinessCheck = false;
            //            Guid Instance_ID = Guid.Parse(InstanceName);
            //            Guid User_Id = Guid.Parse(Session["loginid"].ToString());
            //            //  Get all files from Request object  
            //            //var DefaultList = ['Activities', 'Bwextractors', 'CustomCode', 'HanaDatabaseTables', 'RecommendedFioriApps', 'RelevantSimplificationItems','SAP Readiness Check'];


            //                //string fname;
            //                //string ext;
            //                string NewID = Guid.NewGuid().ToString();
            //                //Checking for Internet Explorer
            //                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            //                {
            //                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
            //                    fname = testfiles[testfiles.Length - 1];
            //                    ext = System.IO.Path.GetExtension(fname);
            //                }
            //                else
            //                {
            //                    fname = file.FileName;
            //                    ext = System.IO.Path.GetExtension(fname);
            //                }
            //                String _fname = NewID + ext;
            //                FileUpload _fileUpload = new FileUpload();
            //                string Folder_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
            //                _Base.CreateIfMissing(Folder_Path);
            //                fname = Path.Combine(Folder_Path, _fname);
            //                file.SaveAs(fname);
            //                if (fname == "Activities")
            //                {
            //                    Result_Process_Activities = _fileUpload.Process_Activities(fname, NewID, Instance_ID, User_Id);
            //                }
            //                else if (fname == "Bwextractors")
            //                {
            //                    Result_Process_Bwextractors = _Base.Upload_Bwextractors(NewID, Instance_ID, User_Id);
            //                }
            //                else if (fname == "CustomCode")
            //                {
            //                    Result_Process_CustomCode = _fileUpload.Process_CustomCode(fname, NewID, Instance_ID, User_Id);
            //                }
            //                else if (fname == "HanaDatabaseTables")
            //                {
            //                    Result_Processup_HanaDatabaseTables = _Base.Upload_HanaDatabaseTables(NewID, Instance_ID, User_Id);
            //                }
            //                else if (fname == "RecommendedFioriApps")
            //                {
            //                    Result_Process_FioriApps = _fileUpload.Process_FioriApps(fname, NewID, Instance_ID, User_Id);
            //                }
            //                else if (fname == "RelevantSimplificationItems")
            //                {
            //                    Result_Process_Simplification = _fileUpload.Process_Simplification(fname, NewID, Instance_ID, User_Id);
            //                }
            //                else if (fname == "SAP Readiness Check")
            //                {
            //                    Result_Process_SAPReadinessCheck = _Base.Upload_SAPReadinessCheck(NewID, Instance_ID, User_Id);
            //                }


            //            //if (Result_Process_Bwextractors & Result_Process_Bwextractors &
            //            //    Result_Process_CustomCode & Result_Processup_HanaDatabaseTables &
            //            //    Result_Process_FioriApps & Result_Process_Simplification &
            //            //    Result_Process_SAPReadinessCheck)
            //            //{

            //            //    //Result_Instance = _Base.AddInstance(IDProject, InstanceName, Instance_ID);

            //            //    return Json("File Uploaded Successfully!");
            //            //}
            //            Session["IsCreateAnalysisDone"] = true;
            //            return Json("File Uploaded Successfully!");
            //            // Returns message that successfully uploaded  

            //        }
            //        catch (Exception ex)
            //        {
            //            _logHelper.createLog(ex);
            //            String msg = ex.Message;
            //            if (msg.Contains("Activities"))
            //            {

            //            }
            //            else if (msg.Contains("Activities"))
            //            {

            //            }
            //            else if (msg.Contains("Activities"))
            //            {

            //            }
            //            else if (msg.Contains("Activities"))
            //            {

            //            }
            //            else if (msg.Contains("Activities"))
            //            {

            //            }
            //            else if (msg.Contains("Activities"))
            //            {

            //            }
            //            else if (msg.Contains("Activities"))
            //            {

            //            }
            //            return Json("Error occurred. Error details: " + ex.Message);
            //        }
            //    }

            //}
            return Json(MyReturn);
        }

        [HttpPost]
        public ActionResult uploadSAPFiles()
        {
            HttpFileCollectionBase files = Request.Files;
            string ProjectID = Request.Params["ProjectID"].ToString();
            string InstanceID = Request.Params["InstanceID"].ToString();
            //string InstanceName = Request.QueryString["InstanceID"].ToString();
            bool Result = false;
            string fname = "", ext, GivenName = "";

            if (files.Count > 0)
            {
                HttpPostedFileBase file = files[0];
                fname = file.FileName;
                GivenName = Path.GetFileNameWithoutExtension(fname);
                if (ProjectID != "" && InstanceID != "")
                {
                    Guid Instance_ID = Guid.Parse(InstanceID);
                    Guid User_Id = Guid.Parse(Session["loginid"].ToString());
                    try
                    {
                        string NewID = Guid.NewGuid().ToString();
                        //Checking for Internet Explorer
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            ext = System.IO.Path.GetExtension(fname);
                        }
                        else
                        {
                            fname = file.FileName;
                            ext = System.IO.Path.GetExtension(fname);
                        }
                        String _fname = NewID + ext;
                        FileUpload _fileUpload = new FileUpload();
                        string Folder_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
                        _base.CreateIfMissing(Folder_Path);
                        fname = Path.Combine(Folder_Path, _fname);
                        file.SaveAs(fname);

                        if (GivenName == "RFC_FM_Output")
                        {
                            Result = _fileUpload.Process_RFC_FM_Output(fname, NewID, Instance_ID, User_Id);
                            Session["IsCreateAnalysisDone"] = true;
                        }
                        else if (GivenName == "BillingDocuments")
                        {
                            Result = _fileUpload.Process_BillingDocuments(fname, NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "OrderDocuments")
                        {
                            Result = _fileUpload.Process_OrderDocuments(fname, NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "SalesDocuments")
                        {
                            Result = _fileUpload.Process_SalesDocuments(fname, NewID, Instance_ID, User_Id); 
                        }
                        else if (GivenName == "ComplexityAnalysis")
                        {
                            Result = _fileUpload.Process_ComplexityAnalysis(fname, NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "MaterialityScore")
                        {
                            Result = _fileUpload.Process_MaterialityScore(fname, NewID, Instance_ID, User_Id);
                        }
                        else if (GivenName == "PurchaseDocuments")
                        {
                            //Result = _fileUpload.Process_PurchaseDocuments(fname, NewID, Instance_ID, User_Id);
                        }
                        _base.Confirm_SAPFileUpload(Instance_ID, User_Id);
                    }
                    catch (Exception ex)
                    {
                        _logHelper.createLog("File Upload Failed..." + ex.ToString());
                        //throw;
                    }
                }
            }
            var MyReturn = new
            {
                Status = Result,
                FileName = GivenName
            };
            return Json(MyReturn);
        }

        //[HttpPost]

        public JsonResult LoadCreateAnalysisInstance(string ProjectId, string pagename)
        {
            GeneralList sP_ = _base.GetInstanceDropdown(ProjectId, pagename);
            List<Lis> Result = new List<Lis>();

            for (int i = 0; i < sP_._List.Count; i++)
            {
                var A = Guid.Parse(sP_._List[i].Value);
                Boolean S = _base.SP_GetInstanceStatus(A);
                if (S)
                {

                    Lis List = new Lis();
                    List.Name = sP_._List[i].Name.ToString();
                    List.Value = sP_._List[i].Value.ToString();

                    Result.Add(List);
                }
            }
            SelectList items = new SelectList(Result, "Value", "Name");
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadAnalysisCompletedInstance(string ProjectId, string pagename, string Instance_Id)
        {
            GeneralList sP_ = _base.GetInstanceDropdown(ProjectId, pagename);
            List<Lis> Result = new List<Lis>();

            for (int i = 0; i < sP_._List.Count; i++)
            {
                var A = Guid.Parse(sP_._List[i].Value);
                Boolean S = _base.SP_GetInstanceStatus(A);
                if (S)
                {

                    Lis List = new Lis();
                    List.Name = sP_._List[i].Name.ToString();
                    List.Value = sP_._List[i].Value.ToString();

                    Result.Add(List);
                }
            }
            SelectList items = new SelectList(Result, "Value", "Name");
            var status = false;
            foreach (var item in items)
            {
                if (item.Value == Instance_Id)
                {
                    status = true;
                }
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SAPFileUploadCompletedInstance(string Instance_Id)
        {
            Guid instance = Guid.Parse(Instance_Id);
            Instances instances = new Instances();
            var SAPUploadList = _base.SAPUploadList(instance);

            return Json(SAPUploadList, JsonRequestBehavior.AllowGet);
        }

        //private Boolean GetInstanceStatus(Guid InstanceId)
        //{
        //    Boolean Status = true;
        //    //var q = from u in db.Instances where (u.Instance_id == InstanceID && u.AssessmentUploadStatus == true) orderby u.InstaceName select u;

        //    var Query = from u in db.ProjectMonitors where (u.InstanceID == InstanceId && u.PhaseId == 1 && u.StatusId != 1 && u.StatusId != 3) select u;
        //    var take = db.ProjectMonitors.Where(x => x.InstanceID == InstanceId).Take(1);

        //    if (Query.Count() > 0)
        //    {
        //        Status = false;
        //    }

        //    if (take.Count() == 0)
        //    {
        //        Status = false;
        //    }

        //    return Status;

        //}
    }
}
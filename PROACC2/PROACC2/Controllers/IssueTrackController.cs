using OfficeOpenXml;
using PROACC2.BL;
using PROACC2.BL.General;
using PROACC2.BL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static PROACC2.BL.Model.Common;

namespace PROACC2.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Consultant,Customer,Project Manager")]
    public class IssueTrackController : Controller
    {
        Base _base = new Base();
        LogHelper _Log = new LogHelper();
        // GET: IssueTrack
        public ActionResult Index(string name)
        {
            var LoginId = Guid.Parse(Session["loginid"].ToString());
            var UserType = Session["UserType"].ToString();
            int Type = 0;
            if (UserType == "Admin")
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
            else if (UserType == "Project Manager")
            {
                Type = 4;
            }
            GeneralList sP_ = _base.spCustomerDropdown(Session["loginid"].ToString(), Type);
            ViewBag.CustomerName = sP_._List;

            List<Instances> result2 = _base.Sp_GetProjectListByUser(LoginId, Type);
            ViewBag.ProjectName = result2;

            List<PhaseModel> Phaselist = _base.Sp_GetPhaselist();
            ViewBag.Phaselist = Phaselist;

            List<ProjectMonitorModel> StatusMaster = _base.Sp_GetStatus();
            ViewBag.StatusMaster = StatusMaster;

            List<SAPIssueTrackModel> SAPStatusMaster = _base.Sp_GetSAPStatus();
            ViewBag.SAPStatus = SAPStatusMaster;

            //IssueTrackModel model = new IssueTrackModel();
            //List<IssueTrackModel> ITM = _base.Sp_GetIssueTrackData(Session["loginid"].ToString(), Type, model);
            //ViewBag.IssueTrack_Count = ITM.Count();

            return PartialView(name);
        }

        public ActionResult _IssueTrackIndex()
        {
            return PartialView("_IssueTrackIndex");
        }

        public JsonResult AssignedTo(Guid Pid)
        {
            var login = Session["loginid"].ToString();
            List<UserModel> B = _base.Sp_AssignedTo(Pid, login);
            return Json(B, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditAssignedTo(Guid Iid, Guid id)
        {
            List<UserModel> B = _base.Sp_EditAssignedTo(Iid, id);
            return Json(B, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTask(int PhaseID, Guid Instance_Id)
        {
            List<ActivityMasterModel> IssueTrackTask = _base.Sp_GetIssueTrackTask(PhaseID, Instance_Id);
            return Json(IssueTrackTask, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult Create(IssueTrackModel ism)
        {
            bool Result = false;

            ism.Cre_By = Guid.Parse(Session["loginid"].ToString());
           
            Result = _base.Sp_InsertIssue(ism);
            if (Result == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _GetIssueTrackData(IssueTrackModel model)
        {
            int userType = 0;
            List<IssueTrackModel> ITM = new List<IssueTrackModel>();
            if (User.IsInRole("Consultant"))
            {
                userType = 2;
            }
            else if (User.IsInRole("Customer"))
            {
                userType = 3;
            }
            else if (User.IsInRole("Project Manager"))
            {
                userType = 4;
            }

            ITM = _base.Sp_GetIssueTrackData(Session["loginid"].ToString(), userType, model);
            var Result = new { _ITM= ITM, _IssueTrack_Count= ITM.Count() };

           // ViewBag.IssueTrack_Count = ITM.Count();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetIssueTrackById(Guid id)
        {
            try
            {
                IssueTrackModel ITM = _base.Sp_EditIssueTrackData(id);

                //List<UserModel> B = _base.Sp_EditAssignedTo(ITM.ProjectInstance_Id, id);
                //ViewBag.AssignedTo = B;

                var loginid = Guid.Parse(Session["loginid"].ToString());
                //var a = (from i in db.Issuetracks
                //         join p in db.Projects on i.Cre_By equals p.ProjectManager_Id
                //         where i.Cre_By == loginid
                //         select i).FirstOrDefault();
                //ViewBag.status = false;

                var a = _base.Sp_AssigneeCount(loginid);
                if (ITM.Cre_By.ToString() == Session["loginid"].ToString())
                {
                    if (a == false)
                    {
                        ITM.AssigneeCount = true;
                    }
                }

                return Json(ITM, JsonRequestBehavior.AllowGet);
                //return PartialView("_GetIssueTrackById");
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->GetIssueTrackById" + Url);
                throw;
            }
        }

        public ActionResult GetStatusMaster()
        {
            List<ProjectMonitorModel> StatusMaster = _base.Sp_GetStatus();
            //ViewBag.StatusMaster = StatusMaster;
            return Json(StatusMaster, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SubmitIssueTrack(Guid Issuetrack_Id, String Status, Guid AssignedTo) //, String Comments
        public ActionResult SubmitIssueTrack(IssueTrackModel Data) //, String Comments
        {
            bool Result = false;

            //IssueTrackModel Data = new IssueTrackModel();
            //Data.Issuetrack_Id = Issuetrack_Id;         
            //Data.Status = Status;            
            //Data.AssignedTo = AssignedTo;

            //Data.Comments = Comments;
            //Data.Description = Description;

            Data.Modified_by = Guid.Parse(Session["loginid"].ToString());

            Result = _base.Sp_UpdateIssueTrack(Data);

            if (Result == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        
        [HttpPost]
        public ActionResult GetIssueTrackComments(Guid Id, int timezoneOffset)
        {
            List<IssueTrackModel> comments = _base.Sp_IssueTrackComments(Id, timezoneOffset);
            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        //public ActionResult SubmitIssueTrackComments(string Comments, Guid id) //Model => Encrypt the Commnets
        public ActionResult SubmitIssueTrackComments(CkEditor ckEditor) //Model => Encrypt the Commnets
        {
            bool Result = false;
            ckEditor.Cre_By = Guid.Parse(Session["loginid"].ToString());
            //Result = _base.Sp_UpdateIssueTrackComments(Comments, id, Cre_By);
            Result = _base.Sp_UpdateIssueTrackComments(ckEditor);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(HttpPostedFileBase aUploadedFile)
        {
            LogHelper _log = new LogHelper();
            var vReturnImagePath = string.Empty;
            try
            {
                if (aUploadedFile.ContentLength > 0)
                {
                    var vFileName = Path.GetFileNameWithoutExtension(aUploadedFile.FileName);
                    var vExtension = Path.GetExtension(aUploadedFile.FileName);
                    string NewID = Guid.NewGuid().ToString();

                    string sImageName = vFileName + NewID; // DateTime.Now.ToString("YYYYMMDDHHMMSS");

                    string Upload_ppt = ConfigurationManager.AppSettings["Upload_ppt"];

                    var vImageSavePath = Server.MapPath(Upload_ppt) + sImageName + vExtension;
                    vReturnImagePath = Upload_ppt + sImageName + vExtension;
                    _log.createLog("vImageSavePath ==>" + vImageSavePath);
                    _log.createLog("vReturnImagePath==>" + vReturnImagePath);

                    //var vImageSavePath = Server.MapPath("/Assets/Uploadedppt/") + sImageName + vExtension;
                    //vReturnImagePath = "/Assets/Uploadedppt/" + sImageName + vExtension;

                    ViewBag.Msg = vImageSavePath;
                    var path = vImageSavePath;
                    _log.createLog("aUploadedFile==>" + aUploadedFile);
                    // Saving Image in Original Mode  
                    aUploadedFile.SaveAs(path);

                    var vImageLength = new FileInfo(path).Length;
                    //here to add Image Path to You Database ,  
                    //TempData["message"] = string.Format("Image was Added Successfully");
                }
            }
            catch (Exception ex)
            {
                _log.createExMsgLog("EX =>" + ex); 
            }
            
            return Json(Convert.ToString(vReturnImagePath), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SAPIssueTrackFileUpload(string ProjectId)
        {
            HttpFileCollectionBase files = Request.Files;
            //string ProjectID = Request.Params["ProjectID"].ToString();
            //string InstanceID = Request.Params["InstanceID"].ToString();
            bool Result = false;
            string fname = "", ext, GivenName = "";
            if (files.Count > 0)
            {
                HttpPostedFileBase file = files[0];
                fname = file.FileName;
                GivenName = Path.GetFileNameWithoutExtension(fname);
                if (ProjectId != "")
                {
                    Guid Project_Id = Guid.Parse(ProjectId);
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

                        if (GivenName == "SAP_Issue_dump_upload_v1")
                        {
                            Result = _fileUpload.Process_SAP_Issue_dump(fname, NewID, Project_Id, User_Id);
                        }
                    }
                    catch(Exception ex)
                    {
                        _Log.createExMsgLog("SAPIssueTrackFileUpload Exception => " + ex);
                    }
                }
            }
            return Json(Result);
        }

        public ActionResult LoadSAPIssueTrack(string ProjectId)
        {
            List<SAPIssueTrackModel> LoadSAPIssueTrack = _base.SP_LoadSAPIssueTrack(ProjectId);
            return Json(LoadSAPIssueTrack, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetSAPIssueTrack(Guid id)
        {
            try
            {
                SAPIssueTrackModel ITM = _base.Sp_EditSAPIssueTrackData(id);
                //ViewBag.SAPStatus = _base.Sp_GetSAPStatus();
                return Json(ITM, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        public ActionResult UpdateSAPIssueTrack(Guid Id,int SAPStatus,string Comments)
        {
            bool Result = false;
            Result = _base.Sp_UpdateSAPIssueTrack(Id, SAPStatus, Comments);

            if (Result == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        //public FileResult DownloadTemplate()
        //{
        //    string filePath = Server.MapPath(ConfigurationManager.AppSettings["Upload_ppt"].ToString());
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath + "SAP_Issue_dump_upload_v1.xlst");
        //    string fileName = "SAP_Issue_dump_upload_v1.xlst";
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}

        //public ActionResult DownloadTemplate()
        //{

        //    // Validate the Model is correct and contains valid data
        //    // Generate your report output based on the model parameters
        //    // This can be an Excel, PDF, Word file - whatever you need.

        //    // As an example lets assume we've generated an EPPlus ExcelPackage

        //    ExcelPackage workbook = new ExcelPackage();
        //    // Do something to populate your workbook

        //    // Generate a new unique identifier against which the file can be stored
        //    string handle = Guid.NewGuid().ToString();

        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        workbook.SaveAs(memoryStream);
        //        memoryStream.Position = 0;
        //        TempData[handle] = memoryStream.ToArray();
        //    }

        //    // Note we are returning a filename as well as the handle
        //    return new JsonResult()
        //    {
        //        Data = new { FileGuid = handle, FileName = "TestReportOutput.xlsx" }
        //    };

        //}
    }
}
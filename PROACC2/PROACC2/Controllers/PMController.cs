using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PROACC2.BL.Model;
using PROACC2.BL.General;
using PROACC2.BL;
using System.Configuration;
using System.IO;

namespace PROACC2.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Project Manager")]
    public class PMController : Controller
    {
        // GET: PM

        Base _base = new Base();
        LogHelper _log = new LogHelper();
        public ActionResult Index(string name)
        {
            var LoginId = Guid.Parse(Session["loginid"].ToString());
            List<PMTaskModel> getPrj = _base.Sp_GetPMProjectList(LoginId);
            ViewBag.Projects = getPrj;
            return PartialView(name);
        }

        public ActionResult PMTaskGetData(Guid ProjectId,bool first)
        {
            var LoginId = Guid.Parse(Session["loginid"].ToString());
            List<PMTaskModel> PM = _base.GetPMTask(ProjectId, first, LoginId);
            return Json(PM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _PMMonitor()
        {
            return PartialView("_PMMonitor");
        }

       // public ActionResult SubmitPMTaskMonitor(PMTaskModel pmTaskModel)
        public ActionResult SubmitPMTaskMonitor(Guid Id, int StatusId,decimal EST_hours,decimal Actual_St_hrs, string PlanedDate, string PlanedEn_Date,string ActualDate, string ActualEn_Date,string Notes)
        {
       
            Boolean Result = false;
            PMTaskModel pmTaskModel = new PMTaskModel();
            pmTaskModel.Cre_By = Guid.Parse(Session["loginid"].ToString());
            pmTaskModel.Id = Id;
            pmTaskModel.StatusId = StatusId;
            pmTaskModel.EST_hours = EST_hours;
            pmTaskModel.Actual_St_hrs = Actual_St_hrs;
            pmTaskModel.Notes = Notes;
            //pmTaskModel.Planed__St_Date = DateTime.ParseExact(PlanedDate, "dd/MM/yyyy", null);
            //pmTaskModel.Planed__En_Date = DateTime.ParseExact(PlanedEn_Date, "dd/MM/yyyy", null);
            //pmTaskModel.Actual_St_Date = DateTime.ParseExact(ActualDate, "dd/MM/yyyy", null);
            //pmTaskModel.Actual_En_Date = DateTime.ParseExact(ActualEn_Date, "dd/MM/yyyy", null);

            pmTaskModel.Planed__St_Date = Convert.ToDateTime(PlanedDate.ToString());
            pmTaskModel.Planed__En_Date = Convert.ToDateTime(PlanedEn_Date.ToString());
            pmTaskModel.Actual_St_Date = Convert.ToDateTime(ActualDate.ToString());
            pmTaskModel.Actual_En_Date = Convert.ToDateTime(ActualEn_Date.ToString());

            Result = _base.Sp_SubmitPMTaskMonitor(pmTaskModel);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _PMFileUpload()
        {
            ViewBag.PDFfilepath = ConfigurationManager.AppSettings["Upload_filePath"].ToString();
            return PartialView("_PMFileUpload");
        }
        [HttpPost]
        public ActionResult PM_Upload(string InstanceID)
        {
            
            if (Request.Files.Count > 0)
            {
                try
                {
                    bool Result_Process = false;
                    Guid Instance_ID = Guid.Parse(InstanceID);
                    Guid User_Id = Guid.Parse(Session["loginid"].ToString());
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        string filetype = "";
                        int FileCount = 0;
                        FileCount = Convert.ToInt32(files.AllKeys[i]) + 1;
                        HttpPostedFileBase file = files[i];
                        string fname;
                        string ext;
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
                        if (FileCount == 1)//9
                        {
                            filetype = "Comprehensive Assessment & Readiness Check Report";
                        }
                        else if (FileCount == 2)//10
                        {
                            filetype = "Landscape & System Management";
                        }
                        else if (FileCount == 3)//11
                        {
                            filetype = "Custom Code Analysis";
                        }
                        else if (FileCount == 4)//12
                        {
                            filetype = "License Optimization";
                        }
                       Result_Process = _base.PMUpload(filetype, NewID, Instance_ID, User_Id);
                        
                    }
                    return Json("File Uploaded Successfully!");

                }
                catch (Exception ex)
                {
                    _log.createLog(ex);
                    String msg = ex.Message;
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("Please choose at least one file...");
            }


        }

        public JsonResult PMReportData(string IDInstance)
        {
            Guid InstanceID = Guid.Parse(IDInstance);
            List<FileUploadMaster> PM = _base.GetPMuploadlist(InstanceID);
            var obj = new { data = PM };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPMById(Guid Id)
        {
            PMTaskModel PM = _base.Sp_GetPMTaskMonitorById(Id);
            return Json(PM, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult TimeChart()
        {
            var result2 = _base.Sp_GetProjectList();
            ViewBag.ProjectName = result2.ToList();
            List<PhaseModel> Phaselist = _base.Sp_GetPhaselist();
            ViewBag.Phaselist = Phaselist.ToList();
            return View();
        }

        public ActionResult _TimeChart()
        {
            return PartialView("_TimeChart");
        }

        #region ProjectView
        public ActionResult ProjectView(string name)
        {
            var LoginId = Guid.Parse(Session["loginid"].ToString());
            List<PMTaskModel> getPrj = _base.Sp_GetPMProjectList(LoginId);
            ViewBag.Projects = getPrj;
            return View(name);
        }
        public ActionResult _ProjectDetailView()
        {
            return PartialView();
        }
        public JsonResult PlannedVsCompletion(string Instance)
        {                                                             
            Guid InstanceId = Guid.Parse(Instance);
           List<PlannedVsActual> sP_ = _base.sp_PlannedVsActual(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Planed_Dates(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            List<Planned_Dates> sP_ = _base.sp_Planned_Dates(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReadinssChaeckOrStatus(string Instance)
        {
            Guid InstanceId = Guid.Parse(Instance);
            ReadinssChaeckOrStatus sP_ = _base.Sp_ReadinssChaeckOrStatus(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectView_Count(Guid InstanceId)
        {
            List<ReadinssChaeckOrStatus> PV = _base.Sp_GetProjectViewCount(InstanceId);
            return Json(PV, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PMRiskAssessment

        public ActionResult _PMRiskAssessment()
        {
            List<RA_Model> RA_Probability = _base.GetRAProbability();
            ViewBag.RAProbability = RA_Probability;

            List<RA_Model> RA_RiskClass = _base.GetRARiskClass();
            ViewBag.RARiskClass = RA_RiskClass;

            List<RA_Model> RA_RiskOwner = _base.GetRARiskOwner();
            ViewBag.RARiskOwner = RA_RiskOwner;

            return PartialView("_PMRiskAssessment");            
        }

        //public ActionResult GetRA_Probability()
        //{
        //    List<RA_Model> RA_Probability = _base.GetRAProbability();
        //    return Json(RA_Probability, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetRA_RiskClass()
        //{
        //    List<RA_Model> RA_RiskClass = _base.GetRARiskClass();
        //    return Json(RA_RiskClass, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetRA_RiskOwner()
        //{
        //    List<RA_Model> RA_RiskOwner = _base.GetRARiskOwner();
        //    return Json(RA_RiskOwner, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SubmitRiskAssessment(RiskAssessmentModel riskAssessment)
        {
            riskAssessment.Cre_By = Guid.Parse(Session["loginid"].ToString());
            bool ra = _base.SubmitRiskAssessment(riskAssessment);
            return Json(ra, JsonRequestBehavior.AllowGet);
        }
         public ActionResult UpdateRiskAssessment(RiskAssessmentModel riskAssessment)
        {
            riskAssessment.Cre_By = Guid.Parse(Session["loginid"].ToString());
            bool ra = _base.UpdateRiskAssessment(riskAssessment);
            return Json(ra, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadRiskAssessmentTable(string ProjectId)
        {
            List<RiskAssessmentModel> RA_Data = _base.GetRiskAssessmentData(ProjectId);
            return Json(RA_Data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetRiskAssessmentById(string id)
        {
            RiskAssessmentModel RAM = _base.GetRiskAssessmentById(id);
            return Json(RAM, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
using PROACC2.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PROACC2.BL.Model;
using PROACC2.BL.General;
using static PROACC2.BL.Model.Common;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace PROACC2.Controllers
{
    [CheckSessionTimeOut]
    //[Authorize(Roles = "Consultant,customer,Project Manager")]
    public class TransactionController : Controller
    {
        // GET: Transaction

        Base _Base = new Base();
        LogHelper _log = new LogHelper();
        #region ProjectInstancePhaseList
        
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult Index(string name)
        {
            // var result2 = _base.Sp_GetProjectList();
            // ViewBag.ProjectName = result2;

            var LoginId = Guid.Parse(Session["loginid"].ToString());
            var UserType = Session["UserType"].ToString();
            int Type=0;
            if (UserType == "Admin")
            {
                Type = 1;
            }
            else if(UserType == "Consultant")
            {
                Type = 2;
            }
            else if(UserType == "Customer")
            {
                Type = 3;
            }
            else if (UserType == "Project Manager")
            {
                Type = 4;
            }
            GeneralList sP_ = _Base.spCustomerDropdown(Session["loginid"].ToString(), Type);
            ViewBag.CustomerName = sP_._List; //new SelectList(sP_._List, "Value", "Name");

            List<Instances> result2 = _Base.Sp_GetProjectListByUser(LoginId, Type);
            ViewBag.ProjectName = result2;

            List<PhaseModel> Phaselist = _Base.Sp_GetPhaselist();
            ViewBag.Phaselist = Phaselist.ToList();

            List<BuildingBlockModel> BuildingBlocklist = _Base.Sp_GetBuildingBlocklist(null);
            ViewBag.BuildingBlock = BuildingBlocklist;

            List<ApplicationAreaModel> ApplicationArealist = _Base.Sp_GetApplicationArealist();
            ViewBag.ApplicationArea = ApplicationArealist;

            List<TaskTypeModel> TaskTypelist = _Base.Sp_GetTaskTypelist();
            ViewBag.TaskTypelist = TaskTypelist;

            List<UserModel> roles = _Base.Sp_GetRoles();
            ViewBag.Role = roles;

            return PartialView(name);
        }
        
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetProjectList()
        {
            var Projresult = _Base.Sp_GetProjectList();
            var data = Projresult.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetProjectByCustomer(Guid id)
        {
            var LoginId = Guid.Parse(Session["loginid"].ToString());
            var ProjectList = _Base.SP_GetProjectByCustomer(LoginId,id);
            return Json(ProjectList, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetInstancesByProject(Guid id)
        {
            var InstanceList = _Base.SP_GetCloneInstance(id);
            return Json(InstanceList, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ResourceAllocation
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult _ResourceAllocation()
        {
            return PartialView("_ResourceAllocation");
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult __GetPhaseByProjectInstance(Guid id)
        {
            List<ActivityMasterModel> ActiveList = _Base.Sp_GetActivityMasterData(id);
            ViewBag.ActiveList = ActiveList.ToList();
            return Json(ActiveList, JsonRequestBehavior.AllowGet);
        }

        //Resource Allocation Data
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult __GetDataByResourceAllocation(int PhaseId, Guid InstanceId, bool? first) 
        {
            Guid CreBy = Guid.Parse(Session["loginid"].ToString());
            if (first == false)
            {
                List<ProjectMonitorModel> PrjtList = _Base.Sp_GetResourceAllocationData(PhaseId, InstanceId, first, CreBy);
                return Json(PrjtList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<ProjectMonitorModel> ActiveList = _Base.Sp_GetResourceAllocationData(PhaseId, InstanceId, first, CreBy);
                return Json(ActiveList, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetUserByRole(int RoleID,Guid InstanceId)
        {
            List<UserModel> UM = _Base.Sp_GetUserByRole(RoleID, InstanceId);
            return Json(UM, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetRoleFromRA(int PhaseId, Guid InstanceId)
        {
            List<Role> UM = _Base.Sp_GetRoleFromRA(PhaseId, InstanceId);
            return Json(UM, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetUserByRoleBulkAllocate(int RoleID, Guid InstanceId)
        {
            List<UserModel> UM = _Base.Sp_GetUserByRoleBulkAllocate(RoleID, InstanceId);
            return Json(UM, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult __GetPhaseResource(int id)
        {
            List<ActivityMasterModel> ActiveList = _Base.Sp_GetPhaseResource(id);
            ViewBag.ActiveList = ActiveList.ToList();
            return Json(ActiveList, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult UpdateOwnerResourceAllocation(int PhaseId, Guid InstanceId, Guid rowID, Guid OwnerId)
        {
            var Cre_By = Guid.Parse(Session["loginid"].ToString());
            var a = _Base.Sp_GetUpdateOwnerResourceAllocation(PhaseId, InstanceId, rowID, OwnerId, Cre_By);
            if (a == true)
                return Json("Success", JsonRequestBehavior.AllowGet);
            else
                return Json("error", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Project Manager")]
        public ActionResult UnassingnedTaskCount(int PhaseId, Guid InstanceId)
        {
            var a = _Base.Sp_GetUnassingnedTaskCount(PhaseId, InstanceId);
            return Json(a.Unassigned_Count, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Project Manager")]
        public ActionResult BulkAllocateOwnerResourceAllocation(int PhaseId, Guid InstanceId, int roleID, Guid OwnerId)
        {
            var Cre_By = Guid.Parse(Session["loginid"].ToString());
            var a = _Base.Sp_GetBulkAllocateOwnerResourceAllocation(PhaseId, InstanceId, roleID, OwnerId, Cre_By);
            if (a == true)
                return Json("Success", JsonRequestBehavior.AllowGet);
            else
                return Json("error", JsonRequestBehavior.AllowGet);
        }
        
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult MasterAdd(int Phase_ID, Guid InstanceId)
        {
            List<ProjectMonitorModel> Result = new List<ProjectMonitorModel>();
            Result = _Base.Sp_GetMasterAdd(InstanceId, Phase_ID);

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult AddResource(Guid InstanceId,int ActivityID)
        {
            Boolean Result = false;
            Result = _Base.Sp_AddResource(InstanceId, ActivityID, Session["loginid"].ToString());
            if (Result==true)
            return Json("Success", JsonRequestBehavior.AllowGet);
            else
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Project Manager")]
        public ActionResult _GetUserRole(Guid InstanceId)
        {
            List<UserModel> userRole = _Base.Sp_GetUserRoleList(InstanceId);            
            return Json(userRole, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Project Manager")]
        public ActionResult PullTaskButton(int PhaseId, Guid InstanceId)
        {
            var a = _Base.Sp_PullTaskButton(PhaseId, InstanceId);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ProgressMonitoring

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult _ProgressMonitoring()
        {
            //List<ProjectMonitorModel> ProgressMonitor = _base.Sp_GetProgressMonitor(PhaseId, InstanceId);
            //ViewBag.ProgressMonitor = ProgressMonitor;
            return PartialView("_ProgressMonitoring");
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetProgressMonitor(int PhaseId, Guid InstanceId)
        {
            List<ProjectMonitorModel> ProgressMonitor = _Base.Sp_GetProgressMonitor(PhaseId, InstanceId);
            
            return Json(ProgressMonitor, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult PreviousTaskPhase(int PhaseId, Guid InstanceId)
        {           
            List<ActivityMasterModel> P = _Base.GetActivity(PhaseId, InstanceId);
            return Json(P, JsonRequestBehavior.AllowGet);
        }

       

        //public ActionResult ProgressEdit(Guid Id)
        //{
        //    List<ProjectMonitorModel> a= _base.Sp_GetProgressMonitorData(Id);
        //    ViewBag.GetPM = a;

        //    var UserType = Session["UserType"].ToString();
        //    bool? bit = false;
        //    if (UserType == "Project Manager")
        //    {
        //        bit = true;
        //    }
        //    List<Status> stat = _base.Sp_GetSatus(bit);
        //    //var _statusList = stat.ToList();

        //    ViewBag.GetStatus = stat;
        //    //return View();
        //    return PartialView("ProgressEdit");
        //}

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult __ProgressEdit(Guid Id)
        {
            List<ProjectMonitorModel> a = _Base.Sp_GetProgressMonitorData(Id);
            ViewBag.GetPM = a;
            var aTS =Convert.ToBase64String(a[0].TS);
            ViewBag.TS = aTS;

            var UserType = Session["UserType"].ToString();
            bool? bit = false;
            if (UserType == "Project Manager")
            {
                bit = true;
            }
            List<Status> stat = _Base.Sp_GetSatus(bit);
            //var _statusList = stat.ToList();            
            ViewBag.GetStatus = stat;

            //List<ProjectMonitorModel> comments = _base.Sp_Comments(Id);
            //ViewBag.Comments = comments;
            //return View();
            return PartialView("__ProgressEdit");
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult __ProgressShowMoreDetail(Guid Id)
        {
            List<ProjectMonitorModel> a = _Base.Sp_GetProgressMonitorData(Id);
            ViewBag.GetMoreDetail = a;
            return PartialView("__ProgressShowMoreDetail");
        }
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        

        [HttpPost]
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetComments(Guid Id, int timezoneOffset)
        {
            List<ProjectMonitorModel> comments = _Base.Sp_Comments(Id,timezoneOffset);
            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetStatus()
        {
            var UserType = Session["UserType"].ToString();
            bool? bit = false;
            if (UserType == "Project Manager")
            {
                bit = true;
            }
            List<Status> stat = _Base.Sp_GetSatus(bit);
            var _statusList = stat.ToList();
            return Json(_statusList, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult SubmitProgressMonitor(ProjectMonitorModel projectMonitor, string TS)
        {
            bool Result= false;

            projectMonitor.Cre_By = Guid.Parse(Session["loginid"].ToString());
            //projectMonitor.Planed__St_Date = DateTime.ParseExact(projectMonitor.PlanedDate, "dd/MM/yyyy", null);
            //projectMonitor.Planed__En_Date = DateTime.ParseExact(projectMonitor.PlanedEn_Date, "dd/MM/yyyy", null);
            //projectMonitor.Actual_St_Date = DateTime.ParseExact(projectMonitor.ActualDate, "dd/MM/yyyy", null); 
            //projectMonitor.Actual_En_Date = DateTime.ParseExact(projectMonitor.ActualEn_Date, "dd/MM/yyyy", null);

            //var rowVersionBytes = System.Convert.FromBase64String(TS);
            //string TS1 = string.Join(",", rowVersionBytes);

            //if (TS1 != "")
            //{
            //    string[] abc = TS1.Split(',');
            //    projectMonitor.TS = abc.Select(byte.Parse).ToArray();
            //}

            if (TS != "")
            {
                projectMonitor.TS = System.Convert.FromBase64String(TS);
            }

            projectMonitor.Planed__St_Date = Convert.ToDateTime(projectMonitor.PlanedDate.ToString());
            projectMonitor.Planed__En_Date = Convert.ToDateTime(projectMonitor.PlanedEn_Date.ToString());
            projectMonitor.Actual_St_Date = Convert.ToDateTime(projectMonitor.ActualDate.ToString());
            projectMonitor.Actual_En_Date = Convert.ToDateTime(projectMonitor.ActualEn_Date.ToString());

            Result = _Base.Sp_UpdateProgressMonitor(projectMonitor);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult SubmitComments(string Comments,Guid id)
        {
            bool Result = false;
            var Cre_By = Guid.Parse(Session["loginid"].ToString());
            Result = _Base.Sp_UpdateComments(Comments,id, Cre_By);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public ActionResult GetTaskCount(int PhaseId, Guid InstanceId)
        {
            List<ProjectMonitorModel> ProgressMonitor = _Base.Sp_GetProgressMonitorTaskCount(PhaseId, InstanceId);
            return Json(ProgressMonitor, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        public JsonResult ExportPDF_ProgressMonitor(string ProjName, string InsName, string PhaName,Guid InsId, string PhaseId)
        {
            Base _base = new Base();
            string filePath = Server.MapPath(ConfigurationManager.AppSettings["Temp_path"].ToString());  //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\Uploadedppt\\Sample.pdf";//Server.MapPath("Content") + "Sample.pdf";
            string imagePath = Server.MapPath(ConfigurationManager.AppSettings["Logo_Path"].ToString()); //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\images\\promantus-new-logo.PNG";
            _base.CreateIfMissing(filePath);
            string ProAcc_imagePath = Server.MapPath(ConfigurationManager.AppSettings["ProAcc_Logo_Path"].ToString());
            Guid InstanceID = Guid.Parse(InsId.ToString());
            //string LoginID = Session["loginid"].ToString();

            string File_NameID = Guid.NewGuid().ToString();
            try
            {
                String ProjectName = ProjName;
                String InstancetName = InsName;
                String PhaseName = PhaName;
                filePath = filePath + File_NameID + ".pdf";
                _Base.FileDelete(filePath);
                List<ProjectMonitorModel> PM = _Base.Sp_GetProgressMonitor(Convert.ToInt32(PhaseId), InsId);
                String description = "Project Name :" + ProjectName + " & Instance Name : " + InstancetName + "& Phase Name :"+ PhaseName + ".";
                ExportPDF(PM, filePath, imagePath, ProAcc_imagePath, description);

            }
            catch (Exception ex)
            {
                _log.createExMsgLog("Exportpdf-->" + ex.Message.ToString());
                //throw;
            }
            return Json(new { fileName = File_NameID + ".pdf", errorMessage = "" });
        }
        
        [Authorize(Roles = "Consultant,customer,Project Manager")]
        private static void ExportPDF(List<ProjectMonitorModel> PM, string filePath, string imagePath, string ProAcc_imagePath, string description)
        {
            Font headerFont = FontFactory.GetFont("arial", 6, Font.BOLD, BaseColor.BLACK);
            Font rowfont = FontFactory.GetFont("arial", 6);
            Document document = new Document(PageSize.A4.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document,
                       new FileStream(filePath, FileMode.Create));

            document.Open();
            document.AddTitle("Progress Monitor");

            //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
            //img.Alignment = Element.ALIGN_LEFT;
            //img.ScaleToFit(120f, 170f);
            //img.SpacingBefore = 10f;
            //img.SpacingAfter = 50f;
            //document.Add(img);

            Image img1 = Image.GetInstance(imagePath);
            img1.ScaleToFit(120f, 170f);
            Image img2 = Image.GetInstance(ProAcc_imagePath);
            img2.ScaleToFit(120f, 170f);
            PdfPTable table1 = new PdfPTable(2);

            PdfPCell cell1 = new PdfPCell();
            Paragraph p = new Paragraph();
            p.Add(new Chunk(img1, 0, 0, true));
            cell1.AddElement(p);
            cell1.Border = Rectangle.NO_BORDER;
            cell1.HorizontalAlignment = 0;

            PdfPCell c2 = new PdfPCell();
            Paragraph pa = new Paragraph();
            pa.Add(new Chunk(img2, 0, 0, true));
            pa.Alignment = Element.ALIGN_RIGHT;
            c2.AddElement(pa);
            c2.Border = Rectangle.NO_BORDER;

            table1.AddCell(cell1);
            table1.AddCell(c2);
            document.Add(table1);

            Paragraph para1 = new Paragraph(description);
            para1.Alignment = Element.ALIGN_CENTER;
            para1.SpacingAfter = 20f;
            document.Add(para1);

            string[] columns = { "SNO", "BuildingBlock","Task", "Role","Owner","TaskType", "Status", "PlanedDate", "PlanedEn_Date", "ActualDate", "ActualEn_Date" };//, "Notes" };
            string[] columnHeadings = { "S.No", "Bulding Block","Task", "Role", "Owner", "Task Type","Status", "Planned Start", "Planned End", "Actual Start", "Actual End"};//, "Commnets" };
            PdfPTable table = new PdfPTable(columns.Length)
            { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
            table.PaddingTop = 300f;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.TotalWidth = 550f;
            float[] widths = new float[] { 20f, 65f, 100f, 40f,60f, 40f,40f,40f,40f, 40f, 40f};//, 50f };
            table.SetWidths(widths);
            foreach (var column in columnHeadings)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }

            foreach (var item in PM)
            {
                foreach (var column in columns)
                {
                    string value = item.GetType().GetProperty(column).GetValue(item).ToString();
                    PdfPCell cell5 = new PdfPCell(new Phrase(value, rowfont));
                    table.AddCell(cell5);
                }
            }
            document.Add(table);
            document.Close();
        }
        #endregion

        #region Activity
        [Authorize(Roles = "Admin,Project Manager")]
        public JsonResult CheckTaskAvailability(string namedata, int? id,Guid VID,Guid InsId)
        {
            if (id != null)
            {
                //var SearchDt = db.ActivityMasters.Where(x => x.Task == namedata).Where(x => x.Activity_ID != id).Where(x => x.isActive == true).FirstOrDefault();
                var SearchDt = _Base.Sp_CheckTaskAvailability(namedata, id, VID, InsId);
                if (SearchDt != true)
                    return Json("error", JsonRequestBehavior.AllowGet);
                else
                    return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var SearchDt = db.ActivityMasters.Where(x => x.Task == namedata).Where(x => x.isActive == true).FirstOrDefault();
                var SearchDt = _Base.Sp_CheckTaskAvailability(namedata, id, VID, InsId);
                if (SearchDt == true)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult ParallelNameByPhase(int id, int taskvalue, Guid instanceid, bool first)
        {
            List<ParallelTypeModel> ParallelTypelist = _Base.Sp_GetParallelTypelist(id, taskvalue, instanceid, first);
            return Json(ParallelTypelist, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin,Project Manager")]
        public JsonResult CheckParallel_NameAvailability(string namedata, Guid InstanceId)
        {
            var SearchDt = _Base.Sp_CheckParallel_NameAvailability(namedata, InstanceId);
            if (SearchDt == true)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult GetAllTaskByParallelType(int id, Guid Parallel_Id, Guid instanceid, bool first,Guid VID)
        {
            List<ActivityModel> Activitylist = _Base.Sp_GetTaskByParallelType(id, Parallel_Id, instanceid, first, VID);
            return Json(Activitylist, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Project Manager")]
        public ActionResult AddActivityTask(ProjectMonitorModel projectMonitor)
        {
            Boolean Result = false;
            projectMonitor.Cre_By = Guid.Parse(Session["loginid"].ToString());
            Result = _Base.Sp_AddTask(projectMonitor);
            if (Result == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetBuildingBlock(string InstanceId)
        {
            List<BuildingBlockModel> BuildingBlocklist = _Base.Sp_GetBuildingBlocklist(InstanceId);
            ViewBag.BuildingBlock = BuildingBlocklist;
            return Json(BuildingBlocklist, JsonRequestBehavior.AllowGet);
        }
       #endregion

            #region SendPdfAttachmentMail
            public ActionResult __SendMail()
        {
            return PartialView("__SendMail");
        }
        [HttpPost]
        public JsonResult SendMail_(string Email, string ProjName, string InsName,string PhaName,Guid InsId, string PhaseId, string Subject, string Body)
        {
            _log.createLog("----------------Inside send mail function-----------------------");
            bool _status = false;
            try
            {                
                var FileName = "";

                Guid _UserId = Guid.Parse(Session["loginid"].ToString());
                var _Email = Email;

                //List<String> filePath2 = Directory.GetFiles(Server.MapPath(ConfigurationManager.AppSettings["Temp_path"])).OrderByDescending(x => new System.IO.FileInfo(x).CreationTimeUtc).ToList();
                //foreach (var item in filePath2)
                //{
                //    System.IO.File.Delete(item);
                //}

                ExportPDF_ProgressMonitor(ProjName, InsName, PhaName, InsId, PhaseId);
                var _Msg = Body + "<br><br/><b>You recently requested to download Project and Instance file of  '" + ProjName + "' for '" + InsName + "' <br> Please Find the Attachment . </b><br/>";
                
                //List<String> filePath = Directory.GetFiles(Server.MapPath(ConfigurationManager.AppSettings["Temp_path"])).OrderByDescending(x => new System.IO.FileInfo(x).CreationTimeUtc).ToList();
                //foreach (var item2 in filePath)
                //{
                //    FileName = Path.GetFileNameWithoutExtension(item2);
                //}

                var directory = new DirectoryInfo(Server.MapPath(ConfigurationManager.AppSettings["Temp_path"]));
                var myFile = (from f in directory.GetFiles()
                              orderby f.LastWriteTime descending
                              select f).First().ToString();
                FileName = Path.GetFileNameWithoutExtension(myFile);

                var _mailSend = _Base.SendMailPdfAttachment(_UserId, _Email, _Msg, FileName, Subject);
                if (_mailSend == true)
                {
                    _status = true;
                }
                else
                {
                    _status = false;
                }
            }
            catch (Exception ex)
            {
                _log.createLog("----------------Inside send mail catch-----------------------");
                _log.createLog(ex);
            }
            

            return Json(_status);
        }
        
        //[HttpPost]
        //public JsonResult TestMail(string Email, string ProjName, string InsName, string PhaName, Guid InsId, string PhaseId, string Subject, string Body)
        //{
        //    _log.createLog("----------------Inside send mail function-----------------------");
        //    bool _status = false;
        //    try
        //    {
        //        var FileName = "";

        //        Guid _UserId = Guid.Parse(Session["loginid"].ToString());
        //        var _Email = Email;
        //        _log.createLog(ConfigurationManager.AppSettings["Temp_path"].ToString());
        //        List<String> filePath2 = Directory.GetFiles(Server.MapPath(ConfigurationManager.AppSettings["Temp_path"])).OrderByDescending(x => new System.IO.FileInfo(x).CreationTimeUtc).ToList();
        //        _log.createLog("---------Middle ---1---send mail-----------");                

        //        foreach (var item in filePath2)
        //        {
        //            _log.createLog(item.ToString());
        //            System.IO.File.Delete(item);
        //        }

        //        ExportPDF_ProgressMonitor(ProjName, InsName, PhaName, InsId, PhaseId);
        //        _log.createLog("---------Middle ----ExportPDF_ProgressMonitor---send mail-----------");
        //        var _Msg = Body + "<br><br/><b>You recently requested to download Project and Instance file of  '" + ProjName + "' for '" + InsName + "' <br> Please Find the Attachment . </b><br/>";
        //        _log.createLog(_Msg);
        //        List<String> filePath = Directory.GetFiles(Server.MapPath(ConfigurationManager.AppSettings["Temp_path"])).OrderByDescending(x => new System.IO.FileInfo(x).CreationTimeUtc).ToList();
                
        //        _log.createLog("---------Middle --2----send mail-----------");
        //        foreach (var item2 in filePath)
        //        {
        //            _log.createLog(item2);
        //            FileName = Path.GetFileNameWithoutExtension(item2);
        //        }

        //        var _mailSend = _Base.SendMailPdfAttachment(_UserId, _Email, _Msg, FileName, Subject);
        //        _log.createLog("---------Middle --_mailSend----send mail-----------");
        //        if (_mailSend == true)
        //        {
        //            _status = true;
        //        }
        //        else
        //        {
        //            _status = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.createLog("----------------Inside send mail catch-----------------------");
        //        _log.createLog(ex);
        //    }
        //    return Json(_status);
        //}
        
        #endregion


    }
}
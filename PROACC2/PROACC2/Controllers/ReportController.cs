using PROACC2.BL;
using PROACC2.BL.General;
using PROACC2.BL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using static PROACC2.BL.Model.Common;

namespace PROACC2.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant,customer,Project Manager")]
    public class ReportController : Controller
    {
        Base _Base = new Base();
        LogHelper _log = new LogHelper();
        // GET: Report
        public ActionResult Index()
        {
            

            return View();
        }


        public PartialViewResult _DetailReport()
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

            GeneralList sP_ = _Base.spCustomerDropdown(Session["loginid"].ToString(), Type);
            ViewBag.CustomerName = sP_._List;

            List<Instances> result2 = _Base.Sp_GetProjectListByUser(LoginId, Type);
            ViewBag.ProjectName = result2;

            //var result2 = _Base.Sp_GetProjectList();
            //ViewBag.ProjectName = result2.ToList();
            return PartialView("_DetailReport");
        }
        public ActionResult GetRportdata(Guid? id)
        {
            List<ProjectMonitorModel> result = _Base.SP_GetReportData(id);
            var _reportList = result.ToList();
            return Json(_reportList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetInstancesByProject(Guid id)
        {
            var InstanceList = _Base.SP_GetCloneInstance(id);
            return Json(InstanceList, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        public ActionResult Exportpdf(string ProjName, string InsName, string InsId)
        {
            Base _base = new Base();
            string filePath = Server.MapPath(ConfigurationManager.AppSettings["Temp_path"].ToString());  //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\Uploadedppt\\Sample.pdf";//Server.MapPath("Content") + "Sample.pdf";
            string imagePath = Server.MapPath(ConfigurationManager.AppSettings["Logo_Path"].ToString()); //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\images\\promantus-new-logo.PNG";
            _base.CreateIfMissing(filePath);
            string ProAcc_imagePath = Server.MapPath(ConfigurationManager.AppSettings["ProAcc_Logo_Path"].ToString());
            Guid InstanceID = Guid.Parse(InsId.ToString());
            //string LoginID = Session["loginid"].ToString();
            
            string File_NameID = "DetailReport_" + DateTime.Now.ToFileTime();
            try
            {
                String ProjectName = ProjName;
            String InstancetName = InsName;
            filePath = filePath + File_NameID + ".pdf";
            _Base.FileDelete(filePath);
            List<ProjectMonitorModel> PM = _Base.Sp_GetReportDataReportPDF(InstanceID);
            String description = "Project Name :" + ProjectName + " & Instance Name : " + InstancetName + ".";
            Export_PDF(PM, filePath, imagePath, ProAcc_imagePath, description);
                
            }
            catch (Exception ex)
            {
                _log.createExMsgLog("Exportpdf-->" + ex.Message.ToString());
                //throw;
            }
            return Json(new { fileName = File_NameID + ".pdf", errorMessage = "" });
        }



        //public ActionResult pdfData(string ProjName,string InsName,Guid InsId)
        //{
        //    try
        //    {
        //        string filePath = Server.MapPath(ConfigurationManager.AppSettings["Temp_path"].ToString());  //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\Uploadedppt\\Sample.pdf";//Server.MapPath("Content") + "Sample.pdf";
        //        string imagePath = Server.MapPath(ConfigurationManager.AppSettings["Logo_Path"].ToString()); //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\images\\promantus-new-logo.PNG";
        //        Guid InstanceID = Guid.Parse(InsId.ToString());
        //        string LoginID = Session["loginid"].ToString();
        //        String ProjectName = ProjName;
        //        String InstancetName = InsName;
        //        filePath = filePath + LoginID + ".pdf";
        //        _Base.FileDelete(filePath);
        //        List<ProjectMonitorModel> PM = _Base.Sp_GetReportDataReportPDF(InstanceID, LoginID);
        //        String description = "Project Name :" + ProjectName + " & Instance Name : " + InstancetName + ".";
        //        ExportPDF(PM, filePath, imagePath, description);
        //        return File(filePath, "application/pdf", "DetailedReport.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private static void Export_PDF(List<ProjectMonitorModel> PM, string filePath, string imagePath,string ProAcc_imagePath, string description)
        {
            LogHelper _log = new LogHelper();
            
            try
            {
                Font headerFont = FontFactory.GetFont("arial", 6, Font.BOLD, BaseColor.BLACK);
                Font rowfont = FontFactory.GetFont("arial", 6);
                Document document = new Document(PageSize.A4.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document,
                           new FileStream(filePath, FileMode.Create));

                document.Open();
                document.AddTitle("Detail Report");

                //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                //img.Alignment = Element.ALIGN_LEFT;
                //img.ScaleToFit(120f, 170f);
                //img.SpacingBefore = 10f;
                //img.SpacingAfter = 50f;
                //document.Add(img);

                //iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(ProAcc_imagePath);
                //img1.Alignment = Element.ALIGN_RIGHT;
                //img1.ScaleToFit(120f, 170f);
                //img1.SpacingBefore = 10f;
                //img1.SpacingAfter = 50f;
                //document.Add(img1);  

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

                string[] columns = { "SNO", "BuildingBlock", "Phase", "Task", "ApplicationArea", "Delay_occurred_Report", "Owner", "Status", "EST_hrs", "Actual_St_hrs", "PlanedDate", "ActualDate", "PlanedEn_Date", "ActualEn_Date" };//, "Notes" };
                string[] columnHeadings = { "S.No", "Bulding Block", "Phase", "Task", "Application Area", "Delay", "Owner", "Status", "EST (hrs)", "Actual (hrs)", "Planned Start", "Actual Start", "Planned End", "Actual End" };//, "Commnets" };
                PdfPTable table = new PdfPTable(columns.Length)
                { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                table.PaddingTop = 300f;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.TotalWidth = 550f;
                float[] widths = new float[] { 20f, 65f, 40f, 100f, 50f, 25f, 40f, 40f, 25f, 25f, 30f, 30f, 30f, 30f };//, 50f };
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
            catch(Exception ex)
            {
                _log.createExMsgLog("Exportpdf-->" + ex.Message.ToString());
            }
            
        }


        [HttpGet]
        [DeleteFileAttribute] 
        public ActionResult Download(string file)
        {
            string filePath = Server.MapPath(ConfigurationManager.AppSettings["Temp_path"].ToString());  //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\Uploadedppt\\Sample.pdf";//Server.MapPath("Content") + "Sample.pdf";

            string fullPath = Path.Combine(filePath, file);
            return File(fullPath, "application/pdf", file);
        }
        public class DeleteFileAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                filterContext.HttpContext.Response.Flush();

                //convert the current filter context to file and get the file path
                string filePath = (filterContext.Result as FilePathResult).FileName;

                //delete the file after download
                System.IO.File.Delete(filePath);
            }
        }



        #region Audit
        public PartialViewResult _AuditReports()
        {
            return PartialView("_AuditReports");
        }
        
        [HttpGet]
        public ActionResult GetAuditDatas(AuditReport.ProjectMonitorModel model,int timezoneOffset)
        {
            List<AuditReport.ProjectMonitorModel> PM = _Base.Sp_GetAuditDatas(model, timezoneOffset);
            //var obj = new { data = PM };
            return Json(PM, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult _AssessmentReport()
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
            GeneralList sP_ = _Base.spCustomerDropdown(Session["loginid"].ToString(), Type);
            ViewBag.CustomerName = sP_._List;

            var result2 = _Base.Sp_GetProjectListByUser(LoginId, Type);
            ViewBag.ProjectName = result2.ToList();
            ViewBag.PDFfilepath = ConfigurationManager.AppSettings["Upload_filePath"].ToString();
            return PartialView("_AssessmentReport");
        }
        public JsonResult PMReportData(string InstanceId)
        {
            Guid InstanceID = Guid.Parse(InstanceId);
            List<FileUploadMaster> PM = _Base.GetPMuploadlist(InstanceID);

            var obj = new { data = PM };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #region _RiskAssessment
        public PartialViewResult _RiskAssessment()
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
            GeneralList sP_ = _Base.spCustomerDropdown(Session["loginid"].ToString(), Type);
            ViewBag.CustomerName = sP_._List;

            var result2 = _Base.Sp_GetProjectListByUser(LoginId, Type);
            ViewBag.ProjectName = result2.ToList();
            return PartialView("_RiskAssessment");
        }

        public ActionResult PMRiskAssessmentDiagram(string Project_Id)
        {
            var Risk = _Base.PMRiskAssessmentDiagram(Project_Id);
            return Json(Risk, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
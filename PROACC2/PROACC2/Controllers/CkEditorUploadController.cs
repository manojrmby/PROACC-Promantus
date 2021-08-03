//using System;
//using System.Web;
//using System.Web.Mvc;
//using System.IO;

//namespace PROACC2.Controllers
//{
//    public class CkEditorUploadController : Controller
//    {
//        // GET: CkEditorUpload
//        const string filesavepath = "~/Content/Uploads/Ckeditor";
//        const string baseUrl = @"/Content/Uploads/Ckeditor/";

//        const string scriptTag = "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}')</script>";

//        public ActionResult Index()
//        {
//            var funcNum = 0;
//            int.TryParse(Request["CKEditorFuncNum"], out funcNum);

//            if (Request.Files == null || Request.Files.Count < 1)
//                return BuildReturnScript(funcNum, null, "No file has been sent");

//            string fileName = string.Empty;
//            SaveAttatchedFile(filesavepath, Request, ref fileName);
//            var url = baseUrl + fileName;

//            return BuildReturnScript(funcNum, url, null);
//        }

//        private ContentResult BuildReturnScript(int functionNumber, string url, string errorMessage)
//        {
//            return Content(
//                string.Format(scriptTag, functionNumber, HttpUtility.JavaScriptStringEncode(url ?? ""), HttpUtility.JavaScriptStringEncode(errorMessage ?? "")),
//                "text/html"
//                );
//        }

//        private void SaveAttatchedFile(string filepath, HttpRequestBase Request, ref string fileName)
//        {
//            for (int i = 0; i < Request.Files.Count; i++)
//            {
//                var file = Request.Files[i];
//                if (file != null && file.ContentLength > 0)
//                {
//                    fileName = Path.GetFileName(file.FileName);
//                    string targetPath = Server.MapPath(filepath);
//                    if (!Directory.Exists(targetPath))
//                    {
//                        Directory.CreateDirectory(targetPath);
//                    }
//                    fileName = Guid.NewGuid() + fileName;
//                    string fileSavePath = Path.Combine(targetPath, fileName);
//                    file.SaveAs(fileSavePath);
//                }
//            }
//        }
//    }
//}
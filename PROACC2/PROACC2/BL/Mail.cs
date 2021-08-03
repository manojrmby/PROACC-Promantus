using PROACC2.BL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using PROACC2;
using PROACC2.BL.General;

namespace PROACC2.BL
{
    public class Mail
    {
        Base _Base = new Base();
        LogHelper _Log = new LogHelper();
        //CrystalReportPDF pDF = new CrystalReportPDF();
        private readonly string _TemplatePath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["Mail_FolderPath"].ToString());
        private readonly string OutputPath_pdf = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["OutputPath_pdf"].ToString());
        string DownloadPath_pdf = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["Temp_path"].ToString());
        private readonly string _Mail_CheckInterval = WebConfigurationManager.AppSettings["Mail_CheckInterval"];        
        private readonly string _MailRunStatus = WebConfigurationManager.AppSettings["MailRunStatus"];
        private readonly string _Mail_EnableTest = ConfigurationManager.AppSettings["Mail_EnableTest"].ToString();
        private readonly string _Mail_TestToID = ConfigurationManager.AppSettings["Mail_TestToID"].ToString();
        private readonly string serverName = WebConfigurationManager.AppSettings["Mail_Client"];

        private readonly string userName = WebConfigurationManager.AppSettings["Mail_UserName"];
        private readonly string password = WebConfigurationManager.AppSettings["Mail_Password"];

        private readonly string Mail_ToAddress =  WebConfigurationManager.AppSettings["Mail_ToAddress"];
        private readonly string Mail_CCAddress = WebConfigurationManager.AppSettings["Mail_CCAddress"];

        //readonly MailAddress fromAddress = new MailAddress(WebConfigurationManager.AppSettings["Mail_UserName"]);
        readonly int port = Convert.ToInt32(WebConfigurationManager.AppSettings["Mail_Port"]);
        

        internal void StartMailSend()
        {
            Boolean MailRunstatus = Convert.ToBoolean(_MailRunStatus);
            if (MailRunstatus)
            {
                _ = StartTimer();
            }
        }
        public async Task StartTimer()
        {
          int T= Convert.ToInt32(_Mail_CheckInterval);
            await Task.Run(async () =>
            {
                while (true)
                {
                    _Log.createLog("Mail Started");
                    _ = SendAsyncMail();
                  //  _ = pDF.Report();
                    await Task.Delay(T);
                    Thread.Sleep(100000);
                    //, cancellationToken);
                    //if (cancellationToken.IsCancellationRequested)
                    //break;
                }
            });

        }
        public async Task SendAsyncMail()
        {
            try
            
            {
                Boolean priority = false,MailTestEnable=true;
                string TestToId = "";
                List<MailModel> MailS = _Base.GetMailList();
                MailTestEnable = Convert.ToBoolean(_Mail_EnableTest);
                TestToId = _Mail_TestToID;

                foreach (var item in MailS)
                {
                    Send_Mail SM = new Send_Mail();

                    SM.subject = item.Subject;
                    SM.body = PopulateBody(item.TemplateName.Trim(),item.Body,item.Name,item.To);
                    SM.priority = priority;
                    SM.ID = item.Running_ID;
                    SM.Q_UserID = item.Q_UserID;
                    SM.LineID = item.LineID;
                    SM.TemplateName = item.TemplateName;


                    string To="",Name="";
                    if (MailTestEnable)
                    {
                        To = TestToId;
                        Name = "Test";

                    }
                    else
                    {
                        To = item.To.ToString();
                        Name = item.Name.ToString();
                    }
                   
                    await Task.Run(() => this.Send(To, Name, SM));
                }
                _Log.createLog("All Mail Sent");
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->SendAsyncMail" + ex.Message.ToString());
            }
            
            
        }
        
        public void Send(string To, string Name, Send_Mail SM)
        {
            Task.Factory.StartNew(() => SendEmail(To, Name, SM), TaskCreationOptions.LongRunning);
        }

        private void SendEmail(string To, string Name, Send_Mail SM)
        {
            try
            {
                //ProAccEntities db = new ProAccEntities();
                MailMessage msg = new MailMessage();
                if (SM.Q_UserID > 0)
                {
                    //To = "akhil@promantus.com";
                    //Name = "Akhil";
                    SM.priority = true;
                    string Toaddresses = Mail_ToAddress;
                    string CCAddress = Mail_CCAddress;
                    foreach (var address in Toaddresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        msg.To.Add(address);
                    }

                    foreach (var CCAdd in CCAddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        msg.CC.Add(CCAdd);
                    }
                }
                else if (SM.subject == "IssueTrack")
                {
                    MailAddress toAddress = new MailAddress(To, Name);
                    msg.To.Add(toAddress);

                    UserModel _email = _Base.Sp_GetMailList(SM.LineID);
                    msg.CC.Add(_email.Email);
                    string CCAddress = Mail_CCAddress;
                    foreach (var CCAdd in CCAddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        msg.CC.Add(CCAdd);
                    }

                }
                else if (SM.subject == "ProAcc Survey Mail")
                {
                    MailAddress toAddress = new MailAddress(To, Name);
                    msg.To.Add(toAddress);

                    string CCAddress = Mail_CCAddress;
                    foreach (var CCAdd in CCAddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        msg.CC.Add(CCAdd);
                    }
                }
                else if (SM.subject == "ResetMail")
                {
                    MailAddress toAddress = new MailAddress(To, Name);
                    msg.To.Add(toAddress);

                }
                else
                {
                    MailAddress toAddress = new MailAddress(To, Name);
                    msg.To.Add(toAddress);
                }

                MailAddress fromAddress = new MailAddress(_Base.Decrypt(WebConfigurationManager.AppSettings["Mail_UserName"]));

                msg.From = fromAddress;

                msg.Subject = SM.subject;
                msg.Body = SM.body;
                msg.IsBodyHtml = true;
                msg.HeadersEncoding = Encoding.UTF8;
                msg.SubjectEncoding = Encoding.UTF8;
                msg.BodyEncoding = Encoding.UTF8;
                if (SM.priority) msg.Priority = MailPriority.High;

                if (SM.Q_UserID > 0)
                {
                    var fs = new FileStream(OutputPath_pdf + SM.Q_UserID + ".pdf", FileMode.Open);
                    msg.Attachments.Add(new System.Net.Mail.Attachment(fs, "Questionnaire.pdf", "application/pdf"));
                }

                if (SM.TemplateName == "T11")
                {
                    var _pdfs = new FileStream(DownloadPath_pdf + SM.LineID + ".pdf", FileMode.Open);
                    msg.Attachments.Add(new System.Net.Mail.Attachment(_pdfs, "ProAcc.Pdf", "application/pdf"));

                }

                SmtpClient client = new SmtpClient(serverName, port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;

                NetworkCredential smtpUserInfo = new NetworkCredential(_Base.Decrypt(userName), _Base.Decrypt(password));
                client.Credentials = smtpUserInfo;
                //client.Credentials = smtpUserInfo;


                MailModel mailMd = _Base.GetEmailStatus(SM.ID);
               // var _MailStatus = mailMd.ToList();
                //string _stat = null;
                
                //foreach(var item4  in _MailStatus)
                //{
                //    _stat = item4.Running_ID.ToString();
                //}
                //var mm = db.MailMasters.Where(x => x.Running_ID == SM.ID).FirstOrDefault().MailStatus;
                //var mm = Convert.ToBoolean(_stat.FirstOrDefault());   
                
                if (mailMd.MailStatus == false)
                {
                     client.Send(msg);
                    _Log.createLog(SM.ID + "---> Email To->" + To.ToString());
                    _Base.UpdateMailList(SM.ID);
                }
                client.Dispose();
                msg.Dispose();
                if(SM.LineID !=Guid.Empty || SM.Q_UserID !=0)
                {
                   
                    if(SM.Q_UserID != 0)
                    {
                        Delete_File(OutputPath_pdf+ SM.Q_UserID + ".pdf");
                    }
                    else if(SM.LineID != Guid.Empty)
                    {
                        Delete_File(DownloadPath_pdf + SM.LineID + ".pdf");
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->SendEmail" + ex.Message.ToString());
            }

        }

        private Boolean Delete_File(string filepath)
        {
            bool status=false;
            if (filepath != "")
            {
                status = true;

                System.IO.File.Delete(filepath);
            }            
            return status;
        }
        private string PopulateBody(string TemplateName,string Body,string Name,string To)
        {
            string body = string.Empty;
            try
            {
                string pattern = "(,)";
                string input = Body;
                string Task = "", Phase = "", projectName = "", Instance = "", Issue="", G_Body = "";//User_ID="",
                Regex regex = new Regex(pattern);

                using (StreamReader reader = new StreamReader(_TemplatePath + TemplateName.Trim() + ".html"))
                {
                    body = reader.ReadToEnd();
                }

                if (TemplateName.Trim()== "T5")
                {
                    //string[] substrings = regex.Split(input);
                    //User_ID= substrings[0].ToString();

                    string[] substrings = regex.Split(input);
                    body = body.Replace("{MailId}", To);

                }
                else if(TemplateName.Trim() == "T6")
                {
                    string[] substrings = regex.Split(input);
                    Issue = substrings[0].ToString();
                    Phase = substrings[2].ToString();
                    Instance = substrings[4].ToString();
                    projectName = substrings[6].ToString();

                    body = body.Replace("{Name}", Name);
                    body = body.Replace("{IssueName}", Issue);
                    body = body.Replace("{PhaseName}", Phase);
                    body = body.Replace("{InstanceName}", Instance);
                    body = body.Replace("{projectName}", projectName);
                }
                else if (TemplateName.Trim() == "T7")
                {
                    string[] substrings = regex.Split(input);
                    Issue = substrings[0].ToString();

                    body = body.Replace("{Name}", Issue);
                }
                else if (TemplateName.Trim() == "T8")
                {
                    string[] substrings = regex.Split(input);
                    G_Body = substrings[0].ToString();

                    body = body.Replace("{Name}", Name);
                    body = body.Replace("{Body}", G_Body);
                }
                else if (TemplateName.Trim() == "T9")
                {
                    string[] substrings = regex.Split(input);
                    Instance = substrings[0].ToString();
                    projectName = substrings[2].ToString();

                    body = body.Replace("{InstanceName}", Instance);
                    body = body.Replace("{projectName}", projectName);
                    body = body.Replace("{Name}", Name);
                }
                else if (TemplateName.Trim() == "T10")
                {
                    string[] substrings = regex.Split(input);
                    projectName = substrings[0].ToString();

                    body = body.Replace("{projectName}", projectName);
                    body = body.Replace("{Name}", Name);
                }
                else if (TemplateName.Trim() == "T11")
                {
                    string[] substrings = regex.Split(input);
                    G_Body = substrings[0].ToString();

                    body = body.Replace("{Name}", Name);
                    body = body.Replace("{Body}", G_Body);
                }
                else
                {
                    string[] substrings = regex.Split(input);
                    Task = substrings[0].ToString();
                    Phase = substrings[2].ToString();
                    Instance = substrings[4].ToString();
                    projectName = substrings[6].ToString();

                    body = body.Replace("{ManagerName}", Name);
                    body = body.Replace("{TaskName}", Task);
                    body = body.Replace("{PhaseName}", Phase);
                    body = body.Replace("{projectName}", projectName);
                    body = body.Replace("{InstanceName}", Instance);

                }
               
                //body = body.Replace("{Description}", description);
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->PopulateBody" + ex.Message.ToString());
            }


            return body;
        }

        public class Send_Mail
        {
            public string subject { get; set; }
            public string body { get; set; }
            public string TemplateName { get; set; }
            public Boolean priority { get; set; }

            public int ID { get; set; }
            public int Q_UserID { get; set; }
            public Guid? LineID { get; set; }


        }


        //private void SendEmail(String ToMailId)
        //{
        //    Boolean Status = false;

        //    try
        //    {
        //        string Client = ConfigurationManager.AppSettings["Mail_Client"].ToString();
        //        string UserName = ConfigurationManager.AppSettings["Mail_UserName"].ToString();
        //        string Password = ConfigurationManager.AppSettings["Mail_Password"].ToString();
        //        int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Port"].ToString());
        //        Boolean SSL = Convert.ToBoolean(ConfigurationManager.AppSettings["Mail_SSL"].ToString());


        //        MailMessage mail = new MailMessage();
        //        SmtpClient SmtpServer = new SmtpClient(Client);

        //        mail.From = new MailAddress(UserName);
        //        mail.To.Add(ToMailId);
        //        mail.Subject = "Test Mail";
        //        mail.Body = "This is for testing SMTP mail from GMAIL";
        //        mail.IsBodyHtml = true;
        //        SmtpServer.Port = Port;
        //        SmtpServer.UseDefaultCredentials = false;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
        //        SmtpServer.EnableSsl = SSL;

        //        SmtpServer.Send(mail);
        //        Status = true;
        //        SmtpServer.Dispose();
        //        mail.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log
        //    }
        //    //return Status;
        //}
    }
}
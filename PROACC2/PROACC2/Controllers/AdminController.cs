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

namespace PROACC2.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        Base _base = new Base();
        LogHelper _Log = new LogHelper();
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Index(string name)
        {
            var result = _base.Sp_Project();
            ViewBag.Result = result.ToList();
            var count = result.Count();
            ViewBag.ProJCount = count;
            
            var Version = _base.Sp_GetVersion();
            ViewBag.VersionName = Version.OrderByDescending(x => x.CreatedOn).ToList();

            var result2 = _base.Sp_GetProjectList();
            ViewBag.ProjectName = result2.ToList();
            
            var result3 = _base.SP_GetRole();
            ViewBag.GetRole = result3.ToList();
            var count2 = result3.Count();
            ViewBag.RoleCount = count2;

            var result4 = _base.Sp_PMDetail();
            ViewBag.PmDetail = result4.ToList();

            var count3 = result4.Count();
            ViewBag.PMCount = count3;


            List<PMTask> ptask2 = _base.Sp_GetTaskCategoryById();
            ViewBag.TaskList = ptask2;

            List<BuildingBlockModel> buildingBlocks = _base.Sp_GetBuldingBlock();
            ViewBag.BuildingBlocks = buildingBlocks;


            List<UserModel> result5 = _base.SP_GetProjectManager();
            ViewBag.ProjectManager = result5.ToList();

            

            List<CustomerModel> result7 = _base.Sp_GetCompany();
            ViewBag.Company = result7.ToList();

            //List<CustomerModel> customers = _base.Sp_GetCustomerList();
            //ViewBag.Customers = customers;

            //List<CustomerModel> IndustrySector = _base.Sp_GetIndustrySectorList();
            //ViewBag.IndustrySector = IndustrySector;

            //List<UserModel> users = _base.Sp_GetUserList();
            //ViewBag.Users = users;

            //List<UserModel> userType = _base.Sp_GetUserTypeList();
            //ViewBag.UserType = userType;

            List<UserModel> roles = _base.Sp_GetRoles();
            ViewBag.Role = roles;
            if (Session["VersionID"] == null)
            { Session["VersionID"] = ""; }
            var VersionID = Session["VersionID"].ToString();
            Guid emptyID = Guid.Empty;
            Guid ResultID = emptyID;
            if (VersionID=="")
            {
                ResultID = emptyID;
            }
            else
            {
                ResultID=Guid.Parse(VersionID.ToString());
            }
            
            List<ActivityModel> Activitylist = _base.Sp_GetActivityList(ResultID);
            string VersionName = null;
            if (Activitylist.Count > 0)
            {
                Guid Version_Id = Guid.Parse(Activitylist[0].Version_Id.ToString());
                VersionName = _base.Sp_GetVersionName(Version_Id);

            }
            ViewBag.Activity = Activitylist;
            ViewBag.Version_Name = VersionName;
            ViewBag.Activity_Count = Activitylist.Count();

            List<BuildingBlockModel> BuildingBlocklist = _base.Sp_GetBuildingBlocklist(null);
            ViewBag.BuildingBlock = BuildingBlocklist;

            List<ApplicationAreaModel> ApplicationArealist = _base.Sp_GetApplicationArealist();
            ViewBag.ApplicationArea = ApplicationArealist;

            List<PhaseModel> Phaselist = _base.Sp_GetPhaselist();
            ViewBag.Phaselist = Phaselist;

            List<TaskTypeModel> TaskTypelist = _base.Sp_GetTaskTypelist();
            ViewBag.TaskTypelist = TaskTypelist;

            //List<ParallelTypeModel> ParallelTypelist = _base.Sp_GetParallelTypelist();
            //ViewBag.ParallelTypelist = ParallelTypelist;


            #region Scenario

            var res_scenarios = _base.Sp_Scenario();
            ViewBag.scenarioDetail = res_scenarios.ToList();
            ViewBag.ScenarioCount = res_scenarios.Count();

            List<Scenario> result6 = _base.Sp_GetScenario();
            ViewBag.Scenario = result6.ToList();


            List<int> Scenario_Status = _base.Sp_GetScenario_Status();
            ViewBag.Scenario_Status = Scenario_Status.ToList();

            #endregion


            return PartialView(name);
        }

        public ActionResult GetRoles()
        {
            List<UserModel> roles = _base.Sp_GetRoles();
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActivityList(Guid VersionID)
        {
            //List<ActivityModel> Activitylist = _base.Sp_GetActivityList(VersionID);
            Session["VersionID"] = VersionID;
            return Json(VersionID, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserSettings(string name)
        {
            List<CustomerModel> customers = _base.Sp_GetCustomerList();
            ViewBag.Customers = customers;

            var Customer_Count = customers.Count();
            ViewBag.Customer_Count = Customer_Count;

            List<UserModel> users = _base.Sp_GetUserList();
            ViewBag.Users = users;

            var User_Count = users.Count();
            ViewBag.User_Count = User_Count;

            List<UserModel> userType = _base.Sp_GetUserTypeList();
            ViewBag.UserType = userType;

            List<UserModel> roles = _base.Sp_GetRoles();
            ViewBag.Role = roles;

            List<CustomerModel> IndustrySector = _base.Sp_GetIndustrySectorList();
            ViewBag.IndustrySector = IndustrySector;

            return PartialView(name);
        }


        #region Scenario
        public JsonResult __Scenario(int id)
        {
            Scenario Scenario = _base.Sp_GetScenarioById(id);

            return Json(Scenario, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ScenarioCreation(string ScenarioName, string BuildingBlockIDs)
        {
            Scenario SC = new Scenario();
            
            SC.ScenarioName = ScenarioName.ToString();
            SC.isActive = true;
            SC.Cre_By = Guid.Parse(Session["loginid"].ToString());
            SC.BuildingBlock_Id = BuildingBlockIDs;

            

            var result = _base.SP_ScenarioCreation(SC);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult UpdateScenario(int Id, string ScenarioName,string BuildingBlockIDs)
        {
            Scenario SC = new Scenario();
            SC.ScenarioId = Id;
            SC.ScenarioName = ScenarioName;
            SC.BuildingBlock_Id = BuildingBlockIDs;
            SC.Modified_by = Guid.Parse(Session["loginid"].ToString());
            var result = _base.SP_UpdateScenario(SC);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        public ActionResult __UpdateScenario(int id)
        {
            Scenario SC = new Scenario();
            SC = _base.SP_GetScenariobyid(id);
            //ViewBag.Instance = Insta;
            return Json(SC, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteScenario(int ID)
        {
            Scenario Data = new Scenario();
            ////pm.PMTaskId = Guid.Parse(ID.ToString());
            Data.ScenarioId = ID;
            //SC.isActive = false;
            //SC.IsDeleted = true;

            Data.Modified_by = Guid.Parse(Session["loginid"].ToString());
            var result = _base.Sp_DeleteScenario(Data);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        public ActionResult CheckAvailability_ScenarioName(string ScenarioName, int ?ID)
        {
            var result = _base.SP_GetScenarioName(ScenarioName,ID);
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


        public ActionResult GetVersionList()
        {
            var Version = _base.Sp_GetVersion();
            var data = Version.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
            

        #region INSTANCE CREATION
        public ActionResult GetInstanceProject(Guid id)
        {
            var Projresult = _base.SP_GetProjectDrpDwnList(id);
            var data = Projresult.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectList()
        {
            var Projresult = _base.Sp_GetProjectList();
            var data = Projresult.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetCompanyName()
        {
            var Projresult = _base.Sp_GetCompany(); ;
            var data = Projresult.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _InstanceProject()
        {
            return PartialView();
        }

        public ActionResult _ScenarioIndex()
        {
            return PartialView();
        }

       
        [HttpPost]
        public ActionResult InstanceCreation(Instances insta)
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

                if (insta.Sys_Landscape == "Automation")
                {
                    RfcConfigParameters parms = new RfcConfigParameters();

                    parms.Add(RfcConfigParameters.Name, "G10");
                    parms.Add(RfcConfigParameters.AppServerHost, insta.AppServerHost);
                    parms.Add(RfcConfigParameters.SystemNumber, insta.SystemNumber);
                    //parms.Add(RfcConfigParameters.SystemID, "M03");
                    parms.Add(RfcConfigParameters.SAPRouter, insta.SAPRouter);
                    parms.Add(RfcConfigParameters.User, insta.User);
                    parms.Add(RfcConfigParameters.Password, insta.Password);
                    parms.Add(RfcConfigParameters.Client, insta.Client);
                    parms.Add(RfcConfigParameters.Language, "EN");
                    RfcDestination dest = RfcDestinationManager.GetDestination(parms);

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
                    for (int i = SAPDATA4.Rows.Count - 1; i >=0; i--)
                    {
                        if (SAPDATA4.Rows[i][1].ToString() == "")
                        {
                            SAPDATA4.Rows[i].Delete();
                        }
                    }
                    SAPDATA4.Rows[0].Delete();
                    SAPDATA4.AcceptChanges();

                }
                //return Json("success");
                insta.Cre_By = Session["loginid"].ToString();
                insta.isActive = true;
                var result = _base.Sp_InstaCreate(insta, SALESDATA, BILLSDATA, ORDERSDATA, FACOUNTDATA, SAPDATA1, SAPDATA2, SAPDATA3, SAPDATA4);
                if (result == true)
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
                return Content(ex.Message);
            }
            
        }

        //public ActionResult _UpdateInstance(Guid id)
        //{
        //    Instances Insta = null;
        //    Insta = _base.Sp_GetInstance(id);
        //    ViewBag.Instance = Insta;

        //    var ProjectName = _base.Sp_Project();
        //    ViewBag.ProjectName = ProjectName.ToList();
        //    return PartialView(Insta);

        //}

        public ActionResult __UpdateInstance(Guid id)
        {
            Instances Insta = null;
            Insta = _base.Sp_GetInstance(id);
            ViewBag.Instance = Insta;
            return Json(Insta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateInstance(Instances Insta)
        {
            //Instances Insta = new Instances();
            //Insta.Instance_id = Guid.Parse(ID.ToString());
            //Insta.InstanceName = Name;
            //Insta.Project_ID = Guid.Parse(ProjectID.ToString());
            //Insta.Description = Description;

            //Insta.isActive = true;
            //Insta.Modified_On = DateTime.UtcNow;
            Insta.Modified_by= Guid.Parse(Session["loginid"].ToString());
            var result = _base.SP_UpdateInstance(Insta);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult DeleteInstance(Guid id)
        {
            Instances insta = new Instances();
            insta.Instance_id = Guid.Parse(id.ToString());
            insta.isActive = false;
            insta.IsDeleted = true;
            var result = _base.Sp_Delete(insta);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        [HttpGet]
        public ActionResult Scenario_BuildingBlock(int id)
        {
            List<BuildingBlockModel> Building_Block = _base.Sp_GetScenario_BuildingBlock(id);
            string bb = " ";
            foreach (var item in Building_Block)
            {
                bb= bb+ item.Block_Name.ToString()+",";
            }
            return Json(bb,JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckInsatanceAvailabilty(string InstaName, Guid? ProjectId,Guid? InsId)
        {
            var result = _base.SP_GetInstaName(InstaName, ProjectId,InsId);
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

        #region InstanceClone
        public ActionResult GetInstancesByProject(Guid id)
        {
            var InstanceList = _base.SP_GetCloneInstance(id);
            return Json(InstanceList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateInstanceClone(string InstaceNameClone,Guid ProjectidClone,Guid InstanceClone,string  DescriptionClone)
        {

            Boolean Result = false;
            Instances Data = new Instances();
            Data.InstanceName = InstaceNameClone;
            Data.Project_ID = Guid.Parse(ProjectidClone.ToString());
            Data.Cre_By = Session["loginid"].ToString();
            Data.Description = DescriptionClone;
            Result = _base.Sp_InstanceClone(Data, InstanceClone);
            if (Result == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        
        #endregion

        #endregion

        #region PROJECT CREATION
        [HttpPost]
        public ActionResult CreateProject(string ProjectName, Guid CustomerName, int ScenerioName, Guid ProjectManagerName, string Description)
        {
            Project P = new Project();
            P.ProjectName = ProjectName;
            P.CustomerID = Guid.Parse(CustomerName.ToString());
            P.CreatedBy = Guid.Parse(Session["loginid"].ToString());
            P.ProjectManagerID = Guid.Parse(ProjectManagerName.ToString());
            P.ScenerioID = ScenerioName;
            P.Description = Description;
            P.IsActive = true;
            var result = _base.SP_CreateProject(P);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        //public ActionResult _UpdateProject(Guid id)
        //{
        //    Project Proj = null;
        //    Proj = _base.SP_GetProject(id);
        //    ViewBag.ProjectDetail = Proj;

        //    var result5 = _base.SP_GetProjectManager();
        //    ViewBag.ProjectManager = result5.ToList();

        //    var result6 = _base.Sp_GetScenario();
        //    ViewBag.Scenario = result6.ToList();

        //    var result7 = _base.Sp_GetCompany();
        //    ViewBag.Company = result7.ToList();

        //    return PartialView(Proj);
        //}
        public ActionResult __UpdateProject(Guid id)
        {
            Project Proj = null;
            Proj = _base.SP_GetProject(id);
            ViewBag.ProjectDetail = Proj;
            return Json(Proj, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateProject(Guid Id, string ProjectName, Guid CustomerName, int ScenerioName, Guid ProjectManagerName, string Description)
        {
            Project P = new Project();
            P.ProjectName = ProjectName;
            P.ProjectId = Guid.Parse(Id.ToString());
            P.CustomerID = Guid.Parse(CustomerName.ToString());
            P.ModifiedBy = Guid.Parse(Session["loginid"].ToString());
            P.ProjectManagerID = Guid.Parse(ProjectManagerName.ToString());
            P.ScenerioID = ScenerioName;
            P.Description = Description;
            P.IsActive = true;
            var result = _base.SP_UpdateProject(P);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult DeleteProject(Guid id)
        {
            int result = _base.Sp_ProjectDelete(id);

            return Json(result);
        }

        public ActionResult CheckProjectAvailabilty(string ProjectName,Guid? Id)
        {
            var result = _base.SP_GetProjName(ProjectName,Id);
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

        #region Role

        [HttpPost]
        public ActionResult CreateRole(string RoleName, string Description)
        {
            Role _role = new Role();
            _role.CreatedBy = Guid.Parse(Session["loginid"].ToString());
            _role.IsActive = true;
            _role.RoleName = RoleName;
            _role.Description = Description;

            bool result = _base.SP_CreateRole(_role);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }

        }

        public ActionResult _RoleIndex()
        {
            return PartialView("_RoleIndex");
        }

        //public ActionResult _UpdateRole(int id)
        //{
        //    Role Proj = null;
        //    Proj = _base.SP_GetRole(id);
        //    ViewBag.RoleDetail = Proj;
        //    return PartialView("_UpdateRole");

        //}


        public JsonResult __UpdateRole(int id)
        {
            Role Proj = null;
            Proj = _base.SP_GetRole(id);
            return Json(Proj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateRole(int Id, string RoleName, string Description,string TS)
        {
            Role _role = new Role();
            _role.RoleId = Convert.ToInt32(Id.ToString());
            _role.ModifiedBy = Guid.Parse(Session["loginid"].ToString());
            _role.IsActive = true;
            _role.RoleName = RoleName;
            _role.Description = Description;

            if (TS != "")
            {
                string[] abc = TS.Split(',');
                _role.TS = abc.Select(byte.Parse).ToArray();
            }

            bool result = _base.SP_UpdateRole(_role);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult DeleteRole(int id)
        {
            Role proj = new Role();
            proj.RoleId = Convert.ToInt32(id.ToString());
            proj.IsActive = false;
            proj.IsDelete = true;
            var result = _base.Sp_DeleteRole(proj);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }


        public ActionResult Checkavailabilty(string RoleName,int? RoleIdUpdt)
        {
            var result = _base.SP_GetRoleName(RoleName, RoleIdUpdt);
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

        #region PMTask

        public ActionResult _PMIndex()
        {
            return PartialView("_PMIndex");
        }

        public ActionResult GetPMList()
        {
            var PMresult = _base.Sp_GetMList();
            var dataPM = PMresult.ToList();


            return Json(dataPM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBlockList()
        {
            var BlockResult = _base.Sp_GetBuldingBlock();
            var dataBlock = BlockResult.ToList();
            return Json(dataBlock, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPMDetailById(Guid id)
        {
            PMTask ptask = _base.Sp_GetPMtaskById(id);
            ViewBag.PTM = ptask;
            return Json(ptask, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PmTaskCreation(decimal EstdHours, int TaskCategory, string TaskName)
        {
            PMTask PM = new PMTask();
            PM.EST_hours = Convert.ToDecimal(EstdHours.ToString());
            PM.isActive = true;
            PM.Cre_By = Guid.Parse(Session["loginid"].ToString());
            PM.PMTaskCategoryID = TaskCategory;
            PM.PMTaskName = TaskName;

            var result = _base.SP_CreatePM(PM);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }

        }


        //public ActionResult _UpdatePm(Guid id)
        //{
        //    PMTask ptask = _base.Sp_GetPMtaskById(id);
        //    ViewBag.PTM = ptask;

        //    List<PMTask> ptask2 = _base.Sp_GetTaskCategoryById();
        //    ViewBag.TaskList = ptask2;
        //    return PartialView("_UpdatePm");
        //}

        public JsonResult __UpdatePM(Guid id)
        {
            PMTask ptask = _base.Sp_GetPMtaskById(id);

            return Json(ptask, JsonRequestBehavior.AllowGet);
        }

        


        [HttpPost]
        public ActionResult UpdatePmTaskCreation(Guid id, decimal EstdHours, int TaskCategory, string TaskName,string TS)
        {
            PMTask PM = new PMTask();
            PM.PMTaskId = Guid.Parse(id.ToString());
            PM.EST_hours = Convert.ToDecimal(EstdHours.ToString());
            PM.isActive = true;
            PM.Modified_by = Guid.Parse(Session["loginid"].ToString());
            PM.PMTaskCategoryID = TaskCategory;
            PM.PMTaskName = TaskName;
            
            if (TS != "")
            {
                string[] abc = TS.Split(',');
                PM.TS = abc.Select(byte.Parse).ToArray();
            }

            var result = _base.SP_UpdatePM(PM);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }
        

            [HttpPost]
        public ActionResult DeletePm(Guid ID)
        {
            PMTask pm = new PMTask();
            pm.PMTaskId = Guid.Parse(ID.ToString());
            pm.isActive = false;
            pm.IsDeleted = true;
            var result = _base.Sp_DeletePM(pm);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }
        public ActionResult CheckTaskAvailabilty(string TaskName,Guid? Pid)
        {
            var result = _base.SP_GetTaskName(TaskName, Pid);
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

        #region Customer
       
        public ActionResult _CustomerIndex()
        {
            //List<CustomerModel> customers = _base.Sp_GetCustomerList();
            //ViewBag.Customers = customers;

            //List<CustomerModel> IndustrySector = _base.Sp_GetIndustrySectorList();
            //ViewBag.IndustrySector = IndustrySector;

            return PartialView("_CustomerIndex");
        }


        [HttpPost]
        public ActionResult Customer_Create_Update(CustomerModel customer,string TS)
        {
            try
            {
                //customer.TS= System.Text.Encoding.Unicode.GetBytes(TS);
                //customer.TS1= System.Text.Decoder.  .GetBytes(TS);
                //byte[] bytes = Convert.FromBase64String(TS);
                //byte[] rr = (byte[])TS.Clone();
                //if (BitConverter.IsLittleEndian)
                //{
                //    Array.Reverse(rr);
                //}
                //ulong a = BitConverter.ToUInt64(rr, 0);
                //customer.TS = a;
                // TS = TS.Replace(",", "");

                //byte[] bytes = System.Text.Encoding.ASCII.GetBytes(TS); //new byte[TS.Length]; 
                //customer.TS = bytes;
                //for (int i = 0; i < TS.Length; i++)
                //{
                //    bytes[i] = Convert.ToByte(TS[i]);
                //    customer.TS = bytes[i].
                //}
                //customer.TS = Convert.ToByte(TS);


                //customer.TS1 = TS;

                //customer.TS = TS.Split(',').Select(s => Convert.ToByte(s, 16)).ToArray();
                //customer.TS = Array.ConvertAll(abc, s => Convert.ToByte(s, 16));
                // customer.TS = Array.ConvertAll(abc, s =>Byte.Parse(s,System.Globalization.NumberStyles.HexNumber));

                if (TS != "")
                {
                    string[] abc = TS.Split(',');
                    customer.TS = abc.Select(byte.Parse).ToArray();
                }                

                customer.Cre_By = Guid.Parse(Session["loginid"].ToString());
                var Status = _base.Sp_Customer_Create_Update(customer);
                if (Status == true)
                    return Json("success", JsonRequestBehavior.AllowGet);
                else
                    return Json("error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "--> Customer_Create_Update" + Url);
                throw;
            }
        }

        public JsonResult CheckCustomersNameAvailability(string namedata, Guid? id)
        {
            if (id != null)
            {
                var SearchDt = _base.Sp_CheckCustomersNameAvailability(namedata, id);
                if (SearchDt != false)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var SearchDt = _base.Sp_CheckCustomersNameAvailability(namedata, id);
                if (SearchDt != true)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }

        }

        public ActionResult GetCustomerById(Guid id)
        {
            var Customer = _base.Sp_GetCustomerById(id);
            return Json(Customer, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCustomer(Guid id)
        {
            var Status = _base.Sp_DeleteCustomerById(id);
            if (Status == true)
                return Json("success", JsonRequestBehavior.AllowGet);
            else
                return Json("error", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region User

        public PartialViewResult _UserIndex()
        {
            //List<UserModel> users = _Base.Sp_GetUserList();
            //ViewBag.Users = users;

            //List<UserModel> userType = _Base.Sp_GetUserTypeList();
            //ViewBag.UserType = userType;

            //List<UserModel> roles = _Base.Sp_GetRoles();
            //ViewBag.Role = roles;

            //List<CustomerModel> customers = _Base.Sp_GetCustomerList();
            //ViewBag.Customer = customers;

            return PartialView("_UserIndex");
        }

        [HttpPost]
        public ActionResult User_Create_Update(UserModel userModel, string TS)
        {
            try
            {
                userModel.Cre_By = Guid.Parse(Session["loginid"].ToString());
                userModel.Password= _base.Encrypt(userModel.Password.ToString());
                if (TS != "")
                {
                    string[] abc = TS.Split(',');
                    userModel.TS = abc.Select(byte.Parse).ToArray();
                }

                var Status = _base.Sp_User_Create_Update(userModel);
                if (Status == true)
                    return Json("success", JsonRequestBehavior.AllowGet);
                else
                    return Json("error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "--> Customer_Create_Update" + Url);
                throw;
            }
        }

        public JsonResult NameAvailability(string namedata, Guid? id)
        {
            if (id != null)
            {
                var SearchDt = _base.Sp_NameAvailability(namedata, id);
                if (SearchDt != true)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var SearchDt = _base.Sp_NameAvailability(namedata, id);
                if (SearchDt != true)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult UsernameAvailability(string userdata, Guid? id)
        {
            if (id != null)
            {
                //var SearchDt = db.UserMasters.Where(x => x.LoginId == userdata).Where(x => x.UserId != id && x.isActive == true).FirstOrDefault();
                var SearchDt = _base.Sp_UsernameAvailability(userdata, id);
                if (SearchDt != true)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //var SearchDt = db.UserMasters.Where(x => x.LoginId == userdata).Where(x => x.isActive == true).FirstOrDefault();
                var SearchDt = _base.Sp_UsernameAvailability(userdata, id);
                if (SearchDt != true)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetUserById(Guid Id)
        {
            var User = _base.Sp_GetUserById(Id);
            User.Password = _base.Decrypt(User.Password.ToString());
            return Json(User, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUser(Guid id)
        {
            var Status = _base.Sp_DeleteUserById(id);
            if (Status == true)
                return Json("success", JsonRequestBehavior.AllowGet);
            else
                return Json("error", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Activity

        public PartialViewResult _ActivityIndex()
        {
            

            return PartialView("_ActivityIndex");
        }

        //public ActionResult ParallelNameByPhase(int id, int taskvalue, Guid instanceid,bool first)
        //{
        //    List<ParallelTypeModel> ParallelTypelist = _base.Sp_GetParallelTypelist(id, taskvalue, instanceid, first);
        //    return Json(ParallelTypelist, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetAllTaskByParallelType(int id, Guid Parallel_Id,Guid instanceid,bool first)
        //{
        //    List<ActivityModel> Activitylist = _base.Sp_GetTaskByParallelType(id, Parallel_Id, instanceid,first);
        //    return Json(Activitylist, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetAllTask(int id,Guid VID)
        {
            List<ActivityModel> Activitylist = _base.Sp_GetAllTask(id, VID);
            return Json(Activitylist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivityCreate(ActivityModel activityMaster)
        {
            try
            {
                activityMaster.Modified_by = Guid.Parse(Session["loginid"].ToString());
                _base.Activity_Master_Add_Update(activityMaster);

                return Json(activityMaster.Activity_ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _Log.createLog("Activity Create Update ==>" + ex);
                throw;
            }
        }

        //public JsonResult CheckParallel_NameAvailability(string namedata,Guid InstanceId)
        //{
        //    //var SearchDt = db.ParallelTypes.Where(x => x.Parallel_Name == namedata && x.isActive == true).FirstOrDefault();
        //    var SearchDt = _base.Sp_CheckParallel_NameAvailability(namedata, InstanceId);
        //    if (SearchDt == true)
        //    {
        //        return Json("error", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("success", JsonRequestBehavior.AllowGet);
        //    }

        //}

        [HttpGet]
        public ActionResult GetActivityCreationById(int? id)
        {
           
            var Activity = _base.Sp_GetActivityCreationById(id);
           
            return Json(Activity, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ActivityDelete(int id)
        {
            var Status = _base.Sp_DeleteActivityById(id);
            if (Status == true)
                return Json("success", JsonRequestBehavior.AllowGet);
            else
                return Json("error", JsonRequestBehavior.AllowGet);
        }


        public ActionResult exportToExcel( Guid VID)
        {
            var grdReport = new System.Web.UI.WebControls.GridView();
            grdReport.DataSource = _base.Sp_GetExcelActivityList(VID);
            grdReport.DataBind();

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grdReport.RenderControl(htw);

            byte[] bindata = System.Text.Encoding.ASCII.GetBytes(sw.ToString());
            return File(bindata, "application/ms-excel", "ActivityExcel.xlsx");
        }

        public ActionResult ActivityFileUpload()
        {
            
            object RS = null;
            try
            {
                var GivenName = "";
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        Base _base = new Base();
                        string fname, ext;
                        fname = file.FileName;
                        GivenName = Path.GetFileNameWithoutExtension(fname);
                        ext = Path.GetExtension(fname);
                        FileUpload _fileUpload = new FileUpload(); 
                        Guid User_Id = Guid.Parse(Session["loginid"].ToString());
                        //ext = ext.Replace(ext, ".jpg");

                        string NewId = Guid.NewGuid().ToString();
                        fname = NewId + ext;
                        string filePath = Server.MapPath(ConfigurationManager.AppSettings["Upload"].ToString());
                        _base.CreateIfMissing(filePath);

                        string fpath = Path.Combine(filePath, fname);
                        file.SaveAs(fpath);
                        
                        RS = _fileUpload.Process_ActivityMaster(fpath, NewId, User_Id);
                        TempData["Error"] = RS;
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Error" + ex.Message);
            }
            return Json(RS);
        }

        public ActionResult GetAllParallel()
        {
            List<ParallelType> ParallelTypelist = _base.Sp_GetAllParallelType();
            return Json(ParallelTypelist, JsonRequestBehavior.AllowGet);
        }
        //public  ActionResult ActivityTemplate()
        //{
        //    string file = "ActivityTemplate.xlsx";
        //    string File_Path = Server.MapPath(ConfigurationManager.AppSettings["MasterFilePath"].ToString());

        //    string fullPath = Path.Combine(File_Path, file);
        //    using (ExcelE)
        //    {

        //    }


        //    //string fullPath = Path.Combine(Server.MapPath("~/MyFiles"), file);
        //    return Json(new { fileName = file + ".pdf", errorMessage = "" });
        //}
        [HttpGet]
        public ActionResult ActivityTemplate()
        {
            string file = "ActivityTemplate.xlsx";
            string filePath = Server.MapPath(ConfigurationManager.AppSettings["MasterFilePath"].ToString());  //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\Uploadedppt\\Sample.pdf";//Server.MapPath("Content") + "Sample.pdf";

            string fullPath = Path.Combine(filePath, file);
            return File(fullPath, "application/ms-excel", file);
        }
        #endregion





        #region Audit

        public ActionResult _AuditIndex()
        {
            return PartialView("_AuditIndex");
        }

        #endregion


       
    }
}
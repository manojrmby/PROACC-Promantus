using PROACC2.Controllers;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using PROACC2.BL.Model;
using static PROACC2.BL.Model.Common;

namespace PROACC2.Controllers
{
    public class SAPController : Controller
    {
        // GET: SAP
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetData(Guid ins,int val)
        {
            ECCDestinationConfig cfg = new ECCDestinationConfig();
            try
            {

                RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                RfcDestination dest = RfcDestinationManager.GetDestination("ABAP_AS_WITH_POOL "+ins);

                RfcRepository repo = dest.Repository;


                IRfcFunction testfn1 = repo.CreateFunction("ZPROACC_SYSTEM_CONFIG_READ_V1");
                testfn1.SetValue("OPT", val);
                testfn1.Invoke(dest);
                var companyCodeList1 = testfn1.GetTable("IT_RESULT");
                DataTable SAPDATA = companyCodeList1.ToDataTable("ZPRO_STRUC1");

                for (int i = SAPDATA.Rows.Count - 1; i >= 0; i--)
                {
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
                    if (SAPDATA.Rows[i][1] == "")
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
                    {
                        SAPDATA.Rows[i].Delete();
                    }
                }
                SAPDATA.AcceptChanges();

                SAPVM VmObj = new SAPVM();
                List<SAPCls> SAPLIST = new List<SAPCls>();
                foreach (DataRow dr in SAPDATA.Rows)
                {
                    SAPCls listItem = new SAPCls();
                    listItem.FIELD1 = Convert.ToString(dr["FIELD1"]);
                    listItem.FIELD2 = Convert.ToString(dr["FIELD2"]);
                    listItem.FIELD3 = Convert.ToString(dr["FIELD3"]);
                    listItem.FIELD4 = Convert.ToString(dr["FIELD4"]);
                    listItem.FIELD5 = Convert.ToString(dr["FIELD5"]);
                    listItem.FIELD6 = Convert.ToString(dr["FIELD6"]);
                    SAPLIST.Add(listItem);
                }

                //for (int i = dt.Rows.Count - 1; i >= 0; i--)
                //{
                //    if (dt.Rows[i][1] == DBNull.Value)
                //    {
                //        dt.Rows[i].Delete();
                //    }
                //}
                //dt.AcceptChanges();
                //return dt;

                VmObj.ZPROACC = SAPLIST;


                //return View("SAPTest", VmObj);
                return Json(SAPLIST, JsonRequestBehavior.AllowGet);
                // companyCodeList now contains a table with companies and codes
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

        [HttpGet]
        public ActionResult GetBillingDocumentsData(Guid ins)
        {
            ECCDestinationConfig cfg = new ECCDestinationConfig();
            try
            {

                RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                RfcDestination dest = RfcDestinationManager.GetDestination("ABAP_AS_WITH_POOL " + ins);

                RfcRepository repo = dest.Repository;

                IRfcFunction testfn1 = repo.CreateFunction("ZPROACC_ENT_STRUC_MINING_OTC");
                //testfn1.SetValue("DATE_FROM", "20150512");
                testfn1.Invoke(dest);
                var companyCodeList1 = testfn1.GetTable("I_BILLS");
                DataTable SAPDATA = companyCodeList1.ToDataTable("ZPRO_STRUC1");

                List<RFCFMReport> SAPLIST = new List<RFCFMReport>();

                foreach (DataRow dr in SAPDATA.Rows)
                {
                    RFCFMReport listItem = new RFCFMReport();
                    listItem.CompanyCode = Convert.ToString(dr["COMPANY_CODE"]);
                    listItem.Currency = Convert.ToString(dr["CURRENCY"]);
                    listItem.FKREL = Convert.ToString(dr["BILL_CATEGORY"]);
                    listItem.BillingCreatedBy = Convert.ToString(dr["CREATED_BY"]);
                    listItem.NoofDocuments = Convert.ToString(dr["DOC_COUNT"]);                    
                    SAPLIST.Add(listItem);
                }

                return Json(SAPLIST, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult GetOrderDocumentsData(Guid ins)
        {
            ECCDestinationConfig cfg = new ECCDestinationConfig();
            try
            {

                RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                RfcDestination dest = RfcDestinationManager.GetDestination("ABAP_AS_WITH_POOL " + ins);

                RfcRepository repo = dest.Repository;

                IRfcFunction testfn1 = repo.CreateFunction("ZPROACC_ENT_STRUC_MINING_OTC");
                //testfn1.SetValue("DATE_FROM", "20150512");
                testfn1.Invoke(dest);
                var companyCodeList1 = testfn1.GetTable("I_ORDERS");
                DataTable SAPDATA = companyCodeList1.ToDataTable("ZPRO_STRUC1");

                List<RFCFMReport> SAPLIST = new List<RFCFMReport>();

                foreach (DataRow dr in SAPDATA.Rows)
                {
                    RFCFMReport listItem = new RFCFMReport();
                    listItem.ControllingArea = Convert.ToString(dr["CONTROLLING_AREA"]);
                    listItem.CCbilled = Convert.ToString(dr["COMPANY_CODE"]);
                    listItem.Plant = Convert.ToString(dr["PLANT"]);
                    listItem.DocCategory = Convert.ToString(dr["DOC_CATEGORY"]);
                    listItem.NoofDocuments = Convert.ToString(dr["DOC_COUNT"]);
                    SAPLIST.Add(listItem);
                }

                return Json(SAPLIST, JsonRequestBehavior.AllowGet);

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

        
        [HttpGet]
        public ActionResult GetSalesDocumentsData(Guid ins)
        {
            ECCDestinationConfig cfg = new ECCDestinationConfig();
            try
            {

                RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                RfcDestination dest = RfcDestinationManager.GetDestination("ABAP_AS_WITH_POOL " + ins);

                RfcRepository repo = dest.Repository;

                IRfcFunction testfn1 = repo.CreateFunction("ZPROACC_ENT_STRUC_MINING_OTC");
                //testfn1.SetValue("DATE_FROM", "20150512");
                testfn1.Invoke(dest);
                var companyCodeList1 = testfn1.GetTable("I_SALES");
                DataTable SAPDATA = companyCodeList1.ToDataTable("ZPRO_STRUC1");

                List<RFCFMReport> SAPLIST = new List<RFCFMReport>();

                foreach (DataRow dr in SAPDATA.Rows)
                {
                    RFCFMReport listItem = new RFCFMReport();
                    listItem.CCbilled = Convert.ToString(dr["COMPANY_CODE"]);
                    listItem.SalesOrg = Convert.ToString(dr["SALES_ORG"]);
                    listItem.DistChannel = Convert.ToString(dr["DIST_CHANNEL"]);
                    listItem.Division = Convert.ToString(dr["DIVISION"]);
                    listItem.Plant = Convert.ToString(dr["PLANT"]);
                    listItem.NoofDocuments = Convert.ToString(dr["DOC_COUNT"]);
                    SAPLIST.Add(listItem);
                }

                return Json(SAPLIST, JsonRequestBehavior.AllowGet);

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

        [HttpGet]
        public ActionResult GetMaterialityScoreData(Guid ins)
        {
            ECCDestinationConfig cfg = new ECCDestinationConfig();
            try
            {
                RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                RfcDestination dest = RfcDestinationManager.GetDestination("ABAP_AS_WITH_POOL " + ins);

                RfcRepository repo = dest.Repository;

                IRfcFunction testfn1 = repo.CreateFunction("ZPROACC_MAT_SCORE");
                //testfn1.SetValue("DATE_FROM", "20200512");
                testfn1.Invoke(dest);
                var companyCodeList1 = testfn1.GetTable("I_FACOUNT");
                DataTable SAPDATA = companyCodeList1.ToDataTable("ZPRO_STRUC1");

                List<RFCFMReport> SAPLIST = new List<RFCFMReport>();

                foreach (DataRow dr in SAPDATA.Rows)
                {
                    RFCFMReport listItem = new RFCFMReport();
                    listItem.FunctionalArea = Convert.ToString(dr["FUNC_AREA"]);
                    listItem.Count = Convert.ToString(dr["FA_COUNT"]);
                    listItem.Percentage = Convert.ToString(dr["FA_PERCENT"]);
                    
                    SAPLIST.Add(listItem);
                }

                return Json(SAPLIST, JsonRequestBehavior.AllowGet);

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

        
        public ActionResult GetSAPBreakdownbyLob_Bar(Guid ins)
        {
            ECCDestinationConfig cfg = new ECCDestinationConfig();
            try
            {
                RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                RfcDestination dest = RfcDestinationManager.GetDestination("ABAP_AS_WITH_POOL " + ins);

                RfcRepository repo = dest.Repository;

                IRfcFunction testfn1 = repo.CreateFunction("ZPROACC_MAT_SCORE");
                //testfn1.SetValue("DATE_FROM", "20200512");
                testfn1.Invoke(dest);
                var companyCodeList1 = testfn1.GetTable("I_FACOUNT");
                DataTable SAPDATA = companyCodeList1.ToDataTable("ZPRO_STRUC1");

                List<RFCFMReport> SAPLIST = new List<RFCFMReport>();
                GeneralList sP_ = new GeneralList();
                List<Lis> _Lob = new List<Lis>();
                foreach (DataRow dr in SAPDATA.Rows)
                {
                    _Lob.Add(
                        new Lis
                        {
                            Name = Convert.ToString(dr["FUNC_AREA"]),
                            _Value = Convert.ToInt32(dr["FA_COUNT"]),
                            percent = Convert.ToString(dr["FA_PERCENT"])
                        });
                }
                sP_._List = _Lob;

                //foreach (DataRow dr in SAPDATA.Rows)
                //{
                //    RFCFMReport listItem = new RFCFMReport();
                //    listItem.Name = Convert.ToString(dr["FUNC_AREA"]);
                //    listItem.Count = Convert.ToString(dr["FA_COUNT"]);
                //    listItem.percent = Convert.ToString(dr["FA_PERCENT"]);
                //    SAPLIST.Add(listItem);
                //}

                return Json(sP_, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
            finally
            {
                RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
            }
        }

        public ActionResult TestSAPConnection(Instances instances)
        {
            ECCDestinationConfig cfg = new ECCDestinationConfig();
            try
            {
                //RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                RfcConfigParameters parms = new RfcConfigParameters();

                parms.Add(RfcConfigParameters.Name, "Test");
                parms.Add(RfcConfigParameters.AppServerHost, instances.AppServerHost);
                parms.Add(RfcConfigParameters.SystemNumber, instances.SystemNumber);
                //parms.Add(RfcConfigParameters.SystemID, "M03");
                parms.Add(RfcConfigParameters.SAPRouter, instances.SAPRouter);
                parms.Add(RfcConfigParameters.User, instances.User);
                parms.Add(RfcConfigParameters.Password, instances.Password);
                parms.Add(RfcConfigParameters.Client, instances.Client);
                parms.Add(RfcConfigParameters.Language, "EN");
                //parms.Add(RfcConfigParameters.Language, instances.Language);
                //parms.Add(RfcConfigParameters.PoolSize, "5");

                RfcDestination dest = RfcDestinationManager.GetDestination(parms);

                dest.Ping();
                //return Json("success", JsonRequestBehavior.AllowGet);

                RfcRepository repo = dest.Repository;

                IRfcFunction testfn1 = repo.CreateFunction("ZPROACC_SYSTEM_CONFIG_READ_V1");
                testfn1.SetValue("OPT", 1);
                testfn1.Invoke(dest);
                var companyCodeList1 = testfn1.GetTable("IT_RESULT");
                DataTable SAPDATA = companyCodeList1.ToDataTable("ZPRO_STRUC1");

                SAPVM VmObj = new SAPVM();
                List<SAPCls> SAPLIST = new List<SAPCls>();
                if (SAPDATA.Rows.Count > 0)
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
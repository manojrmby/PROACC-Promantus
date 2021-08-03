using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class RFCFMReport
    {
        public double S_No { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
        public string Component { get; set; }
        public string Release { get; set; }
        public string SP_Level { get; set; }
        public string Comp_Type { get; set; }
        public string Description { get; set; }
        public string Product { get; set; }
        public string SP_Stack { get; set; }
        public string Vendor { get; set; }
        public string Group { get; set; }
        public string BF_Name { get; set; }
        public string Dependency { get; set; }
        public string CompanyCode { get; set; }
        public string Currency { get; set; }
        public string FKREL { get; set; }
        public string BillingCreatedBy { get; set; }
        public string NoofDocuments { get; set; }
        public string ControllingArea { get; set; }
        public string CCbilled { get; set; }
        public string Plant { get; set; }
        public string DocCategory { get; set; }
        // Complexity Analysis
        public string NewGL { get; set; }
        public string Leadingledger { get; set; }
        public string SpecialPurposeLedger { get; set; }
        public string Treasury_SWF5_FSCM_CLM { get; set; }
        public string Treasury_SWF5_FSCM_BNK { get; set; }
        public string Multicurrencymodel { get; set; }
        public string NewAssetAccounting { get; set; }
        public string BusinessPartner { get; set; }
        public string BPActive { get; set; }
        public string MaterialLedger { get; set; }
        public string Active { get; set; }
        public string SalesOrg { get; set; }
        public string DistChannel { get; set; }
        public string Division { get; set; }
        public string FunctionalArea { get; set; }
        public string Count { get; set; }
        public string Percentage { get; set; }
        public string SAPECC { get; set; }
        public string NetWeaver { get; set; }
        public string TypeVersion { get; set; }
        public string Database { get; set; }
        public string OS { get; set; }


        //ECC S4 Hana Count
        public string User { get; set; }
        public string UserType { get; set; }
        public string UserCategory { get; set; }
        public string User_Status { get; set; }
        public string System { get; set; }
        public string Last_Logon { get; set; }
    }
}
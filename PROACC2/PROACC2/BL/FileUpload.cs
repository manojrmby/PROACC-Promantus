using ClosedXML.Excel;
using PROACC2.BL.General;
using PROACC2.BL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PROACC2.BL
{
    public class FileUpload
    {
        Base _Base = new Base();

        #region Create Analysis
        public Boolean Process_Activities(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Related Simplification Items").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Activities").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Phase").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Condition").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Additional Information").GetString()).ToArray()
                 };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_Activities(dataList);
                Status = _Base.Upload_Activities(dataTable, fileName, Instance_ID, User_Id);
                //To get unique column values, to avoid duplication
                //var uniqueCols = dataTable.DefaultView.ToTable(true, "Solution Number");

                ////Remove Empty Rows or any specify rows as per your requirement
                //for (var i = uniqueCols.Rows.Count - 1; i >= 0; i--)
                //{
                //    var dr = uniqueCols.Rows[i];
                //    if (dr != null && ((string)dr["Solution Number"] == "None" || (string)dr["Title"] == ""))
                //        dr.Delete();
                //}
                //Console.WriteLine("Total number of unique solution number in Excel : " + uniqueCols.Rows.Count);
            }


            return Status;
        }
        private static DataTable ConvertListToDataTable_Activities(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("Activities");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Related Simplification Items");
            table.Columns.Add("Activities");
            table.Columns.Add("Phase");
            table.Columns.Add("Condition");
            table.Columns.Add("Additional Information");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Related Simplification Items"] = list[0][j];
                row["Activities"] = list[1][j];
                row["Phase"] = list[2][j];
                row["Condition"] = list[3][j];
                row["Additional Information"] = list[4][j];
                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_Bwextractors(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                     table.DataRange.Rows().Select(tableRow =>tableRow.Field("Extractor Name").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Application Area").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Related Simplification Items").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Additional Information").GetString()).ToArray(),
                    //table.DataRange.Rows().Select(tableRow => tableRow.Field("Additional Information").GetString()).ToArray()
                 };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_Bwextractors(dataList);
                Status = _Base.Upload_Bwextractors(dataTable, fileName, Instance_ID, User_Id);
            }


            return Status;
        }
        private static DataTable ConvertListToDataTable_Bwextractors(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("Bwextractors");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Extractor Name");
            table.Columns.Add("Application Area");
            table.Columns.Add("Related Simplification Items");
            table.Columns.Add("Additional Information");
            //table.Columns.Add("Additional Information");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Extractor Name"] = list[0][j];
                row["Application Area"] = list[1][j];
                row["Related Simplification Items"] = list[2][j];
                row["Additional Information"] = list[3][j];
                //row["Additional Information"] = list[4][j];
                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_CustomCode(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                     table.DataRange.Rows().Select(tableRow =>tableRow.Field("Custom Code Topic").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Status").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Application Component").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Custom Code Note").GetString()).ToArray(),
                    //table.DataRange.Rows().Select(tableRow => tableRow.Field("Additional Information").GetString()).ToArray()
                 };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_CustomCode(dataList);
                Status = _Base.Upload_CustomCode(dataTable, fileName, Instance_ID, User_Id);
                //To get unique column values, to avoid duplication
                //var uniqueCols = dataTable.DefaultView.ToTable(true, "Solution Number");

                ////Remove Empty Rows or any specify rows as per your requirement
                //for (var i = uniqueCols.Rows.Count - 1; i >= 0; i--)
                //{
                //    var dr = uniqueCols.Rows[i];
                //    if (dr != null && ((string)dr["Solution Number"] == "None" || (string)dr["Title"] == ""))
                //        dr.Delete();
                //}
                //Console.WriteLine("Total number of unique solution number in Excel : " + uniqueCols.Rows.Count);
            }


            return Status;
        }
        private static DataTable ConvertListToDataTable_CustomCode(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("CustomCode");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Custom Code Topic");
            table.Columns.Add("Status");
            table.Columns.Add("Application Component");
            table.Columns.Add("Custom Code Note");
            //table.Columns.Add("Additional Information");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Custom Code Topic"] = list[0][j];
                row["Status"] = list[1][j];
                row["Application Component"] = list[2][j];
                row["Custom Code Note"] = list[3][j];
                //row["Additional Information"] = list[4][j];
                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_HanaDatabaseTables(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                     table.DataRange.Rows().Select(tableRow =>tableRow.Field("Name").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Store Type").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Data Size in GB").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Estimated Number of Records").GetString()).ToArray()
                 };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_HanaDatabase(dataList);
                Status = _Base.Upload_HanaDatabase(dataTable, fileName, Instance_ID, User_Id);
                
            }


            return Status;
        }
        private static DataTable ConvertListToDataTable_HanaDatabase(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("HanaDatabase");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Name");
            table.Columns.Add("Store Type");
            table.Columns.Add("Data Size in GB");
            table.Columns.Add("Estimated Number of Records");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Name"] = list[0][j];
                row["Store Type"] = list[1][j];
                row["Data Size in GB"] = list[2][j];
                row["Estimated Number of Records"] = list[3][j];
                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_FioriApps(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                     table.DataRange.Rows().Select(tableRow =>tableRow.Field("Role").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Name").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Application Area").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Description").GetString()).ToArray(),
                    //table.DataRange.Rows().Select(tableRow => tableRow.Field("Additional Information").GetString()).ToArray()
                 };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_FioriApps(dataList);
                Status = _Base.Upload_FioriApps(dataTable, fileName, Instance_ID, User_Id);
                //To get unique column values, to avoid duplication
                //var uniqueCols = dataTable.DefaultView.ToTable(true, "Solution Number");

                ////Remove Empty Rows or any specify rows as per your requirement
                //for (var i = uniqueCols.Rows.Count - 1; i >= 0; i--)
                //{
                //    var dr = uniqueCols.Rows[i];
                //    if (dr != null && ((string)dr["Solution Number"] == "None" || (string)dr["Title"] == ""))
                //        dr.Delete();
                //}
                //Console.WriteLine("Total number of unique solution number in Excel : " + uniqueCols.Rows.Count);
            }


            return Status;
        }
        private static DataTable ConvertListToDataTable_FioriApps(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("FioriApps");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Role");
            table.Columns.Add("Name");
            table.Columns.Add("Application Area");
            table.Columns.Add("Description");
            //table.Columns.Add("Additional Information");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Role"] = list[0][j];
                row["Name"] = list[1][j];
                row["Application Area"] = list[2][j];
                row["Description"] = list[3][j];
                //row["Additional Information"] = list[4][j];
                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_Simplification(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                     table.DataRange.Rows().Select(tableRow =>tableRow.Field("Title").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Category").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Relevance").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("LoB/Technology").GetString()).ToArray(),

                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Business Area").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Consistency Status").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Manual Status").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("SAP Notes").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Relevance Summary").GetString()).ToArray(),
                    //table.DataRange.Rows().Select(tableRow => tableRow.Field("Additional Information").GetString()).ToArray()
                 };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_Simplification(dataList);
                Status = _Base.Upload_Simplification(dataTable, fileName, Instance_ID, User_Id);
                //To get unique column values, to avoid duplication
                //var uniqueCols = dataTable.DefaultView.ToTable(true, "Solution Number");

                ////Remove Empty Rows or any specify rows as per your requirement
                //for (var i = uniqueCols.Rows.Count - 1; i >= 0; i--)
                //{
                //    var dr = uniqueCols.Rows[i];
                //    if (dr != null && ((string)dr["Solution Number"] == "None" || (string)dr["Title"] == ""))
                //        dr.Delete();
                //}
                //Console.WriteLine("Total number of unique solution number in Excel : " + uniqueCols.Rows.Count);
            }


            return Status;
        }
        private static DataTable ConvertListToDataTable_Simplification(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("Simplification");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Title");
            table.Columns.Add("Category");
            table.Columns.Add("Relevance");
            table.Columns.Add("LoB/Technology");
            table.Columns.Add("Business Area");
            table.Columns.Add("Consistency Status");
            table.Columns.Add("Manual Status");
            table.Columns.Add("SAP Notes");
            table.Columns.Add("Relevance Summary");


            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Title"] = list[0][j];
                row["Category"] = list[1][j];
                row["Relevance"] = list[2][j];
                row["LoB/Technology"] = list[3][j];
                row["Business Area"] = list[4][j];
                row["Consistency Status"] = list[5][j];
                row["Manual Status"] = list[6][j];
                row["SAP Notes"] = list[7][j];
                row["Relevance Summary"] = list[8][j];

                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_SAPUserList(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                     table.DataRange.Rows().Select(tableRow =>tableRow.Field("User").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("User Type").GetString()).ToArray(),
                     table.DataRange.Rows().Select(tableRow => tableRow.Field("Valid from").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Valid through").GetString()).ToArray(),

                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Date of Last Logon").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("System").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Catergory").GetString()).ToArray()
                 };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_SAPUserList(dataList);
                Status = _Base.Upload_SAPUserList(dataTable, fileName, Instance_ID, User_Id);
            }

            return Status;
        }
        private static DataTable ConvertListToDataTable_SAPUserList(IReadOnlyList<string[]> list)
        {
            LogHelper _logHelper = new LogHelper();
            var table = new DataTable("UserList");
            try
            {                
                var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

                table.Columns.Add("User");
                table.Columns.Add("User Type");
                table.Columns.Add("Valid from", typeof(DateTime));
                table.Columns.Add("Valid through", typeof(DateTime));
                table.Columns.Add("Date of Last Logon");
                table.Columns.Add("System");
                table.Columns.Add("Catergory");

                for (var j = 0; j < rows; j++)
                {
                    var row = table.NewRow();
                    row["User"] = list[0][j];
                    row["User Type"] = list[1][j];
                    if (list[2][j] == "")
                    {
                        row["Valid from"] = DBNull.Value;
                    }
                    else
                    {
                        row["Valid from"] = list[2][j];
                    }
                    if (list[3][j] == "")
                    {
                        row["Valid through"] = DBNull.Value;
                    }
                    else
                    {
                        row["Valid through"] = list[3][j];
                    }                   
                                        
                   
                    //_logHelper.createLog("Valid from---" + row["Valid from"]);
                    //_logHelper.createLog("Valid through---" + row["Valid through"]);
                    row["Date of Last Logon"] = list[4][j];
                    row["System"] = list[5][j];
                    row["Catergory"] = list[6][j];

                    table.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                _logHelper.createLog("ConvertListToDataTable_SAPUserList Failed..." + ex.Message);
            }
            
            return table;
        }
        #endregion

        #region SAPFileUpload
        public Boolean Process_RFC_FM_Output(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Parameter").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Value").GetString()).ToArray(),
                    
                 };

                //2nd Sheet
                workSheet = workbook.Worksheet(2);
                firstRowUsed = workSheet.FirstRowUsed();
                firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                lastPossibleAddress = workSheet.LastCellUsed().Address;
                range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList_Sheet2 = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Component").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Release").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("SP Level").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Comp.Type").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Description").GetString()).ToArray(),

                 };

                //3rd Sheet
                workSheet = workbook.Worksheet(3);
                firstRowUsed = workSheet.FirstRowUsed();
                firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                lastPossibleAddress = workSheet.LastCellUsed().Address;
                range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                              // Treat the range as a table (to be able to use the column names)
                table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList_Sheet3 = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Product").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Release").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("SP Stack").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Vendor").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Description").GetString()).ToArray(),

                 };

                //4th Sheet
                workSheet = workbook.Worksheet(4);
                firstRowUsed = workSheet.FirstRowUsed();
                firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                lastPossibleAddress = workSheet.LastCellUsed().Address;
                range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                              // Treat the range as a table (to be able to use the column names)
                table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList_Sheet4 = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Group").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("BF Name").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("BF Description").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Dependency").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("SW Component").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("SW Release").GetString()).ToArray(),

                 };

                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_Process_RFC_FM_Output(dataList);
                DataTable dataTable_Sheet2 = ConvertListToDataTable_Process_INSTALLEDSOFTWARECOMPONENT(dataList_Sheet2);
                DataTable dataTable_Sheet3 = ConvertListToDataTable_Process_INSTALLEDPRODUCTVERSIONS(dataList_Sheet3);
                DataTable dataTable_Sheet4 = ConvertListToDataTable_SFW5REPORT(dataList_Sheet4);

                Status = _Base.Upload_RFC_FM_Output(dataTable, dataTable_Sheet2, dataTable_Sheet3, dataTable_Sheet4, fileName, Instance_ID, User_Id);
               
            }


            return Status;
        }
        private static DataTable ConvertListToDataTable_Process_RFC_FM_Output(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("RFC_FM_Output");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Parameter");
            table.Columns.Add("Value");
            

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Parameter"] = list[0][j];
                row["Value"] = list[1][j];
               
                table.Rows.Add(row);
            }
            return table;
        }

        private static DataTable ConvertListToDataTable_Process_INSTALLEDSOFTWARECOMPONENT(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("INSTALLEDSOFTWARECOMPONENT");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Component");
            table.Columns.Add("Release");
            table.Columns.Add("SP_Level");
            table.Columns.Add("Comp_Type");
            table.Columns.Add("Description");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Component"] = list[0][j];
                row["Release"] = list[1][j];
                row["SP_Level"] = list[2][j];
                row["Comp_Type"] = list[3][j];
                row["Description"] = list[4][j];

                table.Rows.Add(row);
            }
            return table;
        }

        private static DataTable ConvertListToDataTable_Process_INSTALLEDPRODUCTVERSIONS(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("INSTALLEDSOFTWARECOMPONENT");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Product");
            table.Columns.Add("Release");
            table.Columns.Add("SP_Stack");
            table.Columns.Add("Vendor");
            table.Columns.Add("Description");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Product"] = list[0][j];
                row["Release"] = list[1][j];
                row["SP_Stack"] = list[2][j];
                row["Vendor"] = list[3][j];
                row["Description"] = list[4][j];

                table.Rows.Add(row);
            }
            return table;
        }

        private static DataTable ConvertListToDataTable_SFW5REPORT(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("INSTALLEDSOFTWARECOMPONENT");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Group");
            table.Columns.Add("BF_Name");
            table.Columns.Add("BF_Description");
            table.Columns.Add("Dependency");
            table.Columns.Add("SW_Component");
            table.Columns.Add("SW_Release");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Group"] = list[0][j];
                row["BF_Name"] = list[1][j];
                row["BF_Description"] = list[2][j];
                row["Dependency"] = list[3][j];
                row["SW_Component"] = list[4][j];
                row["SW_Release"] = list[5][j];

                table.Rows.Add(row);
            }
            return table;
        }


        public Boolean Process_BillingDocuments(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Company Code (Billing)").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Currency").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("FKREL - Relevant for Billing").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Billing Created by").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("No.of Documents").GetString()).ToArray(),

                 };

                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_BillingDocuments(dataList);
                Status = _Base.Upload_BillingDocuments(dataTable, fileName, Instance_ID, User_Id);

            }

            return Status;
        }

        private static DataTable ConvertListToDataTable_BillingDocuments(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("BillingDocuments");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Company Code (Billing)");
            table.Columns.Add("Currency");
            table.Columns.Add("FKREL - Relevant for Billing");
            table.Columns.Add("Billing Created by");
            table.Columns.Add("No.of Documents");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Company Code (Billing)"] = list[0][j];
                row["Currency"] = list[1][j];
                row["FKREL - Relevant for Billing"] = list[2][j];
                row["Billing Created by"] = list[3][j];                
                row["No.of Documents"] = list[4][j];
                

                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_OrderDocuments(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Controlling Area").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("CC to be billed").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Plant").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Doc.Category").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("No.of Documents").GetString()).ToArray(),

                 };

                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_OrderDocuments(dataList);
                Status = _Base.Upload_OrderDocuments(dataTable, fileName, Instance_ID, User_Id);

            }

            return Status;
        }

        private static DataTable ConvertListToDataTable_OrderDocuments(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("OrderDocuments");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Controlling Area");
            table.Columns.Add("CC to be billed");
            table.Columns.Add("Plant");
            table.Columns.Add("Doc.Category");
            table.Columns.Add("No.of Documents");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Controlling Area"] = list[0][j];
                row["CC to be billed"] = list[1][j];
                row["Plant"] = list[2][j];
                row["Doc.Category"] = list[3][j];
                row["No.of Documents"] = list[4][j];

                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_SalesDocuments(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("CC to be billed").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Sales Org.").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Dist.Channel").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Division").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Plant").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("No.of Documents").GetString()).ToArray(),

                 };

                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_SalesDocuments(dataList);
                Status = _Base.Upload_SalesDocuments(dataTable, fileName, Instance_ID, User_Id);

            }

            return Status;
        }

        private static DataTable ConvertListToDataTable_SalesDocuments(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("SalesDocuments");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("CC to be billed");
            table.Columns.Add("Sales Org.");
            table.Columns.Add("Dist.Channel");
            table.Columns.Add("Division");
            table.Columns.Add("Plant");
            table.Columns.Add("No.of Documents");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["CC to be billed"] = list[0][j];
                row["Sales Org."] = list[1][j];
                row["Dist.Channel"] = list[2][j];
                row["Division"] = list[3][j];
                row["Plant"] = list[4][j];
                row["No.of Documents"] = list[5][j];

                table.Rows.Add(row);
            }
            return table;
        }
        public Boolean Process_ComplexityAnalysis(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Company Code").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("New G/L (Active)").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Leading ledger 0L Active").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Special Purpose Ledger").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Treasury (SWF5_FSCM_CLM)").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Treasury (SWF5_FSCM_BNK)").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Multi-currency model").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("New Asset Accounting").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Business Partner").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("BP Active").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Material Ledger").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Active").GetString()).ToArray()

                 };

                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_ComplexityAnalysis(dataList);
                Status = _Base.Upload_ComplexityAnalysis(dataTable, fileName, Instance_ID, User_Id);

            }

            return Status;
        }
        private static DataTable ConvertListToDataTable_ComplexityAnalysis(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("ComplexityAnalysis");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Company Code");
            table.Columns.Add("New G/L (Active)");
            table.Columns.Add("Leading ledger 0L Active");
            table.Columns.Add("Special Purpose Ledger");
            table.Columns.Add("Treasury (SWF5_FSCM_CLM)");
            table.Columns.Add("Treasury (SWF5_FSCM_BNK)");
            table.Columns.Add("Multi-currency model");
            table.Columns.Add("New Asset Accounting");
            table.Columns.Add("Business Partner");
            table.Columns.Add("BP Active");
            table.Columns.Add("Material Ledger");
            table.Columns.Add("Active");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Company Code"] = list[0][j];
                row["New G/L (Active)"] = list[1][j];
                row["Leading ledger 0L Active"] = list[2][j];
                row["Special Purpose Ledger"] = list[3][j];
                row["Treasury (SWF5_FSCM_CLM)"] = list[4][j];
                row["Treasury (SWF5_FSCM_BNK)"] = list[5][j];
                row["Multi-currency model"] = list[6][j];
                row["New Asset Accounting"] = list[7][j];
                row["Business Partner"] = list[8][j];
                row["BP Active"] = list[9][j];
                row["Material Ledger"] = list[10][j];
                row["Active"] = list[11][j];

                table.Rows.Add(row);
            }
            return table;
        }

        public Boolean Process_MaterialityScore(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Functional Area").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Count").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Percentage").GetString()).ToArray(),
                 };

                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_MaterialityScore(dataList);
                Status = _Base.Upload_MaterialityScore(dataTable, fileName, Instance_ID, User_Id);

            }

            return Status;
        }
        private static DataTable ConvertListToDataTable_MaterialityScore(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("MaterialityScore");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Functional Area");
            table.Columns.Add("Count");
            table.Columns.Add("Percentage");
            

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Functional Area"] = list[0][j];
                row["Count"] = list[1][j];
                row["Percentage"] = list[2][j];

                table.Rows.Add(row);
            }
            return table;
        }


        public Boolean Process_PurchaseDocuments(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Controlling Area").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("CC to be billed").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Plant").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Doc.Category").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("No.of Documents").GetString()).ToArray(),

                 };

                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_PurchaseDocuments(dataList);
                //Status = _Base.Upload_PurchaseDocuments(dataTable, fileName, Instance_ID, User_Id);

            }

            return Status;
        }

        private static DataTable ConvertListToDataTable_PurchaseDocuments(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("PurchaseDocuments");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("Controlling Area");
            table.Columns.Add("CC to be billed");
            table.Columns.Add("Plant");
            table.Columns.Add("Doc.Category");
            table.Columns.Add("No.of Documents");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Controlling Area"] = list[0][j];
                row["CC to be billed"] = list[1][j];
                row["Plant"] = list[2][j];
                row["Doc.Category"] = list[3][j];
                row["No.of Documents"] = list[4][j];

                table.Rows.Add(row);
            }
            return table;
        }
        #endregion
        #region PreConvertion
        public Boolean Process_PreConvertion(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                {
                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Relevance").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow => tableRow.Field("Last Consistency Result").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow => tableRow.Field("Exemption Possible").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow => tableRow.Field("ID").GetString()).ToArray(),

                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Title").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Lob/Technology").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Business Area").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Catetory").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Component").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Status").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Note").GetString()).ToArray(),

                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Application Area").GetString()).ToArray(),
                table.DataRange.Rows().Select(tableRow =>tableRow.Field("Summary").GetString()).ToArray(),
                };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_PreConvertion(dataList);
                Status = _Base.Upload_SAPPreConvertion(dataTable, fileName, Instance_ID, User_Id);
            }
            return Status;
        }
        private static DataTable ConvertListToDataTable_PreConvertion(IReadOnlyList<string[]> list)
        {
            Base _Base = new Base();
            var table = new DataTable("PreConvertion");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            List<PicturetoData> Pic = _Base.Sp_GetPicturetoDatas();


            table.Columns.Add("Relevance");
            table.Columns.Add("Last Consistency Result");
            table.Columns.Add("Exemption Possible");
            table.Columns.Add("ID");
            table.Columns.Add("Title");
            table.Columns.Add("LoB/Technology");
            table.Columns.Add("Business Area");
            table.Columns.Add("Catetory");
            table.Columns.Add("Component");
            table.Columns.Add("Status");
            table.Columns.Add("Note");
            table.Columns.Add("Application Area");
            table.Columns.Add("Summary");


            for (var j = 0; j < rows; j++)
            {
                int Re_Relevance = 0, Re_Result = 0, Re_Possible = 0;
                foreach (var item in Pic)
                {
                    var Relevance = list[0][j];
                    var Result = list[1][j];
                    var Possible = list[2][j];
                    if (item.PictureName == Result)
                    {
                        Re_Relevance = item.ID;
                    }

                    if (item.PictureName == Result)
                    {
                        Re_Result = item.ID;
                    }
                    if (item.PictureName == Possible)
                    {
                        Re_Possible = item.ID;
                    }

                }
                var row = table.NewRow();
                row["Relevance"] = Re_Relevance;
                row["Last Consistency Result"] = Re_Result;
                row["Exemption Possible"] = Re_Possible;
                row["ID"] = list[3][j];
                row["Title"] = list[4][j];
                row["LoB/Technology"] = list[5][j];
                row["Business Area"] = list[6][j];
                row["Catetory"] = list[7][j];
                row["Component"] = list[8][j];
                row["Status"] = list[9][j];
                row["Note"] = list[10][j];
                row["Application Area"] = list[11][j];
                row["Summary"] = list[12][j];

                table.Rows.Add(row);
            }
            return table;
        }
        #endregion


        #region User list
        public Boolean Process_Userlist(string FilePath, string fileName, Guid Instance_ID, Guid User_Id)
        {
            Boolean Status = false;

            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                  // Treat the range as a table (to be able to use the column names)
                var table = range.AsTable();
                //Specify what are all the Columns you need to get from Excel
                var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("User").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("User Type").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Valid from").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Valid through").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Date of Last Logon").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("System").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Catergory").GetString()).ToArray()
                 };
                //Convert List to DataTable
                DataTable dataTable = ConvertListToDataTable_Userlist(dataList);
                Status = _Base.Lic_Upload(dataTable, fileName, Instance_ID, User_Id);
                //To get unique column values, to avoid duplication
                //var uniqueCols = dataTable.DefaultView.ToTable(true, "Solution Number");

                ////Remove Empty Rows or any specify rows as per your requirement
                //for (var i = uniqueCols.Rows.Count - 1; i >= 0; i--)
                //{
                //    var dr = uniqueCols.Rows[i];
                //    if (dr != null && ((string)dr["Solution Number"] == "None" || (string)dr["Title"] == ""))
                //        dr.Delete();
                //}
                //Console.WriteLine("Total number of unique solution number in Excel : " + uniqueCols.Rows.Count);
            }


            return Status;
        }
        private static DataTable ConvertListToDataTable_Userlist(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("Userlist");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("User");
            table.Columns.Add("User Type");
            table.Columns.Add("Valid from");
            table.Columns.Add("Valid through");
            table.Columns.Add("Date of Last Logon");
            table.Columns.Add("System");
            table.Columns.Add("Catergory");
            table.Columns.Add("LastLogon_Date");

            try
            {
                for (var j = 0; j < rows; j++)
                {

                    var row = table.NewRow();
                    row["User"] = list[0][j];
                    row["User Type"] = list[1][j];
                    if (list[2][j].ToString() != "")
                        row["Valid from"] = Convert.ToDateTime(list[2][j].ToString());
                    else
                        row["Valid from"] = null;
                    if (list[3][j].ToString() != "")
                        row["Valid through"] = Convert.ToDateTime(list[3][j].ToString());
                    else
                        row["Valid through"] = null;

                    if (list[4][j].ToString() != "Not in Use" && list[4][j].ToString() != "")
                        row["LastLogon_Date"] = Convert.ToDateTime(list[4][j].ToString());
                    else
                        row["LastLogon_Date"] = null;
                    //row["Valid through"] = list[3][j];
                    //row["Date of Last Logon"] = list[4][j];
                    row["System"] = list[5][j];
                    row["Catergory"] = list[6][j];
                    row["Date of Last Logon"] = list[4][j];
                    table.Rows.Add(row);
                }

            }

            catch (Exception ex)
            {

                throw ex;
            }
            return table;
        }
        #endregion

        #region ActivityMaster

        public object Process_ActivityMaster(string FilePath,string NewId, Guid User_Id)
        {
            object result;
            Boolean Status = false;
            bool proceed = true;
            string Errormessage = "";

            result = new
            {
                Status,
                Errormessage
            };
            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var workSheet = workbook.Worksheet(1);
                if (workSheet.Name == "Activity")
                {

                    var firstRowUsed = workSheet.FirstRowUsed();
                    var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                    var lastPossibleAddress = workSheet.LastCellUsed().Address;
                    var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                      // Treat the range as a table (to be able to use the column names)
                    var table = range.AsTable();
                    //Specify what are all the Columns you need to get from Excel
                    var dataList = new List<string[]>
                 {
                    table.DataRange.Rows().Select(tableRow =>tableRow.Field("Task").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("BuildingBlock").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Phase").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("Role").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("ApplicationArea").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("TaskType").GetString()).ToArray(),
                    table.DataRange.Rows().Select(tableRow => tableRow.Field("ParallelName").GetString()).ToArray()
                 };
                    #region Validation
                    //Empty Check
                    for (int k = 0; k < dataList.Count - 1; k++)
                    {
                        if (proceed)
                        {
                            var datalist = dataList[k];
                            proceed = CheckforEmpty_Column(datalist);
                            if (proceed == false)
                            {
                                string ColumnName = "";
                                if (k == 0)
                                {
                                    ColumnName = "Task";
                                }
                                else if (k == 1)
                                {
                                    ColumnName = "BuildingBlock";
                                }
                                else if (k == 2)
                                {
                                    ColumnName = "Phase";
                                }
                                else if (k == 3)
                                {
                                    ColumnName = "Role";
                                }
                                else if (k == 4)
                                {
                                    ColumnName = "ApplicationArea";

                                }
                                else if (k == 5)
                                {
                                    ColumnName = "TaskType";
                                }
                                Errormessage = "Empty Row OR column value Found in <b>" + ColumnName + "</b>..! Can't proceed ..! ";
                                break;
                            }

                        }
                    }
                    //Task name
                    if (proceed)
                    {

                        foreach (var item in dataList[0])
                        {
                            var totalCats = dataList[0].Count(s => s == item.ToString());
                            if (totalCats > 1)
                            {
                                proceed = false;
                                Errormessage = "Task name duplicate found ..! Can't proceed ..! ";
                                break;
                            }
                        }
                    }
                    //Parallel
                    if (proceed)
                    {
                        for (int i = 0; i < dataList[5].Length; i++)
                        {
                            if (proceed)
                            {
                                //for (int j = 0; j < dataList[6].Length; j++)
                                {
                                    if (dataList[5][i].ToString() == "Parallel" && dataList[6][i].ToString().Trim() == "")
                                    {
                                        proceed = false;
                                        Errormessage = "Empty Parallel Name Found ..! Can't proceed ..! ";
                                        break;
                                    }

                                }
                            }

                        }
                    }

                    #endregion Validation
                    if (proceed)
                    {
                        var Parallel_Names = dataList[6].ToList().Distinct().ToArray();
                        Parallel_Names = Parallel_Names.Where(P => P != "").ToArray();

                        var dt = _Base.Sp_GetParallelNames();
                        var DBParallel_Name = dt.Select(l => l.Parallel_Name).ToList().Distinct().ToArray();

                        Boolean ParallelNames_Duplicate = false;

                        for (int i = 0; i < Parallel_Names.Count(); i++)
                        {
                            if (Parallel_Names[i].Length > 3)
                            {
                                Status = false;
                                Errormessage = "Parallel Length <b> Greater than three </b> found ...!";
                            }
                            ParallelNames_Duplicate = DBParallel_Name.Contains(Parallel_Names[i].ToString());
                            if (ParallelNames_Duplicate)
                                break;
                        }
                        if (ParallelNames_Duplicate)
                        {
                            Status = false;
                            Errormessage = "Duplicate <b> parallel type </b> found ...!";
                        }
                        else
                        {
                            _Base.Sp_InsertParallel(Parallel_Names);
                            dt = _Base.Sp_GetParallelNames();
                            DataTable dataTable = ConvertListToDataTable_ActivityMaster(dataList, dt);
                            Status = _Base.Upload_ActivityMaster(dataTable, NewId, User_Id);
                        }
                    }
                }
                else
                {
                    Status = false;
                    Errormessage = "Check the file with first Sheet Name as <b> IssueDump </b>...!";
                }
            }
            result = new
            {
                Status,
                Errormessage
            };
            return result;
        }

        private static DataTable ConvertListToDataTable_ActivityMaster(IReadOnlyList<string[]> list, List<ParallelType> dt)
        {
            var table = new DataTable("ActivityMaster");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();
            
            

            table.Columns.Add("Task");
            table.Columns.Add("BuildingBlock");
            table.Columns.Add("Phase");
            table.Columns.Add("Role");
            table.Columns.Add("ApplicationArea");
            table.Columns.Add("TaskType");
            table.Columns.Add("ParallelId");

            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["Task"] = list[0][j];
                row["BuildingBlock"] = list[1][j];
                row["Phase"] = list[2][j];
                row["Role"] = list[3][j];
                row["ApplicationArea"] = list[4][j];
                row["TaskType"] = list[5][j];
                var Result = (from i in dt
                         where i.Parallel_Name == list[6][j]
                         select i.ParallelId).FirstOrDefault();
                string ParallelId = null;
                if (Result.ToString()!="00000000-0000-0000-0000-000000000000")
                {
                    ParallelId = Result.ToString();
                }

                row["ParallelId"] = ParallelId;
                table.Rows.Add(row);
            }
            return table;
        }

        private bool CheckforEmpty_Column(string[] Datalist)
        {
            bool proceed=true;
            foreach (var item in Datalist)
            {
                if (item.ToString().Trim() == "")
                {
                    proceed = false;
                    break;
                }
            }
            return proceed;
        }
        #endregion

        #region SAPIssueTrackDump

        public object  Process_SAP_Issue_dump(string FilePath, string NewId, Guid Project_Id, Guid User_Id,string TimeZone)
        {
            object result;
            Boolean Status = false;
            bool proceed = true;
            string Errormessage = "";
            LogHelper _log = new LogHelper();
            result = new
            {
                Status,
                Errormessage
            };
            try
            {
                using (XLWorkbook workbook = new XLWorkbook(FilePath))
                {
                    var workSheet = workbook.Worksheet(1);
                    if (workSheet.Name == "IssueDump")
                    {
                        var firstRowUsed = workSheet.FirstRowUsed();
                        var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                        var lastPossibleAddress = workSheet.LastCellUsed().Address;
                        var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                                          // Treat the range as a table (to be able to use the column names)
                        var table = range.AsTable();
                        //Specify what are all the Columns you need to get from Excel
                        var dataList = new List<string[]>
                         {
                            table.DataRange.Rows().Select(tableRow =>tableRow.Field("Issue No").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Status").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Issue Description").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Category").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Priority").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Assignee").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Raised By").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Applicaiton Area").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Open").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Close").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Resolution").GetString()).ToArray(),
                            table.DataRange.Rows().Select(tableRow => tableRow.Field("Comments").GetString()).ToArray()
                        };
                        #region Validation
                        //Empty Check
                        for (int k = 0; k < dataList.Count - 1; k++)
                        {
                            if (proceed)
                            {
                                var datalist = dataList[k];
                                proceed = CheckforEmpty_Column(datalist);
                                if (proceed == false)
                                {
                                    string ColumnName = "";
                                    if (k == 0)
                                    {
                                        ColumnName = "Issue No";
                                    }
                                    else if (k == 1)
                                    {
                                        ColumnName = "Status";
                                    }
                                    else if (k == 2)
                                    {
                                        ColumnName = "Issue Description";
                                    }
                                    else if (k == 3)
                                    {
                                        ColumnName = "Category";
                                    }
                                    else if (k == 1)
                                    {
                                        ColumnName = "Priority";
                                    }
                                    else if (k == 2)
                                    {
                                        ColumnName = "Assignee";
                                    }
                                    else if (k == 3)
                                    {
                                        ColumnName = "Raised By";
                                    }
                                    else if (k == 4)
                                    {
                                        ColumnName = "Applicaiton Area";

                                    }
                                    else if (k == 4)
                                    {
                                        ColumnName = "Open";

                                    }
                                    else if (k == 5)
                                    {
                                        ColumnName = "Close";
                                    }
                                    else if (k == 5)
                                    {
                                        ColumnName = "Resolution";
                                    }
                                    Errormessage = "Empty Row OR column value Found in <b>" + ColumnName + "</b>..! Can't proceed ..! ";
                                    break;
                                }
                                if(k==0 & proceed == true)
                                {
                                    foreach (var item in datalist)
                                    {
                                        if (item.ToString().Trim() != "")
                                        {
                                            try
                                            {
                                                int issueNo = Convert.ToInt32(item.ToString().Trim());
                                                
                                            }
                                            catch (Exception ex)
                                            {

                                                Status = false;
                                                Errormessage = "Invalid Issue No";
                                                proceed = false;
                                                break;
                                            }
                                            
                                            
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        #endregion
                        //Convert List to DataTable
                        if (proceed)
                        {
                            try
                            {
                                DataTable dataTable = ConvertListToDataTable_SAPIssueTrackDump(dataList, TimeZone);
                                Status = _Base.Upload_SAPIssueTrackDump(dataTable, NewId, User_Id, Project_Id);
                            }
                            catch (Exception ex)
                            {
                                Status = false;
                                Errormessage = "Invalid Date Kindly Check again Make sure given date formate is MM/dd/yyyy ";
                                _log.createLog(ex, "");
                                //throw ex;
                            }
                        }
                        
                    }
                    else
                    {
                        Status = false;
                        Errormessage = "Check the file with first Sheet Name must be <b> IssueDump </b>...!";
                    }
                }
            }
            catch(Exception ex)
            {
                _log.createExMsgLog("Process_SAP_Issue_dump Exception =>"+ ex);
                _log.createExMsgLog("Process_SAP_Issue_dump Exception =>"+ ex.Message);
            }
            result = new
            {
                Status,
                Errormessage
            };
            return result;
        }

        private  DataTable ConvertListToDataTable_SAPIssueTrackDump(IReadOnlyList<string[]> list,string TimeZone)
        {
            LogHelper _log = new LogHelper(); 
            Base _base = new Base();
            var table = new DataTable("SAPIssueTrackDump");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            table.Columns.Add("IssueNo");
            
            table.Columns.Add("IssueName");
            table.Columns.Add("Category");
            table.Columns.Add("Priority");
            table.Columns.Add("Assignee");
            table.Columns.Add("RaisedBy");
            table.Columns.Add("ApplicaitonArea");
            table.Columns.Add("OpenDt");
            table.Columns.Add("CloseDt");
            table.Columns.Add("SAPIssueDumpStatus");
            table.Columns.Add("Resolution");
            table.Columns.Add("Comments");

            for (var j = 0; j < rows; j++)
            {
                try
                {

               
                var row = table.NewRow();
                row["IssueNo"] = list[0][j];                
                row["IssueName"] = list[2][j];
                row["Category"] = list[3][j];
                row["Priority"] = list[4][j];
                row["Assignee"] = list[5][j];
                row["RaisedBy"] = list[6][j];
                row["ApplicaitonArea"] = list[7][j];
                    //DateTime open_Dt = DateTime.Parse(String.Format("{0:MM/dd/yyyy HH:mm:ss tt}", DateTime.Parse(list[8][j])));
                    //DateTime Close_Dt = DateTime.Parse(String.Format("{0:MM/dd/yyyy HH:mm:ss tt}", DateTime.Parse(list[9][j])));
                    //row["OpenDt"] = _base.ConvertLocalToUTC(open_Dt, TimeZone);
                    //_log.createLog("OpenDt =>" + row["OpenDt"]);
                    //row["CloseDt"] = _base.ConvertLocalToUTC(Close_Dt, TimeZone);
                    //_log.createLog("CloseDt =>" + row["CloseDt"]);
                    DateTime open_Dt = DateTime.Parse(String.Format("{0:MM/dd/yyyy HH:mm:ss tt}", DateTime.Parse(list[8][j])));
                    DateTime Close_Dt = DateTime.Parse(String.Format("{0:MM/dd/yyyy HH:mm:ss tt}", DateTime.Parse(list[9][j])));

                    row["OpenDt"] = open_Dt;
                    row["CloseDt"] = Close_Dt;
                    row["SAPIssueDumpStatus"] = list[1][j];
                row["Resolution"] = list[10][j];
                row["Comments"] = list[11][j];
                table.Rows.Add(row);
                }
                catch (Exception ex)
                {
                    _log.createLog("Error at ConvertListToDataTable_SAPIssueTrackDump" + ex);
                    //throw;
                }
            }            
            return table;
        }
        //private DateTime CheckDate(String DateValue)
        //{
        //    LogHelper _log = new LogHelper();
        //    try
        //    {
        //        return DateTime.ParseExact(DateValue, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.createLog(ex, "");
        //        throw ex;
        //    }
        //}
        //private DateTime GetTIme(string Timezone)
        //{
        //    DateTime Dt = DateTime.Now;
            
        //    try
        //    {

        //        var timeZoneIds = TimeZoneInfo.GetSystemTimeZones().Select(t => t.Id);

        //        if(Timezone == "Asia/Calcutta")
        //        {
        //            Timezone = "India Standard Time";
        //        }

        //        var Time = TimeZoneInfo.FindSystemTimeZoneById(Timezone);
        //        DateTimeOffset UTC = TimeZoneInfo.ConvertTime(Dt, Time);
        //        DateTime other = DateTime.SpecifyKind(Dt, DateTimeKind.Utc);


        //        //DateTime localDate = DateTime.Now.Date;
        //        //var s = DateTimeOffset.Now();
        //        //var AUSOffset = new DateTimeOffset(localDate, TimeSpan.Zero);

        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }


        //    return Dt;

        //}

        #endregion
    }
}
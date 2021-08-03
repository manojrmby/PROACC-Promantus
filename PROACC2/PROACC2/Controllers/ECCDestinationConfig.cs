using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SAP.Middleware.Connector;
using PROACC2.BL.Model;
using PROACC2.BL;

namespace PROACC2.Controllers
{
    public class ECCDestinationConfig :IDestinationConfiguration
    {
        Base _base = new Base();

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public bool ChangeEventsSupported()
        {
            return false;
        }

       

        
        public RfcConfigParameters GetParameters(string destinationName)
        {
            try
            {
                RfcConfigParameters parms = new RfcConfigParameters();
                Instances instances = new Instances();
                string[] subs = destinationName.Split(' ');
                destinationName = subs[0];
                var instance = subs[1];

                instances = _base.Sp_GetSAPConnectionData(instance);

                //if (destinationName.Equals(instances.destinationName.Trim()))
                {
                    parms.Add(RfcConfigParameters.AppServerHost, instances.AppServerHost);
                    parms.Add(RfcConfigParameters.SystemNumber, instances.SystemNumber);
                    //parms.Add(RfcConfigParameters.SystemID, "G10");
                    parms.Add(RfcConfigParameters.SAPRouter, instances.SAPRouter);
                    parms.Add(RfcConfigParameters.User, instances.User);
                    parms.Add(RfcConfigParameters.Password, instances.Password);
                    parms.Add(RfcConfigParameters.Client, instances.Client);
                    parms.Add(RfcConfigParameters.Language, "EN");
                    //parms.Add(RfcConfigParameters.Language, instances.Language);
                    //parms.Add(RfcConfigParameters.PoolSize, "5");
                }
                return parms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }

    

    public static class IRfcTableExtentions
    {
        /// <summary>
        /// Converts SAP table to .NET DataTable table
        /// </summary>
        /// <param name="sapTable">The SAP table to convert.</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IRfcTable sapTable, string name)
        {
            DataTable adoTable = new DataTable(name);
            //... Create ADO.Net table.
            for (int liElement = 0; liElement < sapTable.ElementCount; liElement++)
            {
                RfcElementMetadata metadata = sapTable.GetElementMetadata(liElement);
                adoTable.Columns.Add(metadata.Name, GetDataType(metadata.DataType));
            }

            //Transfer rows from SAP Table ADO.Net table.
            foreach (IRfcStructure row in sapTable)
            {
                DataRow ldr = adoTable.NewRow();
                for (int liElement = 0; liElement < sapTable.ElementCount; liElement++)
                {
                    RfcElementMetadata metadata = sapTable.GetElementMetadata(liElement);

                    switch (metadata.DataType)
                    {
                        case RfcDataType.DATE:
                            ldr[metadata.Name] = row.GetString(metadata.Name).Substring(0, 4) + row.GetString(metadata.Name).Substring(5, 2) + row.GetString(metadata.Name).Substring(8, 2);
                            break;
                        case RfcDataType.BCD:
                            ldr[metadata.Name] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.CHAR:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.STRING:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.INT2:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.INT4:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.FLOAT:
                            ldr[metadata.Name] = row.GetDouble(metadata.Name);
                            break;
                        default:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                    }
                }
                adoTable.Rows.Add(ldr);
            }
            return adoTable;
        }

        private static Type GetDataType(RfcDataType rfcDataType)
        {
            switch (rfcDataType)
            {
                case RfcDataType.DATE:
                    return typeof(string);
                case RfcDataType.CHAR:
                    return typeof(string);
                case RfcDataType.STRING:
                    return typeof(string);
                case RfcDataType.BCD:
                    return typeof(decimal);
                case RfcDataType.INT2:
                    return typeof(int);
                case RfcDataType.INT4:
                    return typeof(int);
                case RfcDataType.FLOAT:
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }
    }

}

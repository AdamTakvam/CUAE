using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using MySql.Data.MySqlClient;

namespace Metreos.Native.VendorDb
{
    #region CheckWithGryphon

    [PackageDecl("Metreos.Native.VendorDb")]
    public class CheckWithGryphon : INativeAction 
    {
        [ActionParamField("User ID", true)]
        public string UserId { set { userId = value; } }
        private string userId;

        [ActionParamField("Password", true)]
        public string Password { set { password = value; } }
        private string password;

        [ActionParamField("Host", true)]
        public string Host { set { host = value; } }
        private string host;

        [ActionParamField("PhoneNumber", true)]
        public string PhoneNumber { set { phoneNumber = value; } }
        private string phoneNumber;

        [ActionParamField("License", true)]
        public string License { set { license = value; } }
        private string license;

        [ActionParamField("From", true)]
        public string From { set { from = value; } }
        private string from;

        [ActionParamField("Refkey", false)]
        public string Refkey { set { refkey = value; } }
        private string refkey;

        [ActionParamField("Curfew", false)]
        public bool Curfew { set { curfew = value; } }
        private bool curfew;

        [ActionParamField("ConnectionTimeout", false)]
        public int ConnectionTimeout { set { connectionTimeout = value; } }
        private int connectionTimeout;

        [ActionParamField("Override", false)]
        public int Override { set { overrideNum = value; } }
        private int overrideNum;

        [ResultDataField("DNC Status")]
        public int DNCStatus { get { return dncStatus; } }
        private int dncStatus;

        [ResultDataField("Internal")]
        public int Internal { get { return internalFlag; } }
        private int internalFlag;

        [ResultDataField("State")]
        public int State { get { return state; } }
        private int state;

        [ResultDataField("Federal")]
        public int Federal { get { return federal; } }
        private int federal;

        [ResultDataField("Wireless")]
        public int Wireless { get { return wireless; } }
        private int wireless;

        [ResultDataField("CMA")]
        public int CMA { get { return cma; } }
        private int cma;

        [ResultDataField("Special")]
        public int Special { get { return special; } }
        private int special;

        [ResultDataField("CurfewResult")]
        public int CurfewResult { get { return curfewResult; } }
        private int curfewResult;

        [ResultDataField("EBR")]
        public int EBR { get { return ebr; } }
        private int ebr;

        [ResultDataField("Exemption")]
        public int Exemption { get { return exemption; } }
        private int exemption;

        [ResultDataField("DMA")]
        public int DMA { get { return dma; } }
        private int dma;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public CheckWithGryphon()
        {
            Clear();        
        }

        public void Clear()
        {
            curfew = false;
            userId = String.Empty;
            password = String.Empty;
            host = String.Empty;
            phoneNumber = String.Empty;
            connectionTimeout = 0;
            license = String.Empty;
            from = String.Empty;
            refkey = String.Empty;
            curfewResult = 0;
            overrideNum = 0;
            dncStatus = 0;
            internalFlag = 0;
            state = 0;
            federal = 0;
            wireless = 0;
            cma = 0;
            special = 0;
            ebr = 0;
            exemption = 0;
            dma = 0;
        }

        public bool ValidateInput()
        {
            return true;
        }

        protected enum Result
        {
            Success,  // Can call
            Connectivity, // Can't connect to Gryphon DB
            DoNotCall, // Do not call
            Failure // Db failure
        }

        [ReturnValue(typeof(Result), "")]
        [Action("CheckWithGryphon", false, "Check With Gryphon", "Access the Gryphon DNC database and returns a status code")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            Result result = Result.Failure;
            string connectionString = String.Format("server={0};uid={1};pwd={2}", host, userId, password);
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch(Exception e)
                {
                    result = Result.Connectivity;
                    log.Write(TraceLevel.Error, "Unable to open a connection.\n{0}", 
                        Metreos.Utilities.Exceptions.FormatException(e));
                }

                if(connection.State == ConnectionState.Open)
                {
                    using(SqlCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            command.Parameters.Add("@license", license);
                            command.Parameters.Add("@caller", from);
                            command.Parameters.Add("@refkey", refkey);
                            command.Parameters.Add("@number", phoneNumber);
                            command.Parameters.Add("@curfew", curfew ? 1 : 0);
                            command.Parameters.Add("@override", overrideNum);

                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "voIP_CertifyPhoneNumber";
                            SqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            object dncStatusObj = reader["DNCStatus"];
                            dncStatus = Convert.ToInt32(dncStatusObj);
                            object internalObj = reader["Internal"];
                            internalFlag = Convert.ToInt32(internalObj);
                            object stateObj = reader["State"];
                            state = Convert.ToInt32(internalFlag);
                            object federalObj = reader["Federal"];
                            federal = Convert.ToInt32(federalObj);
                            object wirelessObj = reader["Wireless"];
                            wireless = Convert.ToInt32(wirelessObj);
                            object cmaObj = reader["CMA"];
                            cma = Convert.ToInt32(cmaObj);
                            object specialObj = reader["Special"];
                            special = Convert.ToInt32(specialObj);
                            object curfewResultObj = reader["Curfew"];
                            curfewResult = Convert.ToInt32(curfewResultObj);
                            object ebrObj = reader["EBR"];
                            ebr = Convert.ToInt32(ebrObj);
                            object exemptionObj = reader["Exemption"];
                            exemption = Convert.ToInt32(exemptionObj);
                            object dmaObj = reader["DMA"];
                            dma = Convert.ToInt32(dmaObj);

                            if(dncStatus == 1)
                            {
                                result = Result.DoNotCall;
                            }
                            else
                            {
                                result = Result.Success;
                            }
                        }
                        catch(Exception fe)
                        {
                            result = Result.Failure;
                            log.Write(TraceLevel.Error, "Unable to execute the Gryphon CertifyPhoneNumber Stored Procedure.\n{0}",
                                Metreos.Utilities.Exceptions.FormatException(fe));
                        }
                    }
                }
            }

            return result.ToString();
        }
    }

    #endregion 

    #region AddPhoneNumber

    [PackageDecl("Metreos.Native.VendorDb")]
    public class AddToGryphon : INativeAction 
    {
        [ActionParamField("User ID", true)]
        public string UserId { set { userId = value; } }
        private string userId;

        [ActionParamField("Password", true)]
        public string Password { set { password = value; } }
        private string password;

        [ActionParamField("Host", true)]
        public string Host { set { host = value; } }
        private string host;

        [ActionParamField("PhoneNumber", true)]
        public string PhoneNumber { set { phoneNumber = value; } }
        private string phoneNumber;

        [ActionParamField("License", true)]
        public string License { set { license = value; } }
        private string license;

        [ActionParamField("ConnectionTimeout", false)]
        public int ConnectionTimeout { set { connectionTimeout = value; } }
        private int connectionTimeout;

        [ResultDataField("Return Code")]
        public int ReturnCode { get { return returnCode; } }
        private int returnCode;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public AddToGryphon()
        {
            Clear();        
        }

        public void Clear()
        {
            userId = String.Empty;
            password = String.Empty;
            host = String.Empty;
            phoneNumber = String.Empty;
            connectionTimeout = 0;
            license = String.Empty;
            returnCode = 0;
        }

        public bool ValidateInput()
        {
            return true;
        }

        protected enum AddResult
        {
            Success,  // Can call
            Connectivity, // Can't connect to Gryphon DB
            Failure // Db failure
        }

        protected enum GryphonAddReturnCodes
        {   
            Success = 0,
            Error = 1,
            Duplicate =2
        }

        [ReturnValue(typeof(AddResult), "")]
        [Action("AddToGryphon", false, "Add To Gryphon", "Access the Gryphon DNC database and add a new DNC")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            AddResult result = AddResult.Failure;
            string connectionString = String.Format("server={0};uid={1};pwd={2}", host, userId, password);
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch(Exception e)
                {
                    result = AddResult.Connectivity;
                    log.Write(TraceLevel.Error, "Unable to open a connection.\n{0}", 
                        Metreos.Utilities.Exceptions.FormatException(e));
                }

                if(connection.State == ConnectionState.Open)
                {
                    // Capture disposition code--don't bug me
                    using(SqlCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            command.Parameters.Add("@license", license);
                            command.Parameters.Add("@number", phoneNumber);

                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "voIP_AddPhoneNumber";
                            SqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            object returnCodeObj = reader[0];
                            returnCode = Convert.ToInt32(returnCodeObj);

                            if(returnCode == (int) GryphonAddReturnCodes.Error)
                            {
                                result = AddResult.Failure;
                            }
                            else
                            {
                                result = AddResult.Success;
                            }
                        }
                        catch(Exception fe)
                        {
                            result = AddResult.Failure;
                            log.Write(TraceLevel.Error, "Unable to execute the Gryphon AddPhoneNumber Stored Procedure.\n{0}",
                                Metreos.Utilities.Exceptions.FormatException(fe));
                        }
                    }
                }
            }

            return result.ToString();
        }
    }

    #endregion
}

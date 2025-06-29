using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using ExternNumTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.ExternalNumbers;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the external numbers table, and any information which stem from that
    /// </summary>
    public class ExternalNumbers : DbTable
    {
        public ExternalNumbers(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region ResultValues

        public enum GetCorpNumForUserResults
        {
            Success,
            Failure,
            NoNumberDefined
        }

        #endregion

        #region FormatExternalNumbers
        /// <summary>
        /// Takes a table that is a subset of the ExternalNumbers table, looks at each number,
        /// removes non-numerics, then places the cleaned-up number into a StringCollection. 
        /// </summary>
        internal static StringCollection FormatExternalNumbers(DataTable table)
        {
            StringCollection collection = new StringCollection();
            string pattern = @"(\d+\.?\d*|\.\d+)";

            foreach (DataRow row in table.Rows)
            {
                string numberString = row[SqlConstants.Tables.ExternalNumbers.PhoneNumber] as string;
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                if (numberString != null && ! (numberString.Equals(string.Empty)) )
                {
                    foreach (Match match in Regex.Matches(numberString, pattern))
                        builder.Append(match.ToString());

                    collection.Add(builder.ToString());
                }
            }
            return collection;
        }
        #endregion

        #region SetFindMeStatus

        /// <summary>
        ///     Enables/Disables specified FindMe numbers for a user for use with active relay.
        /// </summary>
        /// <param name="userId"> The id of the user </param>
        /// <param name="filter"> Filter used to match against the number, in SQL format. </param>
        /// <param name="newValue"> true or false value for the fields that are matched by the filter. </param>
        public bool SetFindMeStatus(uint userId, string filter, bool newValue)
        {
            bool success = true;
            SqlBuilder builder = new SqlBuilder(Method.UPDATE, ExternNumTable.TableName);
            builder.AddFieldValue(ExternNumTable.ArEnabled, newValue);
            builder.where[ExternNumTable.UserId] = userId;
            builder.like[ExternNumTable.PhoneNumber] = filter;

            WriteResultContainer resultContainer = ExecuteCommand(builder);
            WriteResult result = resultContainer.result;

            if (result == WriteResult.DbFailure || result == WriteResult.PublisherDown)
            {
                log.Write(TraceLevel.Info, "SetFindMeStatus: Write to database failed. Reason: " + result.ToString());
                success = false;
            }
            
            return success;
        }
        
        #endregion

        #region GetCorporateNumberForUser
        /// <summary>
        /// This method is used to return the voicemail number associated with the provided user account
        /// </summary>
        public GetCorpNumForUserResults GetCorporateNumberForUser(uint userId, out string voiceMailNumber)
        {
            GetCorpNumForUserResults actionResult = GetCorpNumForUserResults.Failure;
            voiceMailNumber = null;

            if (userId < SqlConstants.StandardPrimaryKeySeed)
                return actionResult;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, ExternNumTable.TableName);
            builder.fieldNames.Add(ExternNumTable.PhoneNumber);
            builder.where[ExternNumTable.UserId] = userId;
            builder.where[ExternNumTable.IsCorporate] = true;
                        
            ReadResultContainer result = ExecuteScalar(builder);

            if(result.result == ReadResult.Success)
            {
                if ( ! Convert.IsDBNull(result.scalar))
                {
                    voiceMailNumber = result.scalar as string;
                    if (voiceMailNumber == null || voiceMailNumber == string.Empty)
                    {
                        voiceMailNumber = string.Empty;
                        actionResult = GetCorpNumForUserResults.NoNumberDefined;
                    }
                    else
                        actionResult = GetCorpNumForUserResults.Success;
                }
                else
                    actionResult = GetCorpNumForUserResults.NoNumberDefined;
            }
            else
            {
                log.Write(TraceLevel.Warning, 
                    "Error encountered in the GetCorporateNumberForUser method, using user Id: '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
            }

            return actionResult;
        }
        #endregion
    }
}

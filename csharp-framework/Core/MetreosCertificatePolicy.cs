using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Metreos.Core
{
	/// <summary>Policy for handling HTTPS certificate validation errors.</summary>
	/// <remarks>This value is set on a per-AppDomain basis.</remarks>
    public abstract class MetreosCertificatePolicy
    {
        // The .NET Framework should expose these, but they don't.
        public enum CertificateProblem : long
        {
            CertEXPIRED                   = 2148204801,
            CertVALIDITYPERIODNESTING     = 2148204802,
            CertROLE                      = 2148204803,
            CertPATHLENCONST              = 2148204804,
            CertCRITICAL                  = 2148204805,
            CertPURPOSE                   = 2148204806,
            CertISSUERCHAINING            = 2148204807,
            CertMALFORMED                 = 2148204808,
            CertUNTRUSTEDROOT             = 2148204809,
            CertCHAINING                  = 2148204810,
            CertREVOKED                   = 2148204812,
            CertUNTRUSTEDTESTROOT         = 2148204813,
            CertREVOCATION_FAILURE        = 2148204814,
            CertCN_NO_MATCH               = 2148204815,
            CertWRONG_USAGE               = 2148204816,
            CertUNTRUSTEDCA               = 2148204818
        }

        /// <summary>
        /// ServerCertificateValidationCallback method to ignore problems that we are willing to accept.
        /// </summary>
        public static bool ValidateCertificate(object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors errors)
        {
            // To ignore a problem, simply check problem against one of the enum values above
            //   and return true in that case.

            // Accept any and all errors Cisco can possibly come up with
            return true;
        } 
    }
}

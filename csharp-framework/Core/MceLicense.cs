using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace Metreos.Core
{
    public class MceLicense
    {
        private const string publicKey = @"<DSAKeyValue><P>noKXNiXUgBo2BcGb0nI3TUdnQxJvfHg+oaUFLe907UbHYKCFjg8dwfDoJ3sf3IhxHbbQhvKfqG8XqGwrcpkpekWxbbTl4XSUohA35qAFaUuLGNeqgXxpNWj8CgaxIIqAdDh0DaTVJEBaq3Ku5gHvFEfFHpPKrjk1UfQU8nhLQYM=</P><Q>k8qwIM2zeRy3wpkjVcDWmKB6iH8=</Q><G>M0/yn0hSG9AxPp4RDuuv1lGj5nmhSEQhdTPgYmBAtOwrRxqj3sYQ38W6CScR14qARvlttN6kAivchTBQuqjDUxoGO1JjKhU/6KJxO7s6sgbfBUHLNSTgjLlIsu9eX5E25we5n4Q/8FJwxrPNwbepqnyolGFKpBF1KfYJvrKBsck=</G><Y>Gcq7YYnUL9UFTBgs9KDxZ9ABBtogtTwTwZPsteQZbjJxNmuWpCcNuqnHEd6h4YBm/BV7l3mt3YuECz6Q/7uIABVAIDt2AP1pPTU9ruEpYM9bpiTG9n8ptpDO3H3RlqGJVrTWQtJ3Gli1Goq3qgFlwK9Cb3XIuvUYNRcxuFAqX4I=</Y><J>AAAAARKQ5dYw5Ns0u2ispiPTW4e9OXSesOB/HA/WXOl8htlxdOwBri9cFRYEpkvS7gP+E10mhBTgdKtRytuaMYKG3FC7EkDzOc3TG2ilWKCOFQvJjad/cIb8NNPmEOdYLKM24Fig7dSHFY93USJtfg==</J><Seed>BAt7IW4ttyffK30Ya6dQUm+BuBM=</Seed><PgenCounter>Xg==</PgenCounter></DSAKeyValue>";
        private MceLicenseData license;

        public MceLicense()
        {}

        
        public bool LoadLicense(string filename)
        {
            // REFACTOR Note, this license should be retrieved from the database.

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(filename);
                XmlSerializer xsr = new XmlSerializer(typeof(MceLicenseData));
                license = (MceLicenseData)xsr.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }

            return true;
        }


        public bool ValidateLicense()
        {
            bool licenseValidated = false;

            if(license == null)
            {
                return false;
            }

            string signatureStr = license.Signature;

            // Reset the signature to empty to enable us to validate it as it
            // was originally signed.
            license.Signature = "";

            string licenseStr = license.ToString();

            try
            {
                DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
                dsa.FromXmlString(publicKey);

                byte[] licenseData = System.Text.Encoding.Default.GetBytes(licenseStr);
                byte[] signatureData = System.Text.Encoding.Default.GetBytes(signatureStr);
            
                licenseValidated = dsa.VerifyData(licenseData, signatureData);
            }
            catch(Exception)
            {}

            return licenseValidated;
        }


        public override string ToString()
        {
            return license.ToString();
        }
    }
}

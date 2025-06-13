using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.ApplicationFramework.Collections;

namespace Metreos.Native.EnrollSpeaker
{
	/// <summary>
	/// Encode audio file into Base64 format
	/// </summary>
    [PackageDecl("Metreos.Native.EnrollSpeaker")]
    public class EncodeAudioToBase64 : INativeAction
	{
        [ActionParamField("The recorded audio file path", true)]
        public string AudioFilePath { set { audioFilePath = value; } }
        private string audioFilePath;

        [ResultDataField("Encoded audio data")]
        public string EncodedData { get { return encodedData; } }
        private string encodedData;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public EncodeAudioToBase64() { Clear(); }

        [Action("EncodeAudioToBase64", false, "EncodeAudioToBase64", "Encode audio file to Base64 format")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            log.Write(TraceLevel.Verbose, "Executing EncodeAudioToBase64, audio file is {0}", this.audioFilePath);

            System.IO.FileStream inFile;     
            byte[] binaryData = null;

            try 
            {
                inFile = new FileStream(this.audioFilePath, FileMode.Open, FileAccess.Read);
                binaryData = new Byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0, (int)inFile.Length);
                inFile.Close();
            }
            catch (Exception e) 
            {
                log.Write(TraceLevel.Error, "Error reading file {0}", e.Message);
                return IApp.VALUE_FAILURE;
            }

            // Convert the binary input into Base64 UUEncoded output.
            // Each 3 byte sequence in the source data becomes a 4 byte
            // sequence in the character array. 
            long arrayLength = (long)((4.0d/3.0d)*binaryData.Length);
    
            // If array length is not divisible by 4, go up to the next
            // multiple of 4.
            if (arrayLength%4 != 0) 
                arrayLength += (4-arrayLength%4);
    
            char[] base64CharArray = new char[arrayLength];
            try 
            {
                Convert.ToBase64CharArray(binaryData, 
                                            0,
                                            binaryData.Length,
                                            base64CharArray,
                                            0);
            }
            catch (System.ArgumentNullException) 
            {
                log.Write(TraceLevel.Error, "Binary data array is null");
                return IApp.VALUE_FAILURE;
            }
            catch (System.ArgumentOutOfRangeException) 
            {
                log.Write(TraceLevel.Error, "Char array is not large enough");
                return IApp.VALUE_FAILURE;
            }

            this.encodedData = new string(base64CharArray);
            
            return IApp.VALUE_SUCCESS;
        }

        public void Clear()
        {
            this.audioFilePath = null;
            this.encodedData = null;
        }

        public bool ValidateInput()
        {
            if (this.audioFilePath.Length > 0 && File.Exists(this.audioFilePath))
                return true;

            return false;
        }
    }
}

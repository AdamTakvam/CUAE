using System;
using System.Text;
using SBUtils;
using SBSSHKeyStorage;

namespace SSHServer.NET
{
	/// <summary>
	/// Responsible for storing settings for a particular user account
	/// </summary>
	public class UserInfo
	{
		
		#region Public properties
		
		public string Name 
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		
		public int AuthTypes
		{
			get { return m_AuthTypes; }
			set { m_AuthTypes = value; }
		}

		public string PasswordSalt 
		{
			get { return m_PasswordSalt; }
			set { m_PasswordSalt = value; }
		}

		public string PasswordHash
		{
			get { return m_PasswordHash; }
			set { m_PasswordHash = value; }
		}

		public string PublicKey
		{
			get { return m_PublicKey; }
			set { m_PublicKey = value; }
		}

		public bool AuthAll
		{
			get { return m_AuthAll; }
			set { m_AuthAll = value; }
		}

		#endregion
		
		#region Public methods

		/// <summary>
		/// Simple constructor
		/// </summary>
		public UserInfo()
		{

		}

		/// <summary>
		/// Constructs object from a configuration string
		/// </summary>
		/// <param name="UserLine">User configuration string</param>
		public UserInfo(string UserLine)
		{
			string[] Lexems = UserLine.Split(':');
			try
			{
				Name = Lexems[0];
				PasswordSalt = Lexems[1];
				PasswordHash = Lexems[2];
				string Auths = Lexems[3];
				if (Auths.IndexOf("password") >= 0) 
				{
					AuthTypes |= SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD;
				}
				if (Auths.IndexOf("publickey") >= 0) 
				{
					AuthTypes |= SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY;
				}
				if (Auths.IndexOf("keyboard") >= 0) 
				{
					AuthTypes |= SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD;
				}
				AuthAll = (Auths.IndexOf(" all") >= 0);
				if (Lexems.Length >= 5)
				{
					TElSSHKey Key = new TElSSHKey();
					byte[] PKey = Convert.FromBase64String(Lexems[4]);
					int Result = Key.LoadPublicKey(PKey, PKey.Length); 
					if (Result == 0) 
					{
						PublicKey = SBUtils.Unit.StringOfBytes(PKey, 0, PKey.Length);
					}
				}
			}
			catch(Exception exc) 
			{ 
				Logger.Log("Error loading users " + exc.Message,false);
			}
		}

		/// <summary>
		/// Sets password for user
		/// </summary>
		/// <param name="Password"></param>
		public void SetPassword(string Password)
		{
			byte[] Salt = new byte[4];
			SBUtils.Unit.SBRndGenerate(ref Salt,4);
			PasswordSalt = "";
			for (int i = 0; i < 4; i++)
				PasswordSalt += Salt[i].ToString("X2");
			PasswordHash = CalcPasswordHash(PasswordSalt, Password);
		}
		
		/// <summary>
		/// Checks if password is valid
		/// </summary>
		/// <param name="Password">Password to check</param>
		/// <returns>true if the password is valid</returns>
		public bool PasswordValid(string Password)
		{
			return PasswordHash.Equals(CalcPasswordHash(PasswordSalt, Password));
		}
		
		/// <summary>
		/// Sets public key for user 
		/// </summary>
		/// <param name="Key">Public key</param>
		/// <returns>true if the key was successfully set</returns>
		public bool SetPublicKey(string Key)
		{
			TElSSHKey TestKey = new TElSSHKey();
			byte[] keyBytes = SBUtils.Unit.BytesOfString(Key);
			if (TestKey.LoadPublicKey(keyBytes, keyBytes.Length) == 0)
			{
				PublicKey = Key;
				return true;
			} 
			else return false;
		}

		/// <summary>
		/// Check if a supplied public key is valid 
		/// </summary>
		/// <param name="Key"></param>
		/// <returns>true if the key is valid</returns>
		public bool KeyValid(TElSSHKey Key)
		{
			TElSSHKey CurrentKey = new TElSSHKey();
			try
			{
				byte[] keyBytes = SBUtils.Unit.BytesOfString(PublicKey);
				CurrentKey.LoadPublicKey(keyBytes, keyBytes.Length);
				byte[] Current = SBUtils.Unit.DigestToBinary160(CurrentKey.FingerprintSHA1);
				byte[] Test = SBUtils.Unit.DigestToBinary160(Key.FingerprintSHA1);
				for (int i = 0; i < Current.Length; i++)
					if (Current[i] != Test[i]) return false;
				return true;
			} 
			catch(Exception) 
			{
				return false;
			}
		}

		public override string ToString()
		{
			// encoding key to make it a single line:
			byte[] keyBuf = SBUtils.Unit.BytesOfString(PublicKey);
			string encodedKey = "";
			encodedKey = Convert.ToBase64String(keyBuf, 0, keyBuf.Length);
			string Result = Name + ":" + PasswordSalt + ":" + PasswordHash + 
				":" + GetAuthType() + ":" + encodedKey;
			return Result;
		}

		/// <summary>
		/// Returns authentication types string
		/// </summary>
		/// <returns>authentication types</returns>
		public string GetAuthType()
		{
			string sAuth = "";
			if ((AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD) != 0)
				sAuth += ",password";
			if ((AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY) != 0)
				sAuth += ",publickey";
			if ((AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD) != 0)
				sAuth += ",keyboard";
			if (AuthAll) 
			{
				sAuth += ", all";
			}
			if (sAuth.Length > 0) 
			{
				sAuth = sAuth.Remove(0, 1);
			}
			return sAuth;
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Calculates password hash
		/// </summary>
		/// <param name="Salt">Salt value</param>
		/// <param name="Password">Password</param>
		/// <returns>Password hash</returns>
		private string CalcPasswordHash(string Salt, string Password)
		{
			string sHash = Salt + Password;
			byte[] Hash = new byte[sHash.Length];
			for (int i = 0; i < sHash.Length; i++)
				Hash[i] = (byte)sHash[i];
			return SBUtils.Unit.DigestToStr128(SBMD.Unit.HashMD5(Hash), true);
		}

		#endregion

		#region Class members

		private string m_Name = "";
		private int m_AuthTypes = 0;
		private string m_PasswordSalt = "";
		private string m_PasswordHash = "";
		private string m_PublicKey = "";
		private bool m_AuthAll = false;

		#endregion

	}
}

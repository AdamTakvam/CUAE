using System;
using System.Drawing;
using System.Windows.Forms;
using SBPGPKeys;

namespace PGPKeysDemo 
{
	public class Utils 
	{
		public static void RedrawKeyring(TreeView tv, TElPGPKeyring keyring)
		{
			int i, j, k;
			TreeNode KeyNode, UserNode, SubKeyNode, SigNode;
			tv.Nodes.Clear();
			for(i = 0; i < keyring.PublicCount; i++) 
			{
				/* Creating key node */
				KeyNode = tv.Nodes.Add(GetDefaultUserID(keyring.get_PublicKeys(i)));
				KeyNode.Tag = keyring.get_PublicKeys(i);
				if ((keyring.get_PublicKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA) ||
					(keyring.get_PublicKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_ENCRYPT) ||
					(keyring.get_PublicKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_SIGN)) 
				{
					KeyNode.ImageIndex = 1;
				} 
				else 
				{
					KeyNode.ImageIndex = 0;
				}
				KeyNode.SelectedImageIndex = KeyNode.ImageIndex;
				if (keyring.get_PublicKeys(i).SecretKey != null) 
				{
					//KeyNode.NodeFont = new Font(this.Font, FontStyle.Bold);
				}
				if (keyring.get_PublicKeys(i).Revoked) 
				{
					KeyNode.NodeFont = new Font(tv.Font, FontStyle.Italic);
				}

				/* Creating user nodes */
				for(j = 0; j < keyring.get_PublicKeys(i).UserIDCount; j++) 
				{
					UserNode = KeyNode.Nodes.Add(keyring.get_PublicKeys(i).get_UserIDs(j).Name);
					UserNode.Tag = keyring.get_PublicKeys(i).get_UserIDs(j);
					UserNode.ImageIndex = 2;
					UserNode.SelectedImageIndex = 2;
					/* Creating signature nodes */
					for(k = 0; k < keyring.get_PublicKeys(i).get_UserIDs(j).SignatureCount; k++) 
					{
						if (keyring.get_PublicKeys(i).get_UserIDs(j).get_Signatures(k).IsUserRevocation()) 
						{
							SigNode = UserNode.Nodes.Add("Revocation");
							UserNode.NodeFont = new Font(tv.Font, FontStyle.Italic);
						} 
						else 
						{
							SigNode = UserNode.Nodes.Add("Signature");
						}
						SigNode.Tag = keyring.get_PublicKeys(i).get_UserIDs(j).get_Signatures(k);
					}
				}
				for(j = 0; j < keyring.get_PublicKeys(i).UserAttrCount; j++) 
				{
					UserNode = KeyNode.Nodes.Add("Photo");
					UserNode.Tag = keyring.get_PublicKeys(i).get_UserAttrs(j);
					/* Creating signature nodes */
					for(k = 0; k < keyring.get_PublicKeys(i).get_UserAttrs(j).SignatureCount; k++) 
					{
						if (keyring.get_PublicKeys(i).get_UserAttrs(j).get_Signatures(k).IsUserRevocation()) 
						{
							SigNode = UserNode.Nodes.Add("Revocation");
							UserNode.NodeFont = new Font(tv.Font, FontStyle.Italic);
						} 
						else 
						{
							SigNode = UserNode.Nodes.Add("Signature");
						}
						SigNode.Tag = keyring.get_PublicKeys(i).get_UserAttrs(j).get_Signatures(k);
					}
				}

				/* Subkeys */
				for(j = 0; j < keyring.get_PublicKeys(i).SubkeyCount; j++) 
				{
					SubKeyNode = KeyNode.Nodes.Add(SBPGPUtils.Unit.PKAlg2Str(keyring.get_PublicKeys(i).get_Subkeys(j).PublicKeyAlgorithm) + " subkey");
					SubKeyNode.Tag = keyring.get_PublicKeys(i).get_Subkeys(j);
					/* Creating signature nodes */
					for(k = 0; k < keyring.get_PublicKeys(i).get_Subkeys(j).SignatureCount; k++) 
					{
						if (keyring.get_PublicKeys(i).get_Subkeys(j).get_Signatures(k).IsSubkeyRevocation()) 
						{
							SigNode = SubKeyNode.Nodes.Add("Revocation");
							SubKeyNode.NodeFont = new Font(tv.Font, FontStyle.Italic);
						} 
						else 
						{
							SigNode = SubKeyNode.Nodes.Add("Signature");
						}
						SigNode.Tag = keyring.get_PublicKeys(i).get_Subkeys(j).get_Signatures(k);
					}
				}
			}
		}

		public static string GetDefaultUserID(TElPGPPublicKey key)
		{
			string result;
			if (key.UserIDCount > 0) 
			{
				result = key.get_UserIDs(0).Name;
			} 
			else 
			{
				result = "No name";
			}
			return result;
		}
 
	}
}
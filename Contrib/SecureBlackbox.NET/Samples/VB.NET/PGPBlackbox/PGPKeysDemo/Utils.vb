Public Class Utils

	Public Shared Function GetDefaultUserID(ByVal key As TElPGPPublicKey) As String
		Dim result As String

		If (key.UserIDCount > 0) Then
			result = key.UserIDs(0).Name
		Else
			result = "No name"
		End If
		Return result
	End Function

	Public Shared Sub RedrawKeyring(ByVal tv As TreeView, ByVal keyring As TElPGPKeyring)

		Dim i, j, k As Integer

		Dim KeyNode, UserNode, SubKeyNode, SigNode As TreeNode
		tv.Nodes.Clear()
		For i = 0 To keyring.PublicCount - 1

			'	Creating key node
			KeyNode = tv.Nodes.Add(GetDefaultUserID(keyring.PublicKeys(i)))
			KeyNode.Tag = keyring.PublicKeys(i)
			if keyring.PublicKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA or keyring.PublicKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_ENCRYPT or keyring.PublicKeys(i).PublicKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_SIGN then
				KeyNode.ImageIndex = 1
			Else
				KeyNode.ImageIndex = 0
			End If

			KeyNode.SelectedImageIndex = KeyNode.ImageIndex
			If Not (keyring.PublicKeys(i).SecretKey Is Nothing) Then
				' KeyNode.NodeFont = new Font(this.Font, FontStyle.Bold)
			End If
			If keyring.PublicKeys(i).Revoked Then
				KeyNode.NodeFont = New Font(tv.Font, FontStyle.Italic)
			End If

			' Creating user nodes
			For j = 0 To keyring.PublicKeys(i).UserIDCount - 1

				UserNode = KeyNode.Nodes.Add(keyring.PublicKeys(i).UserIDs(j).Name)
				UserNode.Tag = keyring.PublicKeys(i).UserIDs(j)
				UserNode.ImageIndex = 2
				UserNode.SelectedImageIndex = 2
				' Creating signature nodes
				For k = 0 To keyring.PublicKeys(i).UserIDs(j).SignatureCount - 1

					If (keyring.PublicKeys(i).UserIDs(j).Signatures(k).IsUserRevocation()) Then

						SigNode = UserNode.Nodes.Add("Revocation")
						UserNode.NodeFont = New Font(tv.Font, FontStyle.Italic)
					Else
						SigNode = UserNode.Nodes.Add("Signature")
					End If
					SigNode.Tag = keyring.PublicKeys(i).UserIDs(j).Signatures(k)
				Next k
			Next j
			For j = 0 To keyring.PublicKeys(i).UserAttrCount - 1

				UserNode = KeyNode.Nodes.Add("Photo")
				UserNode.Tag = keyring.PublicKeys(i).UserAttrs(j)
				' Creating signature nodes
				For k = 0 To keyring.PublicKeys(i).UserAttrs(j).SignatureCount - 1
					If (keyring.PublicKeys(i).UserAttrs(j).Signatures(k).IsUserRevocation()) Then
						SigNode = UserNode.Nodes.Add("Revocation")
						UserNode.NodeFont = New Font(tv.Font, FontStyle.Italic)

					Else
						SigNode = UserNode.Nodes.Add("Signature")
					End If
					SigNode.Tag = keyring.PublicKeys(i).UserAttrs(j).Signatures(k)
				Next k
			Next j

			'Subkeys
			For j = 0 To keyring.PublicKeys(i).SubkeyCount - 1
				SubKeyNode = KeyNode.Nodes.Add(SBPGPUtils.Unit.PKAlg2Str(keyring.PublicKeys(i).Subkeys(j).PublicKeyAlgorithm) + " subkey")
				SubKeyNode.Tag = keyring.PublicKeys(i).Subkeys(j)
				' Creating signature nodes
				For k = 0 To keyring.PublicKeys(i).Subkeys(j).SignatureCount - 1

					If keyring.PublicKeys(i).Subkeys(j).Signatures(k).IsSubkeyRevocation() Then
						SigNode = SubKeyNode.Nodes.Add("Revocation")
						SubKeyNode.NodeFont = New Font(tv.Font, FontStyle.Italic)
					Else

						SigNode = SubKeyNode.Nodes.Add("Signature")
					End If
					SigNode.Tag = keyring.PublicKeys(i).Subkeys(j).Signatures(k)
				Next k
			Next j
		Next i
	End Sub



End Class

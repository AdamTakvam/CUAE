Imports System
Imports SBUtils
Imports SBRDN

Module RDN
    Public Function ByteArrayFromBufferType(ByVal mBuff As SBUtils.TBufferTypeConst) As Byte()
        Dim Result(mBuff.Length - 1) As Byte
        Dim i As Integer

        For i = 0 To mBuff.Length - 1
            Result(i) = mBuff.Bytes(i)
        Next
        Return Result
    End Function

    Public Function GetStringByOID(ByVal S As Byte()) As String
        If (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COMMON_NAME))) Then
            Return "CommonName"
        ElseIf (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_COUNTRY))) Then
            Return "Country"
        ElseIf (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_LOCALITY))) Then
            Return "Locality"
        ElseIf (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE))) Then
            Return "StateOrProvince"
        ElseIf (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION))) Then
            Return "Organization"
        ElseIf (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT))) Then
            Return "OrganizationUnit"
        ElseIf (SBUtils.Unit.CompareContent(S, ByteArrayFromBufferType(SBUtils.Unit.SB_CERT_OID_EMAIL))) Then
            Return "Email"
        Else
            Return "UnknownField"
        End If
    End Function

    Public Function GetOIDValue(ByVal RDN As SBRDN.TElRelativeDistinguishedName, ByVal S As Byte()) As String
        Dim t As String = ""
        Dim iCount As Integer = RDN.Count
        Dim i As Integer = 0
        For i = 0 To iCount - 1
            If (SBUtils.Unit.CompareContent(RDN.OIDs(i), S)) Then
                If (t <> "") Then
                    t = t + " / "
                End If

                t = t + SBUtils.Unit.UTF8ToStr(RDN.Values(i))
            End If
        Next i

        Return t
    End Function
End Module

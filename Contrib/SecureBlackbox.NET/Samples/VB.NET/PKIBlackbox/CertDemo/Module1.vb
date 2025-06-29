Module Module1
    Public Function ByteArrayFromBufferType(ByVal mBuff As SBUtils.TBufferTypeConst) As Byte()
        Dim Result(mBuff.Length - 1) As Byte
        Dim i As Integer

        For i = 0 To mBuff.Length - 1
            Result(i) = mBuff.Bytes(i)
        Next
        Return Result
    End Function

End Module

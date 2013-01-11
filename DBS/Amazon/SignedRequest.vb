Imports System.Security.Cryptography
Imports System.Web
Imports System.Text
Imports System.Collections.Generic

Public Class SignedRequest

    Private _EndPoint As String
    Private _Akid As String
    Private _Secret As Byte()
    Private _Signer As HMAC

    Private Const REQUEST_URI As String = "/onca/xml"
    Private Const REQUEST_METHOD As String = "GET"

    Public Property EndPoint() As String
        Get
            Return _EndPoint
        End Get
        Set(ByVal value As String)
            _EndPoint = value
        End Set
    End Property
    Public Property Akid() As String
        Get
            Return _Akid
        End Get
        Set(ByVal value As String)
            _Akid = value
        End Set
    End Property
    Public Property Secret() As Byte()
        Get
            Return _Secret
        End Get
        Set(ByVal value As Byte())
            _Secret = value
        End Set
    End Property
    Public Property Signer() As HMAC
        Get
            Return _Signer
        End Get
        Set(ByVal value As HMAC)
            _Signer = value
        End Set
    End Property

    Public ReadOnly Property AWSTimeStamp() As String
        Get
            Dim currentTime As DateTime = DateTime.UtcNow
            Return currentTime.ToString("yyyy-MM-ddTHH:mm:ssZ")
        End Get
    End Property

    '****SERVICE END POINTS *******
    '  US: ecs.amazonaws.com
    '  JP: ecs.amazonaws.jp
    '  UK: ecs.amazonaws.co.uk
    '  DE: ecs.amazonaws.de
    '  FR: ecs.amazonaws.fr
    '  CA: ecs.amazonaws.ca
    '******************************

    Public Sub New(ByVal AWSAccessKey As String, ByVal AWSSecretKey As String, ByVal Destination As String)

        Me.EndPoint = Destination.ToLower
        Me.Akid = AWSAccessKey
        Me.Secret = Encoding.UTF8.GetBytes(AWSSecretKey)
        Me.Signer = New HMACSHA256(Me.Secret)

    End Sub

    Public Function Sign(ByVal Request As IDictionary(Of String, String))
        Dim PC As New ParamComparer
        Dim SortedMap As New SortedDictionary(Of String, String)(Request, PC)

        SortedMap("AWSAccessKeyId") = Me.Akid
        SortedMap("Timestamp") = Me.AWSTimeStamp

        Dim CanonicalQS As String = ConstructCanonicalQueryString(SortedMap)

        Dim SB As New StringBuilder
        SB.Append(REQUEST_METHOD)
        SB.Append(vbLf)
        SB.Append(Me.EndPoint)
        SB.Append(vbLf) ' \n
        SB.Append(REQUEST_URI)
        SB.Append(vbLf) ' \n
        SB.Append(CanonicalQS)

        Dim StringToSign As String = SB.ToString
        Dim ToSign As Byte() = Encoding.UTF8.GetBytes(StringToSign)

        'Conpute the signature and convert to Base64
        Dim SigHash As Byte() = Signer.ComputeHash(ToSign)
        Dim Signature As String = Convert.ToBase64String(SigHash)

        'Now construct the resulting URL
        Dim QS As New StringBuilder

        QS.Append("http://")
        QS.Append(Me.EndPoint)
        QS.Append(REQUEST_URI)
        QS.Append("?")
        QS.Append(CanonicalQS)
        QS.Append("&Signature=")
        QS.Append(PercentEncodeRfc3986(Signature))

        Return QS.ToString

    End Function

    Public Function Sign(ByVal QueryString As String) As String
        Dim Request As IDictionary(Of String, String) = CreateDictionary(QueryString)
        Return Sign(Request)
    End Function

    Private Function CreateDictionary(ByVal QueryString As String) As IDictionary(Of String, String)
        Dim map As New Dictionary(Of String, String)
        Dim RequestParams() As String = QueryString.Split("&")

        For i As Integer = 0 To RequestParams.Length - 1
            If RequestParams(i).Length < 1 Then
                Continue For
            End If

            Dim Sep As Char() = {"="}
            Dim Param As String() = RequestParams(i).Split(Sep, 2)

            For j As Integer = 0 To Param.Length - 1
                Param(j) = HttpUtility.UrlDecode(Param(j), System.Text.Encoding.UTF8)
            Next

            Select Case Param.Length
                Case 1
                    If RequestParams(i).Length >= 1 Then
                        If RequestParams(i).ToCharArray()(0) = "=" Then
                            map("") = Param(0)
                        Else
                            map(Param(0)) = ""
                        End If
                    End If
                Case 2
                    If Not String.IsNullOrEmpty(Param(0)) Then
                        map(Param(0)) = Param(1)
                    End If
            End Select

        Next

        Return map
    End Function

    Private Function ConstructCanonicalQueryString(ByVal SortedMap As SortedDictionary(Of String, String)) As String
        Dim SB As New StringBuilder

        If SortedMap.Count = 0 Then
            SB.Append("")
            Return SB.ToString
        End If

        For Each kvp As KeyValuePair(Of String, String) In SortedMap
            SB.Append(PercentEncodeRfc3986(kvp.Key))
            SB.Append("=")
            SB.Append(PercentEncodeRfc3986(kvp.Value))
            SB.Append("&")
        Next

        Dim CanonicalString As String = SB.ToString
        CanonicalString = CanonicalString.Substring(0, CanonicalString.Length - 1)
        Return CanonicalString
    End Function

    Private Function PercentEncodeRfc3986(ByVal s As String)

        s = HttpUtility.UrlEncode(s, System.Text.Encoding.UTF8)
        s = s.Replace("'", "%27").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("!", "%21").Replace("%7e", "~").Replace("+", "%20")

        Dim SB As New StringBuilder(s)

        For i As Integer = 0 To SB.Length - 1
            If SB(i) = "%" Then
                If Char.IsLetter(SB(i + 1)) Or Char.IsLetter(SB(i + 2)) Then
                    SB(i + 1) = Char.ToUpper(SB(i + 1))
                    SB(i + 2) = Char.ToUpper(SB(i + 2))
                End If
            End If
        Next

        Return SB.ToString
    End Function

End Class


Public Class ParamComparer
    Implements IComparer(Of String)

    Public Function Compare(ByVal x As String, ByVal y As String) As Integer Implements System.Collections.Generic.IComparer(Of String).Compare
        Return String.CompareOrdinal(x, y)
    End Function
End Class

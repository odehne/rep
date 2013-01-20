Imports Microsoft.VisualBasic
Imports System.Web.Script.Serialization

Public Class Tools

    Public Shared Function RemoveHTMLTags(ByVal Description As String) As String
        If Not String.IsNullOrEmpty(Description) Then

            Dim RegExp As Regex = New Regex("<(.|\n)+?>")
            Return RegExp.Replace(Description, String.Empty)
        End If

        Return String.Empty
    End Function

    Public Shared Function CleanupMovieTitle(ByVal name As String) As String

        If name.Contains(" (") Then
            Return name.Substring(0, name.IndexOf("(") - 1)
        End If


        If name.Contains(" [") Then
            Return name.Substring(0, name.IndexOf("[") - 1)
        End If

        Return name
    End Function

    Public Shared Function BorrowedSinceDays(ByVal BorrowDate As Object) As String

        If TypeOf BorrowDate Is DBNull Then

            Return ""

        Else

            Dim d As Date = #1/1/1990#
            Dim days As Integer = 0

            Try
                d = CType(BorrowDate, Date)

                days = Now.Subtract(d).Days

                If Math.Abs(days) = 0 Then
                    Return "Today"
                ElseIf Math.Abs(days) = 1 Then
                    Return "Yesterday"
                Else
                    Return Math.Abs(days) & " days"
                End If

            Catch ex As Exception
                Return ""
            End Try


        End If

    End Function

    Public Shared Function SendEmail(ByVal From As String, ByVal Recipient As String, ByVal Subject As String, ByVal Msg As String) As String
        Dim MailObj As New System.Net.Mail.SmtpClient
        Dim Host As String = ""

        Host = System.Configuration.ConfigurationManager.AppSettings("MailServerAddress")

        If Host = "" Then
            Return "Mailserver address not set in web.config. Please assign a mailserver to the AppSettings section"
        End If

        MailObj.Host = Host

        Try
            MailObj.Send(From, Recipient, Subject, Msg)
            Return "Your request was sent out to " & Recipient & ". Your requested item is hopefully on it's way"
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function



    Public Shared Function Tojson(ByVal obj As Object) As String
        Dim serializer As New JavaScriptSerializer()
        Return (serializer.Serialize(obj))
    End Function
End Class

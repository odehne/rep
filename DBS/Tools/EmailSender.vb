Imports System.Net.Mail

Public Class EmailSender

    Public Shared Function SendEmail(ByVal subject As String, ByVal body As String, ByVal recipient As String) As String
        Dim ret As String = ""
        Dim myClient As New SmtpClient(BLLSettings.Settings.SmtpServer)

        myClient.Credentials = New System.Net.NetworkCredential(BLLSettings.Settings.SmtpUser, BLLSettings.Settings.SmtpPassword)

        Try
            myClient.Send(BLLSettings.Settings.SmtpReplyToAddress, recipient, subject, body)
        Catch ex As Exception
            ret = ex.Message
        End Try

        If String.IsNullOrEmpty(ret) Then ret = "OK"
        Return ret
    End Function

End Class

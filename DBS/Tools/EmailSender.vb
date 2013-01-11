Imports System.Net.Mail

Public Class EmailSender

    Private _SmtpServerAddress As String
    Private _SmtpUsername As String
    Private _SmtpPassword As String
    Private _Subject As String
    Private _Body As String
    Private _ReciepientAddress As String
    Private _ReplyToAddress As String

    Public ReadOnly Property SmtpServerAddress() As String
        Get
            If String.IsNullOrEmpty(My.Settings.SmtpServer) Then
                Return "127.0.0.1"
            Else
                Return My.Settings.SmtpServer
            End If
        End Get
    End Property

    Public ReadOnly Property SmtpUsername() As String
        Get
            Return My.Settings.SmtpUser
        End Get
    End Property

    Public ReadOnly Property ReplyToAddress() As String
        Get
            If String.IsNullOrEmpty(My.Settings.SmtpReplyToAddress) Then
                Return "no-replyto@mediamanager.de"
            Else
                Return My.Settings.SmtpReplyToAddress
            End If
        End Get
    End Property

    Public ReadOnly Property SmtpPassword() As String
        Get
            Return My.Settings.SmtpPassword
        End Get
    End Property

    Public Function SendEmail(ByVal subject As String, ByVal body As String, ByVal recipient As String) As String
        Dim ret As String = ""
        Dim myClient As New SmtpClient(Me.SmtpServerAddress)

        Try
            myClient.Send(Me.ReplyToAddress, recipient, subject, body)
        Catch ex As Exception
            ret = ex.Message
        End Try

        If String.IsNullOrEmpty(ret) Then ret = "OK"
        Return ret
    End Function

End Class

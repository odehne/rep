Imports System.Web
Public Class PasswordReset
    Implements IHttpHandler

    ''' <summary>
    '''  You will need to configure this handler in the Web.config file of your 
    '''  web and register it with IIS before being able to use it. For more information
    '''  see the following link: http://go.microsoft.com/?linkid=8101007
    ''' </summary>
#Region "IHttpHandler Members"

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            ' Return false in case your Managed Handler cannot be reused for another request.
            ' Usually this would be false in case you have some state information preserved per request.
            Return True
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim req = context.Request
        Dim email As String = ""
        Dim newPassword As String = ""
        Dim uBll As New BLLFriends
        Dim cmd As String
        Dim tempKey As String

        If Not req.QueryString("email") Is Nothing Then
            email = req.QueryString("email")
        End If

        If Not req.QueryString("cmd") Is Nothing Then
            cmd = req.QueryString("cmd")
        End If

        If Not req.QueryString("tempkey") Is Nothing Then
            tempKey = req.QueryString("tempkey")
        End If

        If Not req.QueryString("newPassword") Is Nothing Then
            newPassword = req.QueryString("newPassword")
        End If

        If cmd = "request" Then

            Dim user = uBll.GetUserByEmail(email)

            If user Is Nothing Then
                context.Response.ContentType = "application/json"
                context.Response.Write(Tools.Tojson("Die Emailadresse konnte nicht gefunden werden."))
                Return
            End If

            Try
                uBll.SetPasswordResetRequest(user.ID)
                context.Response.ContentType = "application/json"
                context.Response.Write(Tools.Tojson("OK"))
                Return
            Catch ex As Exception
                context.Response.ContentType = "application/json"
                context.Response.Write(Tools.Tojson("Es konnte leider keine Passwortanfrage and den Dienst gesendet werden."))
                Return
            End Try
        ElseIf cmd = "reset" Then

            If String.IsNullOrEmpty(tempKey) Then
                context.Response.ContentType = "application/json"
                context.Response.Write(Tools.Tojson("Die Anfrage enthielt leider keinen Passwort-Zurücksetzen-Schlüssel."))
                Return
            End If

            If uBll.ValidPasswordRequest(tempKey) = False Then
                context.Response.ContentType = "application/json"
                context.Response.Write(Tools.Tojson("Der Schlüssel zum Zurücksetzen des Passworts ist nicht mehr gültig."))
                Return
            End If

            Try
                uBll.ResetPassword(tempKey, newPassword)
                context.Response.ContentType = "application/json"
                context.Response.Write(Tools.Tojson("OK"))
                Return
            Catch ex As Exception
                context.Response.ContentType = "application/json"
                context.Response.Write(Tools.Tojson(ex.Message))
                Return
            End Try
        End If


    End Sub

#End Region

End Class

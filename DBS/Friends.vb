Imports System.Web
Imports MediaManager2010.WCFContracts.V1

Public Class Friends
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
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim req = context.Request
        Dim id As Integer = 0
        Dim username As String = ""
        Dim password As String = ""
        Dim email As String = ""

        If req.HttpMethod = "POST" Then
            context.Response.ContentType = "application/json"

            If Not req.Form("username") Is Nothing Then
                username = req.Form("username")
            End If

            If Not req.Form("password") Is Nothing Then
                password = req.Form("password")
            End If

            If Not req.Form("email") Is Nothing Then
                email = req.Form("email")
            End If

            Dim user = New BLLFriends().GetUserByName(username)

            If Not user Is Nothing Then
                context.Response.Write(Tools.Tojson("Der Benutzername ist leider schon vergeben."))
            End If

            Dim newId As Long = New BLLFriends().AddUser(New User() With {.Email = email, .Username = username, .Password = password})

            If newId <= 0 Then
                context.Response.Write(Tools.Tojson("Beim Anlegen ist ein Fehler aufgetreten :-|"))
            Else
                context.Response.Write(Tools.Tojson("OK"))
            End If
            Return
        End If

        If Not req.QueryString("username") Is Nothing Then
            username = req.QueryString("username")
        End If

        If Not req.QueryString("password") Is Nothing Then
            password = req.QueryString("password")
        End If

        If Not req.QueryString("id") Is Nothing Then
            Integer.TryParse(req.QueryString("id"), id)
        End If

        If Not String.IsNullOrEmpty(username) AndAlso Not String.IsNullOrEmpty(password) Then
            Dim user = New BLLFriends().GetUserByNameAndPassowrd(username, password)
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(user))
            Return
        End If

        If id > 0 Then
            Dim user = New BLLFriends().GetUserByID(id)
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(user))
        Else
            Dim friends = New BLLFriends().GetData()
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(friends))
        End If

        context.Response.Flush()
        context.Response.End()
    End Sub

#End Region

End Class

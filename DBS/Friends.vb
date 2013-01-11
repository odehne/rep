Imports System.Web
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
            context.Response.Write(Items.ToJson(user))
            Return
        End If

        If id > 0 Then
            Dim user = New BLLFriends().GetUserByID(id)
            context.Response.ContentType = "application/json"
            context.Response.Write(Items.ToJson(user))
        Else
            Dim friends = New BLLFriends().GetData()
            context.Response.ContentType = "application/json"
            context.Response.Write(Items.ToJson(friends))
        End If

        context.Response.Flush()
        context.Response.End()
    End Sub

#End Region

End Class

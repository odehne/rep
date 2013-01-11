Imports System.Web
Public Class Actors
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
        Dim letter As String = String.Empty

        If Not req.QueryString("letter") Is Nothing Then
            letter = req.QueryString("letter")
        End If

        If Not String.IsNullOrEmpty(letter) Then
            Dim actors = New BLLParticipants().GetParticipantsBeginningWith(letter)
            context.Response.ContentType = "application/json"
            context.Response.Write(Items.ToJson(actors))
        Else
            Dim cast = New BLLParticipants().GetData()
            context.Response.ContentType = "application/json"
            context.Response.Write(Items.ToJson(cast))
        End If

        context.Response.Flush()
        context.Response.End()

    End Sub

#End Region

End Class

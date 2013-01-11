Imports System.Web
Imports System.Runtime.Remoting.Contexts
Imports System.Drawing
Imports System.IO
Imports MediaManager2010.WCFContracts.V1

Public Class MovieImageHandler
    Implements IHttpHandler

    ''' <summary>
    '''  You will need to configure this handler in the web.config file of your 
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

        Dim ID As Long = 0
        Dim Size As String = ""
        Dim b() As Byte = Nothing


        If Not String.IsNullOrEmpty(context.Request.QueryString("Size")) Then
            Size = context.Request.QueryString("Size")
        Else
            Size = "medium"
        End If


        If Long.TryParse(context.Request.QueryString("ID"), ID) = False Then
            ShowMessage(context, "Parameter ID not found")
            Return
        End If

        Dim mBLL As BLLItems = New BLLItems()
        Dim iBLL As BLLImages = New BLLImages
        Dim mi As MovieItem = mBLL.GetItemByID(ID)
        Dim AlternativeRedirectUrl As String = ""

        If mi Is Nothing Then
            ShowMessage(context, "Item cannot be found [" & ID & "]")
            Return
        End If

        Select Case Size.ToLower
            Case "small"
                b = BLLImages.LoadImage(ID, BLLImages.ImageSizeE.Small)
                AlternativeRedirectUrl = mi.SmallImageUrl
            Case "medium"
                b = BLLImages.LoadImage(ID, BLLImages.ImageSizeE.Medium)
                AlternativeRedirectUrl = mi.MediumImageUrl
            Case "big", "large"
                b = BLLImages.LoadImage(ID, BLLImages.ImageSizeE.Big)
                AlternativeRedirectUrl = mi.LargeImageUrl
            Case Else
                b = BLLImages.LoadImage(ID, BLLImages.ImageSizeE.Small)
                AlternativeRedirectUrl = mi.LargeImageUrl
        End Select

        If Not b Is Nothing Then
            context.Response.ContentType = "image/jpeg"
            context.Response.BinaryWrite(b)
            Try
                context.Response.Flush()

            Catch ex As Exception

            End Try
            Return
        End If

        If Not String.IsNullOrEmpty(AlternativeRedirectUrl) Then
            context.Response.Redirect(AlternativeRedirectUrl)
            Return
        End If


    End Sub

    Private Sub ShowMessage(ByVal context As HttpContext, ByVal Msg As String)
        Context.Response.ContentType = "text/html"
        context.Response.Write(Msg)
    End Sub

#End Region

End Class

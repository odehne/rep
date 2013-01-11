Imports System.IO
Imports System.Net
Imports System.Data.SqlClient

Public Class BLLImages

    Private Const PREFIX As String = "http://"

    Public Enum ImageSizeE
        Small
        Medium
        Big
    End Enum

    Public Shared Function LoadImage(ByVal ItemID As Long, Optional ByVal Size As ImageSizeE = ImageSizeE.Big) As Byte()
        Dim Image As Byte() = Nothing
        Dim ImageContext As New MediaLibraryLinqDataContext()

        Dim source = From r In ImageContext.tblImages _
                     Where r.ItemID = ItemID _
                     Select r

        If source.Count > 0 Then

            Select Case Size
                Case ImageSizeE.Small
                    If Not source.First.SmallImage Is Nothing Then
                        Image = source.First.SmallImage.ToArray
                    End If

                Case ImageSizeE.Medium
                    If Not source.First.MediumImage Is Nothing Then
                        Image = source.First.MediumImage.ToArray
                    End If
                Case ImageSizeE.Big
                    If Not source.First.LargeImage Is Nothing Then
                        Image = source.First.LargeImage.ToArray
                    End If
            End Select

            Return Image

        End If

        Return Nothing
    End Function

    Public Shared Function StoreImage(ByVal ItemID As Long, ByVal Url As String, Optional ByVal Size As ImageSizeE = ImageSizeE.Big) As String
        Dim Image As Byte() = Nothing
        Dim ImageContext As New MediaLibraryLinqDataContext()
        Dim Img As tblImage = Nothing
        Dim IsNew As Boolean = False

        Dim source = From r In ImageContext.tblImages _
                     Where r.ItemID = ItemID _
                     Select r

        If source.Count > 0 Then
            Img = source.First
        Else
            Img = New tblImage
            Img.ItemID = ItemID
            IsNew = True
        End If

        Select Case Size
            Case ImageSizeE.Small
                Img.SmallImage = Download(Url)
            Case ImageSizeE.Medium
                Img.MediumImage = Download(Url)
            Case ImageSizeE.Big
                Img.LargeImage = Download(Url)
        End Select

        Dim i As Integer = ImageContext.GetChangeSet.Updates().Count

        Try
            If IsNew Then ImageContext.tblImages.InsertOnSubmit(Img)
            ImageContext.SubmitChanges()
        Catch ex As Exception
            Return ex.Message
        End Try

        Return "OK"
    End Function

    Public Shared Function Download(ByVal URL As String) As Byte()
        Dim Img As MemoryStream
        Dim WC As WebClient = New WebClient

        If Not URL.ToLower().StartsWith("http://") Then URL = PREFIX + URL

        Try
            Img = New MemoryStream(WC.DownloadData(URL))
        Catch ex As Exception
            Throw New Exception("Creating memory stream failed [" & ex.Message & "]")
        End Try

        Try
            Dim Buffer(Img.Length) As Byte
            Buffer = Img.ToArray
            Return Buffer
        Catch ex As Exception
            Throw New Exception("Allocatin image buffer failed [" & ex.Message & "]")
        End Try

    End Function

End Class

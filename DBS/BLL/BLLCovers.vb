Imports MediaManager2010.MediaManager2010

Public Class BLLCovers

   

    Public Function GetData() As List(Of WCFContracts.V1.Cover)
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblCovers _
                         Select co = New WCFContracts.V1.Cover() With { _
                            .ID = r.ID, _
                            .ItemID = r.ID, _
                            .Large = r.Large}

            Return source.ToList
        End Using

    End Function

    Public Function GetCoverByID(ByVal ID As Integer) As WCFContracts.V1.Cover

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblCovers _
                     Select co = New WCFContracts.V1.Cover() With { _
                        .ID = r.ID, _
                        .ItemID = r.ItemID, _
                        .Large = r.Large} _
                    Where co.ID = ID

            Return source
        End Using
    End Function

    Public Function GetCover(ByVal ItemID As Integer) As WCFContracts.V1.Cover
      
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblCovers _
                    Select co = New WCFContracts.V1.Cover() With { _
                        .ID = r.ID, _
                        .ItemID = r.ItemID, _
                        .Large = r.Large} _
                    Where co.ItemID = ItemID

            Return source
        End Using
    End Function

    Public Function AddCover(ByVal ItemID As Integer, _
                             ByVal Url As String) As String

        'Dim Cover As Byte()
        'Dim lRet As Integer = 0
        'Dim ID As ImageDownloader = New ImageDownloader

        'Dim c As MediaLibrary.tblCoversDataTable = Nothing

        'Try
        '    Cover = ID.Download(Url)
        'Catch ex As Exception
        '    Return ex.Message
        'End Try

        'c = Me.Adapter.GetDataByItemID(ItemID)

        'If Not c.Rows.Count > 0 Then
        '    lRet = UpdateCover(ItemID, Cover)
        'Else
        '    lRet = AddCover(ItemID, Cover)
        'End If

        'If lRet > 0 Then
        '    Return "OK"
        'Else
        '    Return "Storing cover images failed"
        'End If

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, False)> _
       Public Function AddCover(ByVal ItemID As Integer, _
                                ByVal bCover As Byte()) As Integer

        'Dim ID As Integer = 0
        'Dim Covers As New MediaLibrary.tblCoversDataTable
        'Dim Cover As MediaLibrary.tblCoversRow = Nothing

        'Cover = Covers.NewtblCoversRow

        'Cover.ItemID = ItemID
        'Cover.Large = bCover

        'Covers.AddtblCoversRow(Cover)

        'Try
        '    ID = Adapter.Update(Cover)
        'Catch ex As Exception
        '    Throw New Exception("Storing cover information failed [" & ex.Message & "]")
        'End Try

        'Return ID

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, False)> _
       Public Function UpdateCover(ByVal ItemID As Integer, _
                                    ByVal bCover As Byte()) As Integer

        'Dim Cover As MediaLibrary.tblCoversRow = Nothing
        'Dim Covers As New MediaLibrary.tblCoversDataTable
        'Dim RowsAffected As Integer = 0

        'Covers = Adapter.GetDataByItemID(ItemID)

        'If Covers.Rows.Count > 0 Then
        '    Cover = Covers(0)
        'End If

        'Cover.Large = bCover

        'RowsAffected = Adapter.Update(Cover)

        'Return RowsAffected

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
       Public Function DeleteCovers(ByVal ItemID As Integer) As String
        'Dim rowsAffected As Integer = 0

        'Try
        '    rowsAffected = Adapter.DeleteByItemID(ItemID)
        'Catch ex As Exception
        '    Throw New Exception("Failed to delete Covers", ex)
        'End Try

        'Return rowsAffected = 1
    End Function

    'public Image GetThumbnail(string strFilename, int nPixelSize, bool bPortrait)
    '{
    '  Image img2Scale = Image.FromFile(strFilename);
    '  Image imgThumb = GetThumbnail(img2Scale, nPixelSize, bPortrait);
    '  img2Scale.Dispose();  // cleanup
    '  return imgThumb;      
    '}

    'public Image GetThumbnail(Image imgFullSize, int nPixelSize, bool bPortrait)
    '{
    '  int nImageWidth = imgFullSize.Width;
    '  int nImageHeight = imgFullSize.Height;
    '  int nScalePercentage = 0;
    '  if (bPortrait)
    '  {
    '    nScalePercentage = (int)(nPixelSize * 100.0 / (double)nImageHeight);
    '    nImageWidth = (int)((double)nImageWidth * ((double)nScalePercentage/100.0));
    '    nImageHeight = nPixelSize;
    '  }
    '  else
    '  {
    '    nScalePercentage = (int)(nPixelSize * 100.0 / (double)nImageWidth);  
    '    nImageHeight = (int)((double)nImageHeight * ((double)nScalePercentage/100.0));
    '    nImageWidth = nPixelSize;
    '  }
    '  return GetThumbnail(imgFullSize, nImageWidth, nImageHeight);
    '}

    Private Function CreateDataContext() As MediaLibraryLinqDataContext
        Return New MediaLibraryLinqDataContext
    End Function
End Class

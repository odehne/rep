Imports MediaManager2010.WCFContracts.V1

Public Class BLLMediaFormat
    
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetData() As List(Of MediaFormat)
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblMediaFormats _
                     Select co = New MediaFormat() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description}

        Return source.ToList

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
   Public Function GetMediaFormatByID(ByVal ID As Integer) As MediaFormat
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblMediaFormats _
                     Select co = New MediaFormat() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description} _
                        Where co.ID = ID

        Return source

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetMediaFormatByName(ByVal Name As String) As MediaFormat
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = (From r In d.tblMediaFormats _
                      Select co = New wcfContracts.V1.MediaFormat() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description} _
                        Where co.Name = Name).ToList
        If source.Count > 0 Then
            Return source.First
        Else
            Return Nothing
        End If
    End Function

    Public Function AddMediaFormat(ByVal Name As String, _
                                   Optional ByVal Description As String = "") As Integer

        Dim mf As MediaFormat = GetMediaFormatByName(Name)

        If Not mf Is Nothing Then
            Return mf.ID
        Else
            Dim MyMediaFormat As New tblMediaFormat
            Dim FormatConext As New MediaLibraryLinqDataContext()
            MyMediaFormat.Name = Name
            MyMediaFormat.Description = Description

            FormatConext.tblMediaFormats.InsertOnSubmit(MyMediaFormat)
            FormatConext.SubmitChanges()
            Return MyMediaFormat.ID

        End If

       
    End Function

    Public Function UpdateMediaFormat(ByVal ID As Integer, _
                                   ByVal Name As String, _
                                   Optional ByVal Description As String = "") As Integer

        Dim MyMediaFormat As New tblMediaFormat
        Dim FormatContext As New MediaLibraryLinqDataContext()

        Dim source = From r In FormatContext.tblMediaFormats _
                     Where r.ID = ID _
                     Select co = New MediaFormat() With { _
                       .ID = r.ID, _
                       .Name = r.Name, _
                       .Description = r.Description}

        If source.Count() > 0 Then
            source(0).Name = Name
            source(0).Description = Description
        End If

        FormatContext.SubmitChanges()

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
     Public Function DeleteMediaFormat(ByVal ID As Integer) As Integer
        Dim FormatContext As New MediaLibraryLinqDataContext()

        Dim MFormat = (From mf In FormatContext.tblMediaFormats _
                   Where mf.ID = ID _
                   Select mf).First

        If Not MFormat Is Nothing Then
            FormatContext.tblMediaFormats.DeleteOnSubmit(MFormat)
        End If

        Dim i As Integer = FormatContext.GetChangeSet().Updates.Count

        FormatContext.SubmitChanges()

        Return i

    End Function

End Class

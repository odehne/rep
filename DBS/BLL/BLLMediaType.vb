Imports MediaManager2010.WCFContracts.V1

Public Class BLLMediaType

    'Private _Adapter As MediaLibraryTableAdapters.tblMediaTypeTableAdapter

    'Public ReadOnly Property Adapter() As MediaLibraryTableAdapters.tblMediaTypeTableAdapter
    '    Get
    '        If _Adapter Is Nothing Then _Adapter = New MediaLibraryTableAdapters.tblMediaTypeTableAdapter

    '        Return _Adapter
    '    End Get
    'End Property

    Public Function GetData() As List(Of MediaType)
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblMediaTypes _
                     Select co = New MediaType() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description}

        Return source.ToList
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
   Public Function GetMediaTypeByID(ByVal ID As Integer) As MediaType
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblMediaTypes _
                     Select co = New MediaType() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description} _
                    Where co.ID = ID

        Return source

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
      Public Function GetMediaTypeByName(ByVal name As String) As MediaType
        Dim d As New MediaLibraryLinqDataContext()

       
            Dim source = From r In d.tblMediaTypes _
                         Select co = New MediaType() With { _
                            .ID = r.ID, _
                            .Name = r.Name, _
                            .Description = r.Description} _
                        Where co.Name = name
            If source.Count > 0 Then
                Return source.First
            Else
                Return Nothing
            End If

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, False)> _
       Public Function AddMediaType(ByVal Name As String, _
                                    Optional ByVal Description As String = "") As Integer


        If Not String.IsNullOrEmpty(Name) Then

            Dim mt As MediaType = GetMediaTypeByName(Name)

            If Not mt Is Nothing Then
                Return mt.ID
            Else
                Dim m As New tblMediaType
                Dim c As New MediaLibraryLinqDataContext()

                m.Name = Name
                m.Description = Description

                c.tblMediaTypes.InsertOnSubmit(m)
                c.SubmitChanges()
                Return m.ID

            End If
        End If
        Return 0

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, False)> _
    Public Function UpdateMediaType(ByVal ID As Integer, _
                                    ByVal Name As String, _
                                    Optional ByVal Description As String = "") As Integer

        If Not String.IsNullOrEmpty(Name) Then
            Dim MyMediaType As New tblMediaType
            Dim FormatConext As New MediaLibraryLinqDataContext()

            Dim source = From r In FormatConext.tblMediaTypes _
                         Where r.ID = ID _
                         Select co = New MediaType() With { _
                           .ID = r.ID, _
                           .Name = r.Name, _
                           .Description = r.Description}



            If source.Count() = 1 Then
                source(0).Name = Name
                source(0).Description = Description
            End If

            FormatConext.SubmitChanges()
        End If

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
       Public Function DeleteMediaType(ByVal ID As Integer) As String
            Throw New NotImplementedException

    End Function

End Class


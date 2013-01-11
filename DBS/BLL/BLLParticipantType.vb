
Imports MediaManager2010.WCFContracts.V1

Public Class BLLParticipantType
    'Private _Adapter As MediaLibraryTableAdapters.tblParticipantTypeTableAdapter

    'Public ReadOnly Property Adapter() As MediaLibraryTableAdapters.tblParticipantTypeTableAdapter
    '    Get
    '        If _Adapter Is Nothing Then _Adapter = New MediaLibraryTableAdapters.tblParticipantTypeTableAdapter

    '        Return _Adapter
    '    End Get
    'End Property

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetData() As List(Of ParticipantType)
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblParticipantTypes _
                     Select co = New ParticipantType() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description}

        Return source.ToList
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
  Public Function GetParticipantTypeByName(ByVal Name As String) As ParticipantType
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = (From r In d.tblParticipantTypes _
                     Select co = New ParticipantType() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description} _
                        Where co.Name = Name).ToList
        Return source.First

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
   Public Function GetParticipantTypeByID(ByVal ID As Integer) As ParticipantType
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblParticipants _
                     Select co = New ParticipantType() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description} _
                        Where co.ID = ID
        Return source

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, False)> _
       Public Function AddParticipantType(ByVal Name As String, _
                                          Optional ByVal Description As String = "") As Integer

        Dim m As New tblParticipantType
        Dim c As New MediaLibraryLinqDataContext()

        Dim pt As ParticipantType = Me.GetParticipantTypeByName(Name)

        If Not pt Is Nothing Then
            Return pt.ID
        End If

        m.Name = Name
        m.Description = Description

        c.tblParticipantTypes.InsertOnSubmit(m)
        Try
            c.SubmitChanges()
        Catch ex As Exception

        End Try

        Return m.ID

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, False)> _
      Public Function UpdateParticipantType(ByVal ID As Integer, _
                                            ByVal Name As String, _
                                            Optional ByVal Description As String = "") As Integer

        Dim pm As New tblParticipantType
        Dim FormatConext As New MediaLibraryLinqDataContext()

        Dim source = From r In FormatConext.tblParticipantTypes _
                     Where r.ID = ID _
                     Select co = New Participant() With { _
                       .ID = r.ID, _
                       .Name = r.Name, _
                       .Description = r.Description}



        If source.Count() = 1 Then
            source(0).Name = Name
            source(0).Description = Description
        End If

        FormatConext.SubmitChanges()

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
    Public Function DeleteParticipantType(ByVal ID As Integer) As String
        Throw New NotImplementedException
    End Function
End Class

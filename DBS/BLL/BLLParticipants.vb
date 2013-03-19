
Imports MediaManager2010.BLL.Interfaces
Imports MediaManager2010.WCFContracts.V1

Public Class BLLParticipants
    Implements IParticipantsRepository

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetData() As List(Of Participant) Implements IParticipantsRepository.GetData
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblParticipants _
                     Select co = New Participant() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .ParticipantTypeID = r.ParticipantTypeID, _
                        .Description = r.Description}

        Return source.ToList
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetParticipantsLikeName(ByVal Name As String) As List(Of Participant) Implements IParticipantsRepository.GetParticipantsLikeName
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblParticipants _
                     Select co = New Participant() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .ParticipantTypeID = r.ParticipantTypeID, _
                        .Description = r.Description} _
                        Where co.Name Like Name


        Return source.ToList
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetParticipantsLikeNameAndPTypeID(ByVal Name As String, ByVal ID As Integer) As List(Of Participant) Implements IParticipantsRepository.GetParticipantsLikeNameAndPTypeID
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblParticipants _
                     Select co = New Participant() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .ParticipantTypeID = r.ParticipantTypeID, _
                        .Description = r.Description} _
                        Where co.Name Like Name And co.ParticipantTypeID = ID

        Return source.ToList
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetParticipantsBeginningWith(ByVal letter As String) As List(Of Participant) Implements IParticipantsRepository.GetParticipantsBeginningWith
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblParticipants Order By r.Name _
                     Select co = New Participant() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .ParticipantTypeID = r.ParticipantTypeID, _
                        .Description = r.Description} _
                        Where co.Name.StartsWith(letter)


        Return source.ToList
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetParticipantsByTypeID(ByVal TypeID As Integer) As List(Of Participant) Implements IParticipantsRepository.GetParticipantsByTypeID
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblParticipants _
                     Select co = New Participant() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .ParticipantTypeID = r.ParticipantTypeID, _
                        .Description = r.Description} _
                        Where co.ParticipantTypeID = TypeID

        Return source.ToList
    End Function


    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetParticipantByID(ByVal ID As Integer) As Participant Implements IParticipantsRepository.GetParticipantByID
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblParticipants _
                     Select co = New Participant() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .ParticipantTypeID = r.ParticipantTypeID, _
                        .Description = r.Description} _
                        Where co.ID = ID

        Return source
    End Function




    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetParticipantByName(ByVal Name As String) As Participant Implements IParticipantsRepository.GetParticipantByName
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = (From r In d.tblParticipants _
                     Select co = New Participant() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .ParticipantTypeID = r.ParticipantTypeID, _
                        .Description = r.Description} _
                        Where co.Name = Name).ToList

        If source.Count > 0 Then
            Return source.First
        Else
            Return Nothing
        End If

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, False)> _
    Public Function AddParticipant(ByVal Name As String, _
                                      ByVal ParticipantTypeID As String, _
                                      Optional ByVal Description As String = "") As Integer Implements IParticipantsRepository.AddParticipant

        If Not String.IsNullOrEmpty(Name) Then
            Dim c As New MediaLibraryLinqDataContext()
            Dim p As Participant = GetParticipantByName(Name)

            If Not p Is Nothing Then
                Return p.ID
            Else
                Dim m As New tblParticipant
                m.Name = Name
                m.Description = Description
                m.ParticipantTypeID = ParticipantTypeID

                c.tblParticipants.InsertOnSubmit(m)
                c.SubmitChanges()

                Return m.ID
            End If
        End If
        Return 0
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, False)> _
    Public Function UpdateParticipant(ByVal ID As Integer, _
                                        ByVal Name As String, _
                                        ByVal ParticipantTypeID As String, _
                                        Optional ByVal Description As String = "") As Integer Implements IParticipantsRepository.UpdateParticipant
        Dim pm As New tblParticipant
        Dim FormatConext As New MediaLibraryLinqDataContext()

        If Not String.IsNullOrEmpty(Name) Then

            Dim source = From r In FormatConext.tblParticipants _
                         Where r.ID = ID _
                         Select co = New Participant() With { _
                           .ID = r.ID, _
                           .Name = r.Name, _
                           .Description = r.Description, _
                           .ParticipantTypeID = r.ParticipantTypeID}



            If source.Count() = 1 Then
                source(0).Name = Name
                source(0).Description = Description
                source(0).ParticipantTypeID = ParticipantTypeID
            End If

            FormatConext.SubmitChanges()
        End If

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
    Public Function DeleteParticipant(ByVal ID As Integer) As String Implements IParticipantsRepository.DeleteParticipant

        Throw New NotImplementedException
        'Dim rowsAffected As Integer = 0

        'Try
        '    rowsAffected = Adapter.DeleteParticipant(ID)
        'Catch ex As Exception
        '    Throw New Exception("Failed to delete Participant", ex)
        'End Try

        'Return rowsAffected
    End Function

End Class

'Partial Public Class MediaLibrary

'Partial Public Class tblParticipantsRow

'    Public ReadOnly Property ParticipantTypeName() As String
'        Get
'            Dim bp As BLLParticipantType = New BLLParticipantType
'            Dim r As MediaLibrary.tblParticipantTypeRow = Nothing

'            r = bp.GetParticipantTypeByID(Me.ParticipantTypeID)

'            If Not r Is Nothing Then
'                Return r.Name
'            End If

'            Return Nothing
'        End Get
'    End Property

'    Public Function GetParticipantTypeName() As String
'        Dim bp As BLLParticipantType = New BLLParticipantType
'        Dim r As MediaLibrary.tblParticipantTypeRow = Nothing

'        r = bp.GetParticipantTypeByID(Me.ParticipantTypeID)

'        If Not r Is Nothing Then
'            Return r.Name
'        End If

'        Return Nothing

'    End Function

'End Class

'End Class

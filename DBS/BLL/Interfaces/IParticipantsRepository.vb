Imports MediaManager2010.WCFContracts.V1

Namespace BLL.Interfaces

    Public Interface IParticipantsRepository
        Function GetData() As List(Of Participant)
        Function GetParticipantsLikeName(ByVal Name As String) As List(Of Participant)
        Function GetParticipantsLikeNameAndPTypeID(ByVal Name As String, ByVal ID As Integer) As List(Of Participant)
        Function GetParticipantsBeginningWith(ByVal letter As String) As List(Of Participant)
        Function GetParticipantsByTypeID(ByVal TypeID As Integer) As List(Of Participant)
        Function GetParticipantByID(ByVal ID As Integer) As Participant
        Function GetParticipantByName(ByVal Name As String) As Participant
        Function AddParticipant(ByVal Name As String, _
                                ByVal ParticipantTypeID As String, _
                                Optional ByVal Description As String = "") As Integer
        Function UpdateParticipant(ByVal ID As Integer, _
                                   ByVal Name As String, _
                                   ByVal ParticipantTypeID As String, _
                                   Optional ByVal Description As String = "") As Integer

        Function DeleteParticipant(ByVal ID As Integer) As String
    End Interface
End Namespace
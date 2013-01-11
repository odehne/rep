Imports System.Runtime.Serialization


Namespace WCFContracts.V1
<DataContract(Name:="Participant")> _
Public Class Participant

    <DataMember()> _
    Public ID As Integer

    <DataMember()> _
    Public Name As String

    <DataMember()> _
    Public ParticipantTypeID As Integer

    <DataMember()> _
    Public Description As String

End Class
End Namespace
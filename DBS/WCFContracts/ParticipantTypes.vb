Imports System.Runtime.Serialization


Namespace WCFContracts.V1
<DataContract(Name:="ParticipantType")> _
Public Class ParticipantType

    <DataMember()> _
    Public ID As Integer

    <DataMember()> _
    Public Name As String

    <DataMember()> _
    Public Description As String

End Class
End Namespace
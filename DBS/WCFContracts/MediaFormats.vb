Imports System.Runtime.Serialization


Namespace WCFContracts.V1
<DataContract(Name:="MediaFormat")> _
Public Class MediaFormat

    <DataMember()> _
    Public ID As Integer

    <DataMember()> _
    Public Name As String

    <DataMember()> _
    Public Description As String

End Class
End Namespace
Imports System.Runtime.Serialization

Namespace WCFContracts.V1
    <DataContract(Name:="Genre")> _
    Public Class Genre

        <DataMember()> _
        Public ID As Integer

        <DataMember()> _
        Public Name As String

        <DataMember()> _
        Public Description As String

    End Class

End Namespace
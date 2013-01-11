Imports System.Runtime.Serialization

Namespace WCFContracts.V1

    <DataContract(Name:="Role")> _
    Public Class Role

        <DataMember()>
        Public ID As Integer

        <DataMember()>
        Public Rolename As String

        <DataMember()>
        Public ApplicationName As String

    End Class

    <DataContract(Name:="UsersInRole")> _
    Public Class UsersInRole

        <DataMember()>
        Public ID As Integer

        <DataMember()>
        Public Rolename As String

        <DataMember()>
        Public ApplicationName As String

    End Class

End Namespace

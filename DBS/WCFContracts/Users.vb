Imports System.Runtime.Serialization

Namespace WCFContracts.V1

    <DataContract(Name:="User")> _
    Public Class User

        <DataMember()> _
        Public ID As Integer

        <DataMember()> _
        Public Email As String

        <DataMember()> _
        Public Description As String

        <DataMember()> _
        Public Password As String

        <DataMember()> _
        Public Username As String

        <DataMember()> _
        Public Fon As String

        <DataMember()> _
        Public ApplicationName As String

        <DataMember()>
        Public IsLockedOut As Boolean

        <DataMember()>
        Public IsOnline As Boolean

        <DataMember()>
        Public FailureCount As Integer

        <DataMember()>
        Public LastLoginDate As Date

        <DataMember()>
        Public LastPasswordChangedDate As Date

        <DataMember()>
        Public PasswordAnswer As String

        <DataMember()>
        Public PasswordQuestion As String

    End Class
End Namespace

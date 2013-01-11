Imports System.Runtime.Serialization

Namespace WCFContracts.V1

    <DataContract(Name:="Cover")> _
    Public Class Cover

        <DataMember()> _
        Public ID As Integer

        <DataMember()> _
        Public ItemID As Integer

        <DataMember()> _
        Public Large As System.Data.Linq.Binary

    End Class

End Namespace

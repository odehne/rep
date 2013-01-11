Imports System.Runtime.Serialization


Namespace WCFContracts.V1
<DataContract(Name:="NotReferencedType")> _
Public Class NotReferencedType
    <DataMember()> _
   Public Name As String

    <DataMember()> _
    Public ID As Long
End Class
End Namespace
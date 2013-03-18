Namespace WCFContracts
    <DataContract()>
    Public Class OnlyTitle
        <DataMember()>
        Public title As String
        <DataMember()>
        Public id As Integer

        Public Sub New(r As tblItem)
            id = r.ID
            title = Tools.CleanupMovieTitle(r.Name)
        End Sub
    End Class
End Namespace
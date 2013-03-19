Imports MediaManager2010.WCFContracts.V1

Namespace BLL.Interfaces

    Public Interface IFriendsRepository
        Function GetData() As List(Of User)
        Function ConvertToServiceUser(ByVal dbuser As tblUser) As User
        Function GetUsersLikeName(ByVal Username As String) As List(Of User)
        Function GetUserByID(ByVal ID As Integer) As User
        Function GetTblUserByID(ByVal ID As Integer) As tblUser
        Function GetUserByName(ByVal Username As String) As User
        Function GetUserByOwnerName(ByVal Username As String) As User
        Function GetUserByEmail(ByVal Email As String) As User
        Function AddUser(ByVal U As User) As Long
        Function UpdateUser(ByVal u As User) As Integer
        Function UpdatePassword(ByVal ID As Integer, ByVal Password As String) As Integer
        Function DeleteUser(ByVal ID As Integer) As Integer
        Function GetUserByNameAndPassowrd(name As String, password As String) As UserOnly
        Function ValidPasswordRequest(g As String) As Boolean
        Sub SetPasswordResetRequest(ByVal id As Integer)
        Sub ResetPassword(ByVal tempKey As String, ByVal newPassword As String)
    End Interface
End Namespace
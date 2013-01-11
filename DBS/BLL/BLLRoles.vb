Public Class BLLRoles

    Private _Context As MediaLibraryLinqDataContext

    Public ReadOnly Property Context() As MediaLibraryLinqDataContext
        Get
            If _Context Is Nothing Then _Context = New MediaLibraryLinqDataContext
            Return _Context
        End Get
    End Property

    Public Function GetUsersAndRoles() As List(Of tblUsersInRole)

        Dim tempContext As MediaLibraryLinqDataContext = New MediaLibraryLinqDataContext

        Dim source = (From ur In tempContext.tblUsersInRoles).ToList

        If source.Count > 0 Then
            Return source
        Else
            Return New List(Of tblUsersInRole)
        End If

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRoles() As String()

        Dim s() As String = Nothing

        Dim source = From r In Context.tblRoles _
                     Select r

        If source.Count = 0 Then
            AddRole("admins", "mm2010")
        End If

        Dim ar(source.Count) As String
        Dim i As Integer = 0

        For Each ur As tblRole In source

            ar(i) = ur.Rolename
            i += 1

        Next

        Return ar

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRoleByName(ByVal Rolename As String) As tblRole

        Dim source = From r In Context.tblRoles _
                      Where r.Rolename.ToLower = Rolename.ToLower

        If source.Count > 0 Then
            Return source.First
        Else
            Return Nothing
        End If

    End Function

   

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function AddRole(ByVal Rolename As String, ByVal ApplicationName As String) As String
        Dim ret As String = ""
        Dim RowsAffected As Integer = 0

        Dim r As tblRole = GetRoleByName(Rolename)

        If Not r Is Nothing Then
            Return "OK"
        Else

            r = New tblRole
            r.Rolename = Rolename

            If String.IsNullOrEmpty(ApplicationName) Then ApplicationName = "mm2010"

            r.ApplicationName = ApplicationName

            Context.tblRoles.InsertOnSubmit(r)

            Dim i As Integer = Context.GetChangeSet().Inserts.Count

            Try
                Context.SubmitChanges()
            Catch ex As Exception
                ret = ex.Message
            End Try

        End If

        If ret = "" Then ret = "OK"
        Return ret

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteRole(ByVal Rolename As String, ByVal ApplicationName As String) As String
        Dim ret As String = ""
        Dim RowsAffected As Integer = 0
        Dim Role As tblRole = GetRoleByName(Rolename)


        If Not Role Is Nothing Then
            Context.tblRoles.DeleteOnSubmit(Role)
        End If

        Dim i As Integer = Context.GetChangeSet().Updates.Count

        Try
            Context.SubmitChanges()
        Catch ex As Exception
            ret = ex.Message
        End Try

        If ret = "" Then ret = "OK"
        Return ret
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, False)> _
    Public Function UpdateRole(ByVal Rolename As String, ByVal ApplicationName As String) As String
        Dim ret As String = ""
        Dim RowsAffected As Integer = 0
        Dim Role As tblRole = GetRoleByName(Rolename)


        If Not Role Is Nothing Then

            Role.Rolename = Rolename
            Role.ApplicationName = ApplicationName

            RowsAffected = Context.GetChangeSet().Updates.Count

            Try
                Context.SubmitChanges()
            Catch ex As Exception
                ret = ex.Message
            End Try

        End If



        If ret = "" Then ret = "OK"
        Return ret

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, False)> _
    Public Function AddUserToRole(ByVal Username As String, ByVal Rolename As String, ByVal ApplicationName As String) As String
        Dim ret As String = ""
        Dim ur As tblUsersInRole = New tblUsersInRole
        Dim RowsAffected As Integer = 0

        Dim Users() As String = GetUsersInRole(Rolename)

        ret = AddRole(Rolename, ApplicationName)

        For Each User As String In Users
            If User.ToLower = Username.ToLower Then
                'Es gibt bereits einen benutzer, dem diese Rolle zugewiesen ist
                Return "OK"
            End If
        Next

        'Neue User Reference anlegen
        ur.UserName = Username
        ur.RoleName = Rolename
        ur.ApplicationName = ApplicationName

        Context.tblUsersInRoles.InsertOnSubmit(ur)

        RowsAffected = Context.GetChangeSet().Inserts.Count

        Try
            Context.SubmitChanges()
        Catch ex As Exception
            ret = ex.Message
        End Try

        If ret = "" Then ret = "OK"
        Return ret

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function RoleExists(ByVal Rolename As String) As Boolean

        Dim source = (From ur In Context.tblRoles _
                      Where ur.Rolename = Rolename).ToList

        If source.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function IsUserInRole(ByVal UserName As String, ByVal Rolename As String) As Boolean

        Dim src = From ur In Context.tblUsersInRoles Where ur.RoleName.ToLower = Rolename.ToLower And ur.UserName.ToLower = UserName.ToLower

        If src.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetUsersInRole(ByVal Rolename As String) As String()

        Dim source = (From ur In Context.tblUsersInRoles _
                      Where ur.RoleName = Rolename).ToList

        Dim ar(source.Count - 1) As String
        Dim i As Integer = 0

        For Each ur As tblUsersInRole In source

            ar(i) = ur.UserName
            i += 1

        Next

        Return ar
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetRolesForUser(ByVal UserName As String) As String()

        Dim source = (From ur In Context.tblUsersInRoles _
                      Where ur.UserName.ToLower = UserName.ToLower)


        If source.Count > 0 Then

            Dim ar(source.Count - 1) As String
            Dim i As Integer = 0

            For Each ur As tblUsersInRole In source.ToList

                ar(i) = ur.RoleName
                i += 1

            Next

            Return ar


        End If

        Return Nothing

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function FindUserInRole(ByVal Rolename As String, ByVal EmailToMatch As String) As String()

        Dim source = (From ur In Context.tblUsersInRoles _
                      Where ur.RoleName.ToLower = Rolename.ToLower _
                      And ur.UserName.ToLower = EmailToMatch.ToLower).ToList

        Dim ar(source.Count) As String
        Dim i As Integer = 0

        For Each ur As tblUsersInRole In source

            ar(i) = ur.UserName
            i += 1

        Next

        Return ar
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
    Public Function DeleteUserAndRoleFromUserToRole(ByVal UserName As String, ByVal Rolename As String, ByVal ApplicationName As String) As String
        Dim ret As String = ""

        Dim source = (From ur In Context.tblUsersInRoles _
                      Where ur.UserName.ToLower = UserName.ToLower _
                      And ur.ApplicationName.ToLower = ApplicationName.ToLower _
                      And ur.RoleName.ToLower = Rolename.ToLower _
                      Select ur).First

        If Not source Is Nothing Then Context.tblUsersInRoles.DeleteOnSubmit(source)

        Dim i As Integer = Context.GetChangeSet().Updates.Count

        Try
            Context.SubmitChanges()
        Catch ex As Exception
            ret = ex.Message
        End Try

        If ret = "" Then ret = "OK"

        Return ret

    End Function


    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
    Public Function DeleteUserFromUserToRole(ByVal UserName As String, ByVal ApplicationName As String) As String
        Dim ret As String = ""

        Dim source = (From ur In Context.tblUsersInRoles _
                      Where ur.UserName.ToLower = UserName.ToLower _
                      And ur.ApplicationName.ToLower = ApplicationName.ToLower _
                      Select ur)

        If source.Count > 0 Then
            Context.tblUsersInRoles.DeleteOnSubmit(source)

            Dim i As Integer = Context.GetChangeSet().Updates.Count

            Try
                Context.SubmitChanges()
            Catch ex As Exception
                ret = ex.Message
            End Try

        End If

        If ret = "" Then ret = "OK"

        Return ret

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
    Public Function DeleteRolesFromUsersToRole(ByVal Rolename As String, Optional ByVal ApplicationName As String = "mm2010") As String
        Dim ret As String = ""

        Dim source = (From ur In Context.tblUsersInRoles _
                      Where ur.RoleName.ToLower = Rolename.ToLower _
                      And ur.ApplicationName.ToLower = ApplicationName.ToLower _
                      Select ur).First

        If Not source Is Nothing Then
            Context.tblUsersInRoles.DeleteOnSubmit(source)
        End If

        Dim i As Integer = Context.GetChangeSet().Updates.Count

        Try
            Context.SubmitChanges()
        Catch ex As Exception
            ret = ex.Message
        End Try

        If ret = "" Then ret = "OK"

        Return ret
    End Function

    Public Sub AddUsersToRoles(ByVal usernames() As String, ByVal roleNames() As String, ByVal applicationName As String)
        Dim RoleBLL As New BLLRoles

        For Each rolename As String In roleNames
            If Not RoleExists(rolename) Then
                Throw New ArgumentException("Role name not found.")
            End If
        Next

        For Each username As String In usernames
            If username.Contains(",") Then
                Throw New ArgumentException("User names cannot contain commas.")
            End If
            For Each rolename As String In roleNames
                If IsUserInRole(username, rolename) Then
                    Throw New ArgumentException("User is already in role.")
                End If
            Next
        Next

        For Each username As String In usernames
            For Each rolename As String In roleNames
                RoleBLL.AddUserToRole(username, rolename, applicationName)
            Next
        Next



    End Sub

    Function UnlockUser(ByVal Username As String) As String
        Dim ubll As BLLFriends = New BLLFriends

        Dim user = ubll.GetUserByName(Username)

        If Not user Is Nothing Then
            user.IsLockedOut = False
            ubll.UpdateUser(user)
        End If

        Return "OK"
    End Function

    Public Sub RemoveAllUsersInRole(ByVal usernames() As String, ByVal roleNames() As String)

        Dim RoleBLL As New BLLRoles

        For Each rolename As String In roleNames
            If Not RoleExists(rolename) Then
                Throw New ArgumentException("Role name not found.")
            End If
        Next

        For Each username As String In usernames
            For Each rolename As String In roleNames
                If Not IsUserInRole(username, rolename) Then
                    Throw New ArgumentException("User is not in role.")
                End If
            Next
        Next

        Throw New NotImplementedException

    End Sub

    Sub RemoveUsersFromRoles(ByVal usernames As String(), ByVal rolenames() As String)
        Throw New NotImplementedException
    End Sub

End Class

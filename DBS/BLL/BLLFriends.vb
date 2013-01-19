Imports Microsoft.VisualBasic
Imports MediaManager2010.WCFContracts.V1

Public Class BLLFriends

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetData() As List(Of User)
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblUsers _
                     Select co = New User() With { _
                        .ID = r.ID, _
                        .Email = r.Email, _
                        .Username = r.Username, _
                        .Description = r.Description, _
                        .Password = r.Password, _
                        .ApplicationName = r.ApplicationName, _
                        .IsLockedOut = r.IsLockedOut, _
                        .IsOnline = r.IsOnLine, _
                        .FailureCount = r.FailureCount, _
                        .LastLoginDate = r.LastLoginDate, _
                        .LastPasswordChangedDate = r.LastPasswordChangedDate, _
                        .PasswordAnswer = r.PasswordAnswer}

        If source.Count > 0 Then
            Return source.ToList
        Else
            Return New List(Of User)
        End If
    End Function

    Private Function ConvertToServiceUser(ByVal dbuser As tblUser) As User
        Dim serviceuser As New User

        With serviceuser
            .ID = dbuser.ID
            .Email = dbuser.Email
            .Username = dbuser.Username
            .Description = dbuser.Description
            .Password = dbuser.Password
            .ApplicationName = dbuser.ApplicationName
            .IsLockedOut = dbuser.IsLockedOut
            .IsOnline = dbuser.IsOnLine
            .FailureCount = dbuser.FailureCount
            .LastLoginDate = dbuser.LastLoginDate
            .LastPasswordChangedDate = dbuser.LastPasswordChangedDate
            .PasswordAnswer = dbuser.PasswordAnswer
        End With

        Return serviceuser
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetUsersLikeName(ByVal Username As String) As List(Of User)
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblUsers _
                     Select co = New User() With {.ID = r.ID, _
                        .Email = r.Email, _
                        .Username = r.Username, _
                        .Description = r.Description, _
                        .Password = r.Password, _
                        .ApplicationName = r.ApplicationName, _
                        .IsLockedOut = r.IsLockedOut, _
                        .IsOnline = r.IsOnLine, _
                        .FailureCount = r.FailureCount, _
                        .LastLoginDate = r.LastLoginDate, _
                        .LastPasswordChangedDate = r.LastPasswordChangedDate, _
                        .PasswordAnswer = r.PasswordAnswer}
                        Where co.Username Like Username

        Return source.ToList
    End Function


    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
   Public Function GetUserByID(ByVal ID As Integer) As User
        Dim u As tblUser = GetTblUserByID(ID)

        If Not u Is Nothing Then
            Return ConvertToServiceUser(u)
        End If

        Return New User

    End Function

    Public Function GetTblUserByID(ByVal ID As Integer) As tblUser
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblUsers _
                     Select r Where r.ID = ID

        If source.Count > 0 Then
            Return source.First
        Else
            Return Nothing
        End If
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetUserByName(ByVal Username As String) As User
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblUsers _
                     Select r Where r.Username.ToLower = Username.ToLower

        If source.Count > 0 Then
            Return ConvertToServiceUser(source.First)
        Else
            Return Nothing
        End If
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetUserByOwnerName(ByVal Username As String) As User

        Return GetUserByName(Username)

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetUserByEmail(ByVal Email As String) As User
        Dim d As New MediaLibraryLinqDataContext()

        Dim user = (From r In d.tblUsers _
                     Select r Where r.Email.ToLower = Email.ToLower).SingleOrDefault

        If Not user Is Nothing Then
            Return ConvertToServiceUser(user)
        End If

        Return Nothing

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, False)> _
       Public Function AddUser(ByVal U As User) As Long
        Dim m As New tblUser
        Dim c As New MediaLibraryLinqDataContext()

        If String.IsNullOrEmpty(U.Username) Then
            Return 0
        End If

        With m
            .ID = U.ID
            .Email = U.Email
            .Username = U.Username
            .Description = U.Description
            .Password = U.Password
            .ApplicationName = U.ApplicationName
            .IsLockedOut = U.IsLockedOut
            .IsOnLine = U.IsOnline
            .FailureCount = U.FailureCount
            .LastLoginDate = U.LastLoginDate
            .LastPasswordChangedDate = U.LastPasswordChangedDate
            .PasswordAnswer = U.PasswordAnswer
            .PasswordQuestion = U.PasswordQuestion
            .IsApproved = True
            .FailedPasswordAnswerAttemptCount = 0
            .FailedPasswordAnswerAttemptWindowStart = Nothing
            .FailedPasswordAttemptCount = 0
            .FailedPasswordAttemptWindowStart = Nothing
            .ApplicationName = "mm2010"
            .LastActivityDate = Now
            .LastLockedOutDate = Now
            .LastLoginDate = Now
            .LastPasswordChangedDate = now
        End With
     
        
        c.tblUsers.InsertOnSubmit(m)

        Dim i As Integer = c.GetChangeSet().Inserts.Count
        c.SubmitChanges()

        Return m.ID

    End Function


    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, False)> _
    Public Function UpdateUser(ByVal u As User) As Integer

        Dim UserContext As New MediaLibraryLinqDataContext()

        Dim source = From r In UserContext.tblUsers _
                     Where (r.ID = u.ID) _
                     Select r

        If source.Count > 0 Then
            Dim user = source.First
            user.Email = u.Email
            user.Username = u.Username
            user.Password = u.Password
            user.Fon = u.Fon
            user.Description = u.Description
            user.PasswordQuestion = u.PasswordQuestion
            user.PasswordAnswer = u.PasswordAnswer
            user.LastLoginDate = u.LastLoginDate
            user.IsOnLine = u.IsOnline
        End If

        Dim i As Integer = UserContext.GetChangeSet().Updates.Count

        UserContext.SubmitChanges()

        Return i
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, False)> _
    Public Function UpdatePassword(ByVal ID As Integer, ByVal Password As String) As Integer

        Dim UserContext As New MediaLibraryLinqDataContext()

        Dim source = From r In UserContext.tblUsers _
                     Where (r.ID = ID) _
                     Select r

        source.First.Password = Password

        Dim i As Integer = UserContext.GetChangeSet().Updates.Count

        UserContext.SubmitChanges()

        Return i
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, False)> _
       Public Function DeleteUser(ByVal ID As Integer) As Integer
        Dim UserContext As New MediaLibraryLinqDataContext()

        Dim User = (From us In UserContext.tblUsers _
                   Where us.ID = ID _
                   Select us).First

        If Not User Is Nothing Then
            UserContext.tblUsers.DeleteOnSubmit(User)
        End If

        Dim i As Integer = UserContext.GetChangeSet().Updates.Count

        UserContext.SubmitChanges()

        Return i
    End Function

    Public Function GetUserByNameAndPassowrd(name As String, password As String) As UserOnly
        Dim UserContext As New MediaLibraryLinqDataContext()

        Dim User = (From us In UserContext.tblUsers _
                    Where us.Username = name And us.Password = password _
                    Select us).SingleOrDefault

        If Not User Is Nothing Then
            Return New UserOnly With {.Email = User.Email, .Name = User.Username, .id = User.ID}
        End If

        Return Nothing
    End Function

    Public Function ValidPasswordRequest(g As String) As Boolean

        Using ctx As New MediaLibraryLinqDataContext()

            Dim user = (From us In ctx.tblUsers
            Where us.PasswordAnswer = g).SingleOrDefault

            If Not user Is Nothing Then
                If user.FailedPasswordAnswerAttemptWindowStart Is Nothing Then
                    Return False
                End If
                If DateDiff(DateInterval.Hour, Now, user.FailedPasswordAnswerAttemptWindowStart.Value) > 4 Then
                    Return False
                End If
                Return True
            End If
        End Using


    End Function

    Public Sub SetPasswordResetRequest(ByVal id As Integer)

        Using ctx As New MediaLibraryLinqDataContext()

            Dim user = (From us In ctx.tblUsers _
            Where us.ID = id).SingleOrDefault

            user.PasswordAnswer = Guid.NewGuid.ToString()
            user.FailedPasswordAttemptWindowStart = Date.Now

            ctx.SubmitChanges()

            Dim body = "Hi " & user.Username & vbNewLine & vbNewLine
            body &= "um dein Passwort zurückzusetzen klicke bitte auf den folgenden link: " & vbNewLine & vbNewLine
            body &= BLLSettings.Settings.BaseUrl & "/passwordreset.html?cmd=reset&tempkey=" & user.PasswordAnswer & vbNewLine & vbNewLine
            body &= "Auf der folgenden Seite kannst du ein neues Passwort vergeben." & vbNewLine & vbNewLine
            body &= "Have fun," & vbNewLine & "Movie Manager 2013"

            EmailSender.SendEmail("Passwort zurücksetzen", body, user.Email)

        End Using

    End Sub

    Public Sub ResetPassword(ByVal tempKey As String, ByVal newPassword As String)

        Using ctx As New MediaLibraryLinqDataContext()

            Dim user = (From us In ctx.tblUsers Where us.PasswordAnswer = tempKey).SingleOrDefault

            If user Is Nothing Then
                Throw New ArgumentException("Benutzer konnte nicht gefunden werden.")
            End If

            If String.IsNullOrEmpty(newPassword) Then
                Throw New ArgumentException("Das Passwort darf nicht leer sein.")
            End If

            user.Password = newPassword

            ctx.SubmitChanges()

        End Using

    End Sub
End Class

Public Class UserOnly
    Public Name As String
    Public Email As String
    Public id As Integer
End Class

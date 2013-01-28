Imports MediaManager2010.WCFContracts.V1

Public Class BLLItems
    Implements IItemRepository

    Private _movieIds As Integer()

    Public ReadOnly Property MovieIds() As Integer()
        Get
            If _movieIds Is Nothing Then
                Using db As MediaLibraryLinqDataContext = CreateDataContext()
                    _movieIds = (From r In db.tblItems Select r.ID).ToArray
                End Using

            End If
            Return _movieIds
        End Get
    End Property

    Public Function SearchByTitle(ByVal Title As String) As List(Of MovieItem)
        Return GetItemsLikeName(Title)
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetItemByName(ByVal Name As String) As MovieItem

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                    Select r Where (r.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase))

            If source.Count > 0 Then
                Return ConvertToMovieItem(source.First)
            Else
                Return Nothing
            End If

        End Using

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetItemsByName(ByVal Name As String) As List(Of MovieItem)
        Using db As MediaLibraryLinqDataContext = CreateDataContext()

            Dim source = From r In db.tblItems _
                    Select r Where r.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase)

            Return ConvertToList(source.ToList)

        End Using

    End Function

    Public Function GetItemsLikeName(ByVal name As String) As List(Of MovieItem)

        If Not name.StartsWith("*") Then name = "*" + name
        If Not name.EndsWith("*") Then name &= "*"

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                   Select r Where (r.Name Like name)

            Return ConvertToList(source.ToList)

        End Using

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetItemsByOwnerID(ByVal OwnerID As Integer) As List(Of MovieItem)

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                    Select r Where r.OwnerID = OwnerID

            Return ConvertToList(source.ToList)

        End Using

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetItemsByGenreID(ByVal GenreID As Integer) As List(Of MovieItem)

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                   Select r Where (r.GenreID = GenreID)

            Return ConvertToList(source.ToList)

        End Using

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetItemsByActorID(ByVal ActorID As Integer) As List(Of MovieItem)

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                     Select r Where r.Actor1ID = ActorID Or _
                                    r.Actor2ID = ActorID Or _
                                    r.Actor3ID = ActorID

            Return ConvertToList(source.ToList)

        End Using

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetItemsByDirectorID(ByVal DirectorID As Integer) As List(Of MovieItem)

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                   Select r Where (r.DirectorID = DirectorID)

            Return ConvertToList(source.ToList)

        End Using


    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetItemsByMediaType(ByVal MediaTypeID As Integer) As List(Of MovieItem)

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                   Select r Where (r.MediaTypeID = MediaTypeID)

            Return ConvertToList(source.ToList)
        End Using

    End Function

    ''' <summary>
    ''' Updates a MovieItem in the database
    ''' </summary>
    ''' <exception cref="ArgumentException"></exception>
    ''' <param name="Item"></param>
    ''' <remarks></remarks>
    Public Function UpdateItem(ByVal Item As MovieItem) As MovieItem Implements IItemRepository.UpdateItem
        Dim mBLL As BLLMediaType = New BLLMediaType
        Dim gBLL As BLLGenre = New BLLGenre
        Dim fBLL As BLLMediaFormat = New BLLMediaFormat
        Dim pBLL As BLLParticipants = New BLLParticipants
        Dim ptBLL As BLLParticipantType = New BLLParticipantType
        Dim uBLL As BLLFriends = New BLLFriends
        Dim IsNew As Boolean = False
        Dim ActorTypeID As Integer = 0
        Dim DirectorTypeID As Integer = 0

        Using db As MediaLibraryLinqDataContext = CreateDataContext()

            ActorTypeID = ptBLL.AddParticipantType("Actor")
            DirectorTypeID = ptBLL.AddParticipantType("Director")

            If String.IsNullOrEmpty(Item.GenreName) Then Item.GenreName = "unclassified"

            Item.Actor1ID = pBLL.AddParticipant(Item.Actor1Name, ActorTypeID)
            Item.Actor2ID = pBLL.AddParticipant(Item.Actor2Name, ActorTypeID)
            Item.Actor3ID = pBLL.AddParticipant(Item.Actor3Name, ActorTypeID)
            Item.DirectorID = pBLL.AddParticipant(Item.DirectorName, DirectorTypeID)
            Item.GenreID = gBLL.AddGenre(Item.GenreName)
            Item.MediaFormatID = fBLL.AddMediaFormat(Item.MediaFormatName)
            Item.MediaTypeID = mBLL.AddMediaType(Item.MediaFormatName)

            Dim UserID As Long = Item.OwnerID

            If UserID < 1 Then
                If Not String.IsNullOrEmpty(Item.OwnerName) Then
                    Dim Usr As User = uBLL.GetUserByOwnerName(Item.OwnerName)

                    If Not Usr Is Nothing Then
                        UserID = Usr.ID
                    End If
                End If

            End If

            If UserID = 0 Then
                Throw New ArgumentException("UserID must be greater zero")
            End If

            Dim itm As tblItem = Nothing

            If Item.ID > 0 Then
                Dim source = From r In db.tblItems _
                          Where r.ID = Item.ID _
                          Select r
                If source.Count > 0 Then
                    itm = source.First
                End If

            Else
                Dim source = From r In db.tblItems
                             Where r.Name = Item.Title And r.OwnerID = UserID
                             Select r

                If Not source Is Nothing Then
                    If source.Count > 0 Then
                        itm = source.First
                    End If
                End If

            End If

            If itm Is Nothing Then
                itm = New tblItem
                itm.DateAdded = Now
                IsNew = True
            End If

            itm.GenreID = Item.GenreID
            itm.Name = Item.Title
            itm.MediaTypeID = Item.MediaTypeID
            itm.MediaFormatID = Item.MediaFormatID
            itm.AuthorID = Item.DirectorID
            itm.Actor1ID = Item.Actor1ID
            itm.Actor2ID = Item.Actor2ID
            itm.Actor3ID = Item.Actor3ID
            itm.DirectorID = Item.DirectorID
            itm.PublishDate = Item.PublishDate
            itm.OwnerID = UserID
            itm.EAN = Item.EAN
            itm.ASIN = Item.ASIN
            itm.Description = Tools.RemoveHTMLTags(Item.Description)
            itm.AudienceRank = Item.AudienceRank
            itm.AmazonSalesRank = Item.AmazonSalesRank
            itm.SmallImageUrl = Item.SmallImageUrl
            itm.MediumImageUrl = Item.MediumImageUrl
            itm.LargeImageUrl = Item.LargeImageUrl
            itm.Description = Item.Description
            itm.BorrowedSince = CDate("1/1/1970")

            If Not Item.GenreIDs Is Nothing AndAlso Item.GenreIDs.Count > 0 Then
                itm.GenreIDs = AddGenreIds(Item)
            End If

            If Not String.IsNullOrEmpty(Item.BorrowedByName) Then
                Dim Borrower As User = uBLL.GetUserByOwnerName(Item.BorrowedByName)
                If Not Borrower Is Nothing Then
                    itm.BorrowedByID = Borrower.ID
                End If
            Else
                itm.BorrowedByID = Nothing
            End If

            If IsNew Then
                db.tblItems.InsertOnSubmit(itm)
            End If

            Try
                Dim i As Integer = db.GetChangeSet().Updates.Count
                db.SubmitChanges()
            Catch ex As Exception
                Throw
            End Try

            If Not String.IsNullOrEmpty(Item.SmallImageUrl) Then BLLImages.StoreImage(itm.ID, Item.SmallImageUrl, BLLImages.ImageSizeE.Small)
            If Not String.IsNullOrEmpty(Item.MediumImageUrl) Then BLLImages.StoreImage(itm.ID, Item.MediumImageUrl, BLLImages.ImageSizeE.Medium)
            If Not String.IsNullOrEmpty(Item.LargeImageUrl) Then BLLImages.StoreImage(itm.ID, Item.LargeImageUrl, BLLImages.ImageSizeE.Big)

            Return ConvertToMovieItem(itm)


        End Using


    End Function

    Private Function AddGenreIds(ByRef Item As MovieItem) As String
        Dim s As String = ""

        If Not Item.GenreIDs Is Nothing Then

            For Each genreID In Item.GenreIDs

                s &= genreID & ","

            Next

            If s.Length > 1 Then
                s = s.Substring(0, s.Length - 1)
            End If
        End If

        Return s
    End Function

    Private Function CreateBorrowRequestBody(ByVal OwnerName As String, ByVal LenderName As String, ByVal m As tblItem) As String

        Dim sb As New StringBuilder

        sb.AppendLine("Hi " & OwnerName)
        sb.AppendLine("")
        sb.AppendLine(LenderName & " moechte gern den folgenden Film von dir leihen: ")
        sb.AppendLine("Titel: " & m.Name)
        sb.AppendLine("Format: " & m.tblMediaFormat.Name)
        sb.AppendLine("EAN: " & m.EAN)
        sb.AppendLine("")
        sb.AppendLine("Vielen Dank und viel Spass beim Gucken!")
        sb.AppendLine("Die Media Community")

        Return sb.ToString

    End Function

    Private Function CreateLenderNotificationBody(ByVal OwnerName As String, ByVal LenderName As String, ByVal m As tblItem) As String

        Dim sb As New StringBuilder

        sb.AppendLine("Hi " & LenderName)
        sb.AppendLine("")
        sb.AppendLine(OwnerName & " wurde informiert, dass du den folgenden Film leihen moechtest: ")
        sb.AppendLine("Titel: " & m.Name)
        sb.AppendLine("Format: " & m.tblMediaFormat.Name)
        sb.AppendLine("EAN: " & m.EAN)
        sb.AppendLine("")
        sb.AppendLine("Vielen Dank und viel Spass beim Gucken!")
        sb.AppendLine("Die Media Community")

        Return sb.ToString

    End Function

    Public Function ReturnBorrowedMovie(ByVal ID As Integer) As String

        Using db As MediaLibraryLinqDataContext = CreateDataContext()

            Dim src = From m In db.tblItems Select m Where m.ID = ID
            Dim uBll As BLLFriends = New BLLFriends

            If src.Count > 0 Then
                Dim movie As tblItem = src.First

                movie.BorrowedByID = 0

                Dim i As Integer = db.GetChangeSet.Updates.Count
                Try
                    db.SubmitChanges()
                Catch ex As Exception
                    Return "Update failed [" & ex.Message
                End Try


            End If

        End Using

        Return "OK"
    End Function


    Public Function UpdateBorrow(ByVal ID As Integer, Optional ByVal BorrowedByID As Integer = 0, Optional ByVal BorrowedSince As Date = #1/1/1990#, Optional sendEmail As Boolean = True) As String

        Using db As MediaLibraryLinqDataContext = CreateDataContext()

            Dim src = From m In db.tblItems Select m Where m.ID = ID
            Dim uBll As BLLFriends = New BLLFriends

            If src.Count > 0 Then
                Dim movie As tblItem = src.First

                movie.BorrowedByID = BorrowedByID
                movie.BorrowedSince = Now
                movie.BorrowCount += 1

                Dim i As Integer = db.GetChangeSet.Updates.Count
                Try
                    db.SubmitChanges()
                Catch ex As Exception
                    Return "Update failed [" & ex.Message
                End Try

                'Jetzt noch die Emails versenden
                If sendEmail Then
                    Dim Lender As tblUser = uBll.GetTblUserByID(movie.BorrowedByID)
                    Dim Owner As tblUser = uBll.GetTblUserByID(movie.OwnerID)
                    Dim RequestBody As String = CreateBorrowRequestBody(Owner.Username, Lender.Username, movie)
                    Dim NotifyBody As String = CreateLenderNotificationBody(Owner.Username, Lender.Username, movie)

                    EmailSender.SendEmail("Movie Manager Leihanfrage", RequestBody, Owner.Email)
                    EmailSender.SendEmail("Movie Manager Leihanfrage", NotifyBody, Lender.Email)
                End If

            End If


        End Using

        Return "OK"

      End Function

    Public Function GetItems() As System.Collections.Generic.List(Of MovieItem) Implements MediaManager2010.IItemRepository.GetItems
        Using db As MediaLibraryLinqDataContext = CreateDataContext()

            Dim source = (From r In db.tblItems Select r)

            Return ConvertToList(source.ToList)

        End Using
    End Function

    Public Function PickRandomMovies() As List(Of MovieItem)
        Dim lst = New List(Of MovieItem)
        Dim r As New System.Random()
        Dim i As Integer = 1
        While i <= 15
            Dim id As Integer = r.Next(1, MovieIds.Length)
            Dim mov = GetItemByID(id)
            If Not String.IsNullOrEmpty(mov.Title) Then
                lst.Add(mov)
                i += 1
            End If
        End While
        Return lst
    End Function


    Public Function GetTblItemByID(ByVal ID As Integer) As tblItem
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                 Select r Where (r.ID = ID)

            If source.Count > 0 Then
                Return source.First
            End If

        End Using
        Return Nothing
    End Function

    Public Function GetItemByEAN(ByVal EAN As String) As List(Of WCFContracts.V1.MovieItem)
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                 Select r Where (r.EAN = EAN)

            Return ConvertToList(source.ToList)

        End Using

    End Function

    Public Function GetItemByID(ByVal ID As Integer) As WCFContracts.V1.MovieItem Implements MediaManager2010.IItemRepository.GetItemByID
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                 Select r Where (r.ID = ID)

            If source.Count > 0 Then
                Return ConvertToMovieItem(source.First)
            End If

        End Using
        Return New MovieItem

    End Function

    Private Function CreateDataContext() As MediaLibraryLinqDataContext
        Return New MediaLibraryLinqDataContext
    End Function

    ''' <summary>
    ''' Deletes an item in the database
    ''' </summary>
    ''' <param name="ID"></param>
    ''' <exception cref="ArgumentException"></exception>
    ''' <remarks></remarks>
    Public Sub DeleteItem(ByVal ID As Integer) Implements MediaManager2010.IItemRepository.DeleteItem

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From itm In db.tblItems Where itm.ID = ID

            If source.Count > 0 Then
                Dim m As tblItem = source.First
                db.tblItems.DeleteOnSubmit(m)
                db.SubmitChanges()
            End If

        End Using
    End Sub

    Public Function GetItemByNameAndOwnerID(ByVal Name As String, ByVal OwnerID As Integer) As Object Implements MediaManager2010.IItemRepository.GetItemByNameAndOwnerID
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                         Where (r.OwnerID = OwnerID And r.Name = Name)
            If source.Count > 0 Then
                Return source.First
            Else
                Return Nothing
            End If
        End Using
    End Function

    Public Function GetItemsByMediaFormat(ByVal MediaFormatID As Integer) As System.Collections.Generic.List(Of WCFContracts.V1.MovieItem) Implements MediaManager2010.IItemRepository.GetItemsByMediaFormat
        Using db As MediaLibraryLinqDataContext = CreateDataContext()

            Dim source = From r In db.tblItems _
                    Select r Where r.MediaFormatID = MediaFormatID

            Return ConvertToList(source)

        End Using
    End Function

    Private Function ConvertToMovieItem(ByVal r As tblItem) As MovieItem

        Dim m As New MovieItem

        With m
            .ID = r.ID
            .Title = Tools.CleanupMovieTitle(r.Name)
            .GenreName = r.tblGenre.Name
            .GenreID = r.GenreID
            .Actor1ID = r.Actor1ID
            .Actor2ID = r.Actor2ID
            .Actor3ID = r.Actor3ID
            .DirectorID = r.DirectorID

            If Not r.AuthorID Is Nothing Then
                .AuthorID = r.AuthorID
            End If

            If .Actor1ID > 0 Then
                Try
                    .Actor1Name = r.tblParticipant.Name
                Catch ex As Exception

                End Try
            Else
                .Actor1Name = String.Empty
            End If

            If .Actor2ID > 0 Then
                Try
                    .Actor2Name = r.tblParticipant1.Name
                Catch ex As Exception

                End Try
            Else
                .Actor2Name = String.Empty
            End If

            If .Actor3ID > 0 Then
                Try
                    .Actor3Name = r.tblParticipant2.Name

                Catch ex As Exception

                End Try
            Else
                .Actor3Name = String.Empty
            End If

            If .DirectorID > 0 Then
                Try
                    .DirectorName = r.tblParticipant3.Name

                Catch ex As Exception

                End Try
            Else
                .DirectorName = String.Empty
            End If

            If .AuthorID > 0 Then
                Try
                    .AuthorName = r.tblParticipant4.Name

                Catch ex As Exception

                End Try
            Else
                .AuthorName = String.Empty
            End If

            .AmazonSalesRank = r.AmazonSalesRank
            .AudienceRank = r.AudienceRank

            If Not r.BorrowedByID Is Nothing Then
                .BorrowedByID = r.BorrowedByID
            Else
                .BorrowedByID = 0
            End If

            .BorrowCount = r.BorrowCount
            .DateAdded = r.DateAdded

            If .BorrowedByID > 0 Then
                Try
                    .BorrowedByName = r.tblUser1.Username
                Catch ex As Exception

                End Try
                .BorrowedSince = r.BorrowedSince
            End If

            .CoverUrlMediaManager = BLLSettings.Settings.BaseUrl & "/imagehandler?ID=" & r.ID
            If Not String.IsNullOrEmpty(r.Description) Then
                .Description = Tools.RemoveHTMLTags(r.Description)
                If .Description.Length > 800 Then .Description = .Description.Substring(0, 799) & "..."
            End If
            .EAN = r.EAN
            Try
                .MediaFormatName = r.tblMediaFormat.Name

            Catch ex As Exception

            End Try
            Try

                .MediaTypeName = r.tblMediaType.Name
            Catch ex As Exception

            End Try
            .OwnerID = r.OwnerID

            Try
                .OwnerName = r.tblUser.Username

            Catch ex As Exception

            End Try

            .PublishDate = r.PublishDate
            .MediaFormatID = r.MediaFormatID
            .MediaTypeID = r.MediaTypeID

            .Ratings = New List(Of Rating)

            For Each rat As tblRating In r.tblRatings
                .Ratings.Add(New Rating(rat))
            Next

        End With

        'Genres hinzufuegen, wenn vorhanden
        GetMultipleGenres(r, m)

        Return m
    End Function



    Private Sub GetMultipleGenres(ByRef itm As tblItem, ByRef m As MovieItem)

        If Not itm Is Nothing AndAlso Not m Is Nothing Then

            If Not String.IsNullOrEmpty(itm.GenreIDs) Then

                Dim gBLL As New BLLGenre
                Dim genres As String() = itm.GenreIDs.Split(",")

                For Each g In genres

                    Dim genreID As Integer = 0

                    If Integer.TryParse(g, genreID) Then

                        Dim genre As Genre = gBLL.GetGenreByID(genreID)

                        If Not genre Is Nothing Then
                            m.GenreIDs.Add(genre.ID)
                            m.GenreNames.Add(genre.Name)
                        End If

                    End If

                Next

            End If

        End If

    End Sub


    Private Function ConvertToList(ByVal lst As List(Of tblItem)) As List(Of MovieItem)

        Dim ret As New List(Of MovieItem)

        For Each m In lst
            ret.Add(ConvertToMovieItem(m))
        Next

        Return ret

    End Function

    Public Class OnlyTitle
        Public title As String
        Public id As Integer

        Public Sub New(r As tblItem)
            id = r.ID
            title = Tools.CleanupMovieTitle(r.Name)
        End Sub
    End Class

    Public Function GetTitlesBeginningWith(ByVal letter As String) As List(Of OnlyTitle)
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblItems Order By r.Name Where r.Name.StartsWith(letter) Select co = New OnlyTitle(r)

        Return source.ToList
    End Function

    Public Function GetLatestAdded() As List(Of MovieItem)
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim d As DateTime = Now.AddDays(-20)

            Dim source = (From r In db.tblItems Select r Where r.DateAdded >= d)

            If source.Count <= 0 Then
                source = (From r In db.tblItems Order By r.DateAdded Descending Select r).Take(15)
            End If

            Return ConvertToList(source.ToList)

        End Using
    End Function

    Public Function GetItemsByBorrowerID(ByVal borrowedById As Integer) As List(Of MovieItem)
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                    Select r Where r.BorrowedByID = borrowedById

            Dim lst As List(Of MovieItem) = ConvertToList(source.ToList)

            Return lst
        End Using

    End Function
    Public Function GetItemsByLenderByID(ByVal lenderId As Integer) As List(Of MovieItem)
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblItems _
                    Select r Where r.OwnerID = lenderId And r.BorrowedByID > 0

            Dim lst As List(Of MovieItem) = ConvertToList(source.ToList)

            Return lst

        End Using

    End Function
End Class

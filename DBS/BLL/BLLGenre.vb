Imports MediaManager2010.BLL.Interfaces
Imports MediaManager2010.WCFContracts.V1

Public Class BLLGenre
    Implements IGenreRepository

    Public Function GetGenres() As List(Of Genre) Implements IGenreRepository.GetGenres

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblGenres _
                         Select co = New Genre() With { _
                            .ID = r.ID, _
                            .Name = r.Name, _
                            .Description = r.Description} Order By co.Name Ascending

            Return source.ToList
        End Using

    End Function

    Public Function GetGenresLikeName(ByVal Name As String) As List(Of Genre) Implements IGenreRepository.GetGenresLikeName
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From r In db.tblGenres _
                         Select g = New Genre() With { _
                            .ID = r.ID, _
                            .Name = r.Name, _
                            .Description = r.Description} _
                         Where (g.Name Like Name)
            Return source.ToList
        End Using
    End Function

    Public Function GetGenreByID(ByVal ID As Integer) As Genre Implements IGenreRepository.GetGenreByID

        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Return (From r In db.tblGenres _
                     Select g = New Genre() With { _
                        .ID = r.ID, _
                        .Name = r.Name, _
                        .Description = r.Description} _
                     Where (g.ID = ID)).FirstOrDefault
        End Using

    End Function

    Public Function GetGenreByName(ByVal name As String) As Genre Implements IGenreRepository.GetGenreByName

        Using db As MediaLibraryLinqDataContext = CreateDataContext()

            Dim source = From r In db.tblGenres _
                         Select g = New Genre() With { _
                            .ID = r.ID, _
                            .Name = r.Name, _
                            .Description = r.Description} _
                         Where (g.Name Like name)

            If source.Count > 0 Then
                Return source.First
            Else
                Return Nothing
            End If

        End Using

    End Function

    Public Function AddGenre(ByVal Name As String, _
                             Optional ByVal Description As String = "") As Integer Implements IGenreRepository.AddGenre

        Name = cleanUpName(Name)
        Dim g As Genre = Me.GetGenreByName(Name)

        If Not g Is Nothing Then
            Return g.ID
        Else
            If IsNothing(Me.GetGenreByName(Name)) Then
                Using db As MediaLibraryLinqDataContext = CreateDataContext()

                    Dim m As New tblGenre

                    m.Name = Name
                    m.Description = Description

                    db.tblGenres.InsertOnSubmit(m)

                    Dim i As Integer = db.GetChangeSet().Inserts.Count
                    db.SubmitChanges()

                    Return m.ID

                End Using
            End If
        End If

        Return 0
    End Function

    ''' <summary>
    ''' Updates a Genre in the Database
    ''' </summary>
    ''' <param name="ID"></param>
    ''' <param name="Name"></param>
    ''' <param name="Description"></param>
    ''' <exception cref="ArgumentException"></exception>
    ''' <remarks></remarks>
    Public Sub UpdateGenre(ByVal ID As Integer, _
                           ByVal Name As String, _
                           Optional ByVal Description As String = Nothing) Implements IGenreRepository.UpdateGenre

        If Not String.IsNullOrEmpty(Name) Then

            Name = cleanUpName(Name)

            Using db As MediaLibraryLinqDataContext = CreateDataContext()

                Dim source = From g In db.tblGenres Where g.ID = ID

                If source.Count = 0 Then
                    Throw New ArgumentException("Genre not found [" & ID & "]")
                End If

                source.First.Name = Name
                If Not Description Is Nothing Then source.First.Description = Description

                db.SubmitChanges()

            End Using
        End If

    End Sub

    Private Function cleanUpName(ByVal name As String) As String
        Return name.Replace("&", "und")
    End Function

    ''' <summary>
    ''' Deletes a Genre in the database
    ''' </summary>
    ''' <exception cref="ArgumentException"></exception>
    ''' <param name="ID"></param>
    ''' <remarks></remarks>
    Public Sub DeleteGenre(ByVal ID As Integer) Implements IGenreRepository.DeleteGenre
        Using db As MediaLibraryLinqDataContext = CreateDataContext()
            Dim source = From itm In db.tblItems Where itm.ID = ID

            If Not source Is Nothing Then
                db.tblItems.DeleteOnSubmit(source)
            Else
                Throw New ArgumentException("ID could not be found [" & ID & "]")
            End If

        End Using
    End Sub

    Private Function CreateDataContext() As MediaLibraryLinqDataContext
        Return New MediaLibraryLinqDataContext
    End Function

End Class

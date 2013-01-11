Imports MediaManager2010.WCFContracts.V1

Public Class GenreController

    Private Shared _genreRep As IGenreRepository = New BLLGenre

    Public Shared Property GenreRepository() As IGenreRepository
        Get
            Return _genreRep
        End Get
        Set(ByVal value As IGenreRepository)
            _genreRep = value
        End Set
    End Property
    Public Shared Function GetGenres() As List(Of Genre)
        Return GenreRepository.GetGenres
    End Function
    Public Shared Function GetGenreByID(ByVal ID As Integer) As Genre
        Dim lst As List(Of Genre) = GenreRepository.GetGenres

        Dim source = From r In lst Where (r.ID = ID)

        If source.Count > 0 Then
            Return source.First
        Else
            Return Nothing
        End If

    End Function
    Public Shared Function GetGenreByName(ByVal Name As String) As Genre
        Dim lst As List(Of Genre) = GenreRepository.GetGenres

        Dim source = From r In lst _
             Select g = New Genre() With { _
                .ID = r.ID, _
                .Name = r.Name, _
                .Description = r.Description} _
             Where (g.Name.ToLower = Name.ToLower)

        If source.Count > 0 Then
            Return source.First
        Else
            Return Nothing
        End If

    End Function
    ''' <summary>
    ''' Searches in Genres where Genre.Name like Name
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetGenresLikeName(ByVal Name As String) As List(Of Genre)

        Dim lst As List(Of Genre) = GenreRepository.GetGenres

        Dim source = From r In lst Where (r.Name Like Name)

        Return source.ToList

    End Function
    Public Shared Function AddGenre(ByVal Name As String, Optional ByVal Description As String = Nothing) As Integer

        Dim genre As Genre = GenreRepository.GetGenreByName(Name)

        If genre Is Nothing Then
            Return GenreRepository.AddGenre(Name, Description)
        Else
            Return -1
        End If

    End Function

End Class

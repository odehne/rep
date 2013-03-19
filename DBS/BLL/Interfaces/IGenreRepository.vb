Imports MediaManager2010.WCFContracts.V1

Namespace BLL.Interfaces

    Public Interface IGenreRepository

        Function GetGenres() As List(Of Genre)
        Function GetGenreByID(ByVal ID As Integer) As Genre
        Function GetGenresLikeName(ByVal Name As String) As List(Of Genre)
        Function GetGenreByName(ByVal name As String) As Genre
        Function AddGenre(ByVal Name As String, Optional ByVal Description As String = "") As Integer
        Sub UpdateGenre(ByVal ID As Integer, ByVal Name As String, Optional ByVal Description As String = Nothing)
        Sub DeleteGenre(ByVal ID As Integer)

    End Interface
End Namespace
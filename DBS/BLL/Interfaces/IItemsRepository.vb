Imports MediaManager2010.WCFContracts.V1
Imports MediaManager2010.WCFContracts

Namespace BLL.Interfaces

    Public Interface IItemsRepository

        ReadOnly Property MovieIds() As Integer()
        Function SearchByTitle(ByVal Title As String) As List(Of MovieItem)
        Function GetItemByName(ByVal Name As String) As MovieItem
        Function GetItemsByName(ByVal Name As String) As List(Of MovieItem)
        Function GetItemsLikeName(ByVal name As String) As List(Of MovieItem)
        Function GetItemsByOwnerID(ByVal OwnerID As Integer) As List(Of MovieItem)
        Function GetItemsByGenreID(ByVal GenreID As Integer) As List(Of MovieItem)
        Function GetItemsByActorID(ByVal ActorID As Integer) As List(Of MovieItem)
        Function GetItemsByDirectorID(ByVal DirectorID As Integer) As List(Of MovieItem)
        Function GetItemsByMediaType(ByVal MediaTypeID As Integer) As List(Of MovieItem)

        ''' <summary>
        ''' Updates a MovieItem in the database
        ''' </summary>
        ''' <exception cref="ArgumentException"></exception>
        ''' <param name="Item"></param>
        ''' <remarks></remarks>
        Function UpdateItem(ByVal Item As MovieItem) As MovieItem
        Function ReturnBorrowedMovie(ByVal ID As Integer) As String
        Function UpdateBorrow(ByVal ID As Integer, Optional ByVal BorrowedByID As Integer = 0, Optional ByVal BorrowedSince As Date = #1/1/1990#, Optional sendEmail As Boolean = True) As String
        Function GetItems() As System.Collections.Generic.List(Of MovieItem)
        Function PickRandomMovies() As List(Of MovieItem)
        Function GetTblItemByID(ByVal ID As Integer) As tblItem
        Function GetItemByEAN(ByVal EAN As String) As List(Of WCFContracts.V1.MovieItem)
        Function GetItemByID(ByVal ID As Integer) As WCFContracts.V1.MovieItem

        ''' <summary>
        ''' Deletes an item in the database
        ''' </summary>
        ''' <param name="ID"></param>
        ''' <exception cref="ArgumentException"></exception>
        ''' <remarks></remarks>
        Sub DeleteItem(ByVal ID As Integer)
        Function GetItemByNameAndOwnerID(ByVal Name As String, ByVal OwnerID As Integer) As MovieItem
        Function GetItemsByMediaFormat(ByVal MediaFormatID As Integer) As System.Collections.Generic.List(Of WCFContracts.V1.MovieItem)
        Function GetTitlesBeginningWith(ByVal letter As String) As List(Of OnlyTitle)
        Function GetLatestAdded() As List(Of MovieItem)
        Function GetItemsByBorrowerID(ByVal borrowedById As Integer) As List(Of MovieItem)
        Function GetItemsByLenderByID(ByVal lenderId As Integer) As List(Of MovieItem)
    End Interface
End Namespace
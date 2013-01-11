Imports MediaManager2010.WCFContracts.V1

Public Interface IItemRepository

    Function GetItems() As List(Of MovieItem)
    Function GetItemByID(ByVal ID As Integer) As MovieItem
    Sub DeleteItem(ByVal ID As Integer)
    Function UpdateItem(ByVal Item As MovieItem) As MovieItem
    Function GetItemByNameAndOwnerID(ByVal Name As String, ByVal OwnerID As Integer)
    Function GetItemsByMediaFormat(ByVal MediaFormatID As Integer) As List(Of MovieItem)
  
End Interface

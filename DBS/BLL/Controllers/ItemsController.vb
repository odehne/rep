Imports MediaManager2010.WCFContracts.V1

Public Class ItemsController

    Private Shared _itemsRep As IItemRepository = New BLLItems

    Public Shared Property ItemsRepository() As IItemRepository
        Get
            Return _itemsRep
        End Get
        Set(ByVal value As IItemRepository)
            _itemsRep = value
        End Set
    End Property

    Public Shared Function GetItems() As List(Of MovieItem)
        Return ItemsRepository.GetItems
    End Function

    Public Shared Function GetItem(ByVal ID As Integer) As List(Of MovieItem)
        Return ItemsRepository.GetItems
    End Function

    Public Shared Sub AddItem(ByVal MV As MovieItem)
        UpdateItem(MV)
    End Sub

    Public Shared Sub UpdateItem(ByVal MV As MovieItem)
        ItemsRepository.UpdateItem(MV)
    End Sub

    Public Shared Sub DeleteItem(ByVal ID As Integer)
        ItemsRepository.DeleteItem(ID)
    End Sub

End Class

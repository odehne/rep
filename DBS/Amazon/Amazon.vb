'Imports Microsoft.VisualBasic
'Imports Movie_Contracts

'Public Class BLLAmazonService



'    Public Function SearchByEAN(ByVal EAN As String) As List(Of AmazonItem)
'        Dim Service As New Amazon.AWSECommerceService '   Amazon.AWSECommerceServicePortTypeClient
'        Dim ItmLookup As New Amazon.ItemLookup
'        Dim ItmRequest As Amazon.ItemLookupRequest = New Amazon.ItemLookupRequest
'        Dim ItmResponse As Amazon.ItemLookupResponse
'        Dim l As New List(Of AmazonItem)

'        ' Please Type your token key here 
'        ItmLookup.SubscriptionId = "0MV9826S2QCK6YEX4N82"

'        ItmRequest.IdType = Amazon.ItemLookupRequestIdType.EAN
'        ItmRequest.SearchIndex = "DVD"
'        ItmRequest.ResponseGroup = New String() {"Images", "Large"}
'        ItmRequest.IdTypeSpecified = True
'        ItmRequest.ItemId = New String() {EAN}
'        ItmLookup.Request = New Amazon.ItemLookupRequest() {ItmRequest}

'        ItmResponse = Service.ItemLookup(ItmLookup)

'        If Not ItmResponse.Items(0).Item Is Nothing Then


'            For i As Integer = LBound(ItmResponse.Items(0).Item) To UBound(ItmResponse.Items(0).Item)

'                Dim rtn As New AmazonItem

'                If Not ItmResponse.Items(0).Item(0).MediumImage Is Nothing Then rtn.CoverUrl = ItmResponse.Items(0).Item(0).MediumImage.URL
'                If Not ItmResponse.Items(0).Item(0).SmallImage Is Nothing Then rtn.CoverUrlSmall = ItmResponse.Items(0).Item(0).SmallImage.URL
'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.Title Is Nothing Then rtn.Fullname = ItmResponse.Items(0).Item(0).ItemAttributes.Title

'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.Actor Is Nothing Then
'                    If UBound(ItmResponse.Items(0).Item(0).ItemAttributes.Actor) >= 0 Then rtn.Actor1 = ItmResponse.Items(0).Item(0).ItemAttributes.Actor(0)
'                    If UBound(ItmResponse.Items(0).Item(0).ItemAttributes.Actor) >= 1 Then rtn.Actor2 = ItmResponse.Items(0).Item(0).ItemAttributes.Actor(1)
'                    If UBound(ItmResponse.Items(0).Item(0).ItemAttributes.Actor) >= 2 Then rtn.Actor3 = ItmResponse.Items(0).Item(0).ItemAttributes.Actor(2)
'                End If

'                If Not ItmResponse.Items(0).Item(0).EditorialReviews Is Nothing Then
'                    rtn.Description = ItmResponse.Items(0).Item(0).EditorialReviews(0).Content
'                End If

'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.Director Is Nothing Then rtn.Director = ItmResponse.Items(0).Item(0).ItemAttributes.Director(0)
'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.Manufacturer Is Nothing Then rtn.Producer = ItmResponse.Items(0).Item(0).ItemAttributes.Manufacturer
'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.ProductGroup Is Nothing Then rtn.Media = ItmResponse.Items(0).Item(0).ItemAttributes.ProductGroup
'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.Format Is Nothing Then rtn.Format = ItmResponse.Items(0).Item(0).ItemAttributes.Format(0)
'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.TheatricalReleaseDate Is Nothing Then rtn.ReleaseDate = ItmResponse.Items(0).Item(0).ItemAttributes.TheatricalReleaseDate
'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.Author Is Nothing Then rtn.Author = ItmResponse.Items(0).Item(0).ItemAttributes.Author(0)
'                If Not ItmResponse.Items(0).Item(0).ItemAttributes.AudienceRating Is Nothing Then rtn.AudienceRating = ItmResponse.Items(0).Item(0).ItemAttributes.AudienceRating
'                If Not ItmResponse.Items(0).Item(0).SalesRank Is Nothing Then rtn.AmazonSalesRank = ItmResponse.Items(0).Item(0).SalesRank

'                l.Add(rtn)

'            Next
'        End If

'        Return l

'    End Function

'    Public Function SearchByTitle(ByVal Title As String) As List(Of AmazonItem)
'        Dim Service As New Amazon.AWSECommerceService
'        Dim ItmSearch As New Amazon.ItemSearch
'        Dim ItmSearchRequest As New Amazon.ItemSearchRequest
'        Dim ItmResponse As Amazon.ItemSearchResponse
'        Dim l As New List(Of AmazonItem)

'        ' Please Type your token key here 
'        ItmSearch.SubscriptionId = "7650-9537-3559"
'        ItmSearch.AWSAccessKeyId = "AKIAJGLLZFOW44IYKYHA"

'        ItmSearchRequest.SearchIndex = "DVD"
'        ItmSearchRequest.ResponseGroup = New String() {"Images", "Large"}
'        ItmSearchRequest.Title = Title

'        ItmSearch.Request = New Amazon.ItemSearchRequest() {ItmSearchRequest}
'        ItmResponse = Service.ItemSearch(ItmSearch)

'        If Not ItmResponse.Items(0).Item Is Nothing Then


'            For i As Integer = LBound(ItmResponse.Items(0).Item) To UBound(ItmResponse.Items(0).Item)

'                Dim itm As AmazonItem = New AmazonItem

'                If ItmResponse.Items(0).Item(i) Is Nothing Then
'                    Throw New Exception("Item for requested EAN " & Title & " not found.")
'                    Exit Function
'                End If

'                If Not ItmResponse.Items(0).Item(i).EditorialReviews Is Nothing Then
'                    itm.Description = ItmResponse.Items(0).Item(i).EditorialReviews(0).Content
'                End If

'                If Not ItmResponse.Items(0).Item(i).MediumImage Is Nothing Then itm.CoverUrl = ItmResponse.Items(0).Item(i).MediumImage.URL
'                If Not ItmResponse.Items(0).Item(i).SmallImage Is Nothing Then itm.CoverUrlSmall = ItmResponse.Items(0).Item(i).SmallImage.URL
'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.Title Is Nothing Then itm.Fullname = ItmResponse.Items(0).Item(i).ItemAttributes.Title

'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.Actor Is Nothing Then
'                    If UBound(ItmResponse.Items(0).Item(i).ItemAttributes.Actor) >= 0 Then itm.Actor1 = ItmResponse.Items(0).Item(i).ItemAttributes.Actor(0)
'                    If UBound(ItmResponse.Items(0).Item(i).ItemAttributes.Actor) >= 1 Then itm.Actor2 = ItmResponse.Items(0).Item(i).ItemAttributes.Actor(1)
'                    If UBound(ItmResponse.Items(0).Item(i).ItemAttributes.Actor) >= 2 Then itm.Actor3 = ItmResponse.Items(0).Item(i).ItemAttributes.Actor(2)
'                End If

'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.Director Is Nothing Then itm.Director = ItmResponse.Items(0).Item(i).ItemAttributes.Director(0)
'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.Manufacturer Is Nothing Then itm.Producer = ItmResponse.Items(0).Item(i).ItemAttributes.Manufacturer
'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.ProductGroup Is Nothing Then itm.Media = ItmResponse.Items(0).Item(i).ItemAttributes.ProductGroup
'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.Format Is Nothing Then itm.Format = ItmResponse.Items(0).Item(i).ItemAttributes.Format(0)
'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.TheatricalReleaseDate Is Nothing Then itm.ReleaseDate = ItmResponse.Items(0).Item(i).ItemAttributes.TheatricalReleaseDate
'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.Author Is Nothing Then itm.Author = ItmResponse.Items(0).Item(i).ItemAttributes.Author(0)
'                If Not ItmResponse.Items(0).Item(i).ItemAttributes.AudienceRating Is Nothing Then itm.AudienceRating = ItmResponse.Items(0).Item(i).ItemAttributes.AudienceRating
'                If Not ItmResponse.Items(0).Item(i).SalesRank Is Nothing Then itm.AmazonSalesRank = ItmResponse.Items(0).Item(i).SalesRank

'                l.Add(itm)

'            Next

'        End If

'        Return l


'    End Function

' End Class

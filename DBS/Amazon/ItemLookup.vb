Imports System.Collections.Generic
Imports System.Text
Imports System.Net
Imports System.Xml
Imports MediaManager2010.WCFContracts.V1


Public Class ItemLookup

    Public Enum SearchTypeE
        EAN
        ASIN
        Keyword
    End Enum



    Public Sub New()

    End Sub

    Public Function Search(ByVal searchTerm As String, ByVal searchType As SearchTypeE) As List(Of MovieItem)
        Dim sr As New SignedRequest(BLLSettings.Settings.AWSAccessKey, BLLSettings.Settings.AWSSecretKey, BLLSettings.Settings.AWSDestinationUrl)
        Dim sb As New StringBuilder()

        sb.Append("Service=AWSECommerceService")
        sb.Append("&Version=2009-07-01")
        sb.Append("&AssociateTag=" & BLLSettings.Settings.AWSAssociateTag)

        Select Case searchType
            Case SearchTypeE.EAN
                sb.Append("&Operation=ItemLookup")
                sb.Append("&SearchIndex=DVD")
                sb.Append("&IdType=EAN")
                sb.Append("&ItemId=" & searchTerm)
            Case SearchTypeE.ASIN
                sb.Append("&Operation=ItemLookup")
                sb.Append("&IdType=ASIN")
                sb.Append("&ItemId=" & searchTerm)
            Case SearchTypeE.Keyword
                sb.Append("&Operation=ItemSearch")
                sb.Append("&Keywords=" & searchTerm)
                sb.Append("&SearchIndex=DVD")

        End Select

        sb.Append("&ResponseGroup=Request,Large")

        Dim requestUrl As String = sr.Sign(sb.ToString)

        Dim ret As String = ""
        Dim doc As XmlDocument = DoSearch(requestUrl, ret)

        If ret = "OK" Then
            Dim Itms As List(Of MovieItem) = GetMovieItemsFromXML(doc)

            Return Itms
        End If

        Return Nothing
    End Function

    Private Function GetMovieItemsFromXML(ByVal doc As XmlDocument) As List(Of MovieItem)

        Dim lst As New List(Of MovieItem)

        If Not doc Is Nothing Then
            Dim ndList As XmlNodeList = doc.GetElementsByTagName("Item")
            If ndList.Count > 0 Then
                For Each e As XmlElement In ndList
                    Dim Itm As New MovieItem
                    For Each e1 As XmlElement In e.ChildNodes
                        Select Case e1.Name
                            Case "ASIN"
                                Itm.ASIN = e1.InnerText
                            Case "SalesRank"
                                Itm.AmazonSalesRank = e1.InnerText
                            Case "SmallImage"
                                Itm.SmallImageUrl = e1.FirstChild.InnerText
                            Case "MediumImage"
                                Itm.MediumImageUrl = e1.FirstChild.InnerText
                            Case "LargeImage"
                                Itm.LargeImageUrl = e1.FirstChild.InnerText
                            Case "ItemAttributes"
                                GetItemAttributes(Itm, e1)
                            Case "EditorialReviews"
                                Itm.Description = GetEditorialReviews(e1)
                            Case "BrowseNodes"
                                Itm.GenreName = GetGenreByBrowseNode(e1)
                        End Select
                    Next

                    lst.Add(Itm)
                Next
            End If
        End If

        Return lst

    End Function

    Private Function GetGenreByBrowseNode(ByVal e As XmlElement) As String
        If Not e.ChildNodes Is Nothing Then
            For Each e1 As XmlElement In e.FirstChild.ChildNodes
                If e1.Name.ToLower = "name" Then
                    Return e1.InnerText
                End If
            Next
        End If
        Return String.Empty
    End Function

    Private Function GetEditorialReviews(ByVal e As XmlElement) As String
        Dim reviews As String = ""

        For Each e1 As XmlElement In e.FirstChild.ChildNodes
            If e1.Name.ToLower = "content" Then
                reviews &= e1.InnerText & vbNewLine & vbNewLine
            End If
        Next
        Return Tools.RemoveHTMLTags(reviews)
    End Function

    Private Sub GetItemAttributes(ByRef item As MovieItem, ByVal element As XmlElement)
        Dim actorCount As Integer = 0

        For Each e1 As XmlElement In element
            Select Case e1.Name
                Case "Actor"
                    actorCount += 1
                    Select Case actorCount
                        Case 1
                            item.Actor1Name = e1.InnerText
                        Case 2
                            item.Actor2Name = e1.InnerText
                        Case 3
                            item.Actor3Name = e1.InnerText
                    End Select
                Case "Binding"
                    item.MediaFormatName = e1.InnerText
                Case "Director"
                    item.DirectorName = e1.InnerText
                Case "EAN"
                    item.EAN = e1.InnerText
                Case "Format"
                    item.MediaFormatName = e1.InnerText
                Case "Publisher"
                    item.Publisher = e1.InnerText
                Case "ReleaseDate"
                    item.PublishDate = e1.InnerText
                Case "AudienceRating"
                    item.AudienceRank = e1.InnerText
                Case "Title"
                    item.Title = e1.InnerText
            End Select
        Next
    End Sub


    Private Function DoSearch(ByVal URL As String, ByRef ErrMsg As String) As XmlDocument
        Try
            Dim Request As WebRequest = HttpWebRequest.Create(URL)
            Dim Response As WebResponse = Request.GetResponse

            Dim doc As XmlDocument = New XmlDocument

            doc.Load(Response.GetResponseStream)

            'If IO.File.Exists("c:\response.xml") Then IO.File.Delete("c:\response.xml")
            'doc.Save("c:\response.xml")

            ErrMsg = "OK"

            Return doc

        Catch ex As Exception
            ErrMsg = "Exception: " & ex.Message & vbNewLine & ex.StackTrace
        End Try

        Return Nothing
    End Function

End Class

Imports System.Web
Imports System.Web.Script.Serialization
Imports MediaManager2010.WCFContracts.V1

Public Class Items
    Implements IHttpHandler


    ''' <summary>
    '''  You will need to configure this handler in the Web.config file of your 
    '''  web and register it with IIS before being able to use it. For more information
    '''  see the following link: http://go.microsoft.com/?linkid=8101007
    ''' </summary>
#Region "IHttpHandler Members"

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            ' Return false in case your Managed Handler cannot be reused for another request.
            ' Usually this would be false in case you have some state information preserved per request.
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim req = context.Request
        Dim id As Integer = 0
        Dim friendId As Integer
        Dim genreId As Integer = 0
        Dim actorId As Integer
        Dim letter As String = ""
        Dim borrowToUserName As String = ""
        Dim addByEan As String = String.Empty
        Dim latest As String = ""

        If Not req.QueryString("id") Is Nothing Then
            Integer.TryParse(req.QueryString("id"), id)
        End If

        If Not req.QueryString("genreId") Is Nothing Then
            Integer.TryParse(req.QueryString("genreId"), genreId)
        End If

        If Not req.QueryString("friendId") Is Nothing Then
            Integer.TryParse(req.QueryString("friendId"), friendId)
        End If

        If Not req.QueryString("actorId") Is Nothing Then
            Integer.TryParse(req.QueryString("actorId"), actorId)
        End If

        If Not req.QueryString("letter") Is Nothing Then
            letter = req.QueryString("letter")
        End If

        If Not req.QueryString("latest") Is Nothing Then
            latest = req.QueryString("latest")
        End If

        If Not String.IsNullOrEmpty(latest) Then
            Dim movies = New BLLItems().GetLatestAdded
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.ToJson(movies))
            Return
        End If

        If Not req.QueryString("addByEAN") Is Nothing Then
            addByEan = req.QueryString("addByEAN")
        End If

        If Not req.QueryString("returnMovie") Is Nothing Then
            Dim returnMovieId As Integer = 0
            context.Response.ContentType = "application/json"
            If Integer.TryParse(req.QueryString("returnMovie"), returnMovieId) Then
                Dim ret = New BLLItems().ReturnBorrowedMovie(returnMovieId)
                context.Response.Write(Tools.Tojson(ret))
            Else
                context.Response.Write(Tools.Tojson("Die Id des Films konnte nicht übermittelt werden :-/"))
            End If
            Return
        End If

        If Not String.IsNullOrEmpty(addByEan) Then
            Dim itm As New ItemLookup()
            Dim lst = itm.Search(addByEan, ItemLookup.SearchTypeE.EAN)

            context.Response.ContentType = "application/json"

            If lst.Count > 0 Then
                Dim m As MovieItem = lst.First
                m.OwnerID = friendId
                Try
                    Dim result = New BLLItems().UpdateItem(lst.First)
                    context.Response.Write(Tools.Tojson(New StoreResult(result.Title & " wurde erfolgreich hinzugefügt am " & result.DateAdded, result)))
                Catch ex As Exception
                    context.Response.Write(Tools.Tojson(New StoreResult("Es ist ein Fehler beim Speichern der Anfrage aufgetreten :-|", Nothing)))
                End Try
            Else
                context.Response.Write(Tools.Tojson(New StoreResult("Es konnte kein Film gefunden werden :-|", Nothing)))
            End If
            Return
        End If

        If Not req.QueryString("borrowedById") Is Nothing Then
            Dim borrowedById As Integer = 0
            context.Response.ContentType = "application/json"
            If Not Integer.TryParse(req.QueryString("borrowedById"), borrowedById) Then
                context.Response.Write(Tools.Tojson("Die Id des Freundes konnte nicht übermittelt werden :-/"))
                Return
            End If
            Dim iBll As New BLLItems
            Dim ret = iBll.GetItemsByBorrowerID(borrowedById)
            context.Response.Write(Tools.Tojson(ret))
            Return

        End If

        If Not req.QueryString("lentById") Is Nothing Then
            Dim lentById As Integer = 0
            context.Response.ContentType = "application/json"
            If Not Integer.TryParse(req.QueryString("lentById"), lentById) Then
                context.Response.Write(Tools.Tojson("Die Id des Freundes konnte nicht übermittelt werden :-/"))
                Return
            End If
            Dim iBll As New BLLItems
            Dim ret = iBll.GetItemsByLenderByID(lentById)
            context.Response.Write(Tools.Tojson(ret))
            Return

        End If

        If Not req.QueryString("lentTo") Is Nothing Then
            Dim lentTo As Integer = 0
            context.Response.ContentType = "application/json"
            If Not Integer.TryParse(req.QueryString("lentTo"), lentTo) Then
                context.Response.Write(Tools.Tojson("Die Id des Freundes konnte nicht übermittelt werden :-/"))
                Return
            End If
            If id <= 0 Then
                context.Response.Write(Tools.Tojson("Die Id des Films konnte nicht übermittelt werden :-/"))
                Return
            End If
            Dim iBll As New BLLItems
            Dim ret = iBll.UpdateBorrow(id, friendId, Now, False)
            context.Response.Write(Tools.Tojson(ret))
            Return
        End If

        If Not req.QueryString("borrowTo") Is Nothing Then
            borrowToUserName = req.QueryString("borrowTo")
            context.Response.ContentType = "application/json"
            If id > 0 Then
                Dim user = New BLLFriends().GetUserByName(borrowToUserName)
                If Not user Is Nothing Then
                    Dim result = New BLLItems().UpdateBorrow(id, user.ID, Now)
                    If result <> "OK" Then
                        context.Response.Write(Tools.Tojson("Beim Erstellen der Leihanfrage ist uns ein Fehler unterlaufen :-|"))
                    Else
                        context.Response.Write(Tools.Tojson("Die Leihanfrage ist gespeichert und der Verleiher ist informiert :-)"))
                    End If
                Else
                    context.Response.Write(Tools.Tojson("Der Benutzer konnte leider nicht ermittelt werden :-|"))
                End If
            Else
                context.Response.Write(Tools.Tojson("Die Id des Films wurde nicht korrekt übermittelt :-|"))
            End If
            Return
        End If

        If Not String.IsNullOrEmpty(letter) Then
            Dim titles = New BLLItems().GetTitlesBeginningWith(letter)
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(titles))
            Return
        End If


        If genreId > 0 Then
            Dim movies = New BLLItems().GetItemsByGenreID(genreId)
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(movies))
            Return
        End If

        If actorId > 0 Then
            Dim movies = New BLLItems().GetItemsByActorID(actorId)
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(movies))
            Return
        End If

        If friendId > 0 Then
            Dim movies = New BLLItems().GetItemsByOwnerID(friendId)
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(movies))
            Return
        End If

        If id > 0 Then
            Dim movie = New BLLItems().GetItemByID(id)
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(movie))
        Else
            Dim movies = New BLLItems().PickRandomMovies
            context.Response.ContentType = "application/json"
            context.Response.Write(Tools.Tojson(movies))
        End If

        context.Response.Flush()
        context.Response.End()
    End Sub

#End Region

End Class

Public Class StoreResult
    Public ErrMessage As String
    Public ResultMessage As String
    Public Title As String
    Public Created As Date = Now
    Public CoverUrl As String

    Public Sub New(errmsg As String, m As MovieItem)
        If Not m Is Nothing Then
            Title = m.Title
            Created = m.DateAdded
            CoverUrl = m.CoverUrlMediaManager
            ResultMessage = errmsg
            ErrMessage = "OK"
        Else
            ErrMessage = errmsg
        End If
    End Sub
End Class

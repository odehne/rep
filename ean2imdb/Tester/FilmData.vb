
Imports System.Net
Imports Newtonsoft.Json
Imports Tester.ServiceResultClasses
Imports System.Web

Public Class FilmData

    Public Property Ean As String = ""
    Public Property LastResult As Integer = -1
    Public Property Name As String = ""
    Public Property OfDbId As String = ""
    Public Property ImDbId As String = ""
    Public Property ErrorMessage As String = ""
    Const apiSearchEanUrl = "http://ofdbgw.scheeper.de/searchean_json/"
    Const apiSearchImdbUrl = "http://ofdbgw.metawave.ch/movie/"

    Public Sub EanSearch()

        Dim json As String = New WebClient().DownloadString(apiSearchEanUrl & Me.Ean)
        Dim searchEanResult As ServiceResult = JsonConvert.DeserializeObject(Of ServiceResult)(json)
        Me.LastResult = searchEanResult.Ofdbgw.Status.RCode

        If Me.LastResult = 0 Then
            Me.Name = HttpUtility.HtmlDecode(searchEanResult.Ofdbgw.Resultat.Eintrag.First.Titel_De)
            Me.OfDbId = searchEanResult.Ofdbgw.Resultat.Eintrag.First.FilmId
        Else
            ErrorMessage = String.Format("{0} Not found: {1} - {2}", Me.Ean, searchEanResult.Ofdbgw.Status.RCode, searchEanResult.Ofdbgw.Status.RCodeDesc)
        End If

    End Sub

    Public Sub ImdbIdSearch()
        Dim xml As String = New WebClient().DownloadString(apiSearchImdbUrl & Me.OfDbId)

        If Not String.IsNullOrEmpty(xml) Then
            Dim elem As XElement = XElement.Parse(xml)

            If elem.<status>.<rcode>.Value <> "0" Then
                Me.LastResult = elem.<status>.<rcode>.Value
                ErrorMessage = String.Format("Failed to get Imdb Id from {0}", Me.OfDbId)
                Return
            End If

            Me.ImDbId = elem.<resultat>.<imdbid>.Value
            Me.LastResult = 0

        End If

    End Sub




    'http://ofdbgw.metawave.ch/movie/34402




End Class

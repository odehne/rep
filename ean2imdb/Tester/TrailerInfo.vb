Imports System.Net

Public Class TrailerInfo

    Public Const TrailerBaseUrl As String = "http://api.traileraddict.com/?imdb="

    Public Trailers As List(Of String)

    Public Sub New(ByVal imdbId As String, width As Integer)
        Trailers = New List(Of String)

        If width = 0 Then width = 680

        If Not String.IsNullOrEmpty(imdbId) Then
            
            Dim result As String = New WebClient().DownloadString(TrailerBaseUrl & imdbId & "&width=" & width)

            If Not String.IsNullOrEmpty(result) Then

                Dim xml = XElement.Parse(result)

                Trailers = (From x In xml.Descendants Where x.Name = "embed" Select x.Value).ToList

            End If

        End If

    End Sub
End Class
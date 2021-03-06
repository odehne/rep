﻿
Public Module Module1

	Public Sub Main()

        Dim args = Environment.GetCommandLineArgs()

        If args.Length <= 2 Then
            Console.WriteLine("usage: ean2imdb <imdb|ofdb|trailer|all> {0}connection string{0}", Chr(34))
            End
        End If

        Dim updater As DbUpdater = New DbUpdater(args(2))
        AddHandler updater.StatusChange, AddressOf OnStatusChange

        Do

            If args(1).ToLower = "imdb" Then
                updater.UpdateImdbId()
            ElseIf args(1).ToLower = "ofdb" Then
                updater.UpdateOfDbId()
            ElseIf args(1).ToLower = "trailer" Then
                updater.UpdateTrailer()
            ElseIf args(1).ToLower = "all" Then
                updater.UpdateImdbId()
                updater.UpdateOfDbId()
                updater.UpdateTrailer()
            End If
            Console.WriteLine("Retry in 5 minutes ...")
            Threading.Thread.Sleep(New TimeSpan(0, 0, 5, 0))
        Loop


    End Sub

    Private Sub OnStatusChange(ByVal status As DbUpdater.StatusE, ByVal message As String)
        Console.WriteLine("{0}: {1}", status.ToString, message)
    End Sub
End Module

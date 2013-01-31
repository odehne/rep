
Public Module Module1

	Public Sub Main()

        Dim args = Environment.GetCommandLineArgs()

        If args.Length <= 2 Then
            Console.WriteLine("usage: ean2imdb <imdb|ofdb|both> {0}connection string{0}", Chr(34))
            End
        End If

        Dim updater As DbUpdater = New DbUpdater(args(2))
        AddHandler updater.StatusChange, AddressOf OnStatusChange

        If args(1).ToLower = "imdb" Then
            updater.UpdateImdbId()
        ElseIf args(1).ToLower = "ofdb" Then
            updater.UpdateOfDbId()
        ElseIf args(1).ToLower = "both" Then
            updater.UpdateImdbId()
            updater.UpdateOfDbId()
        End If

    End Sub

    Private Sub OnStatusChange(ByVal status As DbUpdater.StatusE, ByVal message As String)
        Console.WriteLine("{0}: {1}", status.ToString, message)
    End Sub
End Module

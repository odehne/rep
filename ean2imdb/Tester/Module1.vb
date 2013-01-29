
Public Module Module1

	Public Sub Main()

        Dim args = Environment.GetCommandLineArgs()

        If args.Length <= 1 Then
            Console.WriteLine("usage: ean2imdb {0}connection string{0}", Chr(34))
            End
        End If

        Dim updater As DbUpdater = New DbUpdater(args(1))
        AddHandler updater.StatusChange, AddressOf OnStatusChange

        updater.Update()

	End Sub

    Private Sub OnStatusChange(ByVal status As DbUpdater.StatusE, ByVal message As String)
        Console.WriteLine("{0}: {1}", status.ToString, message)
    End Sub
End Module

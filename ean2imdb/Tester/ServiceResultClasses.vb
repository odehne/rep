Option Strict On

Namespace ServiceResultClasses

	Friend Class ServiceResult

		Public Property Ofdbgw As New OfdbGw

	End Class


	Friend Class OfdbGw

		Public Property Status As New Status
		Public Property Resultat As New Resultat

	End Class


	Friend Class Status

		Public Property RCode As String = ""
		Public Property RCodeDesc As String = ""
		Public Property Modul As String = ""
		Public Property OfdbgwVersion As String = ""
		Public Property OfdbgwDate As String = ""
		Public Property Verarbeitungszeit As String = ""

	End Class


	Friend Class Resultat

		Public Property Eintrag As New List(Of Eintrag)

	End Class


	Friend Class Eintrag

		Public Property FilmId As String = ""
		Public Property FassungId As String = ""
		Public Property Titel_De As String = ""
		Public Property Titel_Orig As String = ""
		Public Property Jahr As String = ""

	End Class

End Namespace
Imports System.Runtime.Serialization

Namespace WCFContracts.V1

    <DataContract(Name:="Rating")> _
    Public Class Rating
        <DataMember()> _
        Public ID As Integer = 0

        <DataMember()> _
        Public ItemID As Integer = 0

        <DataMember()> _
        Public UserID As Integer = 0

        <DataMember()> _
        Public UserName As String

        <DataMember()> _
        Public Comment As String

        <DataMember()> _
        Public Subject As String

        <DataMember()> _
        Public Value As Integer = 0

        <DataMember()> _
        Public RatingTable As String

      
        Public Sub New()

        End Sub

        Public Sub New(ByVal t As tblRating)

            Dim uBLL As New BLLFriends

            Me.ItemID = t.ItemID
            Me.ID = t.ID
            Me.Value = t.Rating
            Me.UserID = t.UserID
            Me.Comment = t.Comment
            Me.Subject = t.Subject
           
            Dim u As User = uBLL.GetUserByID(t.UserID)

            If Not u Is Nothing Then
                Me.UserName = u.Username
            End If

        End Sub

    
    End Class


    <DataContract(Name:="MovieItem")> _
    Public Class MovieItem

        <DataMember()> _
        Public ImdbIdb As String

        Public Sub New()
            Me.Ratings = New List(Of Rating)
            Me.GenreIDs = New List(Of Integer)
            Me.GenreNames = New List(Of String)
        End Sub


        <DataMember()> _
        Public Property Comments As String
            Get
                Dim sb As New StringBuilder
                For Each c In Ratings
                    If Not String.IsNullOrEmpty(c.Comment) Then
                        sb.AppendLine("<li>")
                        sb.AppendLine("<p>" & c.UserName & ": " & c.Subject & "<br />")
                        sb.AppendLine(c.Comment)
                        sb.AppendLine("</p><br />")
                        sb.AppendLine("</li>")
                    End If
                Next
                Return sb.ToString
            End Get
            Set(value As String)

            End Set
        End Property

        <DataMember()> _
        Public Property RatingsCount As Integer
            Get
                Return Me.Ratings.Count
            End Get
            Set(value As Integer)

            End Set
        End Property

        <DataMember()> _
        Public Property AggregatedStars As String
            Get
                Dim i As Integer = 0
                Dim strs As Integer = 0
                Dim value As Integer = 0
                Dim sb As New StringBuilder

                If Ratings.Count > 0 Then
                    For Each r In Ratings
                        i += 1
                        strs += r.Value
                    Next
                    value = Math.Floor(strs / i)
                    For i = 1 To 5
                        If i <= value Then
                            sb.AppendLine("<img src='content/images/star_yellow.png' height='16' width='16' alt='Yellow Star' />")
                        Else
                            sb.AppendLine("<img src='content/images/star_grey.png' height='16' width='16' alt='Grey Star' />")
                        End If
                    Next
                End If
                Return sb.ToString
            End Get
            Set(value As String)

            End Set
        End Property

        <DataMember()> _
        Public Trailer As String

        <DataMember()> _
        Public Property RatingUsername As String
            Get
                If Not Ratings Is Nothing AndAlso Ratings.Count > 0 Then
                    Return Ratings(0).UserName
                End If
                Return String.Empty
            End Get
            Set(value As String)

            End Set
        End Property


        <DataMember()> _
        Public ID As Integer = 0

        <DataMember()> _
        Public Title As String = ""

        <DataMember()> _
        Public GenreID As Integer = 0

        <DataMember()> _
        Public GenreName As String = ""

        <DataMember()> _
        Public GenreIDs As List(Of Integer)

        <DataMember()> _
        Public GenreNames As List(Of String)

        <DataMember()> _
        Public MediaTypeID As Integer = 0

        <DataMember()> _
        Public MediaTypeName As String = ""

        <DataMember()> _
        Public MediaFormatID As Integer = 0

        <DataMember()> _
        Public MediaFormatName As String = ""

        <DataMember()> _
        Public AuthorID As Integer = 0

        <DataMember()> _
        Public AuthorName As String = "Unknown"

        <DataMember()> _
        Public Actor1ID As Integer = 0

        <DataMember()> _
        Public Actor1Name As String = "Unknown"

        <DataMember()> _
        Public Actor2ID As Integer = 0

        <DataMember()> _
        Public Actor2Name As String = "Unknown"

        <DataMember()> _
        Public Actor3ID As Integer = 0

        <DataMember()> _
        Public Actor3Name As String = "Unknown"

        <DataMember()> _
        Public DirectorID As Integer = 0

        <DataMember()> _
        Public DirectorName As String = "Unknown"

        <DataMember()> _
        Public PublishDate As String = ""

        <DataMember()> _
        Public Publisher As String = ""

        <DataMember()> _
        Public OwnerName As String = ""

        <DataMember()> _
        Public OwnerID As Integer = 0

        <DataMember()> _
        Public EAN As String = ""

        <DataMember()> _
        Public CoverUrlMediaManager As String = ""

        <DataMember()> _
        Public Description As String = ""

        <DataMember()> _
        Public AudienceRank As String = ""

        <DataMember()> _
        Public AmazonSalesRank As Integer = 0

        <DataMember()> _
        Public ASIN As String = ""

        <DataMember()> _
        Public SmallImageUrl As String = ""

        <DataMember()> _
        Public MediumImageUrl As String = ""

        <DataMember()> _
        Public LargeImageUrl As String = ""

        <DataMember()> _
        Public BorrowedByID As Integer = 0

        <DataMember()> _
        Public BorrowedByName As String = ""

        <DataMember()> _
        Public BorrowedSince As DateTime

        <DataMember()> _
        Public BorrowCount As Integer

        <DataMember()>
        Public DateAdded As DateTime

        <DataMember()>
        Public Ratings As List(Of Rating)

        <DataMember()> _
        Public CtrlID As String = ""

        <DataMember()> _
        Public ResultMessage As String = ""

    End Class



End Namespace
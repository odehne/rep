Imports MediaManager2010.WCFContracts.V1


Public Class BLLRatings
    Public Function GetData() As List(Of Rating)
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblRatings _
                     Select co = New Rating(r)

        Return source.ToList
    End Function

    Public Function MapToRating(ByVal t As tblRating) As Rating
        Dim mappedRating As New Rating

        mappedRating.ID = t.ID
        mappedRating.ItemID = t.ItemID
        mappedRating.UserID = t.UserID
        mappedRating.Value = t.Rating
        mappedRating.Comment = t.Comment

        Return mappedRating
    End Function


    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetRatingsByItemID(ByVal ItemID As Integer) As List(Of Rating)
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblRatings _
                     Select co = New Rating(r)
                     Where co.ItemID = ItemID

        Return source

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetBestRatingByItemID(ByVal ItemID As Integer) As Rating
        Dim d As New MediaLibraryLinqDataContext()

        Dim src = (From r In d.tblRatings _
                     Select r Where r.ItemID = ItemID).Max


        If Not src Is Nothing Then
            Return Me.MapToRating(src)
        End If

        Return New Rating

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetRatingByID(ByVal ID As Integer) As Rating
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblRatings _
                     Select co = New Rating(r)
                     Where co.ID = ID

        Return source

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetTblRatingByID(ByVal ID As Integer) As tblRating
        Dim d As New MediaLibraryLinqDataContext()

        Dim source = From r In d.tblRatings _
                     Select r Where r.ID = ID

        Return source.First

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, False)> _
    Public Function AddRating(ByVal ItemID As Integer, ByVal UserID As Integer, ByVal comment As String, ByVal Value As Integer, ByVal subject As String) As Integer

        If ItemID > 0 And UserID Then

            Dim m As New tblRating
            Dim c As New MediaLibraryLinqDataContext()

            m.ItemID = ItemID
            m.UserID = UserID
            m.Comment = comment
            m.Rating = Value
            m.Subject = subject

            c.tblRatings.InsertOnSubmit(m)
            c.SubmitChanges()

            Return m.ID
        End If

        Return 0

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, False)> _
    Public Function UpdateRating(ByVal ID As Integer,
                                 ByVal UserID As Integer,
                                 ByVal ItemID As Integer,
                                 ByVal comment As String,
                                 ByVal subject As String,
                                 ByVal value As Integer) As Integer

        If ItemID > 0 And UserID > 0 Then
            Dim c As New MediaLibraryLinqDataContext()

            Dim Rating As tblRating = (From r In c.tblRatings
                     Select r Where r.ID = ID).First

            If Not Rating Is Nothing Then
                Rating.Rating = value
                Rating.Comment = comment
                Rating.Subject = subject
            End If

            c.SubmitChanges()
        End If

    End Function
End Class

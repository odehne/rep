Imports System.Runtime.Serialization

<DataContract(Name:="AmazonItem")> _
Public Class AmazonItem

    <DataMember()> _
    Public Genre As String

    <DataMember()> _
    Public CoverUrl As String

    <DataMember()> _
    Public CoverUrlSmall As String

    <DataMember()> _
    Public Author As String

    <DataMember()> _
    Public Producer As String

    <DataMember()> _
    Public Director As String

    <DataMember()> _
    Public ReleaseDate As String

    <DataMember()> _
    Public Actor1 As String

    <DataMember()> _
    Public Actor2 As String

    <DataMember()> _
    Public Actor3 As String

    <DataMember()> _
    Public Fullname As String

    <DataMember()> _
    Public Format As String

    <DataMember()> _
    Public Media As String

    <DataMember()> _
    Public AmazonSalesRank As Integer

    <DataMember()> _
    Public AudienceRating As String

    <DataMember()> _
    Public Description As String

End Class

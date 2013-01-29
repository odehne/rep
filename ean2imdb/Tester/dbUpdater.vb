Imports System.Data.SqlClient

Public Class DbUpdater
    Private _connString As String

    Public Enum StatusE
        Ok
        Failed
    End Enum

    Public Event StatusChange(status As StatusE, message As String)

    Public ReadOnly Property ConnString() As String
        Get
            Return _connString
        End Get
    End Property

    Public Sub New(ByVal connString As String)
        _connString = connString
    End Sub

    Public Sub Update()

        Using conn As SqlConnection = New SqlConnection(ConnString)

            Dim selectQuery As String = "Select ean from tblitems where ean <> '' and ofdbId is null and ofdbStatus<>4"

            Using updateConn As SqlConnection = New SqlConnection(ConnString)

                Try
                    conn.Open()
                    updateConn.Open()
                Catch ex As Exception
                    RaiseEvent StatusChange(StatusE.Failed, "Failed to connect to database [" & ex.Message & "]")
                    Return
                End Try

                Dim cmd As New SqlCommand(selectQuery, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim fd As New FilmData With {.EAN = reader("ean")}
                    fd.EanSearch()

                    If Not String.IsNullOrEmpty(fd.ErrorMessage) Then
                        RaiseEvent StatusChange(DbUpdater.StatusE.Failed, String.Format("{0} failed {1})", fd.EAN, fd.ErrorMessage))
                    Else
                        RaiseEvent StatusChange(DbUpdater.StatusE.Ok, String.Format("{0} -> {1} ({2})", fd.EAN, fd.Name, fd.OFDbId))
                    End If

                    Dim updateCmd As New SqlCommand("update tblItems set ofdbId=@ofdbId, ofdbStatus=@ofdbStatus, imdbId=@imdbId where ean=@ean", updateConn)

                    updateCmd.Parameters.AddWithValue("ean", fd.Ean)
                    updateCmd.Parameters.AddWithValue("ofdbId", fd.OfDbId)
                    updateCmd.Parameters.AddWithValue("imdbId", fd.ImDbId)
                    updateCmd.Parameters.AddWithValue("ofdbStatus", fd.LastResult)

                    Try
                        updateCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        RaiseEvent StatusChange(DbUpdater.StatusE.Failed, String.Format("Failed to update database {0}", ex.Message))
                    End Try

                    Threading.Thread.Sleep(1000)
                End While

            End Using

        End Using


    End Sub


End Class

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

    Public Sub UpdateOfDbId()

        Using conn As SqlConnection = New SqlConnection(ConnString)

            Dim selectQuery As String = "Select ean from tblitems where ean <> '' and ofdbId is null and ofdbStatus<>4"

            Using updateConn As SqlConnection = New SqlConnection(ConnString)

                Try
                    conn.Open()
                    updateConn.Open()
                Catch ex As Exception
                    RaiseEvent StatusChange(StatusE.Failed, "Database connection issue [" & ex.Message & "]")
                    Return
                End Try

                Dim cmd As New SqlCommand(selectQuery, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim fd As New FilmData With {.Ean = reader("ean")}
                    fd.EanSearch()

                    If Not String.IsNullOrEmpty(fd.ErrorMessage) Then
                        RaiseEvent StatusChange(DbUpdater.StatusE.Failed, String.Format("{0} failed {1})", fd.Ean, fd.ErrorMessage))
                    Else
                        RaiseEvent StatusChange(DbUpdater.StatusE.Ok, String.Format("{0} -> {1} ({2})", fd.Ean, fd.Name, fd.OfDbId))
                    End If

                    Dim updateCmd As New SqlCommand("update tblItems set ofdbId=@ofdbId, ofdbStatus=@ofdbStatus where ean=@ean", updateConn)

                    updateCmd.Parameters.AddWithValue("ean", fd.Ean)

                    If Not String.IsNullOrEmpty(fd.OfDbId) Then
                        updateCmd.Parameters.AddWithValue("ofdbId", fd.OfDbId)
                    Else
                        updateCmd.Parameters.AddWithValue("ofdbId", Nothing)
                    End If

                    updateCmd.Parameters.AddWithValue("ofdbStatus", fd.LastResult)

                    Try
                        updateCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        RaiseEvent StatusChange(DbUpdater.StatusE.Failed, String.Format("Database update issue {0}", ex.Message))
                    End Try

                    Threading.Thread.Sleep(1000)
                End While

            End Using

        End Using


    End Sub


    Public Sub UpdateImdbId()

        Using conn As SqlConnection = New SqlConnection(ConnString)

            Dim selectQuery As String = "Select ofdbid from tblitems where ean <> '' and imdbId is null and not OfDbId is null"

            Using updateConn As SqlConnection = New SqlConnection(ConnString)

                Try
                    conn.Open()
                    updateConn.Open()
                Catch ex As Exception
                    RaiseEvent StatusChange(StatusE.Failed, "Database connection issue. Please check connection string [" & ex.Message & "]")
                    Return
                End Try

                Dim cmd As New SqlCommand(selectQuery, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()

                    Dim fd As New FilmData With {.OfDbId = reader("OfDbId")}
                    Dim updateCmd As New SqlCommand()
                    updateCmd.Connection = updateConn

                    fd.ImdbIdSearch()

                    If Not String.IsNullOrEmpty(fd.ErrorMessage) Then
                        RaiseEvent StatusChange(DbUpdater.StatusE.Failed, String.Format("{0} failed {1})", fd.OfDbId, fd.ErrorMessage))
                    Else
                        updateCmd.CommandText = "update tblItems set imdbId=@imdbId where ofdbid=@ofdbid"
                        updateCmd.Parameters.AddWithValue("ofdbid", fd.OfDbId)
                        updateCmd.Parameters.AddWithValue("imdbId", fd.ImDbId)
                        Try
                            updateCmd.ExecuteNonQuery()
                            RaiseEvent StatusChange(DbUpdater.StatusE.Ok, String.Format("{0} -> {1}", fd.OfDbId, fd.ImDbId))
                        Catch ex As Exception
                            RaiseEvent StatusChange(DbUpdater.StatusE.Failed, String.Format("Update imdb record {0}", ex.Message))
                        End Try
                    End If



                  
                    Threading.Thread.Sleep(2500)
                End While

            End Using

        End Using


    End Sub


    Public Sub UpdateTrailer()
        Using conn As SqlConnection = New SqlConnection(ConnString)

            Dim selectQuery As String = "Select imdbid from tblitems where ean <> '' and trailer is null and not imdbid is null"

            Using updateConn As SqlConnection = New SqlConnection(ConnString)

                Try
                    conn.Open()
                    updateConn.Open()
                Catch ex As Exception
                    RaiseEvent StatusChange(StatusE.Failed, "Database connection issue. Please check connection string [" & ex.Message & "]")
                    Return
                End Try

                Dim cmd As New SqlCommand(selectQuery, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim imdbId As String = reader("imdbid")
                    Dim tInfo As New TrailerInfo(imdbId, 420)
                    Dim updateCmd As New SqlCommand()
                    updateCmd.Connection = updateConn

                    If tInfo.Trailers.Count = 0 Then
                        RaiseEvent StatusChange(DbUpdater.StatusE.Failed, String.Format("{0} returned no trailer :/", imdbId))
                    Else
                        updateCmd.CommandText = "update tblItems set trailer=@trailer where imdbid=@imdbid"
                        updateCmd.Parameters.AddWithValue("trailer", tInfo.Trailers(0))
                        updateCmd.Parameters.AddWithValue("imdbId", ImDbId)
                        Try
                            updateCmd.ExecuteNonQuery()
                            RaiseEvent StatusChange(DbUpdater.StatusE.Ok, String.Format("Found a trailer for {0}", imdbId))
                        Catch ex As Exception
                            RaiseEvent StatusChange(DbUpdater.StatusE.Failed, String.Format("Update trailer record {0}", ex.Message))
                        End Try
                    End If




                    Threading.Thread.Sleep(200)
                End While

            End Using

        End Using

    End Sub
End Class
Public Class BLLSettings

    Public Class GlobalSettings
        Public Property AWSAccessKey As String
        Public Property AWSSecretKey As String
        Public Property AWSAssociateTag As String
        Public Property AWSDestinationUrl As String
        Public Property BaseUrl As String

        Public Property SmtpServer As String
        Public Property SmtpUser As String
        Public Property SmtpPassword As String
        Public Property SmtpReplyToAddress As String
    End Class

    Public Sub New()
        LoadAwsSettings()
    End Sub

    Private Shared _settings As GlobalSettings
    Public Shared ReadOnly Property Settings As GlobalSettings
        Get
            If _settings Is Nothing Then
                LoadAwsSettings()
            End If
            Return _settings
        End Get
    End Property

    Public Function NotifyNeeded() As Boolean

        If Now.DayOfWeek = DayOfWeek.Tuesday Then

            Dim lasttime As Date = LastNotifyDate()

            If DateDiff(DateInterval.Day, lasttime, Now.Date) > 1 Then

                Return True

            End If

        End If

        Return False

    End Function

    Private Shared Sub LoadAwsSettings()

        _settings = New GlobalSettings()

        _settings.AWSAccessKey = GetValue("AWS_ACCESS_KEY")
        _settings.AWSSecretKey = GetValue("AWS_SECRET_KEY")
        _settings.AWSAssociateTag = GetValue("AWS_ASSOCIATE_TAG")
        _settings.AWSDestinationUrl = GetValue("AWS_DESTINATION_URL")
        _settings.BaseUrl = GetValue("BaseUrl")
        _settings.SmtpServer = GetValue("SmtpServer")
        _settings.SmtpUser = GetValue("SmtpUser")
        _settings.SmtpPassword = GetValue("SmtpPassword")
        _settings.SmtpReplyToAddress = GetValue("SmtpReplyToAddress")
    End Sub

    Public Shared Function GetValue(name As String) As String

        Dim c As New MediaLibraryLinqDataContext()

        Return (From r In c.tblSettings Where r.Name = name).First.Value

    End Function

    Public Shared Function LastNotifyDate() As Date

        Dim c As New MediaLibraryLinqDataContext()

        Return (From r In c.tblSettings Where r.Name = "LastEmailNotification").First.Updated

    End Function

    Public Shared Sub UpdateNotificationDate()

        Dim c As New MediaLibraryLinqDataContext()
        Dim setting = (From r In c.tblSettings Where r.Name = "LastEmailNotification").First

        setting.Value = Date.Now
        c.SubmitChanges()

    End Sub

End Class

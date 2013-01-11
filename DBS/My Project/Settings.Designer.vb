﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.18010
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My

    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(), _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0"), _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase

        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()), MySettings)

#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region

        Public Shared ReadOnly Property [Default]() As MySettings
            Get

#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl), _
         Global.System.Configuration.DefaultSettingValueAttribute("https://ecs.amazonaws.de/onca/soap?Service=AWSECommerceService")> _
        Public ReadOnly Property MediaManager2010_Amazon_AWSECommerceService() As String
            Get
                Return CType(Me("MediaManager2010_Amazon_AWSECommerceService"), String)
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.DefaultSettingValueAttribute("http://odmobil6/MediaManager2010")> _
        Public ReadOnly Property BaseURI() As String
            Get
                Return CType(Me("BaseURI"), String)
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.DefaultSettingValueAttribute("127.0.0.1")> _
        Public ReadOnly Property SmtpServer() As String
            Get
                Return CType(Me("SmtpServer"), String)
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.DefaultSettingValueAttribute("")> _
        Public ReadOnly Property SmtpUser() As String
            Get
                Return CType(Me("SmtpUser"), String)
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.DefaultSettingValueAttribute("")> _
        Public ReadOnly Property SmtpPassword() As String
            Get
                Return CType(Me("SmtpPassword"), String)
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.DefaultSettingValueAttribute("no-reply@mediamanager.de")> _
        Public ReadOnly Property SmtpReplyToAddress() As String
            Get
                Return CType(Me("SmtpReplyToAddress"), String)
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.DefaultSettingValueAttribute("AKIAJGLLZFOW44IYKYHA")> _
        Public ReadOnly Property AWS_Access_Key() As String
            Get
                Return CType(Me("AWS_Access_Key"), String)
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.DefaultSettingValueAttribute("mBx26Oy2vrwTaiqYIKiHFsyYjjVZOV3ty8U+QDdX")> _
        Public ReadOnly Property AWS_Secret_Key() As String
            Get
                Return CType(Me("AWS_Secret_Key"), String)
            End Get
        End Property

        <Global.System.Configuration.ApplicationScopedSettingAttribute(), _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
         Global.System.Configuration.DefaultSettingValueAttribute("odehnewordpre-20")> _
        Public ReadOnly Property AWS_Associate_Tag() As String
            Get
                Return CType(Me("AWS_Associate_Tag"), String)
            End Get
        End Property
    End Class
End Namespace

Namespace My

    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(), _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()> _
    Friend Module MySettingsProperty

        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")> _
        Friend ReadOnly Property Settings() As Global.MediaManager2010.My.MySettings
            Get
                Return Global.MediaManager2010.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace

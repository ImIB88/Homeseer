Sub Main(ByVal Parms As Object)

    ' Device reference numbers
    Dim deviceRef As Integer = 964 ' Device that triggers the script
    Dim groupDeviceRef As Integer = 1206 ' Device that holds the group/speaker value

    ' Get the current values
    Dim deviceValue As Integer = hs.DeviceValue(deviceRef)
    Dim groupValue As Integer = hs.DeviceValue(groupDeviceRef)

    ' Define the CSV list
    Dim chromecastList As String = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx, Stue, Google Nest Audio, 5;" & _
                                  "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx, Gang, Google Nest Audio, 10;" & _
                                  "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx, Soverom, Google Nest Mini, 15;" & _
                                  "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx, Andre etasje, Google Cast Group, 20;" & _
                                  "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx, Alle Nest, Google Cast Group, 30;" & _
                                  "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxb, Stue og gang, Google Cast Group, 40"

    ' Split the CSV list into an array of Chromecast entries
    Dim chromecastEntries As String() = chromecastList.Split(";"c)

    ' Loop through each entry to find the Chromecast ID based on the group value
    Dim chromecastId As String = ""
    For Each entry As String In chromecastEntries
        Dim parts As String() = entry.Split(","c)
        If parts.Length = 4 AndAlso Convert.ToInt32(parts(3)) = groupValue Then
            chromecastId = parts(0)
            Exit For
        End If
    Next

    ' If Chromecast ID is found, proceed with casting
    If Not String.IsNullOrEmpty(chromecastId) Then
        ' Create the Collection for radio channels
        Dim radioChannels As New Collection
        radioChannels.Add(New String() {"https://lyd.nrk.no/nrk_radio_p1_buskerud_aac_h", "NRK P1"}, "10")
        radioChannels.Add(New String() {"https://lyd.nrk.no/nrk_radio_p2_aac_h", "NRK P2"}, "20")
        radioChannels.Add(New String() {"https://lyd.nrk.no/nrk_radio_p3_aac_h", "NRK P3"}, "30")
        radioChannels.Add(New String() {"https://lyd.nrk.no/nrk_radio_alltid_nyheter_aac_h", "NRK Nyheter"}, "40")
        radioChannels.Add(New String() {"https://lyd.nrk.no/nrk_radio_p13_aac_h", "NRK P13"}, "45")
        radioChannels.Add(New String() {"https://lyd.nrk.no/nrk_radio_jazz_aac_h", "NRK Jazz"}, "50")
        radioChannels.Add(New String() {"https://lyd.nrk.no/nrk_radio_mp3_aac_h", "NRK MP3"}, "60")
        radioChannels.Add(New String() {"https://lyd.nrk.no/nrk_super_aac_h", "NRK Super"}, "70")
        radioChannels.Add(New String() {"https://p4.p4groupaudio.com/P04_AH", "P4"}, "80")
        radioChannels.Add(New String() {"https://p5.p4groupaudio.com/P05_AH", "P5 Hits"}, "90")
        radioChannels.Add(New String() {"https://p6.p4groupaudio.com/P06_AH", "P6 Rock"}, "100")
        radioChannels.Add(New String() {"https://p7.p4groupaudio.com/P07_AH", "P7 Klem"}, "110")
        radioChannels.Add(New String() {"https://p8.p4groupaudio.com/P08_AH", "P8 Pop"}, "120")
        radioChannels.Add(New String() {"https://p9.p4groupaudio.com/P09_AH", "P9 Retro"}, "130")
        radioChannels.Add(New String() {"https://p10.p4groupaudio.com/P10_AH", "P10 Country"}, "140")
        radioChannels.Add(New String() {"https://p11.p4groupaudio.com/P11_AH", "P11 Bandit"}, "150")
        radioChannels.Add(New String() {"https://nrj.p4groupaudio.com/NRJ_AH", "NRJ"}, "160")

        ' Collection for music files
        Dim mediaFiles As New Collection
        mediaFiles.Add("C:\Program Files (x86)\HomeSeer HS4\Media\Spilleliste Wake Up Gently.mp3", "200")
        mediaFiles.Add("C:\Program Files (x86)\HomeSeer HS4\Media\Beep-beep-beep vekkerklokkelyd.mp3", "210")
        '... Add the rest of your music files here

        If deviceValue >= 200 Then
            ' Play media file
            Dim filename As String = mediaFiles(deviceValue.ToString())
            hs.MediaFilename = filename
            hs.MediaPlay("")
        
        Else
            ' Cast radio channel
            Dim channel As String() = radioChannels(deviceValue.ToString())
            Dim mediaUrl As String = channel(0)
            Dim mediaTitle As String = channel(1)
            Dim mediaMimeType As String = "audio/aac" ' Media MIME type
            Dim mediaImageUrl As String = "" ' Media Image URL

            Try
                hs.PluginFunction("Chromecast", "", "CastMedia", New Object() {chromecastId, mediaUrl, mediaMimeType, mediaTitle, mediaImageUrl})
                hs.WriteLog("Cast Radio", "Media has been sent to Chromecast.")
            Catch ex As Exception
                hs.WriteLog("Error", "Error in Cast Radio: " & ex.Message)
            End Try
        End If
    Else
        hs.WriteLog("Chromecast", "Chromecast ID not found for the given group value.")
    End If

End Sub

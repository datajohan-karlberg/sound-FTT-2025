Imports NAudio.Wave
Imports FftSharp
Imports System.Numerics

Public Class Form1

    Dim waveIn As WaveInEvent
    'Dim fft As New FFT(1024)
    Dim buffer(1023) As Double
    Dim sampleIndex As Integer = 0
    Dim fftBins(7) As Double

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        waveIn = New WaveInEvent()
        waveIn.WaveFormat = New WaveFormat(44100, 1)
        AddHandler waveIn.DataAvailable, AddressOf OnDataAvailable
        waveIn.BufferMilliseconds = 25
        waveIn.StartRecording()

        Timer1.Interval = 10
        Timer1.Start()
    End Sub

    Private Sub OnDataAvailable(sender As Object, e As WaveInEventArgs)
        ' Konvertera bytes till samples
        For i As Integer = 0 To e.BytesRecorded - 2 Step 2
            If sampleIndex < buffer.Length Then
                Dim sample As Short = BitConverter.ToInt16(e.Buffer, i)
                buffer(sampleIndex) = sample / 32768.0
                sampleIndex += 1
            End If
        Next

        If sampleIndex >= buffer.Length Then
            ' Kör FFT
            Dim fftResult() As Complex = FFT.Forward(buffer)
            Dim magnitudes() As Double = FFT.Magnitude(fftResult)

            'MsgBox(UBound(magnitudes, 1))  '512
            ' Summera till 8 band



            ' Frekvensgränser för 8 logaritmiska band (i Hz)
            Dim bandLimits() As Integer = {50, 100, 200, 400, 800, 1600, 3200, 6400, 12800}
            fftBins = New Double(bandLimits.Length - 2) {}

            Dim sampleRate As Integer = 44100
            Dim fftSize As Integer = buffer.Length

            For b As Integer = 0 To bandLimits.Length - 2
                Dim fLow = bandLimits(b)
                Dim fHigh = bandLimits(b + 1)

                ' Mappa till FFT-index
                Dim iLow As Integer = CInt(fLow / sampleRate * fftSize)
                Dim iHigh As Integer = CInt(fHigh / sampleRate * fftSize) - 1

                Dim sum As Double = 0
                Dim count As Integer = 0

                For i As Integer = iLow To Math.Min(iHigh, magnitudes.Length - 1)
                    sum += magnitudes(i)
                    count += 1
                Next

                If count > 0 Then
                    Dim avg = sum / count * 2
                    fftBins(b) = Math.Max(0, 20 * Math.Log10(avg + 0.000001)) * 10
                    fftBins(b) = sum / count * 2

                Else
                    fftBins(b) = 0
                End If
            Next



            sampleIndex = 0
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Panel1.Invalidate()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        If fftBins Is Nothing Then Return

        Dim g = e.Graphics
        g.Clear(Color.Black)
        Dim barWidth = Panel1.Width \ fftBins.Length

        For i = 0 To fftBins.Length - 1
            Dim height = CInt(Math.Min(fftBins(i) * 200, Panel1.Height)) * 5
            g.FillRectangle(Brushes.LimeGreen, i * barWidth, Panel1.Height - height, barWidth - 2, height)
        Next
    End Sub
End Class

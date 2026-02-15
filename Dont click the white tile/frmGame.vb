Public Class frmGame

    Shared Property Instance As Object

    Private Actions As Integer = 0
    Private LastCheckedActions As Integer = 0
    Private Stopwatch As New Stopwatch()

    Public Property Points As Integer = 0
    Public Property ActionsPerMinute As Integer
    Public Property GameOver As Boolean
    Public Property Time As String

    Public Property Tiles As New List(Of Tile)
    Private _rnd As New Random

    Private Sub KeyDowns(sender As Object, e As KeyEventArgs) Handles Me.KeyDown, Button1.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape : Me.Close()
            Case Keys.R : Restart()
        End Select
    End Sub

    Private Sub frmGame_Load(sender As Object, e As EventArgs) Handles Me.Load
        setupForm()
        startGame()
    End Sub

    Private Sub setupForm()
        Instance = Me
        Me.BackColor = Color.Azure
        Me.DoubleBuffered = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.TopMost = True ' damit die Form auf bleibt, auch wenn man mal daneben klickt
    End Sub

    Private Sub startGame()
        addNewTiles()
        Me.Invalidate()
        Stopwatch.Start()
        tmrStatistics.Start()
    End Sub

    Private Sub Restart()
        Tiles.Clear()
        addNewTiles()
        GameOver = False
        Me.Invalidate()
        Stopwatch.Reset()
        Stopwatch.Start()
        tmrStatistics.Start()
    End Sub

    Private Sub addNewTiles()
        For row As Integer = 1 To 3
            Dim black = _rnd.Next(1, 4)
            For column As Integer = 1 To 3
                Tiles.Add(New Tile(column = black, column, row))
            Next
        Next
    End Sub

    Private Sub frmGame_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        If GameOver Then Return ' einfach direkt rausspringen, wenn GameOver ist
        Dim tile = getTilefromLocation(e.Location)
        If tile Is Nothing Then Return ' wenn auf Rand geklickt, kann passieren dann return
        Actions += 1
        If tile.rowNumber = 3 AndAlso tile.black Then
            Points += 1

            ' letzte Reihe entfernen
            Tiles.RemoveRange(Tiles.Count - 3, 3)

            ' Nummerierung korrigieren
            For Each t As Tile In Tiles
                t.rowNumber += 1
            Next

            ' neue Reihe hinzufügen
            Dim black = _rnd.Next(1, 4)
            For i As Integer = 1 To 3
                Tiles.Insert(0, New Tile(If(i = black, True, False), i, 1))
            Next
        Else
            GameOver = True
            Stopwatch.Stop()
            Time = Stopwatch.Elapsed.ToString("mm':'ss':'fff")
            tmrStatistics.Stop()
        End If

        ' zeichnen
        Me.Invalidate()
    End Sub

    Private Sub frmGame_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        If Not GameOver Then
            For Each t As Tile In Tiles
                t.Draw(e.Graphics)
            Next
            For i As Integer = 1 To 2
                e.Graphics.DrawLine(Pens.Red, New Point(0, (Me.ClientSize.Height \ 3) * i), New Point(Me.ClientSize.Width, (Me.ClientSize.Height \ 3) * i))
            Next
            Button1.Visible = False
        Else
            Dim text = "Game Over!"
            Dim font = New Font("Segoe UI", 32.0, FontStyle.Bold Or FontStyle.Italic)
            Dim size = e.Graphics.MeasureString(text, font)
            e.Graphics.DrawString(text, font, Brushes.Black, New Point(Me.ClientSize.Width \ 2 - Convert.ToInt32(size.Width) \ 2, 150))

            text = "Press R to restart!"
            font = New Font("Segoe UI", 14.0, FontStyle.Bold Or FontStyle.Italic)
            size = e.Graphics.MeasureString(text, font)
            e.Graphics.DrawString(text, font, Brushes.Black, New Point(Me.ClientSize.Width \ 2 - Convert.ToInt32(size.Width) \ 2, 200))

            text = "Statistics:"
            font = New Font("Segoe UI", 18.0, FontStyle.Bold Or FontStyle.Italic Or FontStyle.Underline)
            e.Graphics.DrawString(text, font, Brushes.Black, New Point(10, 270))

            text = "Points: " & Points.ToString()
            font = New Font("Segoe UI", 14.0, FontStyle.Bold Or FontStyle.Italic)
            e.Graphics.DrawString(text, font, Brushes.Black, New Point(10, 310)) ' klar könnte man bei den ganzen 310s lieber ne KONSTANTE benutzen, allerdings können wir dann aber auch nicht mehr einzelnd
            ' sondern immer nur als Reihe umstellen, wollen wir nicht :P

            text = "APM: " & ActionsPerMinute.ToString()
            font = New Font("Segoe UI", 14.0, FontStyle.Bold Or FontStyle.Italic)
            e.Graphics.DrawString(text, font, Brushes.Black, New Point(145, 310))

            text = "Time: " & Time.ToString()
            font = New Font("Segoe UI", 14.0, FontStyle.Bold Or FontStyle.Italic)
            size = e.Graphics.MeasureString(text, font)
            e.Graphics.DrawString(text, font, Brushes.Black, New Point(Me.ClientSize.Width - 10 - Convert.ToInt32(size.Width), 310))
            Button1.Visible = True
        End If
    End Sub

    Private Function getTilefromLocation(point As Point) As Tile
        For Each t As Tile In Tiles()
            If t.gPath.IsVisible(point) Then Return t
        Next
        Return Nothing
    End Function

    Private Sub tmrStatistics_Tick(sender As Object, e As EventArgs) Handles tmrStatistics.Tick
        ' Beste Beispiel für NICHT mehrere Timer a la: tmrAPM, tmrPPM, tmrWeißDerGeierIchWasteResourcenxD
        ActionsPerMinute = (Actions - LastCheckedActions) * 60 ' Anzahl der Klicks der letzten Sekunde * 60
        LastCheckedActions = Actions
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Restart()
    End Sub
End Class
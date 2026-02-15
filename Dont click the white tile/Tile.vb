Public Class Tile

    Public Property Black As Boolean
    Public Property rowNumber As Integer
    Public Property columnNumber As Integer
    Public Property gPath As Drawing2D.GraphicsPath

    Sub New(black As Boolean, columnNumber As Integer, rowNumber As Integer)
        Me.Black = black
        Me.rowNumber = rowNumber
        Me.columnNumber = columnNumber
    End Sub

    Public Sub Draw(g As Graphics)
        Dim width = frmGame.Instance.ClientSize.Width
        Dim height = frmGame.Instance.ClientSize.Height

        Dim x1 = (width \ 3) * (Me.columnNumber - 1) ' Window Rand links
        Dim y1 = (height \ 3) * (Me.rowNumber - 1) ' Windows Title Bar Höhe

        Dim x2 = (width \ 3) * Me.columnNumber
        Dim y2 = (height \ 3) * (Me.rowNumber - 1)

        Dim x3 = (width \ 3) * Me.columnNumber
        Dim y3 = (height \ 3) * Me.rowNumber

        Dim x4 = (width \ 3) * (Me.columnNumber - 1)
        Dim y4 = (height \ 3) * Me.rowNumber

        Me.gPath = New Drawing2D.GraphicsPath()
        Me.gPath.AddPolygon(New Point() {New Point(x1, y1), New Point(x2, y2), New Point(x3, y3), New Point(x4, y4)})

        g.FillPath(If(Black, Brushes.Black, Brushes.White), gPath)
    End Sub

End Class
Imports System.Drawing

Public Class Tile

    Public Property Black As Boolean
    Public Property RowNumber As Integer
    Public Property ColumnNumber As Integer
    Public Property GPath As Drawing2D.GraphicsPath

    Sub New(black As Boolean, columnNumber As Integer, rowNumber As Integer)
        Me.Black = black
        Me.RowNumber = rowNumber
        Me.ColumnNumber = columnNumber
    End Sub

    Public Sub Draw(g As Graphics)
        Dim width = FrmGame.Instance.ClientSize.Width
        Dim height = FrmGame.Instance.ClientSize.Height

        Dim x1 = (width \ 3) * (Me.ColumnNumber - 1) ' Window Rand links
        Dim y1 = (height \ 3) * (Me.RowNumber - 1) ' Windows Title Bar Höhe

        Dim x2 = (width \ 3) * Me.ColumnNumber
        Dim y2 = (height \ 3) * (Me.RowNumber - 1)

        Dim x3 = (width \ 3) * Me.ColumnNumber
        Dim y3 = (height \ 3) * Me.RowNumber

        Dim x4 = (width \ 3) * (Me.ColumnNumber - 1)
        Dim y4 = (height \ 3) * Me.RowNumber

        Me.GPath = New Drawing2D.GraphicsPath()
        Me.GPath.AddPolygon(New Point() {New Point(x1, y1), New Point(x2, y2), New Point(x3, y3), New Point(x4, y4)})

        g.FillPath(If(Black, Brushes.Black, Brushes.White), GPath)
    End Sub

End Class

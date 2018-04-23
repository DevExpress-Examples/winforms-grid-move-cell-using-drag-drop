Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base.ViewInfo



Namespace WindowsApplication1
	Public Class GridViewHelper

		Private gridView As GridView
		Private sourceGridCell As GridCell
		Private lastMouseDownMoveCell As GridCell


		Public Sub New(ByVal gridView As GridView)
			Me.gridView = gridView
			AddHandler gridView.MouseDown, AddressOf gridView_MouseDown
			AddHandler gridView.MouseMove, AddressOf gridView_MouseMove
			AddHandler gridView.MouseUp, AddressOf gridView_MouseUp
			AddHandler gridView.CustomDrawCell, AddressOf gridView_CustomDrawCell
		End Sub



		Private Sub gridView_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim view As BaseView = gridView.GridControl.GetViewAt(e.Location)
			TryCast(view, GridView).GridControl.Cursor = Cursors.Default
			Dim baseHI As BaseHitInfo = view.CalcHitInfo(e.Location)
			Dim gridHI As GridHitInfo = TryCast(baseHI, GridHitInfo)

			If gridHI.HitTest = GridHitTest.RowEdge Then
				sourceGridCell = Nothing
				lastMouseDownMoveCell = Nothing
				baseHI = view.CalcHitInfo(New Point(e.X, e.Y - 3))
				gridHI = TryCast(baseHI, GridHitInfo)
				If gridHI.HitTest = GridHitTest.RowCell Then
					Dim val As Object = (TryCast(view, GridView)).GetRowCellValue(gridHI.RowHandle, gridHI.Column)
					If val Is Nothing OrElse val.ToString() = String.Empty Then
						Return
					End If
                    sourceGridCell = New GridCell(gridHI.RowHandle, gridHI.Column)
                    lastMouseDownMoveCell = New GridCell(sourceGridCell.RowHandle, sourceGridCell.Column)
				End If
			End If
		End Sub



		Private Sub gridView_CustomDrawCell(ByVal sender As Object, ByVal e As RowCellCustomDrawEventArgs)
			If lastMouseDownMoveCell Is Nothing OrElse sourceGridCell Is Nothing Then
				Return
			End If
            If e.RowHandle = lastMouseDownMoveCell.RowHandle AndAlso e.Column.FieldName = lastMouseDownMoveCell.Column.FieldName Then
                e.Graphics.DrawRectangle(New Pen(Color.Gray), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
            End If
		End Sub



		Private Sub gridView_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim view As BaseView = gridView.GridControl.GetViewAt(e.Location)
			Dim baseHI As BaseHitInfo = view.CalcHitInfo(e.Location)
			Dim gridHI As GridHitInfo = TryCast(baseHI, GridHitInfo)

			If gridHI.HitTest = GridHitTest.RowCell AndAlso sourceGridCell IsNot Nothing Then
				Dim is_changed As Boolean = False

                If lastMouseDownMoveCell.RowHandle <> gridHI.RowHandle OrElse lastMouseDownMoveCell.Column.FieldName <> gridHI.Column.FieldName Then
                    is_changed = True
                    Dim rowHandle As Integer = lastMouseDownMoveCell.RowHandle
                    Dim gridCol As GridColumn = lastMouseDownMoveCell.Column
                    lastMouseDownMoveCell = New GridCell(gridHI.RowHandle, gridHI.Column)
                    TryCast(view, GridView).InvalidateRowCell(rowHandle, gridCol)
                End If


                If is_changed Then
                    TryCast(view, GridView).InvalidateRowCell(lastMouseDownMoveCell.RowHandle, lastMouseDownMoveCell.Column)
                End If

            ElseIf gridHI.HitTest = GridHitTest.RowEdge AndAlso sourceGridCell Is Nothing Then
                baseHI = view.CalcHitInfo(New Point(e.X, e.Y - 3))
                gridHI = TryCast(baseHI, GridHitInfo)
                If gridHI.HitTest = GridHitTest.RowCell Then
                    If gridHI.RowHandle = (TryCast(view, GridView)).FocusedRowHandle AndAlso gridHI.Column Is (TryCast(view, GridView)).FocusedColumn Then
                        TryCast(view, GridView).GridControl.Cursor = Cursors.SizeAll
                    End If
                End If
            Else
                TryCast(view, GridView).GridControl.Cursor = Cursors.Default
            End If
		End Sub



		Private Sub gridView_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
			If sourceGridCell IsNot Nothing AndAlso lastMouseDownMoveCell IsNot Nothing Then
				Dim view As BaseView = gridView.GridControl.GetViewAt(e.Location)
				Dim val As Object = (TryCast(view, GridView)).GetRowCellValue(sourceGridCell.RowHandle, sourceGridCell.Column)
				TryCast(view, GridView).SetRowCellValue(sourceGridCell.RowHandle, sourceGridCell.Column, "")
				TryCast(view, GridView).SetRowCellValue(lastMouseDownMoveCell.RowHandle, lastMouseDownMoveCell.Column, val)
				TryCast(view, GridView).ClearSelection()
				TryCast(view, GridView).SelectCell(lastMouseDownMoveCell.RowHandle, lastMouseDownMoveCell.Column)
				TryCast(view, GridView).FocusedRowHandle = lastMouseDownMoveCell.RowHandle
				TryCast(view, GridView).FocusedColumn = lastMouseDownMoveCell.Column
				sourceGridCell = Nothing
				Dim rowHandle As Integer = lastMouseDownMoveCell.RowHandle
				Dim gridCol As GridColumn = lastMouseDownMoveCell.Column
				lastMouseDownMoveCell = Nothing
				TryCast(view, GridView).InvalidateRowCell(rowHandle, gridCol)
			End If
		End Sub
	End Class
End Namespace

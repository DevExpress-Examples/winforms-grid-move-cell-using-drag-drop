Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid



Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
			gridView1.OptionsView.ColumnAutoWidth = False
			WindowState = FormWindowState.Maximized
			gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused
			gridView1.OptionsSelection.MultiSelect = True
			gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect
			gridControl1.DataSource = CreateTable(20, 20)
			Dim tempVar As New GridViewHelper(gridView1)
		End Sub



		Private Function CreateTable(ByVal columnCount As Integer, ByVal rowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			For i As Integer = 0 To columnCount - 1
				tbl.Columns.Add(String.Format("Column{0}", i), GetType(String))
			Next i
			For i As Integer = 0 To rowCount - 1
				tbl.Rows.Add(New Object() { })
			Next i
			Return tbl
		End Function
	End Class
End Namespace
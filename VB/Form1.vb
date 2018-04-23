Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form
				Private Function CreateTable(ByVal RowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			tbl.Columns.Add("Name", GetType(String))
			tbl.Columns.Add("ID", GetType(Integer))
			tbl.Columns.Add("Number", GetType(Integer))
			tbl.Columns.Add("Date", GetType(DateTime))
			For i As Integer = 0 To RowCount - 1
				tbl.Rows.Add(New Object() { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) })
			Next i
			Return tbl
				End Function


		Public Sub New()
			InitializeComponent()
			gridControl1.DataSource = CreateTable(20)
			Dim TempMyNavigationHelper As MyNavigationHelper = New MyNavigationHelper(gridView1)
		End Sub
	End Class


	Public Class MyNavigationHelper

		Private ReadOnly _View As GridView
		Public Sub New(ByVal view As GridView)
			_View = view
			AddHandler view.GridControl.EditorKeyDown, AddressOf GridControl_EditorKeyDown
			AddHandler view.GridControl.KeyDown, AddressOf GridControl_EditorKeyDown
		End Sub

		Private Sub GridControl_EditorKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.KeyCode = Keys.Tab Then
				e.Handled = True
				PerformNavigation(e.Modifiers = Keys.Shift)
			End If
		End Sub

		Private Sub PerformNavigation(ByVal backward As Boolean)
			Dim delta As Integer
			Dim nextRowHandle As Integer = GetNextRowHandle(backward, delta)
			Dim col As GridColumn = GetNextColumn(backward, delta)
			_View.FocusedColumn = col
			_View.FocusedRowHandle = nextRowHandle
		End Sub

		Private Function GetNextRowHandle(ByVal backward As Boolean, <System.Runtime.InteropServices.Out()> ByRef delta As Integer) As Integer
			delta = 0
			Dim focusedRowHandle As Integer = _View.FocusedRowHandle
			If backward Then
				focusedRowHandle += -1
			Else
				focusedRowHandle += 1
			End If
			If focusedRowHandle < 0 Then
				delta = -1
				Return _View.DataRowCount - 1
			End If
			If focusedRowHandle = _View.DataRowCount Then
				delta = 1
				Return 0
			End If
			Return focusedRowHandle
		End Function
		Private Function GetNextColumn(ByVal backward As Boolean, ByVal delta As Integer) As GridColumn
			Dim visibleIndex As Integer = _View.FocusedColumn.VisibleIndex
			visibleIndex += delta
			If visibleIndex < 0 Then
				visibleIndex = _View.VisibleColumns.Count - 1
			End If
			If visibleIndex = _View.VisibleColumns.Count Then
				visibleIndex = 0
			End If
			Return _View.VisibleColumns(visibleIndex)
		End Function
	End Class
End Namespace
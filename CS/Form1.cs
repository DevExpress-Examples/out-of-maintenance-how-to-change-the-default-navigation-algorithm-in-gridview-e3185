using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
                private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("ID", typeof(int));
            tbl.Columns.Add("Number", typeof(int));
            tbl.Columns.Add("Date", typeof(DateTime));
            for (int i = 0; i < RowCount; i++)
                tbl.Rows.Add(new object[] { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) });
            return tbl;
        }
        

        public Form1()
        {
            InitializeComponent();
            gridControl1.DataSource = CreateTable(20);
            new MyNavigationHelper(gridView1);
        }
    }


    public class MyNavigationHelper
    {

        private readonly GridView _View;
        public MyNavigationHelper(GridView view)
        {
            _View = view;
            view.GridControl.EditorKeyDown += GridControl_EditorKeyDown;
            view.GridControl.KeyDown += GridControl_EditorKeyDown;
        }

        void GridControl_EditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                PerformNavigation(e.Modifiers == Keys.Shift);
            }
        }

        private void PerformNavigation(bool backward)
        {
            int delta;
            int nextRowHandle = GetNextRowHandle(backward, out delta);
            GridColumn col = GetNextColumn(backward, delta);
            _View.FocusedColumn = col;
            _View.FocusedRowHandle = nextRowHandle;
        }

        private int GetNextRowHandle(bool backward, out int delta)
        {
            delta = 0;
            int focusedRowHandle = _View.FocusedRowHandle;
            focusedRowHandle += backward ? -1 : 1;
            if (focusedRowHandle < 0)
            {
                delta = -1;
                return _View.DataRowCount - 1;
            }
            if (focusedRowHandle == _View.DataRowCount)
            {
                delta = 1;
                return 0;
            }
            return focusedRowHandle;
        }
        private GridColumn GetNextColumn(bool backward, int delta)
        {
            int visibleIndex = _View.FocusedColumn.VisibleIndex;
            visibleIndex += delta;
            if (visibleIndex < 0)
                visibleIndex = _View.VisibleColumns.Count - 1;
            if (visibleIndex == _View.VisibleColumns.Count)
                visibleIndex = 0;
            return _View.VisibleColumns[visibleIndex];
        }
    }
}